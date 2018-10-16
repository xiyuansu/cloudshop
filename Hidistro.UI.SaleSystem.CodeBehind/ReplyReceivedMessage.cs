using Hidistro.Context;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ReplyReceivedMessage : MemberTemplatedWebControl
	{
		private Literal litAddresser;

		private Literal litTitle;

		private FormatedTimeLabel litDate;

		private TextBox txtReplyTitle;

		private TextBox txtReplyContent;

		private HtmlTextArea txtReplyRecord;

		private Button btnReplyReceivedMessage;

		private long messageId = 0L;

		private string messagecontent = string.Empty;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReplyReceivedMessage.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litAddresser = (Literal)this.FindControl("litAddresser");
			this.litTitle = (Literal)this.FindControl("litTitle");
			this.litDate = (FormatedTimeLabel)this.FindControl("litDate");
			this.txtReplyTitle = (TextBox)this.FindControl("txtReplyTitle");
			this.txtReplyContent = (TextBox)this.FindControl("txtReplyContent");
			this.txtReplyRecord = (HtmlTextArea)this.FindControl("txtReplyRecord");
			this.btnReplyReceivedMessage = (Button)this.FindControl("btnReplyReceivedMessage");
			this.btnReplyReceivedMessage.Click += this.btnReplyReceivedMessage_Click;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MessageId"]))
			{
				this.messageId = long.Parse(this.Page.Request.QueryString["MessageId"]);
			}
			if (!this.Page.IsPostBack)
			{
				CommentBrowser.PostMemberMessageIsRead(this.messageId);
				MessageBoxInfo memberMessage = CommentBrowser.GetMemberMessage(this.messageId);
				if (memberMessage != null)
				{
					this.litAddresser.Text = "管理员";
					this.litTitle.Text = memberMessage.Title;
					this.txtReplyRecord.Value = memberMessage.Content;
					this.litDate.Time = memberMessage.Date;
				}
			}
		}

		private void btnReplyReceivedMessage_Click(object sender, EventArgs e)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.txtReplyTitle.Text) || this.txtReplyTitle.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtReplyContent.Text) || this.txtReplyContent.Text.Length > 300)
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
				messageBoxInfo.Title = this.txtReplyTitle.Text.Trim();
				string format = "\n\n时间：{0}\t发件人：{1}\n标题:{2}\n内容：{3}\n";
				string str = string.Format(format, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), messageBoxInfo.Sernder, messageBoxInfo.Title, this.txtReplyContent.Text.Trim());
				messageBoxInfo.Content = str + this.txtReplyRecord.Value;
				if (CommentBrowser.SendMessage(messageBoxInfo))
				{
					this.ShowMessage("回复成功", true, "", 1);
				}
				else
				{
					this.ShowMessage("回复失败", false, "", 1);
				}
			}
		}
	}
}
