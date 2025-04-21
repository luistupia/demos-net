
using queue.client.Exceptions;

namespace queue.client;

internal class Connection : IConnection
{
    private readonly HttpClient _http;
    
    public Connection(string baseUrl)
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        
        try
        {
            var resp = _http
                .GetAsync("/", HttpCompletionOption.ResponseHeadersRead)
                .GetAwaiter()
                .GetResult();
            if (!resp.IsSuccessStatusCode)
                throw new QueueEngineException(
                    $"El servicio respondió {(int)resp.StatusCode} ({resp.ReasonPhrase}).");
        }
        catch (Exception ex) when (ex is not QueueEngineException)
        {
            throw new QueueEngineException(
                $"No se pudo conectar al motor en '{baseUrl}'.", ex);
        }
    }

    public IModel CreateModel()
        => new Model(_http);

    public void Dispose() => _http.Dispose();
}