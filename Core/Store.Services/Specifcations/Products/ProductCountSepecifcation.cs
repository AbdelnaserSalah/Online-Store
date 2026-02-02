using Store.Domain.Entities.Products;
using Store.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifcations.Products
{
    public class ProductCountSepecifcation : BaseSpecifications<int ,Product>
    {
        public ProductCountSepecifcation(ProductQueryParameters parameters) : base
            (p => (!parameters.brandid.HasValue || p.BrandId == parameters.brandid)
                 &&
                 (!parameters.typeid.HasValue || p.TypeId == parameters.typeid)
                 &&
                 (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search))
            )
        {
            
        }
    }
}
