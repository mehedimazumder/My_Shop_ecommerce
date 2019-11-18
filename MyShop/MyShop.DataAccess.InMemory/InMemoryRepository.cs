using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        readonly List<T> _items;
        private readonly string _className;

        public InMemoryRepository()
        { 
            _className = typeof(T).Name;
            _items = _cache[_className] as List<T> ?? new List<T>();
        }

        public void Commit()
        {
            _cache[_className] = _items;
        }

        public void Insert(T t)
        {
            _items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = _items.Find(i => i.Id == t.Id);
            if (tToUpdate != null)
                tToUpdate = t;

            else
            {
                throw new Exception(_className + "Not Found");
            }
        }
        public T Find(string id)
        {
            T prodToFind = _items.Find(p => p.Id == id);

            if (!prodToFind.Equals(null))
            {
                return prodToFind;
            }
            else
            {
                throw new Exception(_className + "Not Found");
            }
        }

        public IQueryable<T> Collection()
        {
            return _items.AsQueryable();
        }
        public void Delete(string id)
        {
            T tToDelete = _items.Find(i => i.Id == id);

            if (tToDelete != null)
                _items.Remove(tToDelete);
            else
            {
                throw new Exception(_className + "Not Found");
            }
        }
    }
}
