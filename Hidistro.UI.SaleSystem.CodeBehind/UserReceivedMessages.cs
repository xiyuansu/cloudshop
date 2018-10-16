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
	public class UserReceivedMessages : MemberTemplatedWebControl
	{
		private Common_Messages_UserReceivedMessageList CmessagesList;

		private Repeater repeaterMessageList;

		private Pager pager;

		private IButton btnDeleteSelect;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReceivedMessages.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.CmessagesList = (Common_Messages_UserReceivedMessageList)this.FindControl("Grid_Common_Messages_UserReceivedMessageList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
			this.btnDeleteSelect.Click += this.btnDeleteSelect_Click;
			this.CmessagesList.ItemCommand += this.CmessagesList_ItemCommand;
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("收件箱");
				this.BindData();
			}
		}

		private void CmessagesList_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				IList<long> list = new List<long>();
				list.Add(Convert.ToInt64(e.CommandArgument));
				CommentBrowser.DeleteMemberMessages(list);
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
				}
			}
			else
			{
				this.ShowMessage("请选中要删除的收件", false, "", 1);
			}
			this.BindData();
		}

		private void BindData()
		{
			MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
			messageBoxQuery.PageIndex = this.pager.PageIndex;
			messageBoxQuery.PageSize = this.pager.PageSize;
			messageBoxQuery.Accepter = HiContext.Current.User.UserName;
			DbQueryResult memberReceivedMessages = CommentBrowser.GetMemberReceivedMessages(messageBoxQuery);
			if (memberReceivedMessages.Data.Rows.Count <= 0)
			{
				memberReceivedMessages = CommentBrowser.GetMemberReceivedMessages(messageBoxQuery);
			}
			this.CmessagesList.DataSource = memberReceivedMessages.Data;
			this.CmessagesList.DataBind();
			this.pager.TotalRecords = memberReceivedMessages.TotalRecords;
		}
	}
}
