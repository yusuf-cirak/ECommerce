using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.RequestParameters
{
    public record Pagination // Nesne değil, nesnenin verileri ön planda bu yüzden record olarak tanımlandı.
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
