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

    public CreateCompletedOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<bool> Handle(CreateCompletedOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _orderService.CompleteOrderAsync(request.Id);

        return true;
    }
}