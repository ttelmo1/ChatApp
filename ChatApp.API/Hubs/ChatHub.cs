using ChatApp.Application.Commands.SendMessage;
using ChatApp.Application.Queries.GetMessages;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs;
[Authorize]
public class ChatHub : Hub
{
    private readonly IRequestHandler<SendMessageCommand, Unit> _sendMessageCommandHandler;
    private readonly IMediator _mediator;

    public ChatHub(IRequestHandler<SendMessageCommand, Unit> sendMessageCommandHandler,
        IMediator mediator)
    {
        _sendMessageCommandHandler = sendMessageCommandHandler;
        _mediator = mediator;
    }

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);


        var query = new GetMessagesQuery
        {
            RoomId = roomName,
            Count = 50
        };

        var messages = await _mediator.Send(query);

        await Clients.Caller.SendAsync("LoadPreviousMessages", messages);

        await Clients.Group(roomName).SendAsync("ReceiveMessage", "System", $"{Context.User?.Identity?.Name} joined");
    }


    public async Task SendMessageToRoom(string roomName, string user, string message)
    {
        var command = new SendMessageCommand
        {
            Content = message,
            UserSenderId = user,
            ChatRoomId = roomName
        };

        await _sendMessageCommandHandler.Handle(command, CancellationToken.None);

        if(message.StartsWith("/stock=")) return;

        var timestamp = DateTime.UtcNow.ToString("O");
        await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message, timestamp);
    }

    public async Task LeaveRoom(string roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName).SendAsync("ReceiveMessage", "System", $"{Context.User?.Identity?.Name} left");
    }

}
