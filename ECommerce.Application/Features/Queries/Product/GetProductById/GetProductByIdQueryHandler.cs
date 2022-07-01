using ECommerce.Application.Repositories;
using MediatR;

namespace ECommerce.Application.Features.Queries.Product.GetProductById;

public class GetProductByIdQueryHandler:IRequestHandler<GetProductByIdQueryRequest,GetProductByIdQueryResponse>
{
    private readonly IProductReadRepository _productReadRepository;

    public GetProductByIdQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByIdAsync(request.Id);

        return new() { Name = product.Name, Price = product.Price, Stock = product.Stock };
    }
}