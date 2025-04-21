using queue.client;

        
var factory    = new ConnectionFactory("http://localhost:5000");
using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();

Console.WriteLine("\nEscribe mensajes para encolar. Escribe 'exit' para salir.\n");
        
while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if (line is null)
        continue;

    if (line.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (string.IsNullOrWhiteSpace(line))
        continue;

    // channel.BasicPublish(
    //     exchange: "",          // no se usa por ahora
    //     routingKey: "",        // no se usa por ahora
    //     payload: line
    // );

    try
    {
        await channel.BasicPublishAsync("", "", line);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[Encolado] {line}");
        Console.ResetColor();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}

Console.WriteLine("ProducerApp finalizado.");