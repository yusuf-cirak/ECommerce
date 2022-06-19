using ECommerce.Application.Repositories;
using ECommerce.Application.RequestParameters;
using ECommerce.Application.Services;
using ECommerce.Application.ViewModels.Products;
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
        private readonly IFileService _fileService;
        public TestsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
        public async Task<IActionResult> Upload()
        {
            // _webHostEnvironment.WebRootPath=wwwroot
           var data= await _fileService.UploadAsync("resource/product-images", Request.Form.Files);
            return Ok(data);
        }




    }
}
