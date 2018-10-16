using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserSendMessage : MemberTemplatedWebControl
	{
		private TextBox txtTitle;

		private TextBox txtContent;

		private IButton btnRefer;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserSendMessage.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTitle = (TextBox)this.FindControl("txtTitle");
			this.txtContent = (TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.btnRefer.Click += this.btnRefer_Click;
			if (!this.Page.IsPostBack)
			{
				this.txtTitle.Text = this.txtTitle.Text.Trim();
				this.txtContent.Text = this.txtContent.Text.Trim();
			}
		}

		private void btnRefer_Click(object sender, EventArgs e)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.txtTitle.Text) || this.txtTitle.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtContent.Text) || this.txtContent.Text.Length > 300)
			{
				text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false, "", 1);
			}
			else
			{
				MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
				messageBoxInfo.Sernder = HiContext.Current.User.UserName;
				messageBoxInfo.Accepter = "admin";
				messageBoxInfo.Title = Globals.HtmlEncode(this.txtTitle.Text.Replace("~", ""));
				string text3 = messageBoxInfo.Content = Globals.HtmlEncode(this.txtContent.Text.Replace("~", ""));
				this.txtTitle.Text = string.Empty;
				this.txtContent.Text = string.Empty;
				if (CommentBrowser.SendMessage(messageBoxInfo))
				{
					this.ShowMessage("发送信息成功", true, "", 1);
				}
				else
				{
					this.ShowMessage("发送信息失败", true, "", 1);
				}
			}
		}
	}
}
