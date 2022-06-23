using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Repositories;
using ECommerce.Application.RequestParameters;
using ECommerce.Application.ViewModels.Products;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;


        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;

        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;

        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        private readonly IStorageService _storageService;
        public TestsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IInvoiceFileReadRepository invoiceFileReadRepository, IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
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
        }


        [HttpGet, Route("getallproducts")]
        public IActionResult GetAllProducts([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedTime,
                p.UpdatedTime
            });
            return Ok(new
            {
                totalCount,
                products
            });
        }


        [HttpPost, Route("addproduct")]

        public async Task<IActionResult> AddProduct(VM_Create_Product product)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
            });
            await _productWriteRepository.SaveAsync();
            return Ok(product);
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
        public async Task<IActionResult> Upload(string id)
        {
            var datas=await _storageService.UploadAsync("productimages", Request.Form.Files);
            // _webHostEnvironment.WebRootPath=wwwroot

            Product product = await _productReadRepository.GetByIdAsync(id);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                Path = d.path,
                FileName = d.fileName,
                Storage = _storageService.StorageName,
                Products = new List<Product>(){product}
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            return Ok();
        }




    }
}
