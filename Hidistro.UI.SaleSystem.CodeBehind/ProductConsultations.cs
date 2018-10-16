using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ProductConsultations : HtmlTemplatedWebControl
	{
		private int productId = 0;

		private TextBox txtUserName;

		private TextBox txtContent;

		private IButton btnRefer;

		private HtmlInputText txtConsultationCode;

		private ProductDetailsLink prodetailsLink;

		private string verifyCodeKey = "VerifyCode";

		private bool CheckVerifyCode(string verifyCode)
		{
			return HiContext.Current.CheckVerifyCode(verifyCode, "");
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ProductConsultations.html";
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
			if (!int.TryParse(base.GetParameter("productId", false), out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.txtUserName = (TextBox)this.FindControl("txtUserName");
			this.txtContent = (TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.txtConsultationCode = (HtmlInputText)this.FindControl("txtConsultationCode");
			this.prodetailsLink = (ProductDetailsLink)this.FindControl("ProductDetailsLink1");
			this.btnRefer.Click += this.btnRefer_Click;
			if (!this.Page.IsPostBack && HiContext.Current.UserId != 0)
			{
				this.txtUserName.Text = HiContext.Current.User.UserName;
				this.btnRefer.Text = "咨询";
			}
			PageTitle.AddSiteNameTitle("商品咨询");
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
			if (productSimpleInfo != null)
			{
				this.prodetailsLink.ProductId = this.productId;
				this.prodetailsLink.ProductName = productSimpleInfo.ProductName;
			}
			this.txtConsultationCode.Value = string.Empty;
		}

		public void btnRefer_Click(object sender, EventArgs e)
		{
			ProductConsultationInfo productConsultationInfo = new ProductConsultationInfo();
			productConsultationInfo.ConsultationDate = DateTime.Now;
			productConsultationInfo.ProductId = this.productId;
			productConsultationInfo.UserId = HiContext.Current.UserId;
			productConsultationInfo.UserName = Globals.StripAllTags(this.txtUserName.Text);
			productConsultationInfo.UserEmail = ((HiContext.Current.UserId > 0) ? HiContext.Current.User.Email : string.Empty);
			productConsultationInfo.ConsultationText = Globals.StripAllTags(this.txtContent.Text);
			string empty = string.Empty;
			if (string.IsNullOrEmpty(this.txtConsultationCode.Value))
			{
				this.ShowMessage("请输入验证码", false, "", 1);
			}
			else if (!HiContext.Current.CheckVerifyCode(this.txtConsultationCode.Value.Trim(), ""))
			{
				this.ShowMessage("验证码不正确", false, "", 1);
			}
			else if (ProductBrowser.InsertProductConsultation(productConsultationInfo))
			{
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "咨询成功，管理员回复即可显示", base.GetRouteUrl("productConsultations", new
				{
					ProductId = this.productId
				})));
			}
			else
			{
				this.ShowMessage("咨询失败，请重试", false, "", 1);
			}
		}
	}
}
