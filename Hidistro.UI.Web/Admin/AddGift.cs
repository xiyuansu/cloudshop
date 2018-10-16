using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Gifts)]
	public class AddGift : AdminPage
	{
		protected OnOff chkPromotion;

		protected TextBox txtGiftName;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtUnit;

		protected TextBox txtCostPrice;

		protected TextBox txtMarketPrice;

		protected OnOff onoffIsPointExchange;

		protected TextBox txtNeedPoint;

		protected OnOff chkIsExemptionPostage;

		protected ShippingTemplatesDropDownList ShippingTemplatesDropDownList;

		protected TrimTextBox txtWeight;

		protected TrimTextBox txtVolume;

		protected TextBox txtShortDescription;

		protected Ueditor fcDescription;

		protected TextBox txtGiftTitle;

		protected TextBox txtTitleKeywords;

		protected TextBox txtTitleDescription;

		protected Button btnCreate;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnCreate.Click += this.btnCreate_Click;
			this.chkPromotion.Parameter.Add("onSwitchChange", "fuCheckEnableZFBPage");
			this.onoffIsPointExchange.Parameter.Add("onSwitchChange", "fuCheckEnablePointExchange");
			if (!this.Page.IsPostBack)
			{
				this.ShippingTemplatesDropDownList.DataBind();
			}
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			int shippingTemplateId = 0;
			decimal weight = default(decimal);
			decimal volume = default(decimal);
			bool isExemptionPostage = false;
			decimal? costPrice = default(decimal?);
			decimal? marketPrice = default(decimal?);
			int needPoint = default(int);
			if (this.ValidateValues(out costPrice, out marketPrice, out needPoint, out shippingTemplateId, out weight, out volume, out isExemptionPostage))
			{
				GiftInfo giftInfo = new GiftInfo
				{
					CostPrice = costPrice,
					MarketPrice = marketPrice,
					NeedPoint = needPoint,
					Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim()),
					Unit = this.txtUnit.Text.Trim(),
					ShortDescription = Globals.HtmlEncode(this.txtShortDescription.Text.Trim()),
					LongDescription = (string.IsNullOrEmpty(this.fcDescription.Text) ? null : this.fcDescription.Text.Trim()),
					Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim()),
					Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim()),
					Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim()),
					IsPromotion = this.chkPromotion.SelectedValue,
					IsExemptionPostage = isExemptionPostage,
					ShippingTemplateId = shippingTemplateId,
					Weight = weight,
					Volume = volume,
					IsPointExchange = this.onoffIsPointExchange.SelectedValue
				};
				ProductImagesInfo productImagesInfo = this.SaveGiftImage();
				if (productImagesInfo != null)
				{
					giftInfo.ImageUrl = productImagesInfo.ImageUrl1;
					this.hidOldImages.Value = giftInfo.ImageUrl;
					giftInfo.ThumbnailUrl40 = productImagesInfo.ThumbnailUrl40;
					giftInfo.ThumbnailUrl60 = productImagesInfo.ThumbnailUrl60;
					giftInfo.ThumbnailUrl100 = productImagesInfo.ThumbnailUrl100;
					giftInfo.ThumbnailUrl160 = productImagesInfo.ThumbnailUrl160;
					giftInfo.ThumbnailUrl180 = productImagesInfo.ThumbnailUrl180;
					giftInfo.ThumbnailUrl220 = productImagesInfo.ThumbnailUrl220;
					giftInfo.ThumbnailUrl310 = productImagesInfo.ThumbnailUrl310;
					giftInfo.ThumbnailUrl410 = productImagesInfo.ThumbnailUrl410;
				}
				else if (this.hidUploadImages.Value.Trim().Length == 0)
				{
					this.ShowMsg("必须上传礼品图片", false);
					return;
				}
				ValidationResults validationResults = Validation.Validate(giftInfo, "ValGift");
				string text = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						text += Formatter.FormatErrorMessage(item.Message);
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.ShowMsg(text, false);
				}
				else if (GiftHelper.AddGift(giftInfo))
				{
					base.Response.Redirect("Gifts.aspx?flag=1");
				}
				else
				{
					this.ShowMsg("已经存在相同的礼品名称", false);
				}
			}
		}

		private ProductImagesInfo SaveGiftImage()
		{
			string text = Globals.GetStoragePath() + "/gift/";
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string str = Globals.GetStoragePath() + "/temp/";
			string str2 = HttpContext.Current.Server.MapPath(text + "/images/");
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				return null;
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(str2 + text2))
			{
				return null;
			}
			ProductImagesInfo productImagesInfo = new ProductImagesInfo();
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), str2 + text2);
			string text3 = text + "thumbs40/40_" + text2;
			string text4 = text + "thumbs60/60_" + text2;
			string text5 = text + "thumbs100/100_" + text2;
			string text6 = text + "thumbs160/160_" + text2;
			string text7 = text + "thumbs180/180_" + text2;
			string text8 = text + "thumbs220/220_" + text2;
			string text9 = text + "thumbs310/310_" + text2;
			string text10 = text + "thumbs410/410_" + text2;
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text3), 40, 40);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text4), 60, 60);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text5), 100, 100);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text6), 160, 160);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text7), 180, 180);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text8), 220, 220);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text9), 310, 310);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text10), 410, 410);
			productImagesInfo.ImageUrl1 = text + "images/" + text2;
			productImagesInfo.ThumbnailUrl40 = text3;
			productImagesInfo.ThumbnailUrl60 = text4;
			productImagesInfo.ThumbnailUrl100 = text5;
			productImagesInfo.ThumbnailUrl160 = text6;
			productImagesInfo.ThumbnailUrl180 = text7;
			productImagesInfo.ThumbnailUrl220 = text8;
			productImagesInfo.ThumbnailUrl310 = text9;
			productImagesInfo.ThumbnailUrl410 = text10;
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return productImagesInfo;
		}

		private bool ValidateValues(out decimal? costPrice, out decimal? marketPrice, out int needPoint, out int shippingTemplateId, out decimal weight, out decimal volume, out bool isFreeShipping)
		{
			string text = string.Empty;
			costPrice = null;
			marketPrice = null;
			weight = default(decimal);
			volume = default(decimal);
			isFreeShipping = false;
			shippingTemplateId = 0;
			if (!string.IsNullOrEmpty(this.txtCostPrice.Text.Trim()))
			{
				decimal value = default(decimal);
				if (decimal.TryParse(this.txtCostPrice.Text.Trim(), out value))
				{
					costPrice = value;
				}
				else
				{
					text += Formatter.FormatErrorMessage("成本价金额无效，大小在10000000以内");
				}
			}
			if (!string.IsNullOrEmpty(this.txtMarketPrice.Text.Trim()))
			{
				decimal value2 = default(decimal);
				if (decimal.TryParse(this.txtMarketPrice.Text.Trim(), out value2))
				{
					marketPrice = value2;
				}
				else
				{
					text += Formatter.FormatErrorMessage("市场参考价金额无效，大小在10000000以内");
				}
			}
			if (this.onoffIsPointExchange.SelectedValue)
			{
				needPoint = this.txtNeedPoint.Text.ToInt(0);
				if (needPoint <= 0)
				{
					text += Formatter.FormatErrorMessage("兑换所需积分不能为空，大小1-10000000之间");
				}
			}
			else
			{
				needPoint = 0;
			}
			ValuationMethods valuationMethods = (ValuationMethods)0;
			shippingTemplateId = this.ShippingTemplatesDropDownList.SelectedValue.ToInt(0);
			ShippingTemplateInfo shippingTemplateInfo = null;
			if (shippingTemplateId < 0)
			{
				text += Formatter.FormatErrorMessage("请选择运费模板");
			}
			else
			{
				shippingTemplateInfo = SalesHelper.GetShippingTemplate(shippingTemplateId, false);
			}
			if (shippingTemplateInfo == null)
			{
				text += Formatter.FormatErrorMessage("请选择运费模板");
			}
			else
			{
				valuationMethods = shippingTemplateInfo.ValuationMethod;
				isFreeShipping = shippingTemplateInfo.IsFreeShipping;
			}
			if (valuationMethods == ValuationMethods.Weight && !shippingTemplateInfo.IsFreeShipping)
			{
				decimal num = default(decimal);
				if (decimal.TryParse(this.txtWeight.Text, out num))
				{
					weight = num;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的重量");
				}
			}
			if (valuationMethods == ValuationMethods.Volume && !shippingTemplateInfo.IsFreeShipping)
			{
				decimal num2 = default(decimal);
				if (decimal.TryParse(this.txtVolume.Text, out num2))
				{
					volume = num2;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的体积");
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
