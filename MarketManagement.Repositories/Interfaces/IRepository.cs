using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketManagement.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T> Get(string id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(string id);
    }
}
