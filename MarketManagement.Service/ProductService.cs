using MarketManagement.Entities;
using MarketManagement.Repositories.Interfaces;
using MarketManagement.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Service
{
    public class ProductService : IService<Product>
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepository<Product> repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Create(Product entity)
        {
            _logger.LogInformation("Create Method has been called in Service layer");
            var success = await _repository.Create(entity);
            return success;
        }

        public async Task<bool> Delete(string id)
        {
            var success = await _repository.Delete(id);
            return success;
        }

        public async Task<Product> Get(string id)
        {
            return await _repository.Get(id);
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<bool> Update(Product product)
        {
            var success = await _repository.Update(product);
            return success;
        }
    }
}
