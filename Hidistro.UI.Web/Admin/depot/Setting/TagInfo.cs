using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot.Setting
{
	[PrivilegeCheck(Privilege.TagList)]
	public class TagInfo : AdminCallBackPage
	{
		protected HiddenField hidTagId;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtTagName;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack && !string.IsNullOrEmpty(base.Request.QueryString["id"]))
			{
				this.bindTagInfo(int.Parse(base.Request.QueryString["id"]));
			}
		}

		private void bindTagInfo(int tagId)
		{
			this.btnAdd.Text = "保存";
			StoreTagInfo tagInfo = StoreTagHelper.GetTagInfo(tagId);
			if (tagInfo != null)
			{
				this.hidTagId.Value = tagInfo.TagId.ToString();
				this.txtTagName.Text = tagInfo.TagName;
				this.hidOldImages.Value = tagInfo.TagImgSrc;
			}
			else
			{
				this.ShowMsg("数据读取有误", false);
				this.btnAdd.Enabled = false;
			}
		}

		protected void btnAddTag(object sender, EventArgs e)
		{
			StoreTagInfo storeTagInfo = new StoreTagInfo
			{
				TagName = this.txtTagName.Text.Trim()
			};
			if (this.hidOldImages.Value != this.hidUploadImages.Value)
			{
				storeTagInfo.TagImgSrc = base.UploadImage(this.hidUploadImages.Value, "depot");
				this.hidOldImages.Value = storeTagInfo.TagImgSrc;
			}
			else
			{
				storeTagInfo.TagImgSrc = this.hidOldImages.Value;
			}
			if (!string.IsNullOrEmpty(this.hidTagId.Value))
			{
				storeTagInfo.TagId = int.Parse(this.hidTagId.Value);
				StoreTagInfo tagInfo = StoreTagHelper.GetTagInfo(storeTagInfo.TagId);
				storeTagInfo.DisplaySequence = tagInfo.DisplaySequence;
				this.ShowResultData(StoreTagHelper.EditTag(storeTagInfo));
			}
			else
			{
				this.ShowResultData(StoreTagHelper.AddTag(storeTagInfo));
			}
		}

		private void ShowResultData(int result)
		{
			switch (result)
			{
			case 0:
				this.ShowMsg("标签和名称不能为空", false);
				break;
			case -1:
				this.ShowMsg("标签名称已经存在", false);
				break;
			case -2:
				this.ShowMsg("标签最多只能添加8个", false);
				break;
			case 1:
				base.CloseWindow(null);
				this.ShowMsgCloseWindow("操作成功", true);
				break;
			default:
				this.ShowMsg("标签添加失败！", false);
				break;
			}
		}
	}
}
