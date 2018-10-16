using System.Collections.Generic;

namespace HiShop.API.Setting.Context
{
	public class MessageContainer<T> : List<T>
	{
		public int MaxRecordCount
		{
			get;
			set;
		}

		public MessageContainer()
		{
		}

		public MessageContainer(int maxRecordCount)
		{
			this.MaxRecordCount = maxRecordCount;
		}

		public new void Add(T item)
		{
			base.Add(item);
			this.RemoveExpressItems();
		}

		private void RemoveExpressItems()
		{
			if (this.MaxRecordCount > 0 && base.Count > this.MaxRecordCount)
			{
				base.RemoveRange(0, base.Count - this.MaxRecordCount);
			}
		}

		public new void AddRange(IEnumerable<T> collection)
		{
			base.AddRange(collection);
			this.RemoveExpressItems();
		}

		public new void Insert(int index, T item)
		{
			base.Insert(index, item);
			this.RemoveExpressItems();
		}

		public new void InsertRange(int index, IEnumerable<T> collection)
		{
			base.InsertRange(index, collection);
			this.RemoveExpressItems();
		}
	}
}
