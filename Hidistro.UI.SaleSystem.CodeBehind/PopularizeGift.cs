using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class PopularizeGift : MemberTemplatedWebControl
	{
		private Literal litUserLink;

		private HtmlImage imgQRCode;

		private HtmlImage imgDeduct;

		private Literal litSubMemberDeduct;

		private Literal litSecondLevelDeduct;

		private Literal litThreeLevelDeduct;

		private HtmlGenericControl normalPanel;

		private HtmlGenericControl repeledPanel;

		private HtmlGenericControl divRepeledReason;

		private Literal repeledTime;

		private Literal repeledReason;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-PopularizeGift.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.normalPanel = (HtmlGenericControl)this.FindControl("normalPanel");
			this.repeledPanel = (HtmlGenericControl)this.FindControl("repeledPanel");
			this.repeledTime = (Literal)this.FindControl("repeledTime");
			this.repeledReason = (Literal)this.FindControl("repeledReason");
			this.divRepeledReason = (HtmlGenericControl)this.FindControl("divRepeledReason");
			this.litUserLink = (Literal)this.FindControl("litUserLink");
			this.imgQRCode = (HtmlImage)this.FindControl("imgQRCode");
			this.imgDeduct = (HtmlImage)this.FindControl("imgDeduct");
			this.litSubMemberDeduct = (Literal)this.FindControl("litSubMemberDeduct");
			this.litSecondLevelDeduct = (Literal)this.FindControl("litSecondLevelDeduct");
			this.litThreeLevelDeduct = (Literal)this.FindControl("litThreeLevelDeduct");
			MemberInfo user = Users.GetUser(HiContext.Current.User.UserId);
			if (!user.IsReferral())
			{
				if (user.Referral == null)
				{
					this.Page.Response.Redirect("/User/ReferralRegisterAgreement.aspx");
				}
				else
				{
					this.Page.Response.Redirect("/User/ReferralRegisterresults.aspx");
				}
			}
			if (user.Referral.IsRepeled)
			{
				this.normalPanel.Visible = false;
				this.repeledPanel.Visible = true;
				this.repeledTime.Text = user.Referral.RepelTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
				if (!string.IsNullOrEmpty(user.Referral.RepelReason.ToNullString()))
				{
					this.repeledReason.Text = user.Referral.RepelReason.ToNullString() + "<br>";
					this.divRepeledReason.Visible = true;
				}
			}
			else
			{
				this.normalPanel.Visible = true;
				this.repeledPanel.Visible = false;
				Uri url = HttpContext.Current.Request.Url;
				string text = "";
				this.litUserLink.Text = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}/ReferralAgreement.aspx", new object[3]
				{
					Globals.GetProtocal(HttpContext.Current),
					HttpContext.Current.Request.Url.Host,
					text
				}) + "?ReferralUserId=" + HiContext.Current.UserId;
				string text2 = this.Page.Request.MapPath("/Storage/master/QRCode/");
				if (Directory.Exists(text2 + "referral_" + HiContext.Current.Config.Version + "_" + user.UserId + ".png"))
				{
					this.imgQRCode.Src = "/Storage/master/QRCode/referral_" + HiContext.Current.Config.Version + "_" + user.UserId + ".png";
				}
				else
				{
					string qrCodeUrl = "/Storage/master/QRCode/referral_" + HiContext.Current.Config.Version + "_" + user.UserId + ".png";
					this.imgQRCode.Src = Globals.CreateQRCode(this.litUserLink.Text, qrCodeUrl, false, ImageFormats.Png);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				Literal literal = this.litSubMemberDeduct;
				decimal num = masterSettings.SubMemberDeduct;
				literal.Text = num.ToString() + "%";
				Literal literal2 = this.litSecondLevelDeduct;
				num = masterSettings.SecondLevelDeduct;
				literal2.Text = num.ToString() + "%";
				Literal literal3 = this.litThreeLevelDeduct;
				num = masterSettings.ThreeLevelDeduct;
				literal3.Text = num.ToString() + "%";
				if (!masterSettings.IsOpenSecondLevelCommission)
				{
					HtmlGenericControl htmlGenericControl = (HtmlGenericControl)this.FindControl("SecondDeduct");
					if (htmlGenericControl != null)
					{
						htmlGenericControl.Visible = false;
					}
					this.imgDeduct.Src = "/Templates/pccommon/images/tg_step2.jpg";
				}
				if (!masterSettings.IsOpenThirdLevelCommission)
				{
					HtmlGenericControl htmlGenericControl2 = (HtmlGenericControl)this.FindControl("ThirdDeduct");
					if (htmlGenericControl2 != null)
					{
						htmlGenericControl2.Visible = false;
					}
				}
			}
		}
	}
}
