using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Register : HtmlTemplatedWebControl
	{
		private CheckBox chkAgree;

		private TextBox txtUserName;

		private TextBox txtPassword;

		private TextBox txtPassword2;

		private TextBox txtEmail;

		private TextBox txtCellPhone;

		private TextBox txtNumber;

		private Literal nameTitle;

		private HiddenField hidIsOpenGeetest;

		private HtmlGenericControl divRealName;

		private HtmlGenericControl divBirthday;

		private HtmlGenericControl divSex;

		private HtmlGenericControl divGeetest;

		private HtmlGenericControl divimgcode;

		private HiddenField hidIsValidateEmail;

		private string verifyCodeKey = "VerifyCode";

		private bool CheckVerifyCode(string verifyCode)
		{
			return HiContext.Current.CheckVerifyCode(verifyCode, "");
		}

		protected override void OnInit(EventArgs e)
		{
			if (HiContext.Current.ReferralUserId <= 0)
			{
				goto IL_0014;
			}
			goto IL_0014;
			IL_0014:
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Register.html";
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

		protected override void AttachChildControls()
		{
			this.chkAgree = (CheckBox)this.FindControl("chkAgree");
			this.txtUserName = (TextBox)this.FindControl("txtUserName");
			this.txtPassword = (TextBox)this.FindControl("txtPassword");
			this.txtPassword2 = (TextBox)this.FindControl("txtPassword2");
			this.txtEmail = (TextBox)this.FindControl("txtEmail");
			this.txtCellPhone = (TextBox)this.FindControl("txtCellPhone");
			this.txtNumber = (TextBox)this.FindControl("txtNumber");
			this.nameTitle = (Literal)this.FindControl("NameTitle");
			this.divRealName = (HtmlGenericControl)this.FindControl("divRealName");
			this.divBirthday = (HtmlGenericControl)this.FindControl("divBirthday");
			this.divSex = (HtmlGenericControl)this.FindControl("divSex");
			this.divGeetest = (HtmlGenericControl)this.FindControl("divGeetest");
			this.divimgcode = (HtmlGenericControl)this.FindControl("divimgcode");
			this.hidIsOpenGeetest = (HiddenField)this.FindControl("hidIsOpenGeetest");
			this.hidIsValidateEmail = (HiddenField)this.FindControl("hidIsValidateEmail");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidIsValidateEmail.Value = (masterSettings.IsNeedValidEmail ? "1" : "0");
			if (masterSettings.RegistExtendInfo.Contains("RealName"))
			{
				this.divRealName.Visible = true;
			}
			if (masterSettings.RegistExtendInfo.Contains("Sex"))
			{
				this.divSex.Visible = true;
			}
			if (masterSettings.RegistExtendInfo.Contains("Birthday"))
			{
				this.divBirthday.Visible = true;
			}
			Regex regex = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);
			if (masterSettings.IsSurportEmail && masterSettings.IsSurportPhone && masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings))
			{
				this.nameTitle.Text = "邮箱/手机";
			}
			else if (!masterSettings.IsSurportEmail && (!masterSettings.IsSurportPhone || !masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings)))
			{
				this.nameTitle.Text = "邮箱";
			}
			else if (masterSettings.IsSurportEmail && (!masterSettings.IsSurportPhone || !masterSettings.SMSEnabled || string.IsNullOrEmpty(masterSettings.SMSSettings)))
			{
				this.nameTitle.Text = "邮箱";
			}
			else if (!masterSettings.IsSurportEmail && masterSettings.IsSurportPhone && masterSettings.SMSEnabled && !string.IsNullOrEmpty(masterSettings.SMSSettings))
			{
				this.nameTitle.Text = "手机";
			}
			if (!masterSettings.IsOpenGeetest)
			{
				this.divGeetest.Style.Add("display", "none");
				this.divimgcode.Style.Add("display", "block");
			}
			else
			{
				this.divGeetest.Style.Add("display", "block");
				this.divimgcode.Style.Add("display", "none");
			}
			this.hidIsOpenGeetest.Value = (masterSettings.IsOpenGeetest ? "1" : "0");
			PageTitle.AddSiteNameTitle("会员注册");
		}
	}
}
