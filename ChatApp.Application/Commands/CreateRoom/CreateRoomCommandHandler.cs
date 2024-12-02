using System;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using MediatR;

namespace ChatApp.Application.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public CreateRoomCommandHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new ChatRoom(request.Name);
        await _chatRoomRepository.AddAsync(room);
        return room.Id;
    }
}
