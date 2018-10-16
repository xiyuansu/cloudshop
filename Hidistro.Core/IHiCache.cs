namespace Hidistro.Core
{
	public interface IHiCache
	{
		void Clear();

		void Remove(string key);

		void Insert(string key, object obj);

		void Insert(string key, object obj, int seconds, bool forceOutOfDate = false);

		object Get(string key);

		T Get<T>(string key);
	}
}
