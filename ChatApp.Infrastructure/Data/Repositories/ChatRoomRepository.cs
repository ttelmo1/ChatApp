using System;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ChatRoomRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ChatRoom>> GetAllAsync()
    {
        return await _dbContext.ChatRooms.ToListAsync();
    }

    public async Task AddAsync(ChatRoom chatRoom)
    {
        await _dbContext.ChatRooms.AddAsync(chatRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ChatRoom?> GetByIdAsync(Guid roomId)
    {
        return await _dbContext.ChatRooms.FindAsync(roomId);
    }
}
