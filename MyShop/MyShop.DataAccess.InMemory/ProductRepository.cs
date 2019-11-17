using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product> ?? new List<Product>();
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update(Product product)
        {
            Product prodToUpdate = products.Find(p => p.Id == product.Id);

            if (!prodToUpdate.Equals(null))
            {
                prodToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public Product Find(string id)
        {
            Product prodToFind = products.Find(p => p.Id == id);

            if (!prodToFind.Equals(null))
            {
                return prodToFind;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string id)
        {
            Product product = products.Find(p => p.Id.Equals(id));

            if (product != null)
            {
                products.Remove(product);
            }
            else
            {
                throw new Exception("Product no found");
            }
        }


    }
}
