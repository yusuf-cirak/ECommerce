using System.Net;
using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Features.Commands.Product.CreateProduct;
using ECommerce.Application.Features.Queries.Product.GetAllProduct;
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
    public class TestsController : ControllerBase
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        readonly IWebHostEnvironment _webHostEnvironment;


        readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;

        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;

        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        readonly IStorageService _storageService;

        readonly IConfiguration _configuration;


        private readonly IMediator _mediator;



        public TestsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IInvoiceFileReadRepository invoiceFileReadRepository, IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }


        [HttpGet, Route("getallproducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] Pagination pagination)
        {
            GetAllProductQueryResponse response = await _mediator.Send(new GetAllProductQueryRequest(pagination));
            return Ok(response);
        }


        [HttpPost, Route("createproduct")]

        public async Task<IActionResult> CreateProduct(CreateProductCommandRequest request)
        {
            await _mediator.Send(request);
            return StatusCode((int)HttpStatusCode.Created);
        }


        [HttpPut, Route("updateproduct")]

        public async Task<IActionResult> UpdateProduct(VM_Update_Product product)
        {
            _productWriteRepository.Update(new() { Id = product.Id, Name = product.Name, Price = product.Price, Stock = product.Stock });
            await _productWriteRepository.SaveAsync();
            return Ok(product);
        }


        [HttpDelete, Route("deleteproduct/{id}")]

        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpPost, Route("[action]")]
        public async Task<IActionResult> Upload(string id) // id query string olarak gelecek
        {
            var datas = await _storageService.UploadAsync("productimages", Request.Form.Files);
            // _webHostEnvironment.WebRootPath=wwwroot

            Product product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                Path = d.path,
                FileName = d.fileName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost, Route("[action]/{id}")]
        public async Task<IActionResult> GetProductsImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(p => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                p.FileName,
                p.Id
            }));
        }

        [HttpDelete("[action]/{productId}/{imageId}")]
        public async Task<IActionResult> DeleteProductImage(string productId, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));

            ProductImageFile? productImageFile =
                await _productImageFileReadRepository.Table.FirstOrDefaultAsync(p => p.Id == Guid.Parse(imageId));


            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();

        }


    }
}
