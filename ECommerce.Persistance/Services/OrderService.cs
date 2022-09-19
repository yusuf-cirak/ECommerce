using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Services
{
    public class OrderService : IOrderService
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
            await _orderWriteRepository.AddAsync(new()
            {
                OrderCode = GenerateOrderCode(),
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId!),
                Description = createOrder.Description
            });

            await _orderWriteRepository.SaveAsync();
        }

        public async Task<ListOrder> GetAllOrdersAsync(int page,int size)
        {
             var data= _orderReadRepository.Table.AsNoTracking()
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .ThenInclude(u => u.Baskets)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product);

             return new()
             {
                 TotalOrderCount = await data.CountAsync(),
                 Orders = await data.Select(e => new
                 {
                     CreatedDate=e.CreatedTime,
                     UserName=e.Basket.User.FullName,
                     TotalPrice=e.Basket.BasketItems.Sum(e=>e.Product.Price*e.Quantity),
                     OrderCode=e.OrderCode
                 }).ToListAsync()
             };

        }

        private string GenerateOrderCode()
        {
            string orderCode = new Random().NextDouble().ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf(',') + 1, orderCode.IndexOf(',') + 5);
            var orderCodeExists = _orderReadRepository.GetWhere(e => e.OrderCode == orderCode).Any();

            if (orderCodeExists)
            {
                GenerateOrderCode();
            }
            return orderCode;
        }
    }
}
