using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier
{
	[PrivilegeCheck(Privilege.AddSupplier)]
	public class AddSupplier : AdminPage
	{
		protected TrimTextBox txtSupplierName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TrimTextBox txtContactMan;

		protected TrimTextBox txtTel;

		protected RegionSelector dropRegion;

		protected TrimTextBox txtAddress;

		protected Ueditor editDescription;

		protected TrimTextBox txtUserName;

		protected TrimTextBox txtUserPwd;

		protected TrimTextBox txtUserRePwd;

		protected Button btnAdd;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (this.Page.IsPostBack)
			{
				return;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			this.SaveSupplier(out num);
			if (num > 0)
			{
				this.ShowMsg("添加成功", true, "SupplierList.aspx");
			}
		}

		private void SaveSupplier(out int userId)
		{
			userId = 0;
			SupplierInfo supplierInfo = new SupplierInfo();
			string userName = DataHelper.CleanSearchString(this.txtUserName.Text.Trim());
			string supplierName = Globals.StripAllTags(this.txtSupplierName.Text.Trim());
			string address = Globals.StripAllTags(this.txtAddress.Text);
			string text = this.txtTel.Text;
			string text2 = Globals.StripAllTags(this.txtContactMan.Text);
			if (SupplierHelper.ExistSupplierName(0, supplierName))
			{
				this.ShowMsg("供应商名称已经存在,请重新输入！", false);
			}
			else if (!this.dropRegion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择供应商所在区域！", false);
			}
			else if (text2.Length > 8 || text2.Length < 2)
			{
				this.ShowMsg("请输入联系人,联系人长度必须是2-8位！", false);
			}
			else if (address.Length > 50 || address.Length < 2)
			{
				this.ShowMsg("请输入地址,长度必须为2-50个字符！", false);
			}
			else if (text == "" || !DataHelper.IsTel(text))
			{
				this.ShowMsg("请输入正确的联系电话（手机或者固定电话)！", false);
			}
			else if (ManagerHelper.FindManagerByUsername(userName) != null)
			{
				this.ShowMsg("用户名已经存在,请重新输入！", false);
			}
			else if (this.txtUserPwd.Text.Length < 6 || this.txtUserPwd.Text.Length > 20)
			{
				this.ShowMsg("密码不能为空,用户密码长度必须为6-20个字符！", false);
			}
			else if (string.Compare(this.txtUserPwd.Text, this.txtUserRePwd.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
			}
			else
			{
				supplierInfo.Introduce = this.editDescription.Text;
				supplierInfo.SupplierName = supplierName;
				supplierInfo.Picture = this.UploadImage();
				supplierInfo.Status = 1;
				supplierInfo.RegionId = this.dropRegion.GetSelectedRegionId().Value;
				supplierInfo.Tel = text;
				List<string> list = RegionHelper.GetFullRegion(this.dropRegion.GetSelectedRegionId().Value, ",", true, 0).Split(',').Take(3)
					.ToList();
				list.ForEach(delegate(string c)
				{
					address = address.Replace(c, string.Empty);
				});
				supplierInfo.Address = address;
				supplierInfo.ContactMan = text2;
				supplierInfo.FullRegionPath = RegionHelper.GetFullPath(supplierInfo.RegionId, true);
				int num = SupplierHelper.AddSupplier(supplierInfo);
				if (num > 0)
				{
					ManagerInfo managerInfo = new ManagerInfo();
					managerInfo.RoleId = -2;
					managerInfo.UserName = this.txtUserName.Text.Trim();
					string text3 = this.txtUserPwd.Text;
					managerInfo.CreateDate = DateTime.Now;
					string text4 = Globals.RndStr(128, true);
					text3 = (managerInfo.Password = Users.EncodePassword(text3, text4));
					managerInfo.PasswordSalt = text4;
					managerInfo.StoreId = num;
					userId = ManagerHelper.Create(managerInfo);
				}
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/Supplier/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/Supplier/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/Supplier/" + text2;
		}
	}
}
