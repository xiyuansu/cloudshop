using Enyim.Caching;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
using System;

namespace Hidistro.Core
{
	public class MemCached : IHiCache
	{
		private const int DEFAULT_TMEOUT = 600;

		private MemcachedClient client = MemcachedClientService.Instance.Client;

		private JsonSerializerSettings settings = new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			NullValueHandling = NullValueHandling.Ignore
		};

		public void Clear()
		{
			this.client.FlushAll();
		}

		public void Remove(string key)
		{
			this.client.Remove(key);
		}

		public void Insert(string key, object obj)
		{
			string value = JsonConvert.SerializeObject(obj, this.settings);
			this.client.Store(StoreMode.Set, key, value, DateTime.Now.AddSeconds(600.0) - DateTime.Now);
		}

		public void Insert(string key, object obj, int seconds, bool forceOutOfDate = false)
		{
			if (obj != null)
			{
				string value = JsonConvert.SerializeObject(obj, this.settings);
				this.client.Store(StoreMode.Set, key, value, DateTime.Now.AddSeconds((double)((seconds > 0) ? seconds : 600)) - DateTime.Now);
			}
		}

		public object Get(string key)
		{
			string value = this.client.Get(key) as string;
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			return JsonConvert.DeserializeObject(value, this.settings);
		}

		public T Get<T>(string key)
		{
			T result = default(T);
			string value = this.client.Get(key) as string;
			if (!string.IsNullOrWhiteSpace(value))
			{
				return JsonConvert.DeserializeObject<T>(value, this.settings);
			}
			return result;
		}
	}
}
