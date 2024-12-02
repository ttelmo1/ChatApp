using System;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Repositories;

public interface IChatRoomRepository
{
    Task<IEnumerable<ChatRoom>> GetAllAsync();
    Task AddAsync(ChatRoom chatRoom);
    Task<ChatRoom?> GetByIdAsync(Guid roomId);
}
