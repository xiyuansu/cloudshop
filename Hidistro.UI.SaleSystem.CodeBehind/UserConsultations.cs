using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserConsultations : MemberTemplatedWebControl
	{
		private ThemedTemplatedRepeater dlstPtConsultationReplyed;

		private Pager pagerConsultationReplyed;

		private Literal litreply;

		private Literal litNotReverted;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserConsultations.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dlstPtConsultationReplyed = (ThemedTemplatedRepeater)this.FindControl("dlstPtConsultationReplyed");
			this.pagerConsultationReplyed = (Pager)this.FindControl("pagerConsultationReplyed");
			this.litreply = (Literal)this.FindControl("litreply");
			this.litNotReverted = (Literal)this.FindControl("litNotReverted");
			PageTitle.AddSiteNameTitle("咨询/已回复");
			if (!this.Page.IsPostBack)
			{
				this.BindPtConsultationReplyed();
			}
		}

		private void BindPtConsultationReplyed()
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.PageIndex = this.pagerConsultationReplyed.PageIndex;
			productConsultationAndReplyQuery.UserId = HiContext.Current.UserId;
			productConsultationAndReplyQuery.Type = ConsultationReplyType.Replyed;
			DbQueryResult productConsultationsAndReplys = ProductBrowser.GetProductConsultationsAndReplys(productConsultationAndReplyQuery);
			this.dlstPtConsultationReplyed.DataSource = productConsultationsAndReplys.Data;
			this.dlstPtConsultationReplyed.DataBind();
			this.pagerConsultationReplyed.TotalRecords = productConsultationsAndReplys.TotalRecords;
			Literal literal = this.litreply;
			int totalRecords = productConsultationsAndReplys.TotalRecords;
			literal.Text = totalRecords.ToString();
			productConsultationAndReplyQuery.Type = ConsultationReplyType.NoReply;
			DbQueryResult productConsultationsAndReplys2 = ProductBrowser.GetProductConsultationsAndReplys(productConsultationAndReplyQuery);
			Literal literal2 = this.litNotReverted;
			totalRecords = productConsultationsAndReplys2.TotalRecords;
			literal2.Text = totalRecords.ToString();
		}
	}
}
