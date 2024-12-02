using System;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using MediatR;

namespace ChatApp.Application.Queries.GetRooms;

public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, IEnumerable<ChatRoom>>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetRoomsQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<IEnumerable<ChatRoom>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _chatRoomRepository.GetAllAsync();
    }
}
