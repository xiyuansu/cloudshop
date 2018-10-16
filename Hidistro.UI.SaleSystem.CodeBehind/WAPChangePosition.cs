using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Helper;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPChangePosition : WAPTemplatedWebControl
	{
		private HtmlInputText txtKeyWord;

		private HtmlInputHidden hdQQMapKey;

		private HtmlGenericControl spanCityName;

		private HtmlGenericControl divCurrentPosition;

		private WapTemplatedRepeater rptShippingAddress;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ChangePosition.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = HttpUtility.UrlDecode(this.Page.Request.QueryString["cityName"]);
			this.txtKeyWord = (HtmlInputText)this.FindControl("txtKeyWord");
			this.hdQQMapKey = (HtmlInputHidden)this.FindControl("hdQQMapKey");
			this.spanCityName = (HtmlGenericControl)this.FindControl("spanCityName");
			this.divCurrentPosition = (HtmlGenericControl)this.FindControl("divCurrentPosition");
			this.rptShippingAddress = (WapTemplatedRepeater)this.FindControl("rptShippingAddress");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hdQQMapKey.Value = (string.IsNullOrEmpty(masterSettings.QQMapAPIKey) ? "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP" : masterSettings.QQMapAPIKey);
			if (!string.IsNullOrWhiteSpace(text))
			{
				this.spanCityName.InnerText = HttpUtility.UrlDecode(text);
				this.divCurrentPosition.Visible = false;
			}
			else
			{
				int currentRegionId = WebHelper.GetCookie("UserCoordinateCookie", "RegionId").ToInt(0);
				string cookie = WebHelper.GetCookie("UserCoordinateCookie", "CityName");
				string cookie2 = WebHelper.GetCookie("UserCoordinateCookie", "Address");
				this.txtKeyWord.Value = HttpUtility.UrlDecode(cookie2);
				string fullRegion = RegionHelper.GetFullRegion(currentRegionId, string.Empty, true, 0);
				this.spanCityName.InnerText = HttpUtility.UrlDecode(cookie);
			}
			if (string.IsNullOrWhiteSpace(this.spanCityName.InnerText))
			{
				this.spanCityName.InnerText = "选择城市";
			}
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(true);
			if (shippingAddresses != null)
			{
				this.rptShippingAddress.DataSource = shippingAddresses;
				this.rptShippingAddress.DataBind();
			}
		}
	}
}
