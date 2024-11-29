using System;

namespace ChatApp.Domain.Entities;

public class ChatMessage
{
    public int Id { get; private set; }
    public string Content { get; private set; }
    public DateTime Timestamp { get; private set; }
    public int UserId { get; private set; }
    public int ChatRoomId { get; private set; }
    public bool IsFromBot { get; private set; }
    
    public ChatMessage(string content, int userId, int chatRoomId, bool isFromBot = false)
    {
        Content = content;
        UserId = userId;
        ChatRoomId = chatRoomId;
        IsFromBot = isFromBot;
        Timestamp = DateTime.UtcNow;
    }

}
