using System.Threading;
using System.Threading.Tasks;
using ChatApp.API.Hubs;
using ChatApp.Application.Commands.SendMessage;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace Tests
{
    public class ChatHubTest
    {
        private readonly Mock<IRequestHandler<SendMessageCommand, Unit>> _sendMessageCommandHandlerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IHubCallerClients> _clientsMock;
        private readonly Mock<IGroupManager> _groupsMock;
        private readonly Mock<HubCallerContext> _contextMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly ChatHub _chatHub;

        public ChatHubTest()
        {
            _sendMessageCommandHandlerMock = new Mock<IRequestHandler<SendMessageCommand, Unit>>();
            _mediatorMock = new Mock<IMediator>();
            _clientsMock = new Mock<IHubCallerClients>();
            _groupsMock = new Mock<IGroupManager>();
            _contextMock = new Mock<HubCallerContext>();
            _clientProxyMock = new Mock<IClientProxy>();

            _clientsMock.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_clientProxyMock.Object);

            _chatHub = new ChatHub(_sendMessageCommandHandlerMock.Object, _mediatorMock.Object)
            {
                Clients = _clientsMock.Object,
                Groups = _groupsMock.Object,
                Context = _contextMock.Object
            };
        }

        [Fact]
        public async Task SendMessageToRoom_ShouldSendCommandAndMessage()
        {
            // Arrange
            var roomName = "testRoom";
            var user = "testUser";
            var message = "Hello, World!";
            var command = new SendMessageCommand
            {
                Content = message,
                UserSenderId = user,
                ChatRoomId = roomName
            };

            _sendMessageCommandHandlerMock
                .Setup(handler => handler.Handle(It.IsAny<SendMessageCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            await _chatHub.SendMessageToRoom(roomName, user, message);

            // Assert
            _sendMessageCommandHandlerMock.Verify(handler => handler.Handle(It.Is<SendMessageCommand>(cmd =>
                cmd.Content == message &&
                cmd.UserSenderId == user &&
                cmd.ChatRoomId == roomName), It.IsAny<CancellationToken>()), Times.Once);

            _clientProxyMock.Verify(client => client.SendCoreAsync("ReceiveMessage", It.Is<object[]>(args =>
                args[0].ToString() == user &&
                args[1].ToString() == message &&
                args[2].ToString() == DateTime.UtcNow.ToString("O")), default), Times.Never);
        }

        [Fact]
        public async Task SendMessageToRoom_ShouldNotSendMessage_WhenMessageStartsWithStock()
        {
            // Arrange
            var roomName = "testRoom";
            var user = "testUser";
            var message = "/stock=AAPL";

            // Act
            await _chatHub.SendMessageToRoom(roomName, user, message);

            // Assert
            _sendMessageCommandHandlerMock.Verify(handler => handler.Handle(It.IsAny<SendMessageCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _clientProxyMock.Verify(client => client.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default), Times.Never);
        }
    }
}