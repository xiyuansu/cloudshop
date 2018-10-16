using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class StoreStocks : AdminPage
	{
		private DataTable dtSkuStocks;

		protected HtmlGenericControl divempty;

		protected HtmlGenericControl divlist;

		protected Repeater repStoreStock;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			string text = this.Page.Request.QueryString["productId"];
			if (!string.IsNullOrEmpty(text))
			{
				this.dtSkuStocks = ProductHelper.GetAllStoreSkuStocks(text);
				if (this.dtSkuStocks.IsNullOrEmpty())
				{
					this.divempty.Visible = true;
					this.divlist.Visible = false;
				}
				else
				{
					Dictionary<int, string> dictionary = (from a in this.dtSkuStocks.AsEnumerable()
					group a by new
					{
						StoreId = a.Field<int>("StoreId"),
						StoreName = a.Field<string>("StoreName")
					}).ToDictionary(a => a.Key.StoreId, a => a.Key.StoreName);
					List<StoresInfo> list = new List<StoresInfo>();
					foreach (KeyValuePair<int, string> item in dictionary)
					{
						StoresInfo storesInfo = new StoresInfo();
						storesInfo.StoreId = item.Key;
						storesInfo.StoreName = item.Value;
						list.Add(storesInfo);
					}
					this.repStoreStock.DataSource = list;
					this.repStoreStock.DataBind();
				}
			}
		}

		protected void repStoreStock_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				int storeId = (e.Item.DataItem as StoresInfo).StoreId;
				EnumerableRowCollection<DataRow> source = from a in this.dtSkuStocks.AsEnumerable()
				where a.Field<int>("StoreId") == storeId && !string.IsNullOrEmpty(a.Field<string>("SKUContent"))
				select a;
				if (source.Count() > 0)
				{
					DataTable dataSource = source.CopyToDataTable();
					Repeater repeater = e.Item.FindControl("repStocks") as Repeater;
					repeater.DataSource = dataSource;
					repeater.DataBind();
				}
				else
				{
					this.divempty.Visible = true;
					this.divlist.Visible = false;
				}
			}
		}
	}
}
