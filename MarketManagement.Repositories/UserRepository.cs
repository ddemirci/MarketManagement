using MarketManagement.Context;
using MarketManagement.Entities;
using MarketManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IServiceScope _scope;
        private readonly ProductDbContext _databaseContext;

        public UserRepository(IServiceProvider services)
        {
            _scope = services.CreateScope();
            _databaseContext = _scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        }

        public async Task<bool> Create(User entity)
        {
            _databaseContext.Users.Add(entity);

            var numberOfItemsCreated = await _databaseContext.SaveChangesAsync();
            return numberOfItemsCreated == 1;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        //TODO: FIX HERE
        public async Task<User> Get(string phoneNumber)
        {
            return await _databaseContext.Users.FindAsync(phoneNumber);
        }

        public async Task<IList<User>> GetAll()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        public async Task<bool> Update(User user)
        {
            var existingUser = Get(user.PhoneNumber).Result;

            if (existingUser != null)
            {
                existingUser.Password = user.Password;

                _databaseContext.Entry(user).State = EntityState.Detached;
                var numberOfItemsUpdated = await _databaseContext.SaveChangesAsync();
                return numberOfItemsUpdated == 1;
            }
            return false;
        }

    }
}
