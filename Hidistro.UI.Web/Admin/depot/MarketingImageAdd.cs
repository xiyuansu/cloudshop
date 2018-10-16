using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.MarketingImageList)]
	public class MarketingImageAdd : AdminCallBackPage
	{
		protected HiddenField hidImageId;

		protected TrimTextBox txtImageName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtDescription;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				int num = base.Request.QueryString["ImageId"].ToInt(0);
				if (num > 0)
				{
					this.hidImageId.Value = num.ToString();
					this.bindImageInfo(num);
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			string text = Globals.StripAllTags(this.txtImageName.Text);
			string text2 = Globals.StripAllTags(this.txtDescription.Text);
			string value = this.hidOldImages.Value;
			int num = this.hidImageId.Value.ToInt(0);
			if (text.Trim() == "" || text.Length > 20)
			{
				this.ShowMsg("请输入图库名称，并且图库名称长度不能大于20个字符", false);
			}
			else if (text2.Trim() != "" && text2.Length > 100)
			{
				this.ShowMsg("使用说明长度不能大于100个字符", false);
			}
			else
			{
				MarketingImagesInfo marketingImagesInfo = new MarketingImagesInfo();
				if (num > 0)
				{
					marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(num);
					if (marketingImagesInfo == null)
					{
						this.ShowMsg("错误的营销图库ID", false);
						return;
					}
				}
				marketingImagesInfo.ImageName = text;
				marketingImagesInfo.Description = text2;
				if (this.hidOldImages.Value != this.hidUploadImages.Value)
				{
					marketingImagesInfo.ImageUrl = base.UploadImage(this.hidUploadImages.Value, "depot");
					this.hidOldImages.Value = marketingImagesInfo.ImageUrl;
				}
				else
				{
					marketingImagesInfo.ImageUrl = this.hidOldImages.Value;
				}
				if (marketingImagesInfo.ImageUrl.Trim() == "")
				{
					this.ShowMsg("请上传图库图片", false);
				}
				else
				{
					bool flag = false;
					if ((num <= 0) ? (MarketingImagesHelper.AddMarketingImages(marketingImagesInfo) > 0) : MarketingImagesHelper.UpdateMarketingImages(marketingImagesInfo))
					{
						base.CloseWindow(num);
						this.ShowMsgCloseWindow("营销图库" + ((num > 0) ? "编辑" : "添加") + "成功", true);
					}
					else
					{
						this.ShowMsg("营销图库" + ((num > 0) ? "编辑" : "添加") + "失败", false);
					}
				}
			}
		}

		private void bindImageInfo(int imageId)
		{
			MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
			if (marketingImagesInfo == null)
			{
				this.hidImageId.Value = "0";
			}
			else
			{
				this.btnSave.Text = "保存";
				this.txtDescription.Text = marketingImagesInfo.Description;
				this.txtImageName.Text = marketingImagesInfo.ImageName;
				this.hidOldImages.Value = marketingImagesInfo.ImageUrl;
			}
		}
	}
}
