using System;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using MediatR;

namespace ChatApp.Application.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
{
    private readonly IChatMessageRepository _messageRepository;
    private readonly IStockQuoteService _stockQuoteService;
    private readonly IRabbitMQService _rabbitMQService;
    
    public SendMessageCommandHandler(
        IChatMessageRepository messageRepository,
        IStockQuoteService stockQuoteService,
        IRabbitMQService rabbitMQService)
    {
        _messageRepository = messageRepository;
        _stockQuoteService = stockQuoteService;
        _rabbitMQService = rabbitMQService;
    }

    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (request.Content.StartsWith("/stock="))
        {
            var stockCode = request.Content.Substring(7);
            var quote = await _stockQuoteService.GetStockQuoteAsync(stockCode);

            var botMessage = new ChatMessage(
                $"{stockCode.ToUpper()}: quote is ${quote} per share",
                0,
                request.ChatRoomId,
                true);

            await _rabbitMQService.PublishMessageAsync("stock_quotes", botMessage);
            return Unit.Value;
        }

        var message = new ChatMessage(
            request.Content,
            request.UserSenderId,
            request.ChatRoomId);
        await _messageRepository.AddAsync(message);
        return Unit.Value;
    }
}
