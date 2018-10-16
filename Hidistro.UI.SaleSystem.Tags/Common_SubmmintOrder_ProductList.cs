using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubmmintOrder_ProductList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_SubmmintOrder_ProductList";

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

		public ShoppingCartInfo ShoppingCart
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

		public Common_SubmmintOrder_ProductList()
		{
			base.ID = "Common_SubmmintOrder_ProductList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_SubmmintOrder_ProductList.ascx";
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
			Repeater repeater = e.Item.FindControl("dataListShoppingCrat") as Repeater;
			Label label = e.Item.FindControl("lblSupplierAmount") as Label;
			Label label2 = e.Item.FindControl("lblFreight") as Label;
			if (repeater != null)
			{
				repeater.ItemDataBound += this.dataListShoppingCart_ItemDataBound;
				object dataItem = e.Item.DataItem;
				Type type = dataItem.GetType();
				PropertyInfo property = type.GetProperty("SupplierId", typeof(int));
				int sid = int.Parse(property.GetValue(dataItem, null).ToString());
				List<ShoppingCartItemInfo> dataSource = (from i in this.ShoppingCart.LineItems
				where i.SupplierId == sid
				select i).ToList();
				decimal num = (from i in this.ShoppingCart.LineItems
				where i.SupplierId == sid
				select i.AdjustedPrice.F2ToString("f2").ToDecimal(0) * (decimal)i.Quantity).Sum();
				repeater.DataSource = dataSource;
				repeater.DataBind();
				label.Text = "￥" + num.F2ToString("f2");
				decimal num2 = ShoppingProcessor.CalcSupplierFreight(sid, 0, this.ShoppingCart);
				label2.Text = "￥" + num2.F2ToString("f2") + ((sid == 0 && this.ShoppingCart.LineGifts.Count() > 0) ? "(含礼品)" : "");
				label2.CssClass = "spanFreight" + sid;
			}
		}

		private void dataListShoppingCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
