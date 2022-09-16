using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.ViewModels.Baskets;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IBasketService
    {
        Task AddItemToBasketAsync(VM_Create_BasketItem basketItem);
        Task<List<BasketItem>> GetBasketItemsAsync();
        Task RemoveBasketItemAsync(string basketItemId);
        Task UpdateQuantityAsync(VM_Update_BasketItem basketItem);

        public Basket? GetUserActiveBasket { get; }



    }
}
