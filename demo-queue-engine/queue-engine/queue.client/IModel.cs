using queue.client.Models;

namespace queue.client;

public interface IModel : IDisposable
{
    void BasicPublish(string exchange, string routingKey, string payload);
    Task BasicPublishAsync(string exchange, string routingKey, string payload);
    void BasicConsume(string queue, bool autoAck, Action<Message> onMessage);
    Task BasicConsumeAsync(
        string queue,
        bool autoAck,
        Func<Message, Task> onMessage,
        CancellationToken cancellationToken = default);
    public void BasicAck(int deliveryTag);
}