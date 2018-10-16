using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Hidistro.Core
{
	public static class DbSetExtend
	{
		public static IQueryable<TEntity> FindAll<TEntity>(this DbSet<TEntity> dbSet) where TEntity : BaseModel
		{
			return from item in (IQueryable<TEntity>)dbSet
			where true
			select item;
		}

		public static IQueryable<TEntity> FindAll<TEntity, TKey>(this DbSet<TEntity> dbSet, int pageNumber, int pageSize, out int total, Expression<Func<TEntity, TKey>> orderBy, bool isAsc = true) where TEntity : BaseModel
		{
			total = dbSet.Count();
			IQueryable<TEntity> source = from item in (IQueryable<TEntity>)dbSet
			where true
			select item;
			source = ((!isAsc) ? source.OrderByDescending(orderBy) : source.OrderBy(orderBy));
			return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
		}

		public static TEntity FindById<TEntity>(this DbSet<TEntity> dbSet, object id) where TEntity : BaseModel
		{
			return dbSet.Find(new object[1]
			{
				id
			});
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where) where TEntity : BaseModel
		{
			return dbSet.Where(where);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this IQueryable<TEntity> entities, Expression<Func<TEntity, bool>> where) where TEntity : BaseModel
		{
			return entities.Where(where);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total) where TEntity : BaseModel
		{
			total = dbSet.Count(where);
			return (from item in dbSet.Where(@where)
			orderby item.Id
			select item).Skip((pageNumber - 1) * pageSize).Take(pageSize);
		}

		public static IQueryable<TEntity> FindBy<TEntity>(this IQueryable<TEntity> entities, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total) where TEntity : BaseModel
		{
			total = entities.Count(where);
			return (from item in entities.Where(@where)
			orderby item.Id
			select item).Skip((pageNumber - 1) * pageSize).Take(pageSize);
		}

		public static IQueryable<TEntity> FindBy<TEntity, TKey>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<TEntity, TKey>> orderBy, bool isAsc = true) where TEntity : BaseModel
		{
			total = dbSet.Count(where);
			if (isAsc)
			{
				return dbSet.Where(where).OrderBy(orderBy).Skip((pageNumber - 1) * pageSize)
					.Take(pageSize);
			}
			return dbSet.Where(where).OrderByDescending(orderBy).Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);
		}

		public static IQueryable<TEntity> FindBy<TEntity, TKey>(this IQueryable<TEntity> entities, Expression<Func<TEntity, bool>> where, int pageNumber, int pageSize, out int total, Expression<Func<TEntity, TKey>> orderBy, bool isAsc = true)
		{
			total = entities.Count(where);
			if (isAsc)
			{
				return entities.Where(where).OrderBy(orderBy).Skip((pageNumber - 1) * pageSize)
					.Take(pageSize);
			}
			return entities.Where(where).OrderByDescending(orderBy).Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);
		}

		public static void Remove<TEntity>(this DbSet<TEntity> dbSet, params object[] ids) where TEntity : BaseModel
		{
			List<TEntity> list = new List<TEntity>();
			foreach (object id in ids)
			{
				list.Add(dbSet.FindById(id));
			}
			dbSet.RemoveRange((IEnumerable<TEntity>)list);
		}

		public static void Remove<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where) where TEntity : BaseModel
		{
			IEnumerable<TEntity> entities = dbSet.FindBy(where);
			dbSet.RemoveRange(entities);
		}

		public static bool Exist<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> where) where TEntity : BaseModel
		{
			return dbSet.Count(where) > 0;
		}
	}
}
