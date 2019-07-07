using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        void Add(T entity);
        void Delete (T entity);
        Task<bool> SaveAll();
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
    }
}