using System.ComponentModel.DataAnnotations;

namespace webapi.Models;

public class QueueMessage
{
    public required string Id { get; set; }
    public required string Category { get; set; }
    public string? Tag { get; set; }
    
    [MaxLength(int.MaxValue)]
    public required string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";
}