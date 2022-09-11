using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Repositories.Basket;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories.Basket
{
    public class BasketReadRepository:ReadRepository<Domain.Entities.Basket>,IBasketReadRepository
    {
        public BasketReadRepository(ETradeDbContext context) : base(context)
        {
        }
    }
}
