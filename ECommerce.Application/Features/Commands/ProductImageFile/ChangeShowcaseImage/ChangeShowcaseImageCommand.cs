using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandRequest:IRequest<ChangeShowcaseImageCommandResponse>
    {
        public string ProductId { get; set; }
        public string ImageId { get; set; }
    }
    public class ChangeShowcaseImageCommandHandler:IRequestHandler<ChangeShowcaseImageCommandRequest,ChangeShowcaseImageCommandResponse>
    {
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {

            var query = _productImageFileWriteRepository.Table.Include(p => p.Products)
                .SelectMany(p => p.Products, (productImageFile, product) => new
                {
                    productImageFile,
                    product
                });

            var data = await query.FirstOrDefaultAsync(p =>
                p.product.Id == Guid.Parse(request.ProductId) && p.productImageFile.Showcase);

            if (data != null)
                data.productImageFile.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.productImageFile.Id == Guid.Parse(request.ImageId));
            if (image != null)
                image.productImageFile.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
    public class ChangeShowcaseImageCommandResponse
    {
    }
}
