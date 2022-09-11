using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Repositories.BasketItem;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories.BasketItem
{
    public class BasketItemReadRepository:ReadRepository<Domain.Entities.BasketItem>,IBasketItemReadRepository
    {
        public BasketItemReadRepository(ETradeDbContext context) : base(context)
        {
        }
    }
}
