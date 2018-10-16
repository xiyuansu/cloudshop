using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Hidistro.Core
{
	public class AspNetCache : IHiCache
	{
		private const int DEFAULT_TMEOUT = 600;

		private static Cache _cache;

		public AspNetCache()
		{
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				AspNetCache._cache = current.Cache;
			}
			else
			{
				AspNetCache._cache = HttpRuntime.Cache;
			}
		}

		public void Insert(string key, object obj)
		{
			this.Insert(key, obj, 600, false);
		}

		public void Insert(string key, object obj, int seconds, bool forceOutOfDate = false)
		{
			if (obj != null)
			{
				AspNetCache._cache.Insert(key, obj, null, DateTime.Now.AddSeconds((double)((seconds > 0) ? seconds : 600)), TimeSpan.Zero, CacheItemPriority.Normal, null);
			}
		}

		public object Get(string key)
		{
			return AspNetCache._cache[key];
		}

		public void Clear()
		{
			IDictionaryEnumerator enumerator = AspNetCache._cache.GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				arrayList.Add(enumerator.Key);
			}
			foreach (string item in arrayList)
			{
				AspNetCache._cache.Remove(item);
			}
		}

		public void Remove(string key)
		{
			AspNetCache._cache.Remove(key);
		}

		public T Get<T>(string key)
		{
			object obj = AspNetCache._cache.Get(key);
			return (T)obj;
		}
	}
}
