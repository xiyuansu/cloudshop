using Hidistro.Context;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderManage_OrderItems : AscxTemplatedWebControl
	{
		public const string TagID = "Common_OrderManage_OrderItems";

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

		public event RepeaterItemEventHandler ItemDataBound;

		public Common_OrderManage_OrderItems()
		{
			base.ID = "Common_OrderManage_OrderItems";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_OrderManage_OrderItems.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dataSupplier = (Repeater)this.FindControl("dataSupplier");
			this.dataSupplier.ItemDataBound += this.dataSupplier_ItemDataBound;
		}

		private void dataSupplier_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			Repeater repeater = e.Item.FindControl("dataListOrderItems") as Repeater;
			Label label = e.Item.FindControl("lblsupplier") as Label;
			if (repeater != null)
			{
				repeater.ItemDataBound += this.orderItems_ItemDataBound;
				object dataItem = e.Item.DataItem;
				Type type = dataItem.GetType();
				PropertyInfo property = type.GetProperty("SupplierId", typeof(int));
				int sid = int.Parse(property.GetValue(dataItem, null).ToString());
				List<LineItemInfo> list = (from i in this.orderInfo.LineItems.Values
				where i.SupplierId == sid
				select i).ToList();
				string empty = string.Empty;
				if (HiContext.Current.SiteSettings.OpenMultStore && this.orderInfo.StoreId > 0)
				{
					label.Text = this.orderInfo.StoreName;
					empty = "mtitle_1";
				}
				else if (this.orderInfo.StoreId == 0 && HiContext.Current.SiteSettings.OpenSupplier && sid > 0 && list.Count > 0)
				{
					label.Text = list[0].SupplierName;
					empty = "stitle_1";
				}
				else
				{
					label.Text = "平台";
					empty = "ztitle_1_new";
				}
				label.Attributes.Add("style", string.IsNullOrWhiteSpace(label.Text) ? "display:none" : "display:inline");
				label.Attributes.Add("class", empty);
				label.Visible = true;
				repeater.DataSource = list;
				repeater.DataBind();
			}
		}

		private void orderItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
