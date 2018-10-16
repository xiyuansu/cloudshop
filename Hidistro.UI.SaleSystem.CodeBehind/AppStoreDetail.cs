using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppStoreDetail : AppshopTemplatedWebControl
	{
		private int storeId = 0;

		private HtmlImage imgStore;

		private HtmlInputHidden hidLatitude;

		private HtmlInputHidden hidLongitude;

		private HtmlInputHidden hidTel;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-StoreDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("门店详情页");
			this.imgStore = (HtmlImage)this.FindControl("imgStore");
			this.hidLatitude = (HtmlInputHidden)this.FindControl("hidLatitude");
			this.hidLongitude = (HtmlInputHidden)this.FindControl("hidLongitude");
			this.hidTel = (HtmlInputHidden)this.FindControl("hidTel");
			int.TryParse(this.Page.Request.QueryString["storeId"], out this.storeId);
			if (this.storeId > 0)
			{
				StoresInfo storeById = DepotHelper.GetStoreById(this.storeId);
				if (!string.IsNullOrEmpty(storeById.StoreImages))
				{
					this.imgStore.Src = storeById.StoreImages.Split(',')[0];
				}
				HtmlInputHidden htmlInputHidden = this.hidLatitude;
				double? nullable = storeById.Latitude;
				htmlInputHidden.Value = nullable.ToString();
				HtmlInputHidden htmlInputHidden2 = this.hidLongitude;
				nullable = storeById.Longitude;
				htmlInputHidden2.Value = nullable.ToString();
				this.hidTel.Value = storeById.Tel;
				Literal literal = this.FindControl("ltopentime") as Literal;
				if (literal != null && !string.IsNullOrEmpty(storeById.StoreOpenTime))
				{
					literal.Text = $"<div class=\"opentime\">营业时间:{storeById.StoreOpenTime}</div>";
				}
				PageTitle.AddSiteNameTitle("周边门店-" + storeById.StoreName);
			}
			else
			{
				base.GotoResourceNotFound("参数错误");
			}
		}
	}
}
