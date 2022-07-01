using ECommerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandHandler:IRequestHandler<RemoveProductImageCommandRequest,RemoveProductImageCommandResponse>
{
    private readonly IProductImageFileReadRepository _productImageFileReadRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;

    public RemoveProductImageCommandHandler(IProductImageFileReadRepository productImageFileReadRepository, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
    {
        _productImageFileReadRepository = productImageFileReadRepository;
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
    }

    public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProductId));

        Domain.Entities.ProductImageFile? productImageFile =
            await _productImageFileReadRepository.Table.FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ImageId));


        product.ProductImageFiles.Remove(productImageFile);
        await _productWriteRepository.SaveAsync();
        return new();
    }
}