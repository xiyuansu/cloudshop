using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	public class EditSupplier : SupplierAdminPage
	{
		protected Literal ltSupplierName;

		protected TextBox txtContactMan;

		protected TextBox txtTel;

		protected RegionSelector dropRegion;

		protected TextBox txtAddress;

		protected Ueditor editDescription;

		protected TextBox txtWxOpenId;

		protected HtmlAnchor reGetOpenId;

		protected HtmlGenericControl getOpenId;

		protected Image OpenIdQrCodeImg;

		protected Button btnAdd;

		protected ImageList ImageList;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (!this.Page.IsPostBack)
			{
				if (HiContext.Current.SiteSettings.OpenVstore == 0)
				{
					this.getOpenId.Style.Add("display", "none");
				}
				else
				{
					string text = "";
					if (HiContext.Current.Manager != null)
					{
						text = HiContext.Current.Manager.ManagerId.ToString();
					}
					string content = Globals.FullPath("/Vshop/GetOpenId.aspx?adminName=" + text);
					string qrCodeUrl = "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.ToLower().Replace("https://", "").Replace("http://", "")
						.Replace(".", "") + "_" + text + "_OpenId.png";
					this.OpenIdQrCodeImg.ImageUrl = Globals.CreateQRCode(content, qrCodeUrl, false, ImageFormats.Png);
				}
				this.BindData();
			}
		}

		public void BindData()
		{
			ManagerInfo manager = HiContext.Current.Manager;
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(manager.StoreId);
			this.txtAddress.Text = supplierById.Address;
			this.txtContactMan.Text = supplierById.ContactMan;
			this.ltSupplierName.Text = supplierById.SupplierName;
			this.txtTel.Text = supplierById.Tel;
			this.dropRegion.SetSelectedRegionId(supplierById.RegionId);
			this.txtWxOpenId.Text = supplierById.WXOpenId;
			this.editDescription.Text = supplierById.Introduce;
			if (string.IsNullOrEmpty(supplierById.WXOpenId))
			{
				this.reGetOpenId.Style.Add("display", "none");
			}
			else
			{
				this.getOpenId.Style.Add("display", "none");
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			string address = Globals.StripAllTags(this.txtAddress.Text);
			string text = this.txtTel.Text;
			string text2 = this.txtContactMan.Text;
			ManagerInfo manager = HiContext.Current.Manager;
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(manager.StoreId);
			if (!this.dropRegion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择供应商所在区域！", false);
			}
			else if (text2.Length > 8 || text2.Length < 2)
			{
				this.ShowMsg("请输入联系人,联系人长度必须是2-8位！", false);
			}
			else if (text == "" || !DataHelper.IsTel(text))
			{
				this.ShowMsg("请输入正确的联系电话（手机或者固定电话)！", false);
			}
			else
			{
				supplierById.RegionId = this.dropRegion.GetSelectedRegionId().Value;
				supplierById.Tel = text;
				supplierById.Address = address;
				supplierById.ContactMan = text2;
				supplierById.WXOpenId = Globals.StripAllTags(this.txtWxOpenId.Text);
				supplierById.FullRegionPath = RegionHelper.GetFullPath(supplierById.RegionId, true);
				supplierById.Introduce = this.editDescription.Text;
				SupplierHelper.UpdateSupplier(supplierById);
				this.ShowMsg("保存成功", true);
			}
		}
	}
}
