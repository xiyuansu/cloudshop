using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.Members)]
	public class EditMemberTags : AdminPage
	{
		private string userIds = string.Empty;

		private string userTagIds = string.Empty;

		private int singleUserId = 0;

		protected HiddenField hidTags;

		protected HiddenField hidUserTagIds;

		protected HiddenField hidUserId;

		protected Button btnUpdateProductTags;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpdateProductTags.Click += this.btnUpdateProductTags_Click;
			this.userIds = this.Page.Request.QueryString["userIds"];
			this.userTagIds = this.Page.Request.QueryString["userTagIds"];
			int.TryParse(this.Page.Request.QueryString["singleUserId"], out this.singleUserId);
			if (this.singleUserId > 0)
			{
				if (this.userTagIds != "," && !string.IsNullOrWhiteSpace(this.userTagIds))
				{
					this.userTagIds = "," + this.userTagIds + ",";
				}
				this.hidUserTagIds.Value = this.userTagIds;
				this.hidUserId.Value = this.singleUserId.ToString();
			}
		}

		private void btnUpdateProductTags_Click(object sender, EventArgs e)
		{
			string value = this.hidTags.Value;
			if (string.IsNullOrEmpty(this.userIds))
			{
				this.ShowMsg("请先选择要设置的会员", false);
			}
			else if (string.IsNullOrEmpty(value))
			{
				this.ShowMsg("请选择要设置的标签", false);
			}
			else
			{
				try
				{
					MemberTagHelper.UpdateMemberTags(this.userIds, value);
					string[] array = this.userIds.Split(',');
					foreach (string s in array)
					{
						int num = 0;
						int.TryParse(s, out num);
						if (num > 0)
						{
							Users.ClearUserCache(num, "");
						}
					}
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>alert('设置成功！'); art.dialog.close();</script>");
				}
				catch (Exception)
				{
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>alert('设置失败！'); art.dialog.close();</script>");
				}
			}
		}
	}
}
