namespace ChatApp.Domain.Entities;

public class ChatRoom
{
    public string Id { get; private set; }
    public List<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

    public ChatRoom()
    {
        Id = Guid.NewGuid().ToString();
    }
}
