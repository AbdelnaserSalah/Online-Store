using Store.Domain.Entities.Products;
using Store.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifcations.Products
{
    public class ProductsWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductsWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }

        public ProductsWithBrandAndTypeSpecifications(ProductQueryParameters parameters) : base
            (p=> (!parameters.brandid.HasValue || p.BrandId==parameters.brandid)
                 &&
                 (!parameters.typeid.HasValue || p.TypeId == parameters.typeid)
                 &&
                 (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search))
            ) // filtration to get products by brandid or typeid or both
        {
            ApplyPaging(parameters.pagenum, parameters.pagesize);
            ApplyIncludes();
            ApplySorting(parameters.sort);
        }


        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }

        

    }
}
