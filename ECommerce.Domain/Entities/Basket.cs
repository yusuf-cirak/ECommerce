using System.Security.AccessControl;
using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Domain.Entities;

public class Basket : BaseEntity
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; }
    public Order Order { get; set; }
}