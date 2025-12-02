using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Basket.Data.Processors
{
    public class OutBoxProcessors
        (IServiceProvider serviceProvider, IBus bus, ILogger<OutBoxProcessors> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                    var outboxMessages = await dbContext.outboxMessages
                        .Where(m => m.ProcessedOn == null)
                        .ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var messageType = Type.GetType(message.Type);
                        if (messageType == null)
                        {
                            logger.LogError("Unknown message type: {MessageType}", message.Type);
                            continue;
                        }
                        var @event = System.Text.Json.JsonSerializer.Deserialize(message.Data, messageType);
                        if (@event == null)
                        {
                            logger.LogError("Failed to deserialize message of type: {MessageType}", message.Type);
                            continue;
                        }
                        bus.Publish(@event, stoppingToken).GetAwaiter().GetResult();
                        message.ProcessedOn = DateTime.UtcNow;

                        logger.LogInformation("Processed outbox message {MessageId} of type {MessageType}", message.Id, message.Type);


                    }
                    await dbContext.SaveChangesAsync(stoppingToken);

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing outbox messages");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
