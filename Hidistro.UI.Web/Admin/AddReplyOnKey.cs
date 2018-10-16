using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AddReplyOnKey : AdminPage
	{
		protected TextBox fcContent;

		protected CheckBox chkKeys;

		protected CheckBox chkSub;

		protected CheckBox chkNo;

		protected TextBox txtKeys;

		protected YesNoRadioButtonList radMatch;

		protected YesNoRadioButtonList radDisable;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnSubmit_Click;
			this.radMatch.Items[0].Text = "模糊匹配";
			this.radMatch.Items[1].Text = "精确匹配";
			this.radDisable.Items[0].Text = "启用";
			this.radDisable.Items[1].Text = "禁用";
			this.chkNo.Enabled = (ReplyHelper.GetMismatchReply() == null);
			this.chkSub.Enabled = (ReplyHelper.GetSubscribeReply() == null);
			if (!this.chkNo.Enabled)
			{
				this.chkNo.ToolTip = "该类型已被使用";
			}
			if (!this.chkSub.Enabled)
			{
				this.chkSub.ToolTip = "该类型已被使用";
			}
			if (!base.IsPostBack)
			{
				this.chkKeys.Checked = true;
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.txtKeys.Text) && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
			}
			else
			{
				TextReplyInfo textReplyInfo = textReplyInfo = new TextReplyInfo();
				textReplyInfo.IsDisable = !this.radDisable.SelectedValue;
				if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
				{
					textReplyInfo.Keys = this.txtKeys.Text.Trim();
				}
				textReplyInfo.Text = this.fcContent.Text.Trim();
				textReplyInfo.Content = this.fcContent.Text.Trim();
				textReplyInfo.MatchType = (this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal);
				if (this.chkKeys.Checked)
				{
					textReplyInfo.ReplyType |= ReplyType.Keys;
				}
				if (this.chkSub.Checked)
				{
					textReplyInfo.ReplyType |= ReplyType.Subscribe;
				}
				if (this.chkNo.Checked)
				{
					textReplyInfo.ReplyType |= ReplyType.NoMatch;
				}
				if (textReplyInfo.ReplyType == ReplyType.None)
				{
					this.ShowMsg("请选择回复类型", false);
				}
				else if (ReplyHelper.SaveReply(textReplyInfo))
				{
					this.ShowMsg("添加成功", true);
				}
				else
				{
					this.ShowMsg("添加失败", false);
				}
			}
		}
	}
}
