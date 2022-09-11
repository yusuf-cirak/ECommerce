using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Basket;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories.Basket
{
    public class BasketWriteRepository:WriteRepository<Domain.Entities.Basket>,IBasketWriteRepository
    {
        public BasketWriteRepository(ETradeDbContext context) : base(context)
        {
        }
    }
}
