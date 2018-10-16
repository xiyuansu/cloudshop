using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderManage_ReplaceApply : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_ReplaceApply";

		private Repeater listReplace;

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
				return this.listReplace.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listReplace.DataSource = value;
			}
		}

		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}

		public Common_OrderManage_ReplaceApply()
		{
			base.ID = "Common_OrderManage_ReplaceApply";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_ReplaceApply.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.listReplace = (Repeater)this.FindControl("listReplace");
			this.listReplace.ItemDataBound += this.listReplace_ItemDataBound;
		}

		private void listReplace_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			ReplaceInfo replaceInfo = (ReplaceInfo)e.Item.DataItem;
			int replaceId = replaceInfo.ReplaceId;
			ReplaceStatus handleStatus = replaceInfo.HandleStatus;
			string orderId = replaceInfo.OrderId;
			string skuId = replaceInfo.SkuId;
			HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
			Label label = (Label)e.Item.FindControl("Logistics");
			HyperLink hyperLink = (HyperLink)e.Item.FindControl("hlinkOrderDetails");
			if (hyperLink != null && (replaceInfo.HandleStatus == ReplaceStatus.MerchantsAgreed || replaceInfo.HandleStatus == ReplaceStatus.UserDelivery || replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery))
			{
				hyperLink.Text = "处理";
				hyperLink.Attributes.CssStyle.Add("color", "red");
			}
			if (handleStatus != ReplaceStatus.Replaced && handleStatus != ReplaceStatus.MerchantsDelivery && handleStatus != ReplaceStatus.UserDelivery)
			{
				return;
			}
			label.Attributes.Add("action", "replace");
			label.Attributes.Add("ReplaceId", replaceId.ToString());
			label.Visible = true;
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listReplace.DataSource = this.DataSource;
			this.listReplace.DataBind();
		}
	}
}
