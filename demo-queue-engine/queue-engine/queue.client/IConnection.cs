namespace queue.client;

public interface IConnection : IDisposable
{
    IModel CreateModel();
}