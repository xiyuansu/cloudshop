using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot.Setting
{
	[PrivilegeCheck(Privilege.MarktingList)]
	public class MarkingInfo : AdminCallBackPage
	{
		protected HiddenField hidDisplaySequence;

		protected HiddenField hidId;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected DropDownList ddlType;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.bindType();
				if (!string.IsNullOrEmpty(base.Request.QueryString["id"]))
				{
					this.hidId.Value = base.Request.QueryString["id"];
					this.bindInfo(int.Parse(this.hidId.Value));
				}
			}
		}

		private void bindInfo(int id)
		{
			StoreMarktingInfo storeMarktingInfo = StoreMarktingHelper.GetStoreMarktingInfo(id);
			this.hidOldImages.Value = storeMarktingInfo.IconUrl;
			this.ddlType.SelectedValue = ((byte)storeMarktingInfo.MarktingType).ToString();
			this.hidDisplaySequence.Value = storeMarktingInfo.DisplaySequence.ToString();
			this.btnSave.Text = "保存";
		}

		private void bindType()
		{
			foreach (EnumMarktingType value in Enum.GetValues(typeof(EnumMarktingType)))
			{
				string text = ((Enum)(object)value).ToDescription();
				byte b = (byte)value;
				ListItem item = new ListItem(text, b.ToString());
				this.ddlType.Items.Add(item);
			}
			this.ddlType.Items.Insert(0, new ListItem
			{
				Text = "请选择",
				Value = "0"
			});
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			StoreMarktingInfo storeMarktingInfo = new StoreMarktingInfo
			{
				MarktingType = (EnumMarktingType)int.Parse(this.ddlType.SelectedValue)
			};
			if (this.hidOldImages.Value != this.hidUploadImages.Value)
			{
				storeMarktingInfo.IconUrl = base.UploadImage(this.hidUploadImages.Value, "depot");
				this.hidOldImages.Value = storeMarktingInfo.IconUrl;
			}
			else
			{
				storeMarktingInfo.IconUrl = this.hidOldImages.Value;
			}
			int num = 0;
			if (string.IsNullOrEmpty(this.hidId.Value))
			{
				num = StoreMarktingHelper.AddInfo(storeMarktingInfo);
			}
			else
			{
				storeMarktingInfo.Id = int.Parse(this.hidId.Value);
				storeMarktingInfo.DisplaySequence = int.Parse(this.hidDisplaySequence.Value);
				num = StoreMarktingHelper.Edit(storeMarktingInfo);
			}
			this.ShowResultData(num);
		}

		private void ShowResultData(int result)
		{
			switch (result)
			{
			case -1:
				this.ShowMsg("该跳转类型已经存在", false);
				break;
			case 1:
				base.CloseWindow(null);
				this.ShowMsgCloseWindow("操作成功", true);
				break;
			default:
				this.ShowMsg("操作失败！", false);
				break;
			}
		}
	}
}
