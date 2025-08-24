using SuperShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Interfaces
{
    public interface ICrudService<T> where T : class, IEntity
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        T Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
        int Count();
    }
}
