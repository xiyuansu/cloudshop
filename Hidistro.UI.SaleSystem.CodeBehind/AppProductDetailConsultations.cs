using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppProductDetailConsultations : AppshopTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptProductConsultations;

		private HtmlGenericControl divConsultationEmpty;

		private HtmlGenericControl ulConsultations;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-ProductDetailConsultations.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.divConsultationEmpty = (HtmlGenericControl)this.FindControl("divConsultationEmpty");
			this.ulConsultations = (HtmlGenericControl)this.FindControl("ulConsultations");
			this.rptProductConsultations = (AppshopTemplatedRepeater)this.FindControl("rptProductConsultations");
			ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(this.Page.Request.QueryString["productId"].ToInt(0), null, SettingsManager.GetMasterSettings().OpenMultStore, 0);
			if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
			{
				base.GotoResourceNotFound("该件商品已经被管理员删除");
			}
			else
			{
				DataTable dBConsultations = productBrowseInfo.DBConsultations;
				for (int i = 0; i < dBConsultations.Rows.Count; i++)
				{
					dBConsultations.Rows[i]["UserName"] = DataHelper.GetHiddenUsername(dBConsultations.Rows[i]["UserName"].ToNullString());
				}
				this.rptProductConsultations.DataSource = dBConsultations;
				this.rptProductConsultations.DataBind();
				this.divConsultationEmpty.Visible = dBConsultations.IsNullOrEmpty();
				this.ulConsultations.Visible = !dBConsultations.IsNullOrEmpty();
				PageTitle.AddSiteNameTitle("商品咨询");
			}
		}
	}
}
