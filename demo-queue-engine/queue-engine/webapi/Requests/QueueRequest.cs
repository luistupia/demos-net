namespace webapi.Requests;

public record QueueRequest (
    string? Category,
    string? Tag,
    string  Payload
);