using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.SQL
{
    public class SqlRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext Context;
        internal DbSet<T> DbSet;

        public SqlRepository(DataContext context)
        {
            this.Context = context;
            this.DbSet = Context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return DbSet;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(string id)
        {
            var t = Find(id);
            if (Context.Entry(t).State == EntityState.Detached)
                DbSet.Attach(t);

            DbSet.Remove(t);
        }

        public T Find(string id)
        {
            return DbSet.Find(id);
        }

        public void Insert(T t)
        {
            DbSet.Add(t);
        }

        public void Update(T t)
        {
            DbSet.Attach(t);
            Context.Entry(t).State = EntityState.Modified;
        }
    }
}
