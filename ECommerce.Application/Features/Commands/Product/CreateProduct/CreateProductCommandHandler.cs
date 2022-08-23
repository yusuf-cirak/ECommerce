using ECommerce.Application.Abstractions.Hubs;
using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Commands.Product.CreateProduct;

public class CreateProductCommandHandler:IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductHubService _productHubService;

    public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService)
    {
        _productWriteRepository = productWriteRepository;
        _productHubService = productHubService;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _productWriteRepository.AddAsync(new()
        {
            Name = request.Name,
            Stock = request.Stock,
            Price = request.Price,
        });

        await _productWriteRepository.SaveAsync();
        await _productHubService.ProductAddedMessageAsync($"Veri tabanına {request.Name} adında yeni bir ürün kaydedilmiştir.");

        return new();
    }
}