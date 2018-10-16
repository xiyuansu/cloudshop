namespace Hidistro.Entities.Store
{
	[TableName("Hishop_Regions")]
	public class RegionInfo
	{
		[FieldType(FieldType.KeyField)]
		public int RegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ParentRegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RegionName
		{
			get;
			set;
		}

		public string FullRegionName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Depth
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsLast
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FullRegionPath
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDel
		{
			get;
			set;
		}
	}
}
