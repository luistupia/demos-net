using System.Net;
using System.Net.Http.Json;
using queue.client.Exceptions;
using queue.client.Models;

namespace queue.client;

internal class Model(HttpClient http) : IModel
{
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Publica un mensaje en el exchange especificado con la clave de enrutamiento y el contenido proporcionados.
    /// </summary>
    /// <param name="category">El nombre del exchange al que se publicará el mensaje.</param>
    /// <param name="tag">La clave de enrutamiento que se usará para la entrega del mensaje.</param>
    /// <param name="payload">El contenido del mensaje a publicar.</param>
    /// <exception cref="QueueEngineException">
    /// Se lanza si ocurre un error de red o un error inesperado durante el proceso de publicación.
    /// </exception>
    public void BasicPublish(string category, string tag, string payload)
    {
        PublishAsync(category,tag, payload)
            .GetAwaiter()
            .GetResult();
    }
    
    /// <summary>
    /// Publica un mensaje asincorinicamente en el exchange especificado con la clave de enrutamiento y el contenido proporcionados.
    /// </summary>
    /// <param name="category">El nombre del exchange al que se publicará el mensaje.</param>
    /// <param name="tag">La clave de enrutamiento que se usará para la entrega del mensaje.</param>
    /// <param name="payload">El contenido del mensaje a publicar.</param>
    /// <exception cref="QueueEngineException">
    /// Se lanza si ocurre un error de red o un error inesperado durante el proceso de publicación.
    /// </exception>
    public Task BasicPublishAsync(string category, string tag, string payload)
    {
        return PublishAsync(category, tag, payload);
    }
    
    /// <summary>
    /// Versión asíncrona que expone el loop de consumo.
    /// </summary>
    public Task BasicConsumeAsync(
        string category, string tag,
        bool autoAck,
        Func<Message, Task> onMessage,
        CancellationToken cancellationToken = default)
    {
        return ConsumeLoopAsync(category,tag,autoAck, onMessage, cancellationToken);
    }

    /// <summary>
    /// Versión sincrónica que lanza el loop en background.
    /// </summary>
    public void BasicConsume(string category, string tag, bool autoAck, Action<Message> onMessage)
    {
        _cts = new CancellationTokenSource();

        _ = ConsumeLoopAsync(
            category,
            tag,
            autoAck,
            msg => {
                onMessage(msg);
                return Task.CompletedTask;
            },
            _cts.Token
        );
    }

    /// <summary>
    /// Confirma la entrega de un mensaje identificado por su etiqueta de entrega.
    /// </summary>
    /// <param name="deliveryTag">El identificador del mensaje a confirmar.</param>
    /// <exception cref="QueueEngineException">
    /// Se lanza cuando ocurre un error inesperado durante el proceso de confirmación.
    /// </exception>
    public void BasicAck(int deliveryTag)
    {
        try
        {
            http.PostAsync($"/ack/{deliveryTag}", null).GetAwaiter().GetResult();
        }
        catch (Exception ex) when (ex is not QueueEngineException)
        {
            throw new QueueEngineException(
                "Error inesperado al confirmar el mensaje.", ex);
        }
    }

    public void Dispose()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    /// <summary>
    /// Loop común que consulta /dequeue, maneja autoAck y llama al callback.
    /// </summary>
    private async Task ConsumeLoopAsync(
        string category, string tag,
        bool autoAck,
        Func<Message, Task> onMessage,
        CancellationToken ct)
    {
        const int emptyDelayMs = 200;
        const int retryDelaySec = 3;

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var url = $"/dequeue?category={Uri.EscapeDataString(category)}" +
                          $"&tag={Uri.EscapeDataString(tag)}";
                
                var resp = await http.GetAsync(url, ct);
                if (resp.StatusCode == HttpStatusCode.NoContent)
                {
                    await Task.Delay(emptyDelayMs, ct);
                    continue;
                }

                resp.EnsureSuccessStatusCode();
                var msg = await resp.Content.ReadFromJsonAsync<Message>(cancellationToken: ct);
                if (msg is null)
                    continue;

                if (autoAck)
                    await AcknowledgeAsync(autoAck,msg.Id, ct);

                await onMessage(msg);

                if (!autoAck)
                    await AcknowledgeAsync(autoAck,msg.Id, ct);
            }
            catch (HttpRequestException ex)
            {
                try { await Task.Delay(TimeSpan.FromSeconds(retryDelaySec), ct); }
                catch { /* cancellation */ }

                throw new QueueEngineException("Error de red al consumir mensajes.", ex);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (QueueEngineException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new QueueEngineException("Error inesperado en el consumidor.", ex);
            }
        }
    }
    
    /// <summary>
    /// Extraemos el POST /ack para no repetir código.
    /// </summary>
    private Task AcknowledgeAsync(bool autoAck,string messageId, CancellationToken ct)
    {
        return http.PostAsync($"/ack/{messageId}?autoDelete={autoAck}", null, ct)
            .ContinueWith(t =>
            {
                if (t.IsFaulted)
                    throw new QueueEngineException(
                        $"Error al hacer ack del mensaje {messageId}.", t.Exception);
            }, ct);
    }

    private async Task PublishAsync(string category, string tag, string payload)
    {
        try
        {
            var dto = new QueuePayload(category,tag,payload);
            var resp = await http.PostAsJsonAsync("/enqueue", dto);
            resp.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            throw new QueueEngineException(
                "Error al publicar en la cola.", ex);
        }
        catch (Exception ex) when (ex is not QueueEngineException)
        {
            throw new QueueEngineException(
                "Error inesperado al publicar en la cola.", ex);
        }
    }
}