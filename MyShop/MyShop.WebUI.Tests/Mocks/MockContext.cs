using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T: BaseEntity
    {
        readonly List<T> _items;
        string className;

        public MockContext()
        {
            _items = new List<T>();
        }

        public void Commit()
        {
            return;
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
                throw new Exception(className + "Not Found");
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
                throw new Exception(className + "Not Found");
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
                throw new Exception(className + "Not Found");
            }
        }
    }
}
