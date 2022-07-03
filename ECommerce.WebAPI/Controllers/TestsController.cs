using System.Net;
using Azure.Core;
using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Features.Commands.Product.CreateProduct;
using ECommerce.Application.Features.Commands.Product.RemoveProduct;
using ECommerce.Application.Features.Commands.Product.UpdateProduct;
using ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerce.Application.Features.Queries.Product.GetAllProduct;
using ECommerce.Application.Features.Queries.Product.GetProductById;
using ECommerce.Application.Features.Queries.ProductImageFile.GetProductImage;
using ECommerce.Application.Repositories;
using ECommerce.Application.RequestParameters;
using ECommerce.Application.ViewModels.Products;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : BaseController
    {

        [HttpGet, Route("getallproducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] Pagination pagination)
        {
            GetAllProductQueryResponse response = await Mediator.Send(new GetAllProductQueryRequest(pagination));
            return Ok(response);
        }


        [HttpGet, Route("getproductbyid/{Id}")] // prop name'i ile query param matchleşmeli ki bind işlemi yapılsın
        public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            GetProductByIdQueryResponse response=await Mediator.Send(getProductByIdQueryRequest);
            return Ok(response);
        }




        [HttpPost, Route("createproduct")]

        public async Task<IActionResult> CreateProduct(CreateProductCommandRequest request)
        {
            await Mediator.Send(request);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut, Route("updateproduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            await Mediator.Send(updateProductCommandRequest);
            return Ok();
        }


        [HttpDelete, Route("removeproduct/{Id}")]

        public async Task<IActionResult> RemoveProduct([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            await Mediator.Send(removeProductCommandRequest);
            return Ok();
        }


        [HttpPost, Route("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest) // id query string olarak gelecek
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            await Mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }

        [HttpGet, Route("[action]/{Id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            List<GetProductImagesQueryResponse> response = await Mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest)
        {
            await Mediator.Send(removeProductImageCommandRequest);
            return Ok();

        }


    }
}
