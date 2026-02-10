using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities.Identity;
using Store.Domain.Entities.Products;
using Store.Presistence.Data.Contexts;
using Store.Presistence.Identity.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Presistence
{
    // primary constructor syntax
    public class DbInitializer(
        StoreDbContext _context,
        IdentityStoreDbContext identityDbContext,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IDbInitializer
    {
        //primary constructor instead of this is code below

        //private readonly StoreDbContext _context;
        //public DbInitializer(StoreDbContext context )
        //{
        //    _context = context;
        //}
        public async Task InitializeAsync()
        {

            //create the database if it does not exist and apply any migrations not applyed yet
            if(_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
               await _context.Database.MigrateAsync();
            }



            // Data Seeding mean reading data from files and inserting them into the database tables

            // check if there is any data in ProductBrands table
            // product Brands
            if (! _context.ProductBrands.Any())
            {
                // read data from json file
               
                var brandsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Presistence\Data\DataSeeding\brands.json");
                //convert json data to list of product brand
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);
                // insert list of product brand to database
                if (brands is not null && brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                }
            }

            //  product types
            if (!_context.ProductTypes.Any())
            {
                // read data from json file
                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Presistence\Data\DataSeeding\types.json");
                //convert json data to list of product brand
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);
                // insert list of product brand to database
                if (types is not null && types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                }
            }

            // products
            if (!_context.Products.Any())
            {
                // read data from json file
                var productsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Store.Presistence\Data\DataSeeding\products.json");
                //convert json data to list of product brand
                var products = JsonSerializer.Deserialize<List<Product>>(productsdata);
                // insert list of product brand to database
                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                }
            }


          await  _context.SaveChangesAsync();

        }

        public async Task InitializeIdentityAsync()
        {
            if (identityDbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await identityDbContext.Database.MigrateAsync();
            }

            if (!identityDbContext.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }


            if (!identityDbContext.Users.Any())
            {
                var supeerAdmin = new AppUser()
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01254444458"
                };

                var Admin = new AppUser()
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01254444458"
                };

                await userManager.CreateAsync(supeerAdmin, "P@ssw0rd");
                await userManager.CreateAsync(Admin, "P@ssw0rd");


                await userManager.AddToRoleAsync(supeerAdmin, "SuperAdmin");
                await userManager.AddToRoleAsync(Admin, "Admin");
            }

        }
    }
}
