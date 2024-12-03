using System.Threading;
using System.Threading.Tasks;
using ChatApp.API.Controllers;
using ChatApp.API.Hubs;
using ChatApp.Application.Commands.CreateRoom;
using ChatApp.Application.Queries.GetMessages;
using ChatApp.Application.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using FluentAssertions;
using ChatApp.Domain.Entities;
using ChatApp.Infraestructure.Services.Interfaces;

namespace ChatApp.Tests.Controllers;

public class ChatControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
    private readonly Mock<IHubContext<ChatHub>> _hubContextMock;
    private readonly ChatController _controller;

    public ChatControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _rabbitMQServiceMock = new Mock<IRabbitMQService>();
        _hubContextMock = new Mock<IHubContext<ChatHub>>();
            
        _controller = new ChatController(
            _mediatorMock.Object,
            _rabbitMQServiceMock.Object,
            _hubContextMock.Object
        );
    }

    [Fact]
    public async Task GetRooms_ReturnsOkResult()
    {
        // Arrange
        var expectedRooms = new List<ChatRoom>(); 
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetRoomsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRooms);

        // Act
        var result = await _controller.GetRooms();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRooms = Assert.IsAssignableFrom<IEnumerable<ChatRoom>>(okResult.Value);
        Assert.Equal(expectedRooms, returnedRooms);
    }

    [Fact]
    public async Task CreateRoom_ReturnsCreatedAtAction()
    {
        // Arrange
        var expectedRoomId = "room123";
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateRoomCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRoomId);

        // Act
        var result = await _controller.CreateRoom();

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetRooms), createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task GetMessages_ReturnsOkResult()
    {
        // Arrange
        var roomId = "room123";
        var count = 50;
        var expectedMessages = new List<ChatMessage>(); 
        _mediatorMock.Setup(m => m.Send(It.Is<GetMessagesQuery>(q => 
            q.RoomId == roomId && q.Count == count), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMessages);

        // Act
        var result = await _controller.GetMessages(roomId, count);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMessages = Assert.IsAssignableFrom<IEnumerable<ChatMessage>>(okResult.Value);
        Assert.Equal(expectedMessages, returnedMessages);
    }
}
