using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.CompletedOrder;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
        private readonly ICompletedOrderReadRepository _completedOrderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository,
            ICompletedOrderWriteRepository completedOrderWriteRepository,
            ICompletedOrderReadRepository completedOrderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
            _completedOrderReadRepository = completedOrderReadRepository;
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

        public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
        {
            var queryable = _orderReadRepository.Table.AsNoTracking()
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .ThenInclude(u => u.Baskets)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product);

            var data = queryable.Skip(page * size).Take(size);

            var ordersWithCompletedState =
                from order in data
                join completedOrder in _completedOrderReadRepository.Table
                    on order.Id equals completedOrder.OrderId into completedOrderState
                from _orderCompleted in completedOrderState.DefaultIfEmpty()
                select new
                {
                    Id = order.Id,
                    CreatedDate = order.CreatedTime,
                    UpdatedDate = order.UpdatedTime,
                    OrderCode = order.OrderCode,
                    Basket = order.Basket,
                    Completed = _orderCompleted != null ? true : false
                };

            return new()
            {
                TotalOrderCount = await data.CountAsync(),
                Orders = await ordersWithCompletedState.Select(e => new
                {
                    Id = e.Id,
                    CreatedDate = e.CreatedDate,
                    UserName = e.Basket.User.FullName,
                    TotalPrice = e.Basket.BasketItems.Sum(e => e.Product.Price * e.Quantity),
                    OrderCode = e.OrderCode,
                    e.Completed
                }).ToListAsync()
            };
        }

        public async Task<GetOrder> GetOrderById(string id)
        {
            var data = _orderReadRepository.Table.AsNoTracking()
                .Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product);

            var orderWithCompletedState = await
                (from order in data
                    join orderCompletedState in _completedOrderReadRepository.Table on order.Id equals
                        orderCompletedState.OrderId
                        into _orderWithCompletedState
                    from _orderWithState in _orderWithCompletedState.DefaultIfEmpty()
                    select new
                    {
                        Id = order.Id,
                        Address = order.Address,
                        Description = order.Description,
                        OrderCode = order.OrderCode,
                        Basket = order.Basket,
                        CreatedTime = order.CreatedTime,
                        Completed = _orderWithState != null ? true : false
                    }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));


            return new GetOrder
            {
                Id = orderWithCompletedState.Id.ToString(),
                Address = orderWithCompletedState.Address,
                Description = orderWithCompletedState.Description,
                OrderCode = orderWithCompletedState.OrderCode,
                BasketItems = orderWithCompletedState.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity
                }),
                CreatedDate = orderWithCompletedState.CreatedTime,
                Completed = orderWithCompletedState.Completed
            };
        }

        public async Task CompleteOrderAsync(string id)
        {
            Order order = await _orderReadRepository.GetByIdAsync(id);

            if (order != null)
            {
                await _completedOrderWriteRepository.AddAsync(new() { OrderId = order.Id });

                await _completedOrderWriteRepository.SaveAsync();
            }
            else
            {
                throw new OrderNotFoundException();
            }
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