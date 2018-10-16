using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Articles)]
	public class ArticleList : AdminPage
	{
		protected ArticleCategoryDropDownList dropArticleCategory;

		protected CalendarPanel calendarStartDataTime;

		protected CalendarPanel calendarEndDataTime;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropArticleCategory.DataBind();
		}
	}
}
