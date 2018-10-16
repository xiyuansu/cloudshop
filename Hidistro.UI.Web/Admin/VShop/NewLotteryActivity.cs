using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	[PrivilegeCheck(Privilege.VManageLotteryActivity)]
	public class NewLotteryActivity : AdminPage
	{
		protected int type;

		protected string ActivityName = "";

		protected int OrderStatus = 0;

		protected Literal LitTitle;

		protected HtmlAnchor addactivity;

		protected HtmlGenericControl liStoreFilter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["type"], out this.type))
			{
				this.addactivity.HRef = "AddVLotteryActivity.aspx?type=" + this.type;
				switch (this.type)
				{
				case 1:
					this.addactivity.InnerText = "添加";
					this.LitTitle.Text = "管理";
					break;
				case 2:
					this.addactivity.InnerText = "添加";
					this.LitTitle.Text = "管理";
					break;
				case 3:
					this.addactivity.InnerText = "添加";
					this.LitTitle.Text = "管理";
					break;
				}
				if (!this.Page.IsPostBack)
				{
					this.LoadParameter();
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}

		private void LoadParameter()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ActivityName"]))
			{
				this.ActivityName = Globals.UrlDecode(this.Page.Request.QueryString["ActivityName"]);
			}
			this.OrderStatus = this.Page.Request.QueryString["Stateus"].ToInt(0);
		}
	}
}
