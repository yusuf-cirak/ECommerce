using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Queries.Basket
{
    public class GetBasketItemsQueryRequest:IRequest<List<GetBasketItemsQueryResponse>>
    {
    }

    public class GetBasketItemsQueryHandler:IRequestHandler<GetBasketItemsQueryRequest,List<GetBasketItemsQueryResponse>>
    {
        private readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems=await _basketService.GetBasketItemsAsync();

            return basketItems.Select(b => new GetBasketItemsQueryResponse()
            {
                BasketItemId = b.Id.ToString(),
                Quantity = b.Quantity,
                Price = b.Product.Price,
                ProductName = b.Product.Name
            }).ToList();

        }
    }

    public class GetBasketItemsQueryResponse
    {
        public string BasketItemId { get; set; }

        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }


    }
}
