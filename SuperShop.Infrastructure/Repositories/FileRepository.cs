using SuperShop.Application.Interfaces;
using SuperShop.Domain.Interfaces;
using SuperShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Infrastructure.Repositories
{
    public class FileRepository<T> : ICrudService<T> where T : class, IEntity, new()
    {
        private readonly object _lock = new();

        public IEnumerable<T> GetAll()
        {
            lock (_lock) { return JsonStore.LoadList<T>(); }
        }

        public T? GetById(int id)
        {
            lock (_lock) { return JsonStore.LoadList<T>().FirstOrDefault(e => e.Id == id); }
        }

        public T Add(T entity)
        {
            lock (_lock)
            {
                var list = JsonStore.LoadList<T>();
                var nextId = (list.Count == 0) ? 1 : list.Max(e => e.Id) + 1;
                entity.Id = nextId;
                list.Add(entity);
                JsonStore.SaveList(list);
                return entity;
            }
        }

        public bool Update(T entity)
        {
            lock (_lock)
            {
                var list = JsonStore.LoadList<T>();
                var idx = list.FindIndex(e => e.Id == entity.Id);
                if (idx == -1) return false;
                list[idx] = entity;
                JsonStore.SaveList(list);
                return true;
            }
        }

        public bool Delete(int id)
        {
            lock (_lock)
            {
                var list = JsonStore.LoadList<T>();
                var removed = list.RemoveAll(e => e.Id == id) > 0;
                if (removed) JsonStore.SaveList(list);
                return removed;
            }
        }

        public int Count()
        {
            lock (_lock) { return JsonStore.LoadList<T>().Count; }
        }
    }
}
