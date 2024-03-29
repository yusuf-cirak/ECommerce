﻿using ECommerce.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Queries.Product.GetAllProduct;

public class GetAllProductQueryHandler:IRequestHandler<GetAllProductQueryRequest,GetAllProductQueryResponse>
{
    private readonly IProductReadRepository _productReadRepository;

    public GetAllProductQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        var totalProductCount = _productReadRepository.GetAll(false).Count();
        var products = _productReadRepository.GetAll(false).Skip(request.Pagination.Page * request.Pagination.Size).Take(request.Pagination.Size)
            .Include(p=>p.ProductImageFiles)
            .Select(p => new
        {
            p.Id,
            p.Name,
            p.Price,
            p.Stock,
            p.CreatedTime,
            p.UpdatedTime,
            p.ProductImageFiles
        }).ToList();


        return new() { Products = products, TotalProductCount = totalProductCount };
    }
}