using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Shopping;
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
	public class APPShop_SubmmitCartProducts : AppshopTemplatedWebControl
	{
		public delegate void DataBindEventHandler(object sender, RepeaterItemEventArgs e);

		public const string TagID = "APPShop_SubmmitCartProducts";

		private Repeater dataSupplier;

		public int RegionId
		{
			get;
			set;
		}

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

		public int StoreId
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

		public APPShop_SubmmitCartProducts()
		{
			base.ID = "APPShop_SubmmitCartProducts";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/tags/skin-Common_SubmmitCartProducts.ascx";
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
			Repeater repeater = e.Item.FindControl("rptOrderProducts") as Repeater;
			repeater.ItemDataBound += this.rptOrderProducts_ItemDataBound;
			Label label = e.Item.FindControl("lblSupplierAmount") as Label;
			Label label2 = e.Item.FindControl("lblFreight") as Label;
			Label label3 = e.Item.FindControl("lblSupplierName") as Label;
			HtmlGenericControl htmlGenericControl = (HtmlGenericControl)e.Item.FindControl("divSupplier");
			HtmlGenericControl htmlGenericControl2 = (HtmlGenericControl)e.Item.FindControl("divFreightAndAmount");
			if (repeater != null)
			{
				int supplierId = 0;
				int.TryParse(DataBinder.Eval(e.Item.DataItem, "SupplierId").ToString(), out supplierId);
				if (supplierId == 0)
				{
					htmlGenericControl.Attributes["class"] = "ztitle";
				}
				label3.Text = DataBinder.Eval(e.Item.DataItem, "SupplierName").ToString();
				List<ShoppingCartItemInfo> list = (from i in this.ShoppingCart.LineItems
				where i.SupplierId == supplierId
				select i).ToList();
				decimal num = (from i in this.ShoppingCart.LineItems
				where i.SupplierId == supplierId
				select i.AdjustedPrice.F2ToString("f2").ToDecimal(0) * (decimal)i.Quantity).Sum();
				repeater.DataSource = list;
				repeater.DataBind();
				label.Text = "￥" + num.F2ToString("f2");
				decimal num2 = ShoppingProcessor.CalcSupplierFreight(supplierId, this.RegionId, this.ShoppingCart);
				label2.Text = "￥" + num2.F2ToString("f2") + ((supplierId == 0 && this.ShoppingCart.LineGifts.Count() > 0) ? "(含礼品)" : "");
				if (!HiContext.Current.SiteSettings.OpenSupplier)
				{
					label3.Text = "商品清单";
				}
				if (HiContext.Current.SiteSettings.OpenMultStore && this.StoreId > 0)
				{
					StoresInfo storeById = DepotHelper.GetStoreById(this.StoreId);
					if (storeById != null)
					{
						label3.Text = storeById.StoreName;
						htmlGenericControl.Attributes["class"] = "mtitle";
					}
				}
				else if (HiContext.Current.SiteSettings.OpenSupplier && supplierId > 0 && list.Count > 0)
				{
					label3.Text = list[0].SupplierName;
					htmlGenericControl.Attributes["class"] = "stitle";
				}
				else
				{
					label3.Text = "平台";
					htmlGenericControl.Attributes["class"] = "ztitle";
				}
			}
			if (HiContext.Current.SiteSettings.OpenMultStore && this.StoreId > 0)
			{
				htmlGenericControl2.Visible = false;
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
