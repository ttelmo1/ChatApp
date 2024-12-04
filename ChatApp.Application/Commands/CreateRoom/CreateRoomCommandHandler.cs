using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using MediatR;

namespace ChatApp.Application.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, string>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public CreateRoomCommandHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<string> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new ChatRoom();
        await _chatRoomRepository.AddAsync(room);
        return room.Id;
    }
}
