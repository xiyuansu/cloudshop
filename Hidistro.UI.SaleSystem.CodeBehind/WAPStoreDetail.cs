using Hidistro.Context;
using Hidistro.Core;
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
	public class WAPStoreDetail : WAPTemplatedWebControl
	{
		private int storeId = 0;

		private HtmlImage imgStore;

		private HtmlInputHidden hidLatitude;

		private HtmlInputHidden hidLongitude;

		private HtmlInputHidden hidTel;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

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
			this.imgStore = (HtmlImage)this.FindControl("imgStore");
			this.hidLatitude = (HtmlInputHidden)this.FindControl("hidLatitude");
			this.hidLongitude = (HtmlInputHidden)this.FindControl("hidLongitude");
			this.hidTel = (HtmlInputHidden)this.FindControl("hidTel");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdAppId.Value = HiContext.Current.SiteSettings.WeixinAppId;
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
				this.hdTitle.Value = storeById.StoreName;
				this.hdDesc.Value = storeById.StoreName;
				string storeImages = storeById.StoreImages;
				string local = string.IsNullOrEmpty(storeImages) ? SettingsManager.GetMasterSettings().LogoUrl : storeImages;
				this.hdImgUrl.Value = Globals.FullPath(local);
				this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
				PageTitle.AddSiteNameTitle("周边门店-" + storeById.StoreName);
			}
			else
			{
				base.GotoResourceNotFound("参数错误");
			}
		}
	}
}
