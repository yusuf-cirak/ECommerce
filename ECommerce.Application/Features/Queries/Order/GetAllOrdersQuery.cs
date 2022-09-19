using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Queries.Order
{
    public sealed class GetAllOrdersQueryRequest : IRequest<GetAllOrdersQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }

    public sealed class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetAllOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var listOrder = await _orderService.GetAllOrdersAsync(request.Page,request.Size);


            return new() { Orders = listOrder.Orders,TotalOrderCount = listOrder.TotalOrderCount};
        }
    }

    public sealed class GetAllOrdersQueryResponse
    {
        public int TotalOrderCount { get; set; }
        public object Orders { get; set; }
    }
}
