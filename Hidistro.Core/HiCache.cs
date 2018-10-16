namespace Hidistro.Core
{
	public sealed class HiCache
	{
		private static IHiCache _cache;

		private HiCache()
		{
		}

		static HiCache()
		{
			HiCache._cache = CacheFactory.CreateInstance();
		}

		public static void Clear()
		{
			HiCache._cache.Clear();
		}

		public static void Remove(string key)
		{
			HiCache._cache.Remove(key);
		}

		public static void Insert(string key, object obj)
		{
			HiCache._cache.Insert(key, obj);
		}

		public static void Insert(string key, object obj, int seconds)
		{
			HiCache._cache.Insert(key, obj, seconds, false);
		}

		public static object Get(string key)
		{
			return HiCache._cache.Get(key);
		}

		public static T Get<T>(string key)
		{
			return HiCache._cache.Get<T>(key);
		}
	}
}
