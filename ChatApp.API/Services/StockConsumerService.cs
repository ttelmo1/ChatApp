using ChatApp.API.Hubs;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Services;
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
                _logger.LogInformation("StockConsumerService iniciando...");
                
                await _rabbitMQService.SubscribeAsync<ChatMessage>("stock_quotes", async (message) =>
                {
                    Console.WriteLine($"Mensagem recebida: {message.Content} para sala {message.ChatRoomId}");
                    _logger.LogInformation($"Mensagem recebida: {message.Content} para sala {message.ChatRoomId}");
                    
                    await _hubContext.Clients.Group(message.ChatRoomId)
                        .SendAsync("ReceiveMessage",
                            message.UserId,
                            message.Content,
                            message.Timestamp.ToString("O"),
                            cancellationToken: stoppingToken);
                            
                    _logger.LogInformation("Mensagem enviada via SignalR");
                });

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no StockConsumerService");
                await Task.Delay(5000, stoppingToken); 
            }
        }
    }
}