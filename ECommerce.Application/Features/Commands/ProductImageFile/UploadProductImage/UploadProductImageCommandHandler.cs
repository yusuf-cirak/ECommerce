using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage;

public class
    UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest,
        UploadProductImageCommandResponse>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

    private readonly IStorageService _storageService;

    public UploadProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository, IStorageService storageService)
    {
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
        _storageService = storageService;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        var datas = await _storageService.UploadAsync("productimages", request.Files);
        // _webHostEnvironment.WebRootPath=wwwroot

        Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

        await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new Domain.Entities.ProductImageFile()
        {
            Path = d.path,
            FileName = d.fileName,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() { product }
        }).ToList());

        await _productImageFileWriteRepository.SaveAsync();

        return new();
    }
}