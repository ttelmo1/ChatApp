using ChatApp.Application.Commands.CreateRoom;
using ChatApp.Application.Queries.GetMessages;
using ChatApp.Application.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
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
        var response = roomId;
        return Ok(response);
    }

    [HttpGet("rooms/{roomId}/messages")]
    public async Task<IActionResult> GetMessages(string roomId, [FromQuery] int count = 50)
    {
        var messages = await _mediator.Send(new GetMessagesQuery { RoomId = roomId, Count = count });
        return Ok(messages);
    }
}
