using ECommerce.Application.Features.Commands.Basket.AddItemToBasket;
using ECommerce.Application.Features.Commands.Basket.RemoveBasketItem;
using ECommerce.Application.Features.Commands.Basket.UpdateBasketItemQuantity;
using ECommerce.Application.Features.Queries.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class BasketsController : BaseController
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetBasketItems([FromQuery] GetBasketItemsQueryRequest request)
        {
            List<GetBasketItemsQueryResponse> response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddItemToBasket([FromBody] AddItemToBasketCommandRequest request)
        {
            AddItemToBasketCommandResponse response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateBasketItemQuantityCommandRequest request)
        {
            UpdateBasketItemQuantityCommandResponse response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("[action]/{BasketItemId}")]
        public async Task<IActionResult> RemoveBasketItem( [FromRoute] RemoveBasketItemCommandRequest request)
        {
            RemoveBasketItemCommandResponse response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
