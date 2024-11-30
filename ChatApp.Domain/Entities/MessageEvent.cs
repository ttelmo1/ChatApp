using System;

namespace ChatApp.Domain.Entities;

public class MessageEvent
{
    public ChatMessage Message { get; }
    public DateTime Timestamp { get; }

    public MessageEvent(ChatMessage message)
    {
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}
