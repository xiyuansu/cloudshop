using Hidistro.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderItems_AfterSales : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderItems_AfterSales";

		private Repeater listPrducts;

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

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.listPrducts.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listPrducts.DataSource = value;
			}
		}

		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}

		public Common_OrderItems_AfterSales()
		{
			base.ID = "Common_OrderItems_AfterSales";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderItems_AfterSales.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.listPrducts = (Repeater)this.FindControl("listPrducts");
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listPrducts.DataSource = this.DataSource;
			this.listPrducts.DataBind();
		}
	}
}
