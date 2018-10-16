using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductReviewsManage)]
	public class ReplyReceivedMessages : AdminPage
	{
		private long messageId;

		protected Literal litTitle;

		protected HtmlTextArea txtContent;

		protected TextBox txtTitle;

		protected HtmlGenericControl txtTitleTip;

		protected HtmlTextArea txtContes;

		protected HtmlGenericControl txtContesTip;

		protected Button btnReplyReplyReceivedMessages;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["MessageId"], out this.messageId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnReplyReplyReceivedMessages.Click += this.btnReplyReplyReceivedMessages_Click;
				if (!this.Page.IsPostBack)
				{
					MessageBoxInfo managerMessage = NoticeHelper.GetManagerMessage(this.messageId);
					this.litTitle.Text = managerMessage.Title;
					this.txtContent.Value = managerMessage.Content;
					this.ViewState["Sernder"] = managerMessage.Sernder;
				}
			}
		}

		protected void btnReplyReplyReceivedMessages_Click(object sender, EventArgs e)
		{
			IList<MessageBoxInfo> list = new List<MessageBoxInfo>();
			MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
			messageBoxInfo.Accepter = (string)this.ViewState["Sernder"];
			messageBoxInfo.Sernder = "admin";
			messageBoxInfo.Title = this.txtTitle.Text.Trim();
			string format = "\n\n时间：{0}\t发件人：{1}\n标题:{2}\n内容：{3}\n";
			string str = string.Format(format, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "管理员", messageBoxInfo.Title, this.txtContes.Value.Trim());
			messageBoxInfo.Content = str + this.txtContent.Value;
			list.Add(messageBoxInfo);
			if (NoticeHelper.SendMessageToMember(list) > 0)
			{
				NoticeHelper.PostManagerMessageIsRead(this.messageId);
				this.ShowMsg("成功回复了会员的站内信.", true);
			}
			else
			{
				this.ShowMsg("回复会员的站内信失败.", false);
			}
		}
	}
}
