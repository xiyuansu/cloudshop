using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class EditReplyOnKey : AdminPage
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
				int urlIntParam = base.GetUrlIntParam("id");
				TextReplyInfo textReplyInfo = ReplyHelper.GetReply(urlIntParam) as TextReplyInfo;
				if (textReplyInfo == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.fcContent.Text = textReplyInfo.Text;
					this.txtKeys.Text = textReplyInfo.Keys;
					this.radMatch.SelectedValue = (textReplyInfo.MatchType == MatchType.Like);
					this.radDisable.SelectedValue = !textReplyInfo.IsDisable;
					this.chkKeys.Checked = (ReplyType.Keys == (textReplyInfo.ReplyType & ReplyType.Keys));
					this.chkSub.Checked = (ReplyType.Subscribe == (textReplyInfo.ReplyType & ReplyType.Subscribe));
					this.chkNo.Checked = (ReplyType.NoMatch == (textReplyInfo.ReplyType & ReplyType.NoMatch));
					if (this.chkNo.Checked)
					{
						this.chkNo.Enabled = true;
						this.chkNo.ToolTip = "";
					}
					if (this.chkSub.Checked)
					{
						this.chkSub.Enabled = true;
						this.chkSub.ToolTip = "";
					}
				}
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			int urlIntParam = base.GetUrlIntParam("id");
			TextReplyInfo textReplyInfo = ReplyHelper.GetReply(urlIntParam) as TextReplyInfo;
			if (!string.IsNullOrEmpty(this.txtKeys.Text) && textReplyInfo.Keys != this.txtKeys.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
			}
			else
			{
				textReplyInfo.IsDisable = !this.radDisable.SelectedValue;
				if (this.chkKeys.Checked && !string.IsNullOrWhiteSpace(this.txtKeys.Text))
				{
					textReplyInfo.Keys = this.txtKeys.Text.Trim();
				}
				else
				{
					textReplyInfo.Keys = string.Empty;
				}
				textReplyInfo.Text = this.fcContent.Text.Trim();
				textReplyInfo.MatchType = (this.radMatch.SelectedValue ? MatchType.Like : MatchType.Equal);
				textReplyInfo.ReplyType = ReplyType.None;
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
				else if (ReplyHelper.UpdateReply(textReplyInfo))
				{
					this.ShowMsg("修改成功", true);
				}
				else
				{
					this.ShowMsg("修改失败", false);
				}
			}
		}
	}
}
