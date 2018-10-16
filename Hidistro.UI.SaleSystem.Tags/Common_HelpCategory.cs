using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_HelpCategory : ThemedTemplatedRepeater
	{
		protected override void OnInit(EventArgs e)
		{
			base.DataSource = CommentBrowser.GetHelpCategories();
			base.DataBind();
		}
	}
}
