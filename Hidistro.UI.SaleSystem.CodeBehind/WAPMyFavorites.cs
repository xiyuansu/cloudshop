using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPMyFavorites : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptProducts;

		private HtmlGenericControl fav_fot;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyFavorites.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string url = this.Page.Request.QueryString["returnUrl"];
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["returnUrl"]))
			{
				this.Page.Response.Redirect(url);
			}
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			this.fav_fot = (HtmlGenericControl)this.FindControl("fav_fot");
			Pagination pagination = new Pagination();
			pagination.PageIndex = 1;
			pagination.PageSize = 100;
			DataTable data = ProductBrowser.GetFavorites(null, null, pagination, false).Data;
			this.rptProducts.DataSource = data;
			this.rptProducts.DataBind();
			if (data.Rows.Count == 0)
			{
				this.fav_fot.Visible = false;
			}
			else
			{
				this.fav_fot.Visible = true;
			}
			PageTitle.AddSiteNameTitle("我的收藏");
		}
	}
}
