namespace Hidistro.Entities.Supplier
{
	public class SupplierExportModel
	{
		public string UserName
		{
			get;
			set;
		}

		public string SupplierName
		{
			get;
			set;
		}

		public string ContactMan
		{
			get;
			set;
		}

		public int ProductNums
		{
			get;
			set;
		}

		public string Tel
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string StatusText
		{
			get
			{
				return (this.Status == 1) ? "正常" : "冻结";
			}
		}

		public int OrderNums
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}
	}
}
