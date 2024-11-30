using System;
using MediatR;

namespace ChatApp.Application.Commands.SendMessage;

public class SendMessageCommand : IRequest<Unit>
{
    public string Content { get; set; }
    public string UserSenderId { get; set; }
    public int ChatRoomId { get; set; }
}

