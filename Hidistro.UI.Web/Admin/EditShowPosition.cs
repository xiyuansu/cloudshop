using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class EditShowPosition : AdminCallBackPage
	{
		protected RadioButtonList radShowPosition;

		protected HiddenField hidShowPosition;

		protected TextBox txtYPosition;

		protected Button btnOK;

		protected ServiceTypeDropDownList dropServiceType;

		protected HiddenField hidServiceType;

		protected TextBox txtAccount;

		protected HtmlGenericControl txtAccountTip;

		protected TextBox txtNickName;

		protected HtmlGenericControl txtNickNameTip;

		protected TextBox txtOrderId;

		protected HtmlGenericControl txtOrderIdTip;

		protected TextBox txtImageType;

		protected YesNoRadioButtonList radioShowStatus;

		protected HiddenField hidShowStatus;

		protected HiddenField hidID;

		protected Button btnSubmit;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.dropServiceType.AllowNull = false;
			this.dropServiceType.DataBind();
			this.btnOK.Click += this.btnOK_Click;
			this.btnSubmit.Click += this.btnSubmit_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindOnlineService();
			}
		}

		private void BindOnlineService()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.txtYPosition.Text = masterSettings.ServiceCoordinate;
			this.radShowPosition.SelectedValue = masterSettings.ServicePosition.ToString();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			string value = this.hidShowPosition.Value;
			if (value != "1" && value != "2" && value != "3" && value != "4")
			{
				this.ShowMsg("请选择在线客服显示位置", false);
			}
			else
			{
				int num = 0;
				int.TryParse(this.txtYPosition.Text, out num);
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.ServicePosition = Convert.ToInt32(value);
				masterSettings.ServiceCoordinate = num.ToString();
				SettingsManager.Save(masterSettings);
				base.CloseWindow(null);
				this.BindOnlineService();
			}
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
						int.TryParse(this.txtImageType.Text, out imageType);
						int orderId = 0;
						int.TryParse(this.txtOrderId.Text, out orderId);
						OnlineServiceInfo onlineServiceInfo = new OnlineServiceInfo();
						onlineServiceInfo.Account = text;
						onlineServiceInfo.ImageType = imageType;
						onlineServiceInfo.NickName = text2;
						onlineServiceInfo.OrderId = orderId;
						onlineServiceInfo.ServiceType = this.hidServiceType.Value.ToInt(0);
						onlineServiceInfo.Status = (this.hidShowStatus.Value.ToBool() ? 1 : 0);
						if (this.hidID.Value.ToInt(0) > 0)
						{
							onlineServiceInfo.Id = this.hidID.Value.ToInt(0);
							if (OnlineServiceHelper.Update(onlineServiceInfo))
							{
								this.ShowMsg("成功编辑了一个在线客服", true);
								this.BindOnlineService();
							}
							else
							{
								this.ShowMsg("未知错误", false);
							}
						}
						else if (OnlineServiceHelper.SaveOnlineService(onlineServiceInfo))
						{
							this.ShowMsg("成功添加了一个在线客服", true);
							this.BindOnlineService();
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
			this.txtOrderId.Text = string.Empty;
			this.hidID.Value = "0";
			this.radioShowStatus.SelectedValue = false;
			this.dropServiceType.SelectedIndex = 0;
		}
	}
}
