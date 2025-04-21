namespace Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public bool Processed { get; set; } = false;
}