using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderManage_ReturnsApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_ReturnsApply";

		private Repeater listReturns;

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
				return this.listReturns.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listReturns.DataSource = value;
			}
		}

		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}

		public Common_OrderManage_ReturnsApply()
		{
			base.ID = "Common_OrderManage_ReturnsApply";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReturnsApply.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.listReturns = (Repeater)this.FindControl("listReturns");
			this.listReturns.ItemDataBound += this.listReturns_ItemDataBound;
		}

		private void listReturns_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			ReturnInfo returnInfo = (ReturnInfo)e.Item.DataItem;
			ReturnStatus handleStatus = returnInfo.HandleStatus;
			string skuId = returnInfo.SkuId;
			string orderId = returnInfo.OrderId;
			int returnId = returnInfo.ReturnId;
			Label label = (Label)e.Item.FindControl("Logistics");
			HyperLink hyperLink = (HyperLink)e.Item.FindControl("hlinkOrderDetails");
			if (hyperLink != null && (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed || returnInfo.HandleStatus == ReturnStatus.Deliverying))
			{
				hyperLink.Text = "处理";
				hyperLink.Attributes.CssStyle.Add("color", "red");
			}
			string adminRemark = returnInfo.AdminRemark;
			if (string.IsNullOrEmpty(adminRemark))
			{
				adminRemark = returnInfo.AdminRemark;
			}
			if (handleStatus != ReturnStatus.Returned && handleStatus != ReturnStatus.Deliverying && handleStatus != ReturnStatus.GetGoods)
			{
				return;
			}
			if (returnInfo.AfterSaleType != AfterSaleTypes.OnlyRefund)
			{
				label.Attributes.Add("action", "return");
				label.Attributes.Add("ReturnId", returnId.ToString());
				label.Visible = true;
			}
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listReturns.DataSource = this.DataSource;
			this.listReturns.DataBind();
		}
	}
}
