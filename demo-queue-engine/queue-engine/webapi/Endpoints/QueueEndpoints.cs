using System.Text.Json;
using webapi.Context;
using webapi.Models;
using webapi.Requests;

namespace webapi.Endpoints;

public abstract class QueueEndpoints
{
    public static void AddEndpoints(WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<QueueEndpoints>>();
        const string Default = "default";
        
        app.MapGet("/", () => Results.Ok("Ok"));
        
        app.MapPost("/enqueue", async (QueueRequest request, AppDbContext db) =>
        {
            try
            {
                var category = string.IsNullOrWhiteSpace(request.Category) ? Default : request.Category!;
                var tag = string.IsNullOrWhiteSpace(request.Tag) ? Default : request.Tag!;
    
                var msg = new QueueMessage {
                    Category  = category,
                    Tag       = tag,
                    Payload   = request.Payload,
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString(),
                };
                
                db.QueueMessages.Add(msg);
                await db.SaveChangesAsync();

                logger.LogInformation("Enqueued: {MessageJson}", JsonSerializer.Serialize(msg));

                return Results.Accepted();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
        
        app.MapGet("/dequeue", async (string? category, string? tag,AppDbContext db) =>
        {
            var cat = string.IsNullOrWhiteSpace(category) ? Default : category!;
            var tg  = string.IsNullOrWhiteSpace(tag)      ? Default : tag!;

            var query = db.QueueMessages
                          .Where(m => m.Status == "Pending" && m.Category == cat);

            if (!string.IsNullOrEmpty(tg))
                query = query.Where(m => m.Tag == tg);

            var msg = await query.OrderBy(m => m.CreatedAt)
                                 .FirstOrDefaultAsync();

            if (msg is null)
                return Results.NoContent();

            msg.Status = "Processing";
            await db.SaveChangesAsync();
            return Results.Ok(new { msg.Id, msg.Payload });
        });
        
        app.MapPost("/ack/{id}", async (string id,bool autoDelete, AppDbContext db) =>
        {
            var msg = await db.QueueMessages
                .FirstOrDefaultAsync(m => m.Id == id && m.Status == "Processing");

            if (msg is null)
                return Results.NotFound();

            if (autoDelete)
                db.QueueMessages.Remove(msg);
            else
                msg.Status = "Completed";

            msg.Status = "Completed";
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}