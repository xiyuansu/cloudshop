using System;

namespace Hidistro.Core
{
	public class RedisCache : IHiCache
	{
		public void Clear()
		{
			throw new NotImplementedException();
		}

		public void Remove(string key)
		{
			throw new NotImplementedException();
		}

		public void Insert(string key, object obj)
		{
			throw new NotImplementedException();
		}

		public void Insert(string key, object obj, int seconds, bool forceOutOfDate = false)
		{
			throw new NotImplementedException();
		}

		public object Get(string key)
		{
			throw new NotImplementedException();
		}

		public T Get<T>(string key)
		{
			throw new NotImplementedException();
		}
	}
}
