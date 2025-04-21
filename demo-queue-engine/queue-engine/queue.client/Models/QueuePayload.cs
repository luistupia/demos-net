namespace queue.client.Models;

public record QueuePayload(
  string? Category,
  string? Tag,
  string  Payload
);