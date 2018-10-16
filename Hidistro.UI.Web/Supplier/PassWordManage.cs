using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	public class PassWordManage : SupplierAdminPage
	{
		protected HtmlGenericControl lipass0;

		protected HtmlGenericControl lipass1;

		protected HtmlGenericControl divright0;

		protected Literal lblUserName;

		protected TextBox txtoldPassword;

		protected TextBox txtnewPassword;

		protected TextBox txtnewPasswordConfirm;

		protected Button btnSavePass;

		protected HtmlGenericControl divright1;

		protected Literal ltistradepass;

		protected HtmlGenericControl liTradePass_Empty;

		protected HtmlGenericControl liTradePass_Old;

		protected TextBox txtoldTradePassword;

		protected TextBox txtTradePassword;

		protected TextBox txtTradePasswordConfirm;

		protected Button btnSaveTradePass;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSavePass.Click += this.btnSavePass_Click;
			this.btnSaveTradePass.Click += this.btnSaveTradePass_Click;
			if (!base.IsPostBack)
			{
				this.txtoldPassword.Attributes.Add("onkeydown", "ClickEnter(event,'" + this.btnSavePass.ClientID.Trim() + "')");
				this.txtnewPassword.Attributes.Add("onkeydown", "ClickEnter(event,'" + this.btnSavePass.ClientID.Trim() + "')");
				this.txtnewPasswordConfirm.Attributes.Add("onkeydown", "ClickEnter(event,'" + this.btnSavePass.ClientID.Trim() + "')");
				this.txtoldTradePassword.Attributes.Add("onkeydown", "ClickEnter(event,'" + this.btnSaveTradePass.ClientID.Trim() + "')");
				this.txtTradePassword.Attributes.Add("onkeydown", "ClickEnter(event,'" + this.btnSaveTradePass.ClientID.Trim() + "')");
				this.txtTradePasswordConfirm.Attributes.Add("onkeydown", "ClickEnter(event,'" + this.btnSaveTradePass.ClientID.Trim() + "')");
				this.BindData();
			}
		}

		public void BindData()
		{
			ManagerInfo manager = HiContext.Current.Manager;
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(manager.StoreId);
			this.lblUserName.Text = manager.UserName;
			if (string.IsNullOrEmpty(supplierById.TradePassword))
			{
				this.liTradePass_Empty.Visible = true;
				this.liTradePass_Old.Visible = false;
				this.ltistradepass.Text = "0";
				this.lipass0.Attributes.Add("class", "");
				this.divright0.Attributes.Add("style", "display:none");
				this.lipass1.Attributes.Add("class", "hover");
				this.divright1.Attributes.Add("style", "");
			}
			else
			{
				this.liTradePass_Empty.Visible = false;
				this.liTradePass_Old.Visible = true;
				this.ltistradepass.Text = "1";
				this.lipass0.Attributes.Add("class", "hover");
				this.divright0.Attributes.Add("style", "");
				this.lipass1.Attributes.Add("class", "");
				this.divright1.Attributes.Add("style", "display:none");
			}
		}

		private void btnSavePass_Click(object sender, EventArgs e)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (this.txtnewPassword.Text.Length < 6 || this.txtnewPassword.Text.Length > 20)
			{
				this.ShowMsg("登录密码长度必须为6-20个字符！", false);
			}
			else if (string.Compare(this.txtnewPassword.Text, this.txtnewPasswordConfirm.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
			}
			else
			{
				string strA = Users.EncodePassword(this.txtoldPassword.Text, manager.PasswordSalt);
				if (string.Compare(strA, manager.Password) != 0)
				{
					this.ShowMsg("输入的当前登录密码与原始登录密码不一致，请正确输入", false);
				}
				else
				{
					string text = Globals.RndStr(128, true);
					string text3 = manager.Password = Users.EncodePassword(this.txtnewPassword.Text, text);
					manager.PasswordSalt = text;
					if (ManagerHelper.Update(manager))
					{
						this.ShowMsg("修改登录密码成功", true, "PassWordManage.aspx");
					}
					else
					{
						this.ShowMsg("修改登录密码失败", true);
					}
				}
			}
		}

		private void btnSaveTradePass_Click(object sender, EventArgs e)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(manager.StoreId);
			if (manager == null || supplierById == null)
			{
				this.ShowMsg("参数错误，请找管理员处理！", false);
			}
			else
			{
				if (!string.IsNullOrEmpty(supplierById.TradePassword))
				{
					string strA = Users.EncodePassword(this.txtoldTradePassword.Text, supplierById.TradePasswordSalt);
					if (string.Compare(strA, supplierById.TradePassword) != 0)
					{
						this.ShowMsg("输入的当前交易密码与原始交易密码不一致，请正确输入！", false);
						return;
					}
				}
				if (this.txtTradePassword.Text.Length < 6 || this.txtTradePassword.Text.Length > 20)
				{
					this.ShowMsg("交易密码长度必须为6-20个字符！", false);
				}
				else if (string.Compare(this.txtTradePassword.Text, this.txtTradePasswordConfirm.Text) != 0)
				{
					this.ShowMsg("请确保两次输入的密码相同", false);
				}
				else
				{
					string text = Globals.RndStr(128, true);
					string text3 = supplierById.TradePassword = Users.EncodePassword(this.txtTradePassword.Text, text);
					supplierById.TradePasswordSalt = text;
					if (SupplierHelper.UpdateSupplier(supplierById))
					{
						this.ShowMsg("修改交易密码成功", true, "PassWordManage.aspx");
					}
					else
					{
						this.ShowMsg("修改交易密码失败", true);
					}
				}
			}
		}
	}
}
