using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Commands.Basket.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityCommandRequest:IRequest<UpdateBasketItemQuantityCommandResponse>
    {
        public string BasketItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateBasketItemQuantityCommandHandler:IRequestHandler<UpdateBasketItemQuantityCommandRequest, UpdateBasketItemQuantityCommandResponse>
    {
        private readonly IBasketService _basketService;

        public UpdateBasketItemQuantityCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<UpdateBasketItemQuantityCommandResponse> Handle(UpdateBasketItemQuantityCommandRequest request, CancellationToken cancellationToken)
        {
           await _basketService.UpdateQuantityAsync(new()
            {
                Quantity = request.Quantity,
                BasketItemId = request.BasketItemId
            });
           return new();
        }
    }

    public class UpdateBasketItemQuantityCommandResponse
    {
    }
}
