using E_Commerce.Domain.Entities.ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications.ProductSpecifications
{
    internal class TypesWithSubTypesSpecification : BaseSpecifications<ProductType, int>
    {

        public TypesWithSubTypesSpecification() : base()
        {
            AddInclude(x => x.ProductSubTypes);
        }

        public TypesWithSubTypesSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(x => x.ProductSubTypes);
        }
       
    }
}
