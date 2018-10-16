using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.WeChartApplet;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.home
{
	public class AppletConfig : StoreAdminPage
	{
		protected LinkButton lkbAddAppletFloor;

		protected Button Button1;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected HiddenField hidAllSelectedProducts;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected HiddenField hidSKUUploadImages;

		protected HiddenField hidSKUOldImages;

		protected void Page_Load(object sender, EventArgs e)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			string storeSlideImagesByStoreId = DepotHelper.GetStoreSlideImagesByStoreId(storeId);
			this.hidOldImages.Value = storeSlideImagesByStoreId;
			IList<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(storeId, FloorClientType.O2OApplet);
			int pageIndex = 1;
			int pageSize = 1000000000;
			DbQueryResult showProductList = WeChartAppletHelper.GetShowProductList(0, pageIndex, pageSize, storeId, ProductType.ServiceProduct);
			DataTable data = showProductList.Data;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Rows.Count; i++)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Insert(0, data.Rows[i]["ProductId"] + "|||" + data.Rows[i]["ProductName"] + ",,,");
				}
				else
				{
					stringBuilder.Append(data.Rows[i]["ProductId"] + "|||" + data.Rows[i]["ProductName"]);
				}
			}
			this.hidSelectProducts.Value = stringBuilder.ToString();
			this.lkbAddAppletFloor.Click += this.lkbAddAppletFloor_Click;
		}

		private void lkbAddAppletFloor_Click(object sender, EventArgs e)
		{
			this.SaveUploadFile();
			base.Response.Redirect("/Depot/home/AddAppletFloor.aspx");
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			this.SaveUploadFile();
			base.Response.Write("<script>alert('保存成功!');window.location.href='/Depot/home/AppletConfig.aspx'</script>");
		}

		public void SaveUploadFile()
		{
			string text = HiContext.Current.GetStoragePath() + "depot/";
			string originalSavePath = HttpContext.Current.Server.MapPath(text);
			string tempPath = HiContext.Current.GetStoragePath() + "temp/";
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string[] source = this.hidOldImages.Value.Trim().Split(',');
			string text2 = this.hidUploadImages.Value.Trim();
			string[] aryImgs = text2.Split(',');
			List<string> list = (from a in source
			where !aryImgs.Contains(a) && a.Length > 0
			select a).ToList();
			list.ForEach(delegate(string c)
			{
				c = c.Replace("//", "/");
				if (c.Length > 0 && !c.Contains("http:"))
				{
					string str = c.Split('/')[4];
					string path = originalSavePath + str;
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					string path2 = HttpContext.Current.Server.MapPath(tempPath + str);
					if (File.Exists(path2))
					{
						File.Delete(path2);
					}
				}
			});
			string text3 = "";
			foreach (string text4_i in aryImgs)
			{
				string text4 = text4_i.Replace("//", "/");
				if (text4.Length != 0)
				{
					if (text4.Contains("http:") || text4.Contains("https:"))
					{
						text3 = ((!(text3 == "")) ? (text3 + "," + text4) : text4);
					}
					else
					{
						string text5 = (text4.Split('/').Length == 6) ? text4.Split('/')[5] : text4.Split('/')[4];
						if (File.Exists(originalSavePath + text5))
						{
							text3 = ((!(text3 == "")) ? (text3 + "," + text + text5) : (text + text5));
						}
						else if (File.Exists(HttpContext.Current.Server.MapPath(text4)))
						{
							File.Copy(HttpContext.Current.Server.MapPath(text4), originalSavePath + text5);
							if (File.Exists(HttpContext.Current.Server.MapPath(text4)))
							{
								File.Delete(HttpContext.Current.Server.MapPath(text4));
							}
							text3 = ((!(text3 == "")) ? (text3 + "," + text + text5) : (text + text5));
						}
					}
				}
			}
			DepotHelper.UpdateStoreSlideImages(HiContext.Current.Manager.StoreId, text3);
			if (this.hidProductIds.Value != "")
			{
				WeChartAppletHelper.AddChoiceProdcutByPC(this.hidProductIds.Value, HiContext.Current.Manager.StoreId);
			}
		}
	}
}
