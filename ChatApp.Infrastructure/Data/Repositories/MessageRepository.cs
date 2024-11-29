using System;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly ApplicationDbContext _context;

    public ChatMessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

     public async Task<IEnumerable<ChatMessage>> GetLastMessagesAsync(int chatRoomId, int count = 50)
    {
        return await _context.ChatMessages
            .Where(m => m.ChatRoomId == chatRoomId)
            .OrderByDescending(m => m.Timestamp)
            .Take(count)
            .ToListAsync();
    }
    
    public async Task AddMessageAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }
}
