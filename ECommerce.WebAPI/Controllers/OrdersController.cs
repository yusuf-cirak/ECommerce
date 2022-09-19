﻿using ECommerce.Application.Features.Commands.Order.CreateOrder;
using ECommerce.Application.Features.Queries.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrdersController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
        {
            CreateOrderCommandResponse response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery]GetAllOrdersQueryRequest request)
        {
            GetAllOrdersQueryResponse response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
