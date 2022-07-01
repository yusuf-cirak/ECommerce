using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdQueryRequest:IRequest<GetProductByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
