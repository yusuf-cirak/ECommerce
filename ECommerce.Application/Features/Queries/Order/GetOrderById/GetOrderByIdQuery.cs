using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryRequest:IRequest<GetOrderByIdQueryResponse>
    {
        public string Id { get; set; }
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            GetOrder getOrder = await _orderService.GetOrderById(request.Id);

            return new()
            {
                Id = getOrder.Id,
                Address = getOrder.Address,
                OrderCode = getOrder.OrderCode,
                BasketItems = getOrder.BasketItems,
                Description = getOrder.Description,
                CreatedDate = getOrder.CreatedDate
            };
        }
    }

    public class GetOrderByIdQueryResponse:GetOrder
    {

    }
}
