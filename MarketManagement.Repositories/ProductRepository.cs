using MarketManagement.Context;
using MarketManagement.Entities;
using MarketManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace MarketManagement.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly IServiceScope _scope;
        private readonly ProductDbContext _databaseContext;

        public ProductRepository(IServiceProvider services)
        {
            _scope = services.CreateScope();
            _databaseContext = _scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        }

        public async Task<bool> Create(Product entity)
        {
            _databaseContext.Products.Add(entity);

            var numberOfItemsCreated = await _databaseContext.SaveChangesAsync();
            return numberOfItemsCreated == 1;
        }

        public async Task<bool> Delete(string id)
        {
            var existingProduct = Get(id).Result;
            if (existingProduct != null)
            {
                _databaseContext.Products.Remove(existingProduct);
                var numberOfItemsDeleted = await _databaseContext.SaveChangesAsync();
                return numberOfItemsDeleted == 1;
            }
            return false;
        }

        public async Task<Product> Get(string id)
        {
            //var result = _databaseContext.Products
            //                   .Where(x => x.Id == id)
            //                   .FirstOrDefault();

            return await _databaseContext.Products.FindAsync(id);
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _databaseContext.Products.ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var existingProduct =  Get(product.Id).Result;

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Currency = product.Currency;

                _databaseContext.Entry(product).State = EntityState.Detached;
                var numberOfItemsUpdated = await _databaseContext.SaveChangesAsync();
                return numberOfItemsUpdated == 1;
            }
            return false;
        }
    }
}
