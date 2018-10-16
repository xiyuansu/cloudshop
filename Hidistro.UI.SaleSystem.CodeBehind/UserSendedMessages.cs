using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserSendedMessages : MemberTemplatedWebControl
	{
		private Common_Messages_UserSendedMessageList CmessagesList;

		private Pager pager;

		private IButton btnDeleteSelect;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserSendedMessages.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.CmessagesList = (Common_Messages_UserSendedMessageList)this.FindControl("Common_Messages_UserSendedMessageList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
			this.CmessagesList.ItemCommand += this.CmessagesList_ItemCommand;
			this.btnDeleteSelect.Click += this.btnDeleteSelect_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		private void btnDeleteSelect_Click(object sender, EventArgs e)
		{
			IList<long> list = new List<long>();
			string text = this.Page.Request["CheckBoxGroup"];
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(',');
				foreach (string value in array)
				{
					list.Add(Convert.ToInt64(value));
				}
				if (list.Count > 0)
				{
					CommentBrowser.DeleteMemberMessages(list);
					this.BindData();
				}
			}
			else
			{
				this.ShowMessage("请选中要删除的信息", false, "", 1);
			}
		}

		private void CmessagesList_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			IList<long> list = new List<long>();
			if (e.CommandName == "Delete")
			{
				list.Add(Convert.ToInt64(e.CommandArgument));
				CommentBrowser.DeleteMemberMessages(list);
				this.BindData();
			}
		}

		private void BindData()
		{
			MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
			messageBoxQuery.PageIndex = this.pager.PageIndex;
			messageBoxQuery.PageSize = this.pager.PageSize;
			messageBoxQuery.Sernder = HiContext.Current.User.UserName;
			DbQueryResult memberSendedMessages = CommentBrowser.GetMemberSendedMessages(messageBoxQuery);
			this.CmessagesList.DataSource = memberSendedMessages.Data;
			this.CmessagesList.DataBind();
			this.pager.TotalRecords = memberSendedMessages.TotalRecords;
		}
	}
}
