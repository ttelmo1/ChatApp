using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using MediatR;

namespace ChatApp.Application.Queries.GetMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, IEnumerable<ChatMessage>>
{
    private readonly IChatMessageRepository _chatMessageRepository;

    public GetMessagesQueryHandler(IChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }

    public async Task<IEnumerable<ChatMessage>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _chatMessageRepository.GetLastMessagesAsync(request.RoomId, request.Count);
    }
}
