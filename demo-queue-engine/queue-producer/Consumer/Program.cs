using queue.client;

var factory = new ConnectionFactory("http://localhost:5000");

using var conn    = factory.CreateConnection();
using var channel = conn.CreateModel();
using var cts = new CancellationTokenSource();
/*channel.BasicConsume(
    queue: "ignored",    // no se usa en esta versión
    autoAck: false,      // false → ack tras el callback
    onMessage: msg =>
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[Recibido {msg.Id}] {msg.Payload}");
        Console.ResetColor();

        // channel.BasicAck(msg.Id);
    }
);*/

try
{
    var consumingTask = channel
        .BasicConsumeAsync(
            category: "",
            tag:"log",
            autoAck: true,
            onMessage: async msg =>
            {
                Console.WriteLine($"[{msg.Id}] {msg.Payload}");
                await Task.CompletedTask;
            },
            cancellationToken: cts.Token
        );

    await consumingTask;
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

Console.ReadKey();