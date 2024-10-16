using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Repository<T>
        where T : class
    {

        protected DbContext Context { get; }

        public Repository(DbContext context)
        {
            Context = context;
        }

        /// <summary>
        ///  Add a new entity to the Table.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            Context.Add(entity);
        }

        /// <summary>
        ///  Remove an entity from the Table.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>true if removed and false otherwise.</returns>
        public T Remove(T entity)
        {
            return Context.Remove(entity).Entity;
        }

        /// <summary>
        ///  Find a set of entities that match a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        /// <summary>
        ///  Find the first entity that match a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        ///  Is this repository empty?
        /// </summary>
        /// <returns>true is it is empty, false otherwise.</returns>
        public bool IsEmpty()
        {
            return Context.Set<T>().Count() == 0;
        }

        /// <summary>
        ///  Count the entities in the Table.
        /// </summary>
        /// <returns>the number of entities.</returns>
        public int Count()
        {
            return Context.Set<T>().Count();
        }
    }
}
