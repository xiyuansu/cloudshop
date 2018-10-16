using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductPreSale)]
	public class ProductPreSaleLink : AdminPage
	{
		private int preSaleId;

		protected HiddenField hidProductId;

		protected Label lblReferralsLink;

		protected Button btnDownLoad;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["preSaleId"], out this.preSaleId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.preSaleId);
				if (productPreSaleInfo == null || productPreSaleInfo.ProductId <= 0)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.hidProductId.Value = productPreSaleInfo.ProductId.ToString();
					string text = Globals.HostPath(HttpContext.Current.Request.Url) + "/ProductDetails.aspx?productId=" + productPreSaleInfo.ProductId;
					this.lblReferralsLink.Text = text;
				}
			}
		}

		protected void btnDownLoad_Click(object sender, EventArgs e)
		{
			string str = "QRCode" + this.hidProductId.Value + ".png";
			string qrCodeUrl = "/Storage/master/QRCode/preSale_" + this.hidProductId.Value + ".png";
			string text = Globals.HostPath(HttpContext.Current.Request.Url) + "/ProductDetails.aspx?productId=" + this.hidProductId.Value;
			string path = Globals.CreateQRCode(text.Contains("http://") ? text : ("http://" + text), qrCodeUrl, false, ImageFormats.Png);
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
