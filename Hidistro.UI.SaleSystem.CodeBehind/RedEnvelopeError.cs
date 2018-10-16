using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class RedEnvelopeError : WAPTemplatedWebControl
	{
		private Literal literrorInfo;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-RedEnvelopeError.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.literrorInfo = (Literal)this.FindControl("ErrorInfo");
			string text = HiContext.Current.Context.Request.QueryString["errorInfo"];
			if (string.IsNullOrEmpty(text))
			{
				this.literrorInfo.Text = "未知错误";
			}
			else
			{
				this.literrorInfo.Text = text;
			}
		}
	}
}
