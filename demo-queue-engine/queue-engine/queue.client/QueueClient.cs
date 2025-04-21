using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using queue.client.Models;

namespace queue.client;

public class QueueClient(string baseUrl)
{
    private readonly HttpClient _http = new()
    {
        BaseAddress = new Uri(baseUrl)
    };
    
    public async Task EnqueueAsync(string payload)
    {
        var dto = new QueuePayload(payload);
        var resp = await _http.PostAsJsonAsync("/enqueue", dto);
        resp.EnsureSuccessStatusCode();
    }

    private async Task<Message?> DequeueAsync()
    {
        var resp = await _http.GetAsync("/dequeue");
        if (resp.StatusCode == HttpStatusCode.NoContent)
            return null;
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<Message>();
    }
    
    /// <summary>
    /// Bucle simple para levantar mensajes según van llegando.
    /// </summary>
    public async IAsyncEnumerable<Message> ListenAsync(
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested)
        {
            var msg = await DequeueAsync();
            if (msg != null)
                yield return msg;
            else
                await Task.Delay(200, ct);
        }
    }
}