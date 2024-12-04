namespace ChatApp.Domain.Entities;

public class ChatMessage
{
    public int Id { get; private set; }
    public string Content { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string UserId { get; private set; }
    public string ChatRoomId { get; private set; }
    
    public ChatMessage(string content, string userId, string chatRoomId)
    {
        Content = content;
        UserId = userId;
        ChatRoomId = chatRoomId;
        Timestamp = DateTime.UtcNow;
    }

}
