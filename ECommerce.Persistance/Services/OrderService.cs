using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Repositories;

namespace ECommerce.Persistance.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            string orderCode = GenerateRandomOrderCode();
            await _orderWriteRepository.AddAsync(new()
            {
                OrderCode = orderCode,
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId!),
                Description = createOrder.Description
            });

            await _orderWriteRepository.SaveAsync();
        }

        private string GenerateRandomOrderCode()
        {
            string orderCode = (new Random().NextDouble() * 1000000).ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf('.') + 1, orderCode.Length - orderCode.IndexOf('.')+7);
            var orderCodeExists =  _orderReadRepository.GetWhere(e=>e.OrderCode==orderCode).Any();

            if (orderCodeExists)
            {
                GenerateRandomOrderCode();
            }
            return orderCode;
        }
    }
}
