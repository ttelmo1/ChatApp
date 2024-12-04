using ChatApp.Domain.Entities;
using ChatApp.Domain.Repositories;
using ChatApp.Infraestructure.Services.Interfaces;
using MediatR;

namespace ChatApp.Application.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Unit>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IStockQuoteService _stockQuoteService;
    private readonly IRabbitMQService _rabbitMQService;
    
    public SendMessageCommandHandler(
        IChatMessageRepository messageRepository,
        IStockQuoteService stockQuoteService,
        IRabbitMQService rabbitMQService)
    {
        _chatMessageRepository = messageRepository;
        _stockQuoteService = stockQuoteService;
        _rabbitMQService = rabbitMQService;
    }

    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (request.Content.Trim().StartsWith("/stock="))
        {
            var stockCode = request.Content.Substring(7);
            var quote = await _stockQuoteService.GetStockQuoteAsync(stockCode);
            if(quote.IsSuccess == false)
            {
                var notFoundMessage = new ChatMessage(
                    quote.Message,
                    "StockBot",
                    request.ChatRoomId);

                await _rabbitMQService.PublishAsync("stock_quotes", notFoundMessage);
                return Unit.Value;
            }

            var botMessage = new ChatMessage(
                $"{stockCode.ToUpper()}: quote is ${quote.Data} per share",
                "StockBot",
                request.ChatRoomId);

            await _rabbitMQService.PublishAsync("stock_quotes", botMessage);
            return Unit.Value;
        }

        var message = new ChatMessage(
            request.Content,
            request.UserSenderId,
            request.ChatRoomId);
        await _chatMessageRepository.AddMessageAsync(message);
        return Unit.Value;
    }
}
