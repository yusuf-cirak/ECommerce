﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public ICollection<Order> Orders { get; set; } // Bir ürünün birden fazla siparişi olabilir. Çoka çok ilişki.
        
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
