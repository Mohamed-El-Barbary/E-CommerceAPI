using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.ProductModule
{
    public class ProductImage: BaseEntity<int>
    {
        public string PictureUrl { get; set; } = default!;
    }
}
