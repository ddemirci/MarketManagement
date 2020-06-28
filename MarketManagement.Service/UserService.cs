using MarketManagement.Entities;
using MarketManagement.Repositories.Interfaces;
using MarketManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Service
{
    public class UserService : IService<User>
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Create(User entity)
        {
            var success = await _repository.Create(entity);
            return success;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(string id)
        {
            return await _repository.Get(id);
        }

        public async Task<IList<User>> GetAll()
        {
            return await _repository.GetAll();
        }

        public Task<bool> Update(User blogPost)
        {
            throw new NotImplementedException();
        }

        //public async Task<User> GetUserFromPhoneNumber(string phoneNumber)
        //{
        //    return await _repository.GetUserFromPhoneNumber(phoneNumber)
        //}
    }
}
