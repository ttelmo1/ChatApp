using ChatApp.Domain.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ChatApp.Infrastructure.Data.Services;

public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMQService(
        IConfiguration configuration)
    {
        try
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest"
            };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;

        }
        catch (Exception ex)
        {
            throw new Exception("Error connecting to RabbitMQ", ex);
        }

    }
    
    public void Dispose()
    {
        try
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            _channel?.DisposeAsync();
            _connection?.DisposeAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error disposing RabbitMQ connection", ex);
        }
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        try
        {
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var messageJson = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("Error publishing message to RabbitMQ", ex);
        }
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> messageHandler)
    {
        try
        {
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(messageJson);

                await messageHandler(message);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true,
                consumer: consumer);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception("Error subscribing to RabbitMQ queue", ex);
        }
    }
}
