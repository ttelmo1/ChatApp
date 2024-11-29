using System;
using MediatR;

namespace ChatApp.Application.Commands.SendMessage;

public class SendMessageCommand : IRequest<Unit>
{
    public string Content { get; set; }
    public int UserSenderId { get; set; }
    public int ChatRoomId { get; set; }
}

