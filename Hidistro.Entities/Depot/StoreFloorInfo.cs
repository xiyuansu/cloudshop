using System.Collections.Generic;

namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_StoreFloors")]
	public class StoreFloorInfo
	{
		private IList<StoreProductBaseModel> _Products = null;

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int FloorId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ImageId
		{
			get;
			set;
		}

		public string ImageName
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FloorName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public FloorClientType FloorClientType
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public IList<StoreProductBaseModel> Products
		{
			get
			{
				if (this._Products == null)
				{
					return new List<StoreProductBaseModel>();
				}
				return this._Products;
			}
			set
			{
				this._Products = value;
			}
		}
	}
}
