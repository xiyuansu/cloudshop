using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class ReferralRegister : MemberTemplatedWebControl
	{
		private TextBox txtRealName;

		private IButton btnReferral;

		private TextBox txtAddress;

		private TextBox txtEmail;

		private TextBox txtPhone;

		private TextBox txtPhoneCode;

		private TextBox txtNumber;

		private TextBox txtShopName;

		private RegionSelector dropRegionsSelect;

		private HtmlInputHidden hidValidateRealName;

		private HtmlInputHidden hidValidatePhone;

		private HtmlInputHidden hidValidateEmail;

		private HtmlInputHidden hidValidateAddress;

		private HtmlInputHidden hidIsPromoterValidatePhone;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReferralRegister.html";
			}
			if (!string.IsNullOrEmpty(HttpContext.Current.Request["isCallback"]) && HttpContext.Current.Request["isCallback"] == "true")
			{
				string verifyCode = HttpContext.Current.Request["code"];
				string text = "";
				text = (this.CheckVerifyCode(verifyCode) ? "1" : "0");
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ContentType = "application/json";
				HttpContext.Current.Response.Write("{ ");
				HttpContext.Current.Response.Write($"\"flag\":\"{text}\"");
				HttpContext.Current.Response.Write("}");
				HttpContext.Current.Response.End();
			}
			base.OnInit(e);
		}

		private bool CheckVerifyCode(string verifyCode)
		{
			return HiContext.Current.CheckVerifyCode(verifyCode, "");
		}

		protected override void AttachChildControls()
		{
			this.txtRealName = (TextBox)this.FindControl("txtRealName");
			this.txtAddress = (TextBox)this.FindControl("txtAddress");
			this.btnReferral = ButtonManager.Create(this.FindControl("btnReferral"));
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.txtEmail = (TextBox)this.FindControl("txtEmail");
			this.txtPhone = (TextBox)this.FindControl("txtPhone");
			this.hidValidateRealName = (HtmlInputHidden)this.FindControl("hidValidateRealName");
			this.hidValidatePhone = (HtmlInputHidden)this.FindControl("hidValidatePhone");
			this.hidValidateEmail = (HtmlInputHidden)this.FindControl("hidValidateEmail");
			this.hidValidateAddress = (HtmlInputHidden)this.FindControl("hidValidateAddress");
			this.hidIsPromoterValidatePhone = (HtmlInputHidden)this.FindControl("hidIsPromoterValidatePhone");
			this.txtPhoneCode = (TextBox)this.FindControl("txtPhoneCode");
			this.txtNumber = (TextBox)this.FindControl("txtNumber");
			this.txtShopName = (TextBox)this.FindControl("txtShopName");
			this.btnReferral.Click += this.btnReferral_Click;
			PageTitle.AddSiteNameTitle("分销员申请表单");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				ReferralInfo referralInfo = Users.GetReferralInfo(HiContext.Current.UserId);
				int referralUserId = user.ReferralUserId;
				int.TryParse(HttpContext.Current.Request.QueryString["ReferralUserId"], out referralUserId);
				if (referralInfo != null && referralInfo.ReferralStatus != 2 && referralInfo.ReferralStatus == 3 && this.Page.Request.QueryString["again"] != "1" && referralUserId <= 0)
				{
					this.Page.Response.Redirect("/user/ReferralRegisterresults.aspx", true);
				}
				if (user.IsReferral())
				{
					this.Page.Response.Redirect("/user/PopularizeGift.aspx");
				}
				ReferralExtInfo referralExtInfo = new ReferralExtInfo();
				if (referralInfo != null)
				{
					referralExtInfo = MemberProcessor.GetReferralExtInfo(referralInfo.RequetReason);
					this.txtShopName.Text = referralInfo.ShopName;
				}
				string fullRegion = RegionHelper.GetFullRegion((referralExtInfo.RegionId > 0) ? referralExtInfo.RegionId : user.RegionId, " ", true, 0);
				this.txtAddress.Text = (string.IsNullOrEmpty(referralExtInfo.Address) ? user.Address : referralExtInfo.Address);
				this.txtRealName.Text = (string.IsNullOrEmpty(referralExtInfo.RealName) ? user.RealName : referralExtInfo.RealName);
				this.txtEmail.Text = (string.IsNullOrEmpty(referralExtInfo.Email) ? user.Email : referralExtInfo.Email);
				this.txtPhone.Text = (string.IsNullOrEmpty(referralExtInfo.CellPhone) ? user.CellPhone : referralExtInfo.CellPhone);
				this.dropRegionsSelect.SetSelectedRegionId(user.RegionId);
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string promoterNeedInfo = masterSettings.PromoterNeedInfo;
				if (!string.IsNullOrEmpty(promoterNeedInfo))
				{
					this.hidValidateRealName.Value = (promoterNeedInfo.Contains("1") ? "1" : "");
					this.hidValidatePhone.Value = (promoterNeedInfo.Contains("2") ? "1" : "");
					this.hidValidateEmail.Value = (promoterNeedInfo.Contains("3") ? "1" : "");
					this.hidValidateAddress.Value = (promoterNeedInfo.Contains("4") ? "1" : "");
					this.hidIsPromoterValidatePhone.Value = (masterSettings.IsPromoterValidatePhone ? "1" : "");
				}
			}
		}

		private void btnReferral_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = DataHelper.CleanSearchString(this.txtRealName.Text.Trim());
			string text2 = DataHelper.CleanSearchString(this.txtEmail.Text.Trim());
			string text3 = DataHelper.CleanSearchString(this.txtPhone.Text.Trim());
			string text4 = DataHelper.CleanSearchString(this.txtAddress.Text.Trim());
			string promoterNeedInfo = masterSettings.PromoterNeedInfo;
			string text5 = DataHelper.CleanSearchString(this.txtShopName.Text.Trim());
			string bannerUrl = "";
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (HiContext.Current.SiteSettings.ApplyReferralCondition == 1 && HiContext.Current.User.Expenditure < HiContext.Current.SiteSettings.ApplyReferralNeedAmount)
			{
				this.ShowMessage("您的消费金额还没达到系统设置的金额(" + HiContext.Current.SiteSettings.ApplyReferralNeedAmount.F2ToString("f2") + ")元", false, "", 1);
			}
			else if (string.IsNullOrEmpty(text5) || text5.Length > 100)
			{
				this.ShowMessage("请输入店铺名称,长度在1-10个之间", false, "", 1);
			}
			else if (string.IsNullOrEmpty(text) && promoterNeedInfo.Contains("1"))
			{
				this.ShowMessage("请填写真实姓名,长度在1-10个之间", false, "", 1);
			}
			else if (string.IsNullOrEmpty(text4) && promoterNeedInfo.Contains("4"))
			{
				this.ShowMessage("请填写详细地址", false, "", 1);
			}
			else
			{
				if (promoterNeedInfo.Contains("3"))
				{
					if (text2 != null && text2.Length > 0 && !DataHelper.IsEmail(text2))
					{
						this.ShowMessage("请输入正确的邮箱地址", false, "", 1);
						return;
					}
					if (text2 != null && text2.Length > 0)
					{
						MemberInfo memberInfo = MemberProcessor.FindMemberByEmail(text2);
						if (memberInfo != null && memberInfo.UserId != user.UserId)
						{
							this.ShowMessage("该邮箱地址已被其他用户使用", false, "", 1);
							return;
						}
					}
				}
				if (promoterNeedInfo.Contains("2"))
				{
					if (text3 != null && text3.Length > 0 && !DataHelper.IsMobile(text3))
					{
						this.ShowMessage("请输入正确的手机号码", false, "", 1);
						return;
					}
					if (text3 != null && text3.Length > 0)
					{
						MemberInfo memberInfo2 = MemberProcessor.FindMemberByCellphone(text3);
						if (memberInfo2 != null && memberInfo2.UserId != user.UserId)
						{
							this.ShowMessage("该手机号码已被其他用户使用", false, "", 1);
							return;
						}
						if (SettingsManager.GetMasterSettings().IsPromoterValidatePhone)
						{
							string msg = "";
							if (!HiContext.Current.CheckVerifyCode(this.txtNumber.Text.Trim(), ""))
							{
								this.ShowMessage("图形验证码错误", false, "", 1);
								return;
							}
							if (!HiContext.Current.CheckPhoneVerifyCode(this.txtPhoneCode.Text.Trim(), text3, out msg))
							{
								this.ShowMessage(msg, false, "", 1);
								return;
							}
						}
					}
				}
				int num = 0;
				int topRegionId = 0;
				if (this.hidValidateAddress.Value == "1" && this.dropRegionsSelect.GetSelectedRegionId().HasValue)
				{
					num = this.dropRegionsSelect.GetSelectedRegionId().Value;
					topRegionId = RegionHelper.GetTopRegionId(num, true);
				}
				if (MemberProcessor.ReferralRequest(HiContext.Current.UserId, text, text3, topRegionId, num, text4, text2, text5, bannerUrl))
				{
					Users.ClearUserCache(HiContext.Current.UserId, "");
					this.Page.Response.Redirect("/user/ReferralRegisterresults.aspx");
				}
				else
				{
					this.ShowMessage("申请失败，请重试", false, "", 1);
				}
			}
		}
	}
}
