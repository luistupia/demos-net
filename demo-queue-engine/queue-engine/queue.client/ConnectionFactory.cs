namespace queue.client;

public sealed class ConnectionFactory(string baseUrl)
{
    private string BaseUrl { get; set; } = baseUrl.TrimEnd('/');

    public IConnection CreateConnection()
        => new Connection(BaseUrl);
}