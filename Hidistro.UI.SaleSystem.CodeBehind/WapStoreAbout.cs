using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapStoreAbout : WAPTemplatedWebControl
	{
		private Literal litSupplierName;

		private Literal litSupplierAbout;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreAbout.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litSupplierName = (Literal)this.FindControl("litSupplierName");
			this.litSupplierAbout = (Literal)this.FindControl("litSupplierAbout");
			this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
			this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
			this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
			this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
			this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
			this.hdAppId.Value = HiContext.Current.SiteSettings.WeixinAppId;
			int storeId = base.GetParameter("StoreId").ToInt(0);
			StoresInfo storeById = DepotHelper.GetStoreById(storeId);
			if (storeById == null)
			{
				base.GotoResourceNotFound("");
			}
			else
			{
				this.hdTitle.Value = storeById.StoreName;
				this.hdDesc.Value = storeById.StoreName;
				string storeImages = storeById.StoreImages;
				string local = string.IsNullOrEmpty(storeImages) ? SettingsManager.GetMasterSettings().LogoUrl : storeImages;
				this.hdImgUrl.Value = Globals.FullPath(local);
				this.hdLink.Value = Globals.FullPath(this.Page.Request.Url.ToString());
				this.litSupplierName.SetWhenIsNotNull(storeById.StoreName);
				this.litSupplierAbout.SetWhenIsNotNull(storeById.Introduce);
			}
		}
	}
}
