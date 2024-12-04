using ChatApp.API.Hubs;
using ChatApp.Domain.Entities;
using ChatApp.Infraestructure.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Services;

public class StockConsumerService : BackgroundService
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<StockConsumerService> _logger;

    public StockConsumerService(
        IRabbitMQService rabbitMQService,
        IHubContext<ChatHub> hubContext,
        ILogger<StockConsumerService> logger)
    {
        _rabbitMQService = rabbitMQService;
        _hubContext = hubContext;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("StockConsumerService starting...");
                
                await _rabbitMQService.SubscribeAsync<ChatMessage>("stock_quotes", async (message) =>
                {
                    _logger.LogInformation($"Message received: {message.Content} for room {message.ChatRoomId}");
                    
                    await _hubContext.Clients.Group(message.ChatRoomId)
                        .SendAsync("ReceiveMessage",
                            message.UserId,
                            message.Content,
                            message.Timestamp.ToString("O"),
                            cancellationToken: stoppingToken);
                            
                    _logger.LogInformation("Message sent via SignalR");
                });

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StockConsumerService");
                await Task.Delay(5000, stoppingToken); 
            }
        }
    }
}