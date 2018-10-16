using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SupplierList : ThemedTemplatedRepeater
	{
		public const string TagID = "list_Common_SupplierList";

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		public int MaxNum
		{
			get;
			set;
		}

		public Common_SupplierList()
		{
			base.ID = "list_Common_SupplierList";
		}

		protected override void OnLoad(EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				base.DataSource = SupplierHelper.GetSupplierAll(this.MaxNum);
				base.DataBind();
			}
		}
	}
}
