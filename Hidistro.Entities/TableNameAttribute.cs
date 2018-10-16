using System;

namespace Hidistro.Entities
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class TableNameAttribute : Attribute
	{
		private string tableName;

		public string TableName
		{
			get
			{
				return this.tableName;
			}
		}

		public TableNameAttribute(string tableName)
		{
			this.tableName = tableName;
		}
	}
}
