using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Helps : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptHelps;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Helps.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptHelps = (ThemedTemplatedRepeater)this.FindControl("rptHelps");
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(base.GetParameter("CategoryId", false)))
				{
					int categoryId = 0;
					int.TryParse(base.GetParameter("CategoryId", false), out categoryId);
					HelpCategoryInfo helpCategory = CommentBrowser.GetHelpCategory(categoryId);
					if (helpCategory != null)
					{
						PageTitle.AddSiteNameTitle(helpCategory.Name);
					}
				}
				else
				{
					PageTitle.AddSiteNameTitle("帮助中心");
				}
				this.BindList();
			}
		}

		private void BindList()
		{
			HelpQuery helpQuery = this.GetHelpQuery();
			DbQueryResult dbQueryResult = new DbQueryResult();
			dbQueryResult = CommentBrowser.GetHelpList(helpQuery);
			this.rptHelps.DataSource = dbQueryResult.Data;
			this.rptHelps.DataBind();
			this.pager.TotalRecords = dbQueryResult.TotalRecords;
		}

		private HelpQuery GetHelpQuery()
		{
			HelpQuery helpQuery = new HelpQuery();
			if (!string.IsNullOrEmpty(base.GetParameter("categoryId", false)))
			{
				int value = 0;
				if (int.TryParse(base.GetParameter("categoryId", false), out value))
				{
					helpQuery.CategoryId = value;
				}
			}
			helpQuery.PageIndex = this.pager.PageIndex;
			helpQuery.PageSize = this.pager.PageSize;
			helpQuery.SortBy = "AddedDate";
			helpQuery.SortOrder = SortAction.Desc;
			return helpQuery;
		}
	}
}
