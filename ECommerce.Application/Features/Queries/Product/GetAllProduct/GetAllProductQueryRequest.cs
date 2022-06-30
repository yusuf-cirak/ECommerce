using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.RequestParameters;
using MediatR;

namespace ECommerce.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryRequest:IRequest<GetAllProductQueryResponse>
    {
        public Pagination Pagination { get; set; }
        public GetAllProductQueryRequest(Pagination pagination)
        {
            Pagination = pagination;
        }
    }
}
