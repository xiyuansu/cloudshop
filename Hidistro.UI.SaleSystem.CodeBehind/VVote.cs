using Hidistro.Core;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class VVote : VActivityidTemplatedWebControl
	{
		private int voteId;

		private int voteNum = 0;

		private Literal litVoteName;

		private Literal litVoteNum;

		private WapTemplatedRepeater rptVoteItems;

		private HtmlInputHidden hidCheckNum;

		private HtmlGenericControl divVoteOk;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VVote.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["voteId"], out this.voteId))
			{
				base.GotoResourceNotFound("");
			}
			this.litVoteName = (Literal)this.FindControl("litVoteName");
			this.litVoteNum = (Literal)this.FindControl("litVoteNum");
			this.rptVoteItems = (WapTemplatedRepeater)this.FindControl("rptVoteItems");
			this.hidCheckNum = (HtmlInputHidden)this.FindControl("hidCheckNum");
			this.divVoteOk = (HtmlGenericControl)this.FindControl("divVoteOk");
			string empty = string.Empty;
			int num = 1;
			DataTable vote = VshopBrowser.GetVote(this.voteId, out empty, out num, out this.voteNum);
			if (vote == null)
			{
				base.GotoResourceNotFound("");
			}
			this.LoadVoteItemTable(vote);
			this.rptVoteItems.DataSource = vote;
			this.rptVoteItems.DataBind();
			this.litVoteName.Text = empty;
			this.hidCheckNum.Value = num.ToString();
			this.litVoteNum.Text = $"共有{this.voteNum}人参与投票";
			if (VshopBrowser.IsVote(this.voteId))
			{
				Literal literal = this.litVoteNum;
				literal.Text += "(您已投票)";
				this.divVoteOk.Visible = false;
			}
			PageTitle.AddSiteNameTitle("投票调查");
		}

		private void LoadVoteItemTable(DataTable table)
		{
			table.Columns.Add("Lenth");
			table.Columns.Add("Percentage");
			foreach (DataRow row in table.Rows)
			{
				row["Lenth"] = this.GetVoteItemCountString((int)row["ItemCount"]);
				if (this.voteNum != 0)
				{
					row["Percentage"] = (decimal.Parse(row["ItemCount"].ToString()) * 100m / decimal.Parse(this.voteNum.ToString())).F2ToString("f2");
				}
				else
				{
					row["Percentage"] = 0.0;
				}
			}
		}

		private string GetVoteItemCountString(int num)
		{
			string text = string.Empty;
			if (this.voteNum != 0)
			{
				int num2 = num * 30 / this.voteNum;
				for (int i = 0; i < num2; i++)
				{
					text += "&nbsp;";
				}
			}
			return text;
		}
	}
}
