using ChatApp.API.Hubs;
using ChatApp.Application.Commands.SendMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Tests;

public class ChatHubTest
{
    private readonly Mock<IHubCallerClients> _mockClients;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IClientProxy> _clientProxyMock;
    private readonly Mock<IGroupManager> _groupManager;
    private readonly ChatHub _chatHub;

    public ChatHubTest()
    {
        _mockClients = new Mock<IHubCallerClients>();
        _mediator = new Mock<IMediator>();
        _clientProxyMock = new Mock<IClientProxy>();
        _groupManager = new Mock<IGroupManager>();
        
        _mockClients.Setup(clients => clients.Group(It.IsAny<string>()))
            .Returns(_clientProxyMock.Object);

        _chatHub = new ChatHub(_mediator.Object)
        {
            Clients = _mockClients.Object,
            Groups = _groupManager.Object
        };
    }

    [Fact]
    public async Task SendMessageToRoom_ShouldSendCommandAndMessage()
    {
        // Arrange
        var roomName = new Guid().ToString();
        var user = "testUser";
        var message = "Hello, World!";

        _mediator.Setup(m => m.Send(It.IsAny<SendMessageCommand>(), default))
            .ReturnsAsync(Unit.Value);

        // Act
        await _chatHub.SendMessageToRoom(roomName, user, message);

        // Assert
        _mediator.Verify(m => m.Send(It.Is<SendMessageCommand>(cmd =>
            cmd.Content == message &&
            cmd.UserSenderId == user &&
            cmd.ChatRoomId == roomName), default), Times.Once);

        _clientProxyMock.Verify(client => client.SendCoreAsync(
            "ReceiveMessage",
            It.Is<object[]>(args => 
                args[0].ToString() == user &&
                args[1].ToString() == message),
            default),
            Times.Once);
    }
}