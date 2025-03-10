using Microsoft.EntityFrameworkCore;
using New_Eticket.DataAccess;
using System.Linq.Expressions;
using System.Linq;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket.Repositories
{
    
        public class Repository<T>: IRepository<T> where T : class
        {
        ApplicationDbContext dbContext;//= new ApplicationDbContext();
        public DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        
        }
         
            //public Repository()
            //{
            //    dbSet = dbContext.Set<T>();
            //}
            // CRUD
            public void Create(T entity)
            {
                dbSet.Add(entity);
            }
            public void Create(List<T> entities)
            {
                dbSet.AddRange(entities);
            }
            public void Edit(T entity)
            {
                dbSet.Update(entity);
            }

            public void Delete(T entity)
            {
                dbSet.Remove(entity);
            }

            public void Delete(List<T> entities)
            {
                dbSet.RemoveRange(entities);
            }
            public void Commit()
            {
                dbContext.SaveChanges();
            }
            public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
            {
                IQueryable<T> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (!tracked)
                {
                    query = query.AsNoTracking();
                }

                return query.ToList();
            }
            public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
            {
                return Get(filter, includes, tracked).FirstOrDefault();
            }
        }
    }

    