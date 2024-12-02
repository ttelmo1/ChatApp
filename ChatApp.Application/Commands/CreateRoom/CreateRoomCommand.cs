using System;
using MediatR;

namespace ChatApp.Application.Commands.CreateRoom;

public class CreateRoomCommand : IRequest<int>
{
    public string Name { get; set; }
}
