using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public string Description { get; set; }
    public string Address { get; set; } // Value object olarak tutulabilir.


    public Basket Basket { get; set; }

}