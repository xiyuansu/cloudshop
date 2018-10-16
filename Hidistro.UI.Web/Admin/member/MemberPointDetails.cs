using Hidistro.Core;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	public class MemberPointDetails : AdminPage
	{
		protected int userId;

		public string userName = "";

		private string type = "";

		protected Literal litUserName;

		protected Literal litCanUsePoints;

		protected Literal litHistoryPoints;

		protected SourcePointDrowpDownList dropPointSource;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				this.userId = 0;
			}
			if (this.userId <= 0)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.LoadParameter();
				if (!this.Page.IsPostBack)
				{
					this.dropPointSource.DataBind();
					if (!string.IsNullOrWhiteSpace(this.type))
					{
						this.dropPointSource.SelectedValue = this.type.ToInt(0);
					}
				}
			}
		}

		private void LoadParameter()
		{
			this.type = this.Page.Request.QueryString["type"];
			Literal literal = this.litUserName;
			string text2 = this.userName = (literal.Text = this.Page.Request.QueryString["userName"]);
			this.litCanUsePoints.Text = this.Page.Request.QueryString["points"];
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["historyPoint"]))
			{
				int historyPoints = MemberHelper.GetHistoryPoints(this.userId);
				this.litHistoryPoints.Text = historyPoints.ToString();
			}
			else
			{
				this.litHistoryPoints.Text = this.Page.Request.QueryString["historyPoint"];
			}
		}
	}
}
