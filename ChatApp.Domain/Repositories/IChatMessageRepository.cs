using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Repositories;

public interface IChatMessageRepository
{
    Task<IEnumerable<ChatMessage>> GetLastMessagesAsync(string chatRoomId, int count = 50);
    Task AddMessageAsync(ChatMessage message);
}
