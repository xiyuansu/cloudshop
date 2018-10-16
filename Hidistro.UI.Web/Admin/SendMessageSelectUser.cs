using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddMessage)]
	public class SendMessageSelectUser : AdminPage
	{
		private int userId = 0;

		protected HtmlInputRadioButton rdoName;

		protected HtmlInputRadioButton rdoRank;

		protected TextBox txtMemberNames;

		protected MemberGradeDropDownList rankList;

		protected Button btnSendToRank;

		public string MessageTitle
		{
			get
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Title"];
				if (httpCookie != null)
				{
					return Globals.UrlDecode(httpCookie.Value);
				}
				return string.Empty;
			}
		}

		public string Content
		{
			get
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Content"];
				if (httpCookie != null)
				{
					return Globals.UrlDecode(httpCookie.Value);
				}
				return string.Empty;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnSendToRank.Click += this.btnSendToRank_Click;
				if (!this.Page.IsPostBack)
				{
					this.rankList.DataBind();
					if (this.userId > 0)
					{
						MemberInfo user = Users.GetUser(this.userId);
						if (user == null)
						{
							base.GotoResourceNotFound();
						}
						else
						{
							this.txtMemberNames.Text = user.UserName;
						}
					}
				}
			}
		}

		private void btnSendToRank_Click(object sender, EventArgs e)
		{
			IList<MessageBoxInfo> list = new List<MessageBoxInfo>();
			if (this.rdoName.Checked && !string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
			{
				string text = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n");
				string[] array = text.Replace("\n", "*").Split('*');
				for (int i = 0; i < array.Length; i++)
				{
					MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(array[i]);
					if (memberInfo != null)
					{
						MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
						messageBoxInfo.Accepter = array[i];
						messageBoxInfo.Sernder = "admin";
						messageBoxInfo.Title = this.MessageTitle;
						messageBoxInfo.Content = this.Content;
						list.Add(messageBoxInfo);
					}
				}
				if (list.Count > 0)
				{
					NoticeHelper.SendMessageToMember(list);
					this.ShowMsg($"成功给{list.Count}个用户发送了消息.", true, "SendMessageSelectUser.aspx");
				}
				else
				{
					this.ShowMsg("没有要发送的对象", true, "SendMessageSelectUser.aspx");
				}
			}
			if (this.rdoRank.Checked)
			{
				IList<string> membersByRank = NoticeHelper.GetMembersByRank(this.rankList.SelectedValue);
				foreach (string item in membersByRank)
				{
					MessageBoxInfo messageBoxInfo2 = new MessageBoxInfo();
					messageBoxInfo2.Accepter = item;
					messageBoxInfo2.Sernder = "admin";
					messageBoxInfo2.Title = this.MessageTitle;
					messageBoxInfo2.Content = this.Content;
					list.Add(messageBoxInfo2);
				}
				if (list.Count > 0)
				{
					NoticeHelper.SendMessageToMember(list);
					this.ShowMsg($"成功给{list.Count}个用户发送了消息.", true, "SendMessageSelectUser.aspx");
				}
				else
				{
					this.ShowMsg("没有要发送的对象", true, "SendMessageSelectUser.aspx");
				}
			}
		}
	}
}
