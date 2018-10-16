using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class BatchReplyProductReviews : AdminCallBackPage
	{
		private IList<long> reviewIdList = new List<long>();

		protected Literal lblReply;

		protected TextBox txtReply;

		protected Button btnReplyProductConsultation;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnReplyProductConsultation.Click += this.btnReplyProductConsultation_Click;
			string idList = base.Request.QueryString["ReviewIds"].ToNullString();
			this.reviewIdList = DataHelper.GetSafeIDList(idList, ',', true);
			if (this.reviewIdList == null || this.reviewIdList.Count == 0)
			{
				this.ShowMsg("请选择要回复的评论信息", false);
			}
		}

		protected void btnReplyProductConsultation_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtReply.Text))
			{
				this.ShowMsg("请输入回复内容", false);
			}
			if (ProductCommentHelper.BatchReplyProductReviews(this.reviewIdList, this.txtReply.Text))
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
