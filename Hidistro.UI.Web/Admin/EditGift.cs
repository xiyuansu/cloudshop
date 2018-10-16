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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Gifts)]
	public class EditGift : AdminPage
	{
		private int giftId;

		protected HtmlGenericControl liprize;

		protected Label lblprizeMsg;

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

		protected HtmlGenericControl weightRow;

		protected TrimTextBox txtWeight;

		protected HtmlGenericControl volumeRow;

		protected TrimTextBox txtVolume;

		protected TextBox txtShortDescription;

		protected Ueditor fcDescription;

		protected TextBox txtGiftTitle;

		protected TextBox txtTitleKeywords;

		protected TextBox txtTitleDescription;

		protected Button btnUpdate;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["giftId"], out this.giftId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnUpdate.Click += this.btnUpdate_Click;
				this.chkPromotion.Parameter.Add("onSwitchChange", "fuCheckEnableZFBPage");
				this.onoffIsPointExchange.Parameter.Add("onSwitchChange", "fuCheckEnablePointExchange");
				if (!this.Page.IsPostBack)
				{
					this.LoadGift();
					if (ActivityHelper.ExistGiftNoReceive(this.giftId))
					{
						this.liprize.Visible = true;
						this.lblprizeMsg.Text = "有用户存在此礼品未领取,如编辑领取的礼品将以最新信息的为准";
					}
					else if (ActivityHelper.ExistValueInActivity(this.giftId, ActivityEnumPrizeType.Gift))
					{
						this.liprize.Visible = true;
						this.lblprizeMsg.Text = "有活动中正在使用此礼品做为奖品,如编辑中奖的礼品将以最新的信息为准";
					}
				}
			}
		}

		private void LoadGift()
		{
			GiftInfo giftDetails = GiftHelper.GetGiftDetails(this.giftId);
			if (giftDetails == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				Globals.EntityCoding(giftDetails, false);
				this.txtGiftName.Text = Globals.HtmlDecode(giftDetails.Name);
				this.txtNeedPoint.Text = giftDetails.NeedPoint.ToString();
				this.ShippingTemplatesDropDownList.DataBind();
				this.ShippingTemplatesDropDownList.SelectedValue = giftDetails.ShippingTemplateId;
				if (!string.IsNullOrEmpty(giftDetails.Unit))
				{
					this.txtUnit.Text = giftDetails.Unit;
				}
				if (giftDetails.CostPrice.HasValue)
				{
					this.txtCostPrice.Text = $"{giftDetails.CostPrice:F2}";
				}
				if (giftDetails.MarketPrice.HasValue)
				{
					this.txtMarketPrice.Text = $"{giftDetails.MarketPrice:F2}";
				}
				if (!string.IsNullOrEmpty(giftDetails.ShortDescription))
				{
					this.txtShortDescription.Text = Globals.HtmlDecode(giftDetails.ShortDescription);
				}
				if (!string.IsNullOrEmpty(giftDetails.LongDescription))
				{
					this.fcDescription.Text = giftDetails.LongDescription;
				}
				if (!string.IsNullOrEmpty(giftDetails.Title))
				{
					this.txtGiftTitle.Text = Globals.HtmlDecode(giftDetails.Title);
				}
				if (!string.IsNullOrEmpty(giftDetails.Meta_Description))
				{
					this.txtTitleDescription.Text = Globals.HtmlDecode(giftDetails.Meta_Description);
				}
				if (!string.IsNullOrEmpty(giftDetails.Meta_Keywords))
				{
					this.txtTitleKeywords.Text = Globals.HtmlDecode(giftDetails.Meta_Keywords);
				}
				if (!string.IsNullOrEmpty(giftDetails.ImageUrl))
				{
					HiddenField hiddenField = this.hidUploadImages;
					HiddenField hiddenField2 = this.hidOldImages;
					string text2 = hiddenField.Value = (hiddenField2.Value = giftDetails.ImageUrl);
				}
				this.chkPromotion.SelectedValue = giftDetails.IsPromotion;
				this.chkIsExemptionPostage.SelectedValue = giftDetails.IsExemptionPostage;
				this.onoffIsPointExchange.SelectedValue = giftDetails.IsPointExchange;
				int shippingTemplateId = giftDetails.ShippingTemplateId;
				ShippingTemplateInfo shippingTemplate = SalesHelper.GetShippingTemplate(shippingTemplateId, false);
				if (shippingTemplate != null)
				{
					if (shippingTemplate.ValuationMethod != ValuationMethods.Weight)
					{
						this.weightRow.Style.Add("display", "none");
					}
					if (shippingTemplate.ValuationMethod != ValuationMethods.Volume)
					{
						this.volumeRow.Style.Add("display", "none");
					}
					if (shippingTemplate.ValuationMethod == ValuationMethods.Number)
					{
						this.volumeRow.Style.Add("display", "none");
						this.weightRow.Style.Add("display", "none");
					}
				}
				else
				{
					this.volumeRow.Style.Add("display", "none");
					this.weightRow.Style.Add("display", "none");
				}
				this.txtVolume.Text = giftDetails.Volume.F2ToString("f2");
				this.txtWeight.Text = giftDetails.Weight.F2ToString("f2");
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			GiftInfo giftDetails = GiftHelper.GetGiftDetails(this.giftId);
			if (ActivityHelper.ExistGiftNoReceive(this.giftId))
			{
				this.liprize.Visible = true;
				this.ShowMsg("有用户存在此礼品未领取,如编辑领取的礼品将以最新信息的为准", false);
			}
			else if (ActivityHelper.ExistValueInActivity(this.giftId, ActivityEnumPrizeType.Gift))
			{
				this.liprize.Visible = true;
				this.ShowMsg("有活动中正在使用此礼品最为奖品,如编辑中奖的礼品将以最新的信息为准", false);
			}
			int shippingTemplateId = 0;
			decimal weight = default(decimal);
			decimal volume = default(decimal);
			bool isExemptionPostage = false;
			Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			decimal? costPrice = default(decimal?);
			decimal? marketPrice = default(decimal?);
			int needPoint = default(int);
			if (this.ValidateValues(out costPrice, out marketPrice, out needPoint, out shippingTemplateId, out weight, out volume, out isExemptionPostage))
			{
				giftDetails.CostPrice = costPrice;
				giftDetails.MarketPrice = marketPrice;
				giftDetails.NeedPoint = needPoint;
				giftDetails.Name = Globals.HtmlEncode(this.txtGiftName.Text.Trim());
				giftDetails.Unit = this.txtUnit.Text.Trim();
				giftDetails.ShortDescription = Globals.HtmlEncode(this.txtShortDescription.Text.Trim());
				giftDetails.LongDescription = this.fcDescription.Text.Trim();
				giftDetails.Title = Globals.HtmlEncode(this.txtGiftTitle.Text.Trim());
				giftDetails.Meta_Description = Globals.HtmlEncode(this.txtTitleDescription.Text.Trim());
				giftDetails.Meta_Keywords = Globals.HtmlEncode(this.txtTitleKeywords.Text.Trim());
				giftDetails.IsPromotion = this.chkPromotion.SelectedValue;
				giftDetails.IsExemptionPostage = isExemptionPostage;
				giftDetails.IsPointExchange = this.onoffIsPointExchange.SelectedValue;
				ProductImagesInfo productImagesInfo = this.SaveGiftImage();
				if (productImagesInfo != null)
				{
					giftDetails.ImageUrl = productImagesInfo.ImageUrl1;
					this.hidOldImages.Value = giftDetails.ImageUrl;
					giftDetails.ThumbnailUrl40 = productImagesInfo.ThumbnailUrl40;
					giftDetails.ThumbnailUrl60 = productImagesInfo.ThumbnailUrl60;
					giftDetails.ThumbnailUrl100 = productImagesInfo.ThumbnailUrl100;
					giftDetails.ThumbnailUrl160 = productImagesInfo.ThumbnailUrl160;
					giftDetails.ThumbnailUrl180 = productImagesInfo.ThumbnailUrl180;
					giftDetails.ThumbnailUrl220 = productImagesInfo.ThumbnailUrl220;
					giftDetails.ThumbnailUrl310 = productImagesInfo.ThumbnailUrl310;
					giftDetails.ThumbnailUrl410 = productImagesInfo.ThumbnailUrl410;
				}
				else if (this.hidUploadImages.Value.Trim().Length == 0)
				{
					if (!string.IsNullOrEmpty(giftDetails.ImageUrl))
					{
						this.hidUploadImages.Value = giftDetails.ImageUrl;
					}
					this.ShowMsg("必须上传礼品图片", false);
					return;
				}
				giftDetails.IsExemptionPostage = isExemptionPostage;
				giftDetails.Weight = weight;
				giftDetails.Volume = volume;
				giftDetails.ShippingTemplateId = shippingTemplateId;
				ValidationResults validationResults = Validation.Validate(giftDetails, "ValGift");
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
				else if (GiftHelper.UpdateGift(giftDetails))
				{
					base.Response.Redirect("Gifts.aspx?flag=2");
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
			shippingTemplateId = 0;
			weight = default(decimal);
			volume = default(decimal);
			isFreeShipping = false;
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
