using Enyim.Caching;

namespace Hidistro.Core
{
	internal class MemcachedClientService
	{
		private static readonly MemcachedClientService _instance;

		private static readonly MemcachedClient _client;

		public MemcachedClient Client
		{
			get
			{
				return MemcachedClientService._client;
			}
		}

		public static MemcachedClientService Instance
		{
			get
			{
				return MemcachedClientService._instance;
			}
		}

		static MemcachedClientService()
		{
			MemcachedClientService._instance = new MemcachedClientService();
			MemcachedClientService._client = new MemcachedClient("memcached");
		}
	}
}
