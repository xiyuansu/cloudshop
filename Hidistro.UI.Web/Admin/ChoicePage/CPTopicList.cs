using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.ChoicePage
{
	[PrivilegeCheck(Privilege.TopicManager)]
	public class CPTopicList : AdminCallBackPage
	{
		protected string returnUrl;

		protected string formData;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.returnUrl = this.Page.Request["returnUrl"].ToNullString();
			this.formData = this.Page.Request["formData"].ToNullString();
		}
	}
}
