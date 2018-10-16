using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	[TableName("Hishop_ProductInputItems")]
	public class ProductInputItemInfo
	{
		private IList<string> _Values = new List<string>();

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int Id
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string InputFieldTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int InputFieldType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsRequired
		{
			get;
			set;
		}

		public IList<string> InputFileValues
		{
			get
			{
				if (this._Values == null)
				{
					this._Values = new List<string>();
				}
				return this._Values;
			}
			set
			{
				this._Values = value;
			}
		}
	}
}
