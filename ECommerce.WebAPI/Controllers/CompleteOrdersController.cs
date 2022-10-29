using ECommerce.Application.Features.Commands.CompletedOrder.Create;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompleteOrdersController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> CompleteOrder([FromRoute]CreateCompletedOrderCommandRequest request)
    {
        bool response = await Mediator.Send(request);
        return Ok(response.ToString());
    }
}