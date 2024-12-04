using ChatApp.API.Controllers;
using ChatApp.Application.Commands.CreateRoom;
using ChatApp.Application.Queries.GetMessages;
using ChatApp.Application.Queries.GetRooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ChatApp.Domain.Entities;

namespace Tests;

public class ChatControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ChatController _controller;

    public ChatControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ChatController(_mediatorMock.Object);
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
    public async Task CreateRoom_ReturnsOkResult_WithRoomId()
    {
        // Arrange
        var expectedRoomId = Guid.NewGuid().ToString();
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateRoomCommand>(), default))
                .ReturnsAsync(expectedRoomId);

        // Act
        var result = await _controller.CreateRoom();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<string>(okResult.Value);
        Assert.Equal(expectedRoomId, response);
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
