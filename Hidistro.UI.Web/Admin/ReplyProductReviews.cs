using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class ReplyProductReviews : AdminCallBackPage
	{
		private int reviewId;

		public string Score;

		protected Literal litConsultationText;

		protected HtmlAnchor linkImageUrl1;

		protected Image imgImageUrl1;

		protected HtmlAnchor linkImageUrl2;

		protected Image imgImageUrl2;

		protected HtmlAnchor linkImageUrl3;

		protected Image imgImageUrl3;

		protected HtmlAnchor linkImageUrl4;

		protected Image imgImageUrl4;

		protected HtmlAnchor linkImageUrl5;

		protected Image imgImageUrl5;

		protected Literal lblReply;

		protected TextBox txtReply;

		protected Button btnReplyProductConsultation;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ReviewId"], out this.reviewId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnReplyProductConsultation.Click += this.btnReplyProductConsultation_Click;
				if (!this.Page.IsPostBack)
				{
					ProductReviewInfo productReview = ProductCommentHelper.GetProductReview(this.reviewId);
					if (productReview == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.Score = productReview.Score.ToString();
						this.litConsultationText.Text = productReview.ReviewText;
						this.lblReply.Text = productReview.ReplyText;
						if (productReview.ReplyDate.HasValue)
						{
							this.txtReply.Visible = false;
							this.btnReplyProductConsultation.Visible = false;
						}
						else
						{
							this.lblReply.Visible = false;
						}
						if (!string.IsNullOrEmpty(productReview.ImageUrl1))
						{
							this.linkImageUrl1.Visible = true;
							this.linkImageUrl1.HRef = productReview.ImageUrl1;
							this.imgImageUrl1.ImageUrl = "/Admin/PicRar.aspx?P=" + productReview.ImageUrl1 + "&W=40&H=40";
						}
						if (!string.IsNullOrEmpty(productReview.ImageUrl2))
						{
							this.linkImageUrl2.Visible = true;
							this.linkImageUrl2.HRef = productReview.ImageUrl2;
							this.imgImageUrl2.ImageUrl = "/Admin/PicRar.aspx?P=" + productReview.ImageUrl2 + "&W=40&H=40";
						}
						if (!string.IsNullOrEmpty(productReview.ImageUrl3))
						{
							this.linkImageUrl3.Visible = true;
							this.linkImageUrl3.HRef = productReview.ImageUrl3;
							this.imgImageUrl3.ImageUrl = "/Admin/PicRar.aspx?P=" + productReview.ImageUrl3 + "&W=40&H=40";
						}
						if (!string.IsNullOrEmpty(productReview.ImageUrl4))
						{
							this.linkImageUrl4.Visible = true;
							this.linkImageUrl4.HRef = productReview.ImageUrl4;
							this.imgImageUrl4.ImageUrl = "/Admin/PicRar.aspx?P=" + productReview.ImageUrl4 + "&W=40&H=40";
						}
						if (!string.IsNullOrEmpty(productReview.ImageUrl5))
						{
							this.linkImageUrl5.Visible = true;
							this.linkImageUrl5.HRef = productReview.ImageUrl5;
							this.imgImageUrl5.ImageUrl = "/Admin/PicRar.aspx?P=" + productReview.ImageUrl5 + "&W=40&H=40";
						}
					}
				}
			}
		}

		protected void btnReplyProductConsultation_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtReply.Text))
			{
				this.ShowMsg("请输入回复内容", false);
			}
			if (ProductCommentHelper.ReplyProductReview(this.reviewId, this.txtReply.Text))
			{
				this.txtReply.Text = string.Empty;
				base.CloseWindow(null);
			}
			else
			{
				this.ShowMsg("回复商品评论失败", false);
			}
		}
	}
}
