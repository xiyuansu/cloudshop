using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderManage_RefundApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_RefundApply";

		private Repeater listRefunds;

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
				return this.listRefunds.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listRefunds.DataSource = value;
			}
		}

		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}

		public Common_OrderManage_RefundApply()
		{
			base.ID = "Common_OrderManage_RefundApply";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_RefundApply.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.listRefunds = (Repeater)this.FindControl("listRefundOrder");
			this.listRefunds.ItemDataBound += this.listRefundOrder_ItemDataBound;
		}

		private void listRefundOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
			RefundStatus refundStatus = (RefundStatus)DataBinder.Eval(e.Item.DataItem, "HandleStatus");
			Label label = (Label)e.Item.FindControl("lblHandleStatus");
			if (label != null)
			{
				label.Text = EnumDescription.GetEnumDescription((Enum)(object)(RefundStatus)(int)DataBinder.Eval(e.Item.DataItem, "HandleStatus"), 0);
			}
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listRefunds.DataSource = this.DataSource;
			this.listRefunds.DataBind();
		}
	}
}
