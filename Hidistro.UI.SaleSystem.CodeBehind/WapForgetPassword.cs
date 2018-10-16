using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapForgetPassword : WAPTemplatedWebControl
	{
		private HtmlInputText txtCellPhone;

		private Button btnNext;

		private Literal message;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-ForgetPassword.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtCellPhone = (HtmlInputText)this.FindControl("txtCellPhoneOrEmail");
			this.btnNext = (Button)this.FindControl("btnNext");
			this.message = (Literal)this.FindControl("message");
			if (this.btnNext != null)
			{
				this.btnNext.Click += this.btnNext_Click;
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			HttpCookie httpCookie = new HttpCookie("ChangPassword");
			httpCookie.HttpOnly = true;
			httpCookie.Value = HiCryptographer.Encrypt(this.txtCellPhone.Value.Trim());
			httpCookie.Expires = DateTime.Now.AddMinutes(20.0);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
			this.Page.Response.Redirect("ChangePassword.aspx", true);
		}
	}
}
