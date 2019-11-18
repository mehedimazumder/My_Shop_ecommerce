using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.DataAccess.InMemory;

namespace MyShop.Core.Models
{
    public class ProductCategory:BaseEntity
    {
        public string Category { get; set; }

    }
}
