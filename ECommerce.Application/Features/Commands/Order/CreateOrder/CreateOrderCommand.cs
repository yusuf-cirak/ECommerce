using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Hubs;
using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Commands.Order.CreateOrder;

public class CreateOrderCommandRequest:IRequest<CreateOrderCommandResponse>
{
    public string Description { get; set; }
    public string Address { get; set; }
}

public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommandRequest,CreateOrderCommandResponse>
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;
    private readonly IOrderHubService _orderHubService;
    public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService)
    {
        _orderService = orderService;
        _basketService = basketService;
        _orderHubService = orderHubService;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _orderService.CreateOrderAsync(new()
        {
            Address = request.Address,
            Description = request.Description,
            BasketId = _basketService.GetUserActiveBasket?.Id.ToString()
        });

       await _orderHubService.OrderCreatedMessageAsync($"Yeni sipariş gelmiştir!\nAdres:{request.Address}");

        return new();
    }
}

public class CreateOrderCommandResponse
{
}

