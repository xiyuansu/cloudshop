using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_ProductList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShoppingCart_ProductList";

		private Repeater rptSupplier;

		private Panel pnlShopProductCart;

		private int invalidSupplierId = -1;

		private IList<ShoppingCartItemInfo> listCartItems;

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

		public IList<ShoppingCartItemInfo> ListCartItems
		{
			get
			{
				return this.listCartItems;
			}
			set
			{
				this.listCartItems = value;
			}
		}

		public SiteSettings settings
		{
			get;
			set;
		}

		public event RepeaterItemEventHandler ItemDataBound;

		public Common_ShoppingCart_ProductList()
		{
			base.ID = "Common_ShoppingCart_ProductList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_ProductList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.pnlShopProductCart = (Panel)this.FindControl("pnlShopProductCart");
			this.pnlShopProductCart.Visible = false;
			this.rptSupplier = (Repeater)this.FindControl("rptSupplier");
			if (this.rptSupplier != null && this.ListCartItems != null)
			{
				if (HiContext.Current.User.UserId > 0)
				{
					IEnumerable<ShoppingCartItemInfo> enumerable = this.listCartItems.Where(delegate(ShoppingCartItemInfo x)
					{
						if (x.IsValid && x.HasEnoughStock)
						{
							return false;
						}
						return x.StoreId == 0;
					});
					if (enumerable != null && enumerable.Count() > 0)
					{
						this.invalidSupplierId = this.listCartItems.Max((ShoppingCartItemInfo x) => x.SupplierId) + 100;
						foreach (ShoppingCartItemInfo item in enumerable)
						{
							item.SupplierId = this.invalidSupplierId;
							item.SupplierName = "";
						}
					}
				}
				this.rptSupplier.ItemDataBound += this.rptSupplier_ItemDataBound;
				var dataSource = from x in this.ListCartItems
				where x.StoreId == 0
				group x by new
				{
					x.SupplierId,
					x.SupplierName
				} into x
				orderby x.Key.SupplierId
				select x;
				this.rptSupplier.DataSource = dataSource;
				this.rptSupplier.DataBind();
			}
		}

		protected void rptSupplier_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Literal literal = e.Item.FindControl("ltsupplierId") as Literal;
			Literal literal2 = e.Item.FindControl("ltsupplierName") as Literal;
			if (literal != null && literal2 != null)
			{
				int intsupplierId = literal.Text.ToInt(0);
				if (intsupplierId == this.invalidSupplierId || !this.settings.OpenSupplier)
				{
					literal2.Visible = false;
				}
				else
				{
					string str = string.Format("<input type=\"checkbox\" checked=\"checked\" name=\"chksupplier\" id='chksupplier_{0}' value='{0}' class=\"icheck\" onclick='setchksupplier({0})' />", intsupplierId);
					string arg = (intsupplierId == 0) ? "pcztitle_new" : "pcstitle";
					literal2.Text = str + $"<span class=\"{arg}\">{literal2.Text}</span>";
				}
				Repeater repeater = e.Item.FindControl("dataListShoppingCart") as Repeater;
				if (repeater != null && this.ListCartItems != null)
				{
					repeater.ItemDataBound += this.dataListShoppingCart_ItemDataBound;
					IEnumerable<ShoppingCartItemInfo> source = this.listCartItems.Where(delegate(ShoppingCartItemInfo x)
					{
						if (x.SupplierId == intsupplierId)
						{
							return x.StoreId == 0;
						}
						return false;
					});
					Func<ShoppingCartItemInfo, bool> keySelector = (ShoppingCartItemInfo x) => x.IsValid;
					IOrderedEnumerable<ShoppingCartItemInfo> orderedEnumerable2 = (IOrderedEnumerable<ShoppingCartItemInfo>)(repeater.DataSource = source.OrderByDescending(keySelector));
					repeater.DataBind();
				}
			}
		}

		private void dataListShoppingCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			this.ItemDataBound(sender, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rptSupplier.DataSource != null)
			{
				this.rptSupplier.DataBind();
			}
		}

		public void ShowProductCart()
		{
			if (this.ListCartItems == null)
			{
				this.pnlShopProductCart.Visible = false;
			}
			else
			{
				this.pnlShopProductCart.Visible = true;
			}
		}
	}
}
