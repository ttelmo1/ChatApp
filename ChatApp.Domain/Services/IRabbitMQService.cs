using System;

namespace ChatApp.Domain.Services;

public interface IRabbitMQService
{
    Task PublishAsync<T>(string queueName, T message);
    Task SubscribeAsync<T>(string queueName, Func<T, Task> messageHandler);
    void Dispose();
}
