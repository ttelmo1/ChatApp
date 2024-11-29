using System;

namespace ChatApp.Domain.Entities;

public class ChatRoom
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public List<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

    public ChatRoom(string name)
    {
        Name = name;
    }
}
