using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoreProductLink : AdminPage
	{
		private int ProductId;

		private int StoreId;

		protected HiddenField hidProductId;

		protected HiddenField hidStoreId;

		protected Label lblReferralsLink;

		protected Button btnDownLoad;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ProductId"], out this.ProductId))
			{
				base.GotoResourceNotFound();
			}
			else if (!int.TryParse(this.Page.Request.QueryString["StoreId"], out this.StoreId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.hidStoreId.Value = this.StoreId.ToString();
				this.hidProductId.Value = this.ProductId.ToString();
				ProductInfo productById = ProductHelper.GetProductById(this.ProductId);
				if (productById != null)
				{
					if (productById.ProductType == 0.GetHashCode())
					{
						this.lblReferralsLink.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/vshop/StoreProductDetails?ProductId=" + this.hidProductId.Value + "&StoreId=" + this.hidStoreId.Value;
					}
					else
					{
						this.lblReferralsLink.Text = Globals.HostPath(HttpContext.Current.Request.Url) + "/vshop/ServiceProductDetails?ProductId=" + this.hidProductId.Value + "&StoreId=" + this.hidStoreId.Value;
					}
				}
			}
		}

		protected void btnDownLoad_Click(object sender, EventArgs e)
		{
			string str = "Store_" + this.hidStoreId.Value + "_Product_" + this.hidProductId.Value + ".png";
			string qrCodeUrl = "/Storage/master/QRCode/Store_" + this.hidStoreId.Value + "_Product_" + this.hidProductId.Value + ".png";
			string text = Globals.HostPath(HttpContext.Current.Request.Url) + "/vshop/StoreProductDetails?ProductId=" + this.hidProductId.Value + "&StoreId=" + this.hidStoreId.Value;
			string path = Globals.CreateQRCode(text.Contains("http") ? text : ("http://" + text), qrCodeUrl, false, ImageFormats.Png);
			FileStream fileStream = new FileStream(base.Server.MapPath(path), FileMode.Open);
			byte[] array = new byte[(int)fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			base.Response.ContentType = "application/octet-stream";
			base.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(str, Encoding.UTF8));
			base.Response.BinaryWrite(array);
			base.Response.Flush();
			base.Response.End();
		}
	}
}
