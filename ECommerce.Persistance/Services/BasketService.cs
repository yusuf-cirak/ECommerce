using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Basket;
using ECommerce.Application.Repositories.BasketItem;
using ECommerce.Application.ViewModels.Baskets;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Services
{
    public class BasketService:IBasketService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IBasketWriteRepository _basketWriteRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;
        private readonly IBasketItemWriteRepository _basketItemWriteRepository;
        private readonly IBasketReadRepository _basketReadRepository;
        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            // get userName from token and fetch user with basket datas from db
            string? userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                AppUser? user = await _userManager.Users
                    .Include(u=>u.Baskets)
                    .FirstOrDefaultAsync(u => u.UserName == userName);



                // fetch orders from user.Baskets and return as anonymous type
                var userBasketOrder = from basket in user.Baskets
                    join order in _orderReadRepository.Table
                        on basket.Id equals order.Id into basketOrder
                    from order in basketOrder.DefaultIfEmpty()
                    select new
                    {
                        Basket = basket,
                        Order = order
                    };


                // Create a Basket targetBasket variable. If userBasketOrder.Order is null than targetBasket should be setted as userBasketOrder.Basket. Else we need to create a Basket instance and save it to user.Baskets

                Basket? targetBasket = null;
                if (userBasketOrder.Any(ubo=>ubo.Order is null))
                {
                    targetBasket = userBasketOrder.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    targetBasket = new Basket();
                    user.Baskets.Add(targetBasket);
                }

                await _basketWriteRepository.SaveAsync();

                return targetBasket;

            }

            throw new Exception("Something went wrong");
        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket!=null)
            {
                BasketItem? item =
                    await _basketItemReadRepository.Table.FirstOrDefaultAsync(b => b.BasketId == basket.Id && b.ProductId==Guid.Parse(basketItem.ProductId));
                if (item!=null)
                {
                    item.Quantity++;
                }
                else
                { // Update
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity
                    });
                }

                await _basketItemWriteRepository.SaveAsync();
            }

        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket= await ContextUser();
            basket=await _basketReadRepository.Table.Include(b => b.BasketItems).ThenInclude(bi=>bi.Product)
                .FirstOrDefaultAsync(b => b.Id == basket!.Id);

            return basket!.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            _basketItemWriteRepository.Remove(basketItem);
            await _basketItemWriteRepository.SaveAsync();
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? item = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if (item!=null)
            {
                item.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }
    }
}
