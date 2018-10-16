using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Store;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppletMessageTemplate)]
	public class AppletTemplateSetting : Page
	{
		protected Repeater grdWxTempletsNew;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.grdWxTempletsNew.DataSource = MessageTemplateHelper.GetWxAppletMessageTemplates();
				this.grdWxTempletsNew.DataBind();
			}
		}
	}
}
