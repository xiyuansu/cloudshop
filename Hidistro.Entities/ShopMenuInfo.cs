using System.Collections.Generic;

namespace Hidistro.Entities
{
	[TableName("Hishop_NavMenu")]
	public class ShopMenuInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int MenuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ParentMenuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Type
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
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShopMenuPic
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ClientType
		{
			get;
			set;
		}

		public IList<ShopMenuInfo> SubMenus
		{
			get;
			set;
		}
	}
}
