using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.ProductModule
{
    public class ProductSubType : BaseEntity<int>
    {
        public string Name { get; set; } = default!;

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = default!;
    }
}
