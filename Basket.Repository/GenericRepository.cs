using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Basket.Repository.Contracts;

namespace Basket.Repository
{
	// Veritabanına veri ekleme, güncelleme ve okuma gibi CRUD (Create Read Update Delete) işlemleri için oluşturmuştur
	// Kodların tekrar tekrar yazılmaması için Repository Design Pattern kullanulara CRUD işlemleri gerçekleştirilebiliyor
	// Burada kullanılması öngörülen birtakım CRUD metotları oluşturulmuştur

	public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

		public GenericRepository(IUnitOfWork uow)
        {
            dbContext = uow.DbContext;
            dbSet = dbContext.Set<TEntity>();
		}

		public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}


			if (orderBy != null)
			{
				return orderBy(query);
			}
			return query;
		}

		public virtual IQueryable<TEntity> GetQuery<TProperty>(IEnumerable<Expression<Func<TEntity, TProperty>>> includeProperties, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
				{
					query.Include(includeProperty);
				}
			}

			if (orderBy != null)
			{
				return orderBy(query);
			}
			return query;
		}

		public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null)
			{
				return query.Count(filter);
			}
			return query.Count();
		}

		public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null)
			{
				return query.CountAsync(filter);
			}
			return query.CountAsync();
		}

		public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null)
			{
				return query.Any(filter);
			}
			return query.Any();
		}

		public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = dbSet;
			if (filter != null)
			{
				return query.AnyAsync(filter);
			}
			return query.AnyAsync();
		}

		public void Update(TEntity entityToUpdate)
		{
			dbSet.Attach(entityToUpdate);
			dbContext.Entry(entityToUpdate).State = EntityState.Modified;
		}

		public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			return query.SingleOrDefault(filter);
		}

		public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			return await query.SingleOrDefaultAsync(filter);
		}

		public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			return query.FirstOrDefault(filter);
		}

		public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			return await query.FirstOrDefaultAsync(filter);
		}

		public virtual void Insert(TEntity entity)
		{
			dbSet.Add(entity);
		}

		public virtual void InsertRange(List<TEntity> entity)
		{
			dbSet.AddRange(entity);
		}

		public virtual void Delete(Expression<Func<TEntity, bool>> filter) 
		{
			TEntity entityToDelete = dbSet.SingleOrDefault(filter);
			if (entityToDelete != null)
			{
				Delete(entityToDelete);
			}
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
			{
				dbSet.Attach(entityToDelete);
			}
			dbSet.Remove(entityToDelete);
		}

		public int Save()
		{
			return dbContext.SaveChanges();
		}

		public Task<int> SaveAsync()
		{
			return dbContext.SaveChangesAsync();
		}

		public void Dispose() { }

		public IDbContextTransaction BeginTransaction()
		{
			return dbContext.Database.BeginTransaction();
		}

		public virtual TEntity LastOrDefault(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			return query.LastOrDefault(filter);
		}

		public virtual async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
		{
			IQueryable<TEntity> query = dbSet;
			return await query.LastOrDefaultAsync(filter);
		}
	}
}
