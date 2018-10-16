using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserConsultationsNotReverted : MemberTemplatedWebControl
	{
		private ThemedTemplatedRepeater dlstPtConsultationReply;

		private Pager pagerConsultationReply;

		private Literal litNotReverted;

		private Literal litreply;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserConsultationsNotReverted.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dlstPtConsultationReply = (ThemedTemplatedRepeater)this.FindControl("dlstPtConsultationReply");
			this.pagerConsultationReply = (Pager)this.FindControl("pagerConsultationReply");
			this.litNotReverted = (Literal)this.FindControl("litNotReverted");
			this.litreply = (Literal)this.FindControl("litreply");
			PageTitle.AddSiteNameTitle("咨询/未回复");
			if (!this.Page.IsPostBack)
			{
				this.BindPtConsultationReply();
			}
		}

		private void BindPtConsultationReply()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pagerConsultationReply.PageIndex;
			productConsultationAndReplyQuery.UserId = HiContext.Current.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.NoReply;
			DbQueryResult productConsultationsAndReplys = ProductBrowser.GetProductConsultationsAndReplys(productConsultationAndReplyQuery);
			this.dlstPtConsultationReply.DataSource = productConsultationsAndReplys.Data;
			this.dlstPtConsultationReply.DataBind();
			this.pagerConsultationReply.TotalRecords = productConsultationsAndReplys.TotalRecords;
			Literal literal = this.litNotReverted;
			int totalRecords = productConsultationsAndReplys.TotalRecords;
			literal.Text = totalRecords.ToString();
			productConsultationAndReplyQuery.UserId = HiContext.Current.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.Replyed;
			DbQueryResult productConsultationsAndReplys2 = ProductBrowser.GetProductConsultationsAndReplys(productConsultationAndReplyQuery);
			Literal literal2 = this.litreply;
			totalRecords = productConsultationsAndReplys2.TotalRecords;
			literal2.Text = totalRecords.ToString();
		}
	}
}
