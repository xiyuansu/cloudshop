using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[WeiXinCheck(true)]
	public class SearchIbeaconEquipment : AdminCallBackPage
	{
		private long page_id = 0L;

		protected HtmlGenericControl divSearchBox;

		protected ImageLinkButton btnAdd;

		protected HiddenField hfdevice;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.page_id = base.Request.QueryString["page_id"].ToLong(0);
			this.btnAdd.Click += this.btnAdd_Click;
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择一台设备！", false);
			}
			else
			{
				List<long> devices = new List<long>();
				text.Split(',').ForEach(delegate(string c)
				{
					devices.Add(c.ToLong(0));
				});
				bool flag = true;
				foreach (long item in devices)
				{
					DeviceApply_Data_Device_Identifiers deviceApply_Data_Device_Identifiers = new DeviceApply_Data_Device_Identifiers();
					deviceApply_Data_Device_Identifiers.device_id = item;
					long[] pageIds = new long[1]
					{
						this.page_id
					};
					WxJsonResult wxJsonResult = WXStoreHelper.BindPage(deviceApply_Data_Device_Identifiers, pageIds, ShakeAroundBindType.建立关联关系, ShakeAroundAppendType.新增);
					if (!wxJsonResult.errcode.Equals(ReturnCode.请求成功))
					{
						flag = false;
					}
				}
				if (flag)
				{
					base.CloseWindow(null);
				}
				else
				{
					this.ShowMsg("选择的设备没有配置成功！", false);
				}
			}
		}
	}
}
