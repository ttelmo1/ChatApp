using System;

namespace ChatApp.Infraestructure.Services.Interfaces;

public interface IRabbitMQService
{
    Task PublishAsync<T>(string queueName, T message);
    Task SubscribeAsync<T>(string queueName, Func<T, Task> messageHandler);
    void Dispose();
}
