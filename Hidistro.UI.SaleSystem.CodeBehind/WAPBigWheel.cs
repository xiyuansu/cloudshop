using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPBigWheel : WAPTemplatedWebControl
	{
		private int activityid;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		private HtmlInputHidden hdIsLogined;

		private HtmlInputHidden hidPoint;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BigWheel.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = this.Page.Request["ActivityId"].ToInt(0);
			if (num <= 0)
			{
				base.GotoResourceNotFound("活动不存在");
			}
			else
			{
				this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
				this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
				this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
				this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
				this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
				this.hdIsLogined = (HtmlInputHidden)this.FindControl("hdIsLogined");
				this.hdIsLogined.Value = ((HiContext.Current.UserId == 0) ? "0" : "1");
				this.hidPoint = (HtmlInputHidden)this.FindControl("hidPoint");
				ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(num);
				if (activityInfo == null || activityInfo.ActivityType != 1)
				{
					base.GotoResourceNotFound("活动不存在");
				}
				else
				{
					if (HiContext.Current.User != null)
					{
						this.hidPoint.Value = HiContext.Current.User.Points.ToString();
					}
					if (base.ClientType == ClientType.VShop)
					{
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						string local = (!string.IsNullOrWhiteSpace(activityInfo.SharePic)) ? activityInfo.SharePic : string.Format("http://{0}/{1}", masterSettings.SiteUrl, "Templates/common/images/bigwheelShare.jpg");
						this.hdImgUrl.Value = Globals.FullPath(local);
						this.hdTitle.Value = activityInfo.ActivityName;
						this.hdDesc.Value = activityInfo.ShareDetail;
						this.hdLink.Value = Globals.FullPath($"/vshop/BigWheel.aspx?ActivityId={num}");
						this.hdAppId.Value = masterSettings.WeixinAppId;
					}
					PageTitle.AddSiteNameTitle(activityInfo.ActivityName);
				}
			}
		}
	}
}
