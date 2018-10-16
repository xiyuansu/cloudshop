using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_MemberOrderProducts : WAPTemplatedWebControl
	{
		public delegate void DataBindEventHandler(object sender, RepeaterItemEventArgs e);

		public const string TagID = "Common_MemberOrderProducts";

		private Repeater dataSupplier;

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

		public OrderInfo orderInfo
		{
			get;
			set;
		}

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dataSupplier.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataSupplier.DataSource = value;
			}
		}

		public event DataBindEventHandler ItemDataBound;

		public Common_MemberOrderProducts()
		{
			base.ID = "Common_MemberOrderProducts";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/tags/skin-Common_MemberOrderProducts.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dataSupplier = (Repeater)this.FindControl("rptSuppliers");
			this.dataSupplier.ItemDataBound += this.dataSupplier_ItemDataBound;
		}

		private void dataSupplier_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			Repeater repeater = e.Item.FindControl("listOrderItems") as Repeater;
			repeater.ItemDataBound += this.rptOrderProducts_ItemDataBound;
			Label label = e.Item.FindControl("lblSupplierName") as Label;
			HtmlGenericControl htmlGenericControl = (HtmlGenericControl)e.Item.FindControl("divSupplier");
			HtmlGenericControl htmlGenericControl2 = (HtmlGenericControl)e.Item.FindControl("divSendRedEnvelope");
			if (this.orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || this.orderInfo.OrderStatus == OrderStatus.Finished || this.orderInfo.OrderStatus == OrderStatus.WaitReview || this.orderInfo.OrderStatus == OrderStatus.History)
			{
				WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
				bool visible = false;
				if (openedWeiXinRedEnvelope != null && openedWeiXinRedEnvelope.EnableIssueMinAmount <= this.orderInfo.GetPayTotal() && this.orderInfo.OrderDate >= openedWeiXinRedEnvelope.ActiveStartTime && this.orderInfo.OrderDate <= openedWeiXinRedEnvelope.ActiveEndTime)
				{
					visible = true;
				}
				if (htmlGenericControl2 != null)
				{
					htmlGenericControl2.Visible = visible;
					if (base.ClientType == ClientType.VShop)
					{
						htmlGenericControl2.InnerHtml = "<a href=\"/vshop/SendRedEnvelope.aspx?OrderId=" + this.orderInfo.OrderId + "\"></a>";
					}
					else
					{
						htmlGenericControl2.InnerHtml = "<a href=\"javascript:ShowMsg('代金红包请前往微信端领取!','false')\"></a>";
					}
				}
			}
			if (repeater != null)
			{
				int supplierId = 0;
				int.TryParse(DataBinder.Eval(e.Item.DataItem, "SupplierId").ToString(), out supplierId);
				List<LineItemInfo> list = (from i in this.orderInfo.LineItems.Values
				where i.SupplierId == supplierId
				select i).ToList();
				if (HiContext.Current.SiteSettings.OpenMultStore && this.orderInfo.StoreId > 0)
				{
					label.Text = this.orderInfo.StoreName;
					htmlGenericControl.Attributes["class"] = "mtitle";
				}
				else if (HiContext.Current.SiteSettings.OpenSupplier && (from i in list
				where i.SupplierId > 0
				select i).Count() > 0 && list.Count > 0)
				{
					label.Text = list[0].SupplierName;
					htmlGenericControl.Attributes["class"] = "stitle";
				}
				else
				{
					label.Text = "平台";
					htmlGenericControl.Attributes["class"] = "ztitle";
				}
				repeater.DataSource = list;
				repeater.DataBind();
			}
		}

		private void rptOrderProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			this.ItemDataBound(sender, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataSupplier.DataSource != null)
			{
				this.dataSupplier.DataBind();
			}
		}
	}
}
