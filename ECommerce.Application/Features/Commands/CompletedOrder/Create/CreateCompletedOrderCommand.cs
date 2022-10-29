using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Commands.CompletedOrder.Create;

public sealed class CreateCompletedOrderCommandRequest:IRequest<bool>
{
    public string Id { get; set; }
}

public sealed class CreateCompletedOrderCommandHandler:IRequestHandler<CreateCompletedOrderCommandRequest,bool>
{
    private readonly IOrderService _orderService;
    private readonly IMailService _mailService;

    public CreateCompletedOrderCommandHandler(IOrderService orderService, IMailService mailService)
    {
        _orderService = orderService;
        _mailService = mailService;
    }

    public async Task<bool> Handle(CreateCompletedOrderCommandRequest request, CancellationToken cancellationToken)
    {
        (bool succeded, DTOs.Order.CompletedOrderDto completedOrder) = await _orderService.CompleteOrderAsync(request.Id);

        if (succeded)
        {
            await _mailService.SendCompletedOrderMailAsync(completedOrder.EmailAddress, completedOrder.OrderCode,
                completedOrder.OrderDate, completedOrder.UserName);

            return true;
        }

        return false;
    }
}