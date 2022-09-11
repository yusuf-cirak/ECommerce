using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string Description { get; set; }
    public string Address { get; set; } // Value object olarak tutulabilir.

    public ICollection<Product> Products { get; set; } // Bir siparişte birden fazla ürün olabilir. Çoka çok ilişki (n to n)

    public Customer Customer { get; set; }
    public Basket Basket { get; set; }

}