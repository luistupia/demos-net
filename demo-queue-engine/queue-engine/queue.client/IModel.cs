using queue.client.Models;

namespace queue.client;

public interface IModel : IDisposable
{
    void BasicPublish(string category, string tag, string payload);
    Task BasicPublishAsync(string category, string tag, string payload);
    void BasicConsume(string category, string tag, bool autoAck, Action<Message> onMessage);
    Task BasicConsumeAsync(
        string category, 
        string tag,
        bool autoAck,
        Func<Message, Task> onMessage,
        CancellationToken cancellationToken = default);
    public void BasicAck(int deliveryTag);
}