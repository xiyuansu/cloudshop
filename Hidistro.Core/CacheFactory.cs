using Hidistro.Core.Configuration;

namespace Hidistro.Core
{
	public class CacheFactory
	{
		public static IHiCache CreateInstance()
		{
			string text = HiConfiguration.GetConfig().CachePolicy.ToLower();
			string a = text;
			if (!(a == "redis"))
			{
				if (a == "memcached")
				{
					return new MemCached();
				}
				return new AspNetCache();
			}
			return new RedisCache();
		}
	}
}
