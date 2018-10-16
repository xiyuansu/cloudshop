using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderManage_OrderGifts : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_OrderGifts";

		private Repeater dataListOrderItems;

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
				return this.dataListOrderItems.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListOrderItems.DataSource = value;
			}
		}

		public Common_OrderManage_OrderGifts()
		{
			base.ID = "Common_OrderManage_OrderGifts";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderGifts.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dataListOrderItems = (Repeater)this.FindControl("dataListOrderItems");
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListOrderItems.DataSource != null)
			{
				this.dataListOrderItems.DataBind();
			}
		}
	}
}
