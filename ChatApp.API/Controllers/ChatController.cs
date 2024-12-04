using ChatApp.API.Hubs;
using ChatApp.Application.Commands.CreateRoom;
using ChatApp.Application.Queries.GetMessages;
using ChatApp.Application.Queries.GetRooms;
using ChatApp.Infraestructure.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRabbitMQService _rabbitMQService;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IMediator mediator,
        IRabbitMQService rabbitMQService,
        IHubContext<ChatHub> hubContext)
    {
        _mediator = mediator;
        _rabbitMQService = rabbitMQService;
        _hubContext = hubContext;
    }


    [HttpGet("rooms")]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _mediator.Send(new GetRoomsQuery());
        return Ok(rooms);
    }

    [HttpPost("rooms")]
    public async Task<IActionResult> CreateRoom()
    {
        var roomId = await _mediator.Send(new CreateRoomCommand());
        return CreatedAtAction(nameof(GetRooms), new { roomId }, null);
    }

    [HttpGet("rooms/{roomId}/messages")]
    public async Task<IActionResult> GetMessages(string roomId, [FromQuery] int count = 50)
    {
        var messages = await _mediator.Send(new GetMessagesQuery { RoomId = roomId, Count = count });
        return Ok(messages);
    }
}
