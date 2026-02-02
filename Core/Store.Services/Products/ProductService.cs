using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions.NotFound;
using Store.Services.Abstractions.Products;
using Store.Services.Specifcations.Products;
using Store.Shared;
using Store.Shared.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Products
{
    public class ProductService(IUnitofWork unitofWork , IMapper mapper) : IProductService
    {
     
        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters)
        {

            var spec = new ProductsWithBrandAndTypeSpecifications(parameters);


            var products= await unitofWork.GetRepository<int, Product>().GetAllAsync(spec);
            var result= mapper.Map<IEnumerable<ProductResponse>>(products);
            var speccount = new ProductCountSepecifcation(parameters);
            var count= await unitofWork.GetRepository<int, Product>().CountAsync(speccount);

            return new PaginationResponse<ProductResponse>(parameters.pagenum,parameters.pagesize,count,result);
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var spec = new ProductsWithBrandAndTypeSpecifications(id);
            var product = await unitofWork.GetRepository<int, Product>().GetAsync(spec);

            if (product is null) throw new ProductNotFoundExeception(id);//404

            var result = mapper.Map<ProductResponse>(product);
            return result;
        }

        public async Task<IEnumerable<BrandTypeResponse>> GetAllTypesAsync()
        {
            var types = await unitofWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandTypeResponse>>(types);
            return result;
        }
        public async Task<IEnumerable<BrandTypeResponse>> GetAllBrandsAsync()
        {
            var brands = await unitofWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandTypeResponse>>(brands);
            return result;
        }


    }
}
