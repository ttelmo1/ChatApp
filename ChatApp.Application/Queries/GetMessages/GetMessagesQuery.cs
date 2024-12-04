using ChatApp.Domain.Entities;
using MediatR;

namespace ChatApp.Application.Queries.GetMessages;

public class GetMessagesQuery : IRequest<IEnumerable<ChatMessage>>
{
    public string RoomId { get; set; }
    public int Count { get; set; }
}
