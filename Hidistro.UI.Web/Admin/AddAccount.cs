using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class AddAccount : AdminCallBackPage
	{
		protected ServiceTypeDropDownList dropServiceType;

		protected HiddenField hidServiceType;

		protected TextBox txtAccount;

		protected TextBox txtNickName;

		protected TextBox txtOrderId;

		protected OnOff ooShowStatus;

		protected HiddenField hidShowStatus;

		protected HiddenField hidID;

		protected Button btnSubmit;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.dropServiceType.AllowNull = false;
			this.dropServiceType.DataBind();
			this.ooShowStatus.SelectedValue = true;
			this.btnSubmit.Click += this.btnSubmit_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (this.hidServiceType.Value.ToInt(0) < 1)
			{
				this.ShowMsg("请选择客服类型", false);
			}
			else
			{
				string text = Globals.StripAllTags(this.txtAccount.Text.Trim());
				if (string.IsNullOrEmpty(text) || text.Length > 50)
				{
					this.ShowMsg("请输入客服帐号，帐号长度必须在50个字符以内", false);
				}
				else
				{
					string text2 = Globals.StripAllTags(this.txtNickName.Text.Trim());
					if (string.IsNullOrEmpty(text2) || text2.Length > 50)
					{
						this.ShowMsg("请输入显示昵称，昵称长度必须在50个字符以内", false);
					}
					else
					{
						int imageType = 1;
						int orderId = 0;
						int.TryParse(this.txtOrderId.Text, out orderId);
						OnlineServiceInfo onlineServiceInfo = new OnlineServiceInfo();
						onlineServiceInfo.Account = text;
						onlineServiceInfo.ImageType = imageType;
						onlineServiceInfo.NickName = text2;
						onlineServiceInfo.OrderId = orderId;
						onlineServiceInfo.ServiceType = this.hidServiceType.Value.ToInt(0);
						onlineServiceInfo.Status = (this.ooShowStatus.SelectedValue ? 1 : 0);
						if (OnlineServiceHelper.SaveOnlineService(onlineServiceInfo))
						{
							base.CloseWindow(null);
						}
						else
						{
							this.ShowMsg("未知错误", false);
						}
					}
				}
			}
		}
	}
}
