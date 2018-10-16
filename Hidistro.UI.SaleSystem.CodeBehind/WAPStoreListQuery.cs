using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPStoreListQuery : WAPTemplatedWebControl
	{
		private Literal literlitNoMatchSwitchal;

		private Literal litTag;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-StoreListQuery.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			base.CheckOpenMultStore();
			this.literlitNoMatchSwitchal = (Literal)this.FindControl("litNoMatchSwitch");
			this.litTag = (Literal)this.FindControl("litTag");
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagId"]))
			{
				StoreTagInfo storeTagInfo = (from t in StoreListHelper.GetTagsList()
				where t.TagId == this.Page.Request.QueryString["TagId"].ToInt(0)
				select t).FirstOrDefault();
				if (storeTagInfo != null)
				{
					this.litTag.Text = (from t in StoreListHelper.GetTagsList()
					where t.TagId == this.Page.Request.QueryString["TagId"].ToInt(0)
					select t).FirstOrDefault().TagName;
				}
			}
		}
	}
}
