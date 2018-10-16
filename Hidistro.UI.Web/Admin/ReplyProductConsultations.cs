using Hidistro.Context;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class ReplyProductConsultations : AdminCallBackPage
	{
		private int consultationId;

		protected Literal litUserName;

		protected FormatedTimeLabel lblTime;

		protected Literal litConsultationText;

		protected TextBox txtReply;

		protected Button btnReplyProductConsultation;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ConsultationId"], out this.consultationId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnReplyProductConsultation.Click += this.btnReplyProductConsultation_Click;
				if (!this.Page.IsPostBack)
				{
					ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
					if (productConsultation == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.litUserName.Text = productConsultation.UserName;
						this.litConsultationText.Text = productConsultation.ConsultationText;
						this.lblTime.Time = productConsultation.ConsultationDate;
					}
				}
			}
		}

		protected void btnReplyProductConsultation_Click(object sender, EventArgs e)
		{
			ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
			productConsultation.ReplyText = this.txtReply.Text;
			if (string.IsNullOrEmpty(productConsultation.ReplyText))
			{
				this.ShowMsg("回复内容不能为空", false);
			}
			else
			{
				productConsultation.ReplyUserId = HiContext.Current.ManagerId;
				productConsultation.ReplyDate = DateTime.Now;
				ValidationResults validationResults = Validation.Validate(productConsultation, "Reply");
				string text = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						text += Formatter.FormatErrorMessage(item.Message);
					}
					this.ShowMsg(text, false);
				}
				else if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
				{
					this.txtReply.Text = string.Empty;
					base.CloseWindow(null);
				}
				else
				{
					this.ShowMsg("回复商品咨询失败", false);
				}
			}
		}
	}
}
