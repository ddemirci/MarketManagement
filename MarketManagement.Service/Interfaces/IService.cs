using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Service.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<T> Get(string id);
        Task<IList<T>> GetAll();
        Task<bool> Create(T entity);
        Task<bool> Update(T blogPost);
        Task<bool> Delete(string id);
    }
}
