using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AddOnlineService : AdminPage
	{
		protected ServiceTypeDropDownList dropServiceType;

		protected HtmlGenericControl dropServiceTypeTip;

		protected TextBox txtAccount;

		protected HtmlGenericControl txtAccountTip;

		protected TextBox txtNickName;

		protected HtmlGenericControl txtNickNameTip;

		protected TextBox txtOrderId;

		protected HtmlGenericControl txtOrderIdTip;

		protected TextBox txtImageType;

		protected OnOff radioShowStatus;

		protected HtmlGenericControl radioShowStatusTip;

		protected Button btnSubmit;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.dropServiceType.DataBind();
			}
			this.btnSubmit.Click += this.btnSubmit_Click;
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (!this.dropServiceType.SelectedValue.HasValue)
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
						int.TryParse(this.txtImageType.Text, out imageType);
						int orderId = 0;
						int.TryParse(this.txtOrderId.Text, out orderId);
						OnlineServiceInfo onlineServiceInfo = new OnlineServiceInfo();
						onlineServiceInfo.Account = text;
						onlineServiceInfo.ImageType = imageType;
						onlineServiceInfo.NickName = text2;
						onlineServiceInfo.OrderId = orderId;
						onlineServiceInfo.ServiceType = this.dropServiceType.SelectedValue.Value;
						onlineServiceInfo.Status = (this.radioShowStatus.SelectedValue ? 1 : 0);
						if (OnlineServiceHelper.SaveOnlineService(onlineServiceInfo))
						{
							this.ShowMsg("成功添加了一个在线客服", true);
						}
						else
						{
							this.ShowMsg("未知错误", false);
						}
						this.Reset();
					}
				}
			}
		}

		private void Reset()
		{
			this.txtAccount.Text = string.Empty;
			this.txtNickName.Text = string.Empty;
		}
	}
}
