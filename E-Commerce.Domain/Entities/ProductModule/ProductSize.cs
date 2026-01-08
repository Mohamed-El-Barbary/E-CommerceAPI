using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.ProductModule
{
    public class ProductSize : BaseEntity<int>
    {
        public string Size { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
