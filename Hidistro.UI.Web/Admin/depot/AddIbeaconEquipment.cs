using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[WeiXinCheck(true)]
	public class AddIbeaconEquipment : AdminPage
	{
		protected DropDownList ddlStores;

		protected TextBox txtNumber;

		protected TextBox txtRemark;

		protected Button btnAdd;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (!base.IsPostBack)
			{
				this.GetWXStores();
			}
		}

		public void GetWXStores()
		{
			int available_state_pass = 3;
			int num = 0;
			int num2 = 50;
			List<GetStoreList_Business> list = new List<GetStoreList_Business>();
			GetStoreListResultJson poiList = WXStoreHelper.GetPoiList(num, num2);
			while (poiList.business_list.Count > 0)
			{
				list.AddRange(poiList.business_list);
				num += num2;
				poiList = WXStoreHelper.GetPoiList(num, num2);
			}
			list = (from c in list
			where c.base_info.available_state == available_state_pass
			select c).ToList();
			list.Insert(0, new GetStoreList_Business
			{
				base_info = new GetStoreList_BaseInfo
				{
					business_name = "请选择",
					poi_id = decimal.Zero.ToString()
				}
			});
			this.ddlStores.DataTextField = "DataTextField";
			this.ddlStores.DataValueField = "DataValueField";
			this.ddlStores.DataSource = (from c in list
			select new
			{
				DataTextField = ((c.base_info.poi_id == decimal.Zero.ToString()) ? c.base_info.business_name : (string.IsNullOrEmpty(c.base_info.branch_name) ? c.base_info.business_name : $"{c.base_info.business_name}({c.base_info.branch_name})")),
				DataValueField = c.base_info.poi_id
			}).ToList();
			this.ddlStores.DataBind();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			long value = 0L;
			int.TryParse(this.txtNumber.Text, out num);
			long.TryParse(this.ddlStores.SelectedValue, out value);
			string text = this.txtRemark.Text.Trim();
			string text2 = this.ddlStores.SelectedItem.Text;
			if (num <= 0)
			{
				this.ShowMsg("设备数量不能为空，设备数量只能输入正整数型数值！", false);
			}
			else if (this.ddlStores.SelectedIndex == 0)
			{
				this.ShowMsg("请选择放置的门店！", false);
			}
			else if (string.IsNullOrEmpty(text) || text.Length > 15)
			{
				this.ShowMsg("备注不能为空，长度必须小于或等于15个字符", false);
			}
			else
			{
				DeviceApplyResultJson deviceApplyResultJson = WXStoreHelper.ApplyEquipment(num, text, text, value);
				if (deviceApplyResultJson.errcode.Equals(ReturnCode.请求成功))
				{
					HiCache.Remove("Devices");
					this.ShowMsg("新增设备成功", true);
				}
			}
		}
	}
}
