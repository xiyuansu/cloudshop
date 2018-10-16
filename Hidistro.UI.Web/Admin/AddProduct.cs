using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AddProduct : ProductBasePage
	{
		private int categoryId = 0;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected HiddenField hidSKUUploadImages;

		protected HiddenField hidSKUOldImages;

		protected HiddenField hidOpenMultReferral;

		protected HiddenField hidOpenSecondReferral;

		protected HiddenField hidOpenThirdReferral;

		protected Literal litCategoryName;

		protected HyperLink lnkEditCategory;

		protected RadioButton radPhysicalProduct;

		protected RadioButton radServiceProduct;

		protected ProductTypeDownList dropProductTypes;

		protected BrandCategoriesDropDownList dropBrandCategories;

		protected TrimTextBox txtProductName;

		protected TrimTextBox txtShortDescription;

		protected HtmlGenericControl l_tags;

		protected ProductTagsLiteral litralProductTag;

		protected TrimTextBox txtProductTag;

		protected TrimTextBox txtProductCode;

		protected TrimTextBox txtUnit;

		protected HtmlGenericControl liIsCrossborder;

		protected CheckBox chkIsCrossborder;

		protected RadioButton radOnSales;

		protected RadioButton radUnSales;

		protected RadioButton radInStock;

		protected ShippingTemplatesDropDownList ShippingTemplatesDropDownList;

		protected TrimTextBox txtWeight;

		protected TrimTextBox txtVolume;

		protected OnOff ChkisfreeShipping;

		protected YesNoRadioButtonList radlEnableMemberDiscount;

		protected RadioButton valid;

		protected RadioButton customValid;

		protected CalendarPanel validStartDate;

		protected CalendarPanel validEndDate;

		protected RadioButton IsRefund;

		protected RadioButton IsRefund2;

		protected RadioButton IsOverRefund;

		protected RadioButton IsOverRefund2;

		protected HtmlInputHidden hidJson;

		protected RadioButton IsGenerateMore;

		protected RadioButton IsGenerateMore2;

		protected TrimTextBox txtAttributes;

		protected TrimTextBox txtMarketPrice;

		protected TrimTextBox txtSalePrice;

		protected TrimTextBox txtMemberPrices;

		protected TrimTextBox txtCostPrice;

		protected TrimTextBox txtSku;

		protected TrimTextBox txtStock;

		protected TrimTextBox txtWarningStock;

		protected TrimTextBox txtSkus;

		protected CheckBox chkSkuEnabled;

		protected TrimTextBox txtTitle;

		protected TrimTextBox txtMetaDescription;

		protected TrimTextBox txtMetaKeywords;

		protected HtmlGenericControl ReferralConfig;

		protected HtmlGenericControl enableDeduct;

		protected OnOff ofEnableDeduct;

		protected HtmlGenericControl liSubMemberDeduct;

		protected TextBox txtSubMemberDeduct;

		protected HtmlGenericControl liReferralDeduct;

		protected TextBox txtSecondLevelDeduct;

		protected HtmlGenericControl liSubReferralDeduct;

		protected TextBox txtThreeLevelDeduct;

		protected Ueditor editDescription;

		protected Ueditor editmobbileDescription;

		protected CheckBox ckbIsDownPic;

		protected Button btnAdd;

		protected ImageList ImageList;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdd.Click += this.btnAdd_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				base.DoCallback();
			}
			else if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				string text = (base.Request.UrlReferrer == (Uri)null) ? "" : base.Request.UrlReferrer.PathAndQuery.ToString();
				this.ofEnableDeduct.Parameter.Add("onSwitchChange", "fuenableDeduct");
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenReferral == 1)
				{
					this.hidOpenMultReferral.Value = (masterSettings.OpenMultReferral ? "1" : "0");
					this.hidOpenSecondReferral.Value = (masterSettings.IsOpenSecondLevelCommission ? "1" : "0");
					this.hidOpenThirdReferral.Value = (masterSettings.IsOpenThirdLevelCommission ? "1" : "0");
				}
				else
				{
					this.hidOpenMultReferral.Value = "0";
					this.hidOpenSecondReferral.Value = "0";
					this.hidOpenThirdReferral.Value = "0";
				}
				this.ReferralConfig.Visible = (masterSettings.OpenReferral == 1 && true);
				if (this.ReferralConfig.Visible)
				{
					this.ofEnableDeduct.SelectedValue = true;
				}
				if (!this.Page.IsPostBack || text.ToLower().IndexOf("selectcategory.aspx") > -1)
				{
					this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
					CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
					if (category == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						if (!string.IsNullOrEmpty(this.litralProductTag.Text))
						{
							this.l_tags.Visible = true;
						}
						this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
						this.dropProductTypes.DataBind();
						this.dropProductTypes.SelectedValue = category.AssociatedProductType;
						this.ShippingTemplatesDropDownList.DataBind();
						this.dropBrandCategories.DataBind();
						int num = 5;
						if (string.IsNullOrEmpty(category.SKUPrefix))
						{
							num = 8;
						}
						TrimTextBox trimTextBox = this.txtProductCode;
						TrimTextBox trimTextBox2 = this.txtSku;
						string sKUPrefix = category.SKUPrefix;
						int num2 = new Random().Next(1, Math.Pow(10.0, (double)num).ToInt(0) - 1);
						string text4 = trimTextBox.Text = (trimTextBox2.Text = sKUPrefix + num2.ToString(CultureInfo.InvariantCulture).PadLeft(num, '0'));
						SiteSettings siteSettings = HiContext.Current.SiteSettings;
						AttributeCollection attributes = this.txtSubMemberDeduct.Attributes;
						decimal num3 = siteSettings.SubMemberDeduct;
						attributes.Add("placeholder", "全站统一比例：" + num3.ToString() + " %");
						AttributeCollection attributes2 = this.txtSecondLevelDeduct.Attributes;
						num3 = siteSettings.SecondLevelDeduct;
						attributes2.Add("placeholder", "全站统一比例：" + num3.ToString() + " %");
						AttributeCollection attributes3 = this.txtThreeLevelDeduct.Attributes;
						num3 = siteSettings.ThreeLevelDeduct;
						attributes3.Add("placeholder", "全站统一比例：" + num3.ToString() + " %");
						this.liIsCrossborder.Visible = siteSettings.IsOpenCertification;
					}
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			bool isfreeShipping = false;
			int shippingTemplateId = 0;
			int num = default(int);
			decimal num2 = default(decimal);
			decimal? nullable = default(decimal?);
			decimal? marketPrice = default(decimal?);
			int stock = default(int);
			int warningStock = default(int);
			decimal weight = default(decimal);
			decimal? secondLevelDeduct = default(decimal?);
			decimal? subMemberDeduct = default(decimal?);
			decimal? threeLevelDeduct = default(decimal?);
			decimal num3 = default(decimal);
			if (this.ValidateConverts(this.chkSkuEnabled.Checked, out num, out num2, out nullable, out marketPrice, out stock, out warningStock, out weight, out secondLevelDeduct, out subMemberDeduct, out threeLevelDeduct, out shippingTemplateId, out num3, out isfreeShipping))
			{
				if (!this.chkSkuEnabled.Checked)
				{
					if (num2 <= decimal.Zero)
					{
						this.ShowMsg("商品一口价必须大于0", false);
						return;
					}
					if (nullable.HasValue && nullable.Value >= num2)
					{
						this.ShowMsg("商品成本价必须小于商品一口价", false);
						return;
					}
				}
				string content = Globals.StripScriptTags(this.txtProductName.Text.Trim());
				content = Globals.StripHtmlXmlTags(content).Replace("\\", "").Replace("'", "");
				if (string.IsNullOrEmpty(content) || content == "")
				{
					this.ShowMsg("产品名称不能为空，且不能包含脚本标签、HTML标签、XML标签、反斜杠(\\)、单引号(')！", false);
				}
				else
				{
					string text = this.editDescription.Text;
					string text2 = this.editmobbileDescription.Text;
					if (this.ckbIsDownPic.Checked)
					{
						text = base.DownRemotePic(text);
						text2 = base.DownRemotePic(text2);
					}
					ProductImagesInfo productImagesInfo = this.SaveProductImages();
					Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
					ProductInfo productInfo = new ProductInfo
					{
						CategoryId = this.categoryId,
						TypeId = this.dropProductTypes.SelectedValue,
						ProductName = content,
						ProductCode = this.txtProductCode.Text,
						MarketPrice = marketPrice,
						Unit = this.txtUnit.Text,
						ImageUrl1 = productImagesInfo.ImageUrl1,
						ImageUrl2 = productImagesInfo.ImageUrl2,
						ImageUrl3 = productImagesInfo.ImageUrl3,
						ImageUrl4 = productImagesInfo.ImageUrl4,
						ImageUrl5 = productImagesInfo.ImageUrl5,
						ThumbnailUrl40 = productImagesInfo.ThumbnailUrl40,
						ThumbnailUrl60 = productImagesInfo.ThumbnailUrl60,
						ThumbnailUrl100 = productImagesInfo.ThumbnailUrl100,
						ThumbnailUrl160 = productImagesInfo.ThumbnailUrl160,
						ThumbnailUrl180 = productImagesInfo.ThumbnailUrl180,
						ThumbnailUrl220 = productImagesInfo.ThumbnailUrl220,
						ThumbnailUrl310 = productImagesInfo.ThumbnailUrl310,
						ThumbnailUrl410 = productImagesInfo.ThumbnailUrl410,
						ShortDescription = this.txtShortDescription.Text,
						Description = ((!string.IsNullOrEmpty(text) && text.Length > 0) ? regex.Replace(text, "") : null),
						MobbileDescription = ((!string.IsNullOrEmpty(text2) && text2.Length > 0) ? regex.Replace(text2, "") : null),
						Title = this.txtTitle.Text,
						Meta_Description = this.txtMetaDescription.Text,
						Meta_Keywords = this.txtMetaKeywords.Text,
						AddedDate = DateTime.Now,
						BrandId = this.dropBrandCategories.SelectedValue,
						MainCategoryPath = CatalogHelper.GetCategory(this.categoryId).Path + "|",
						IsfreeShipping = isfreeShipping,
						SecondLevelDeduct = secondLevelDeduct,
						SubMemberDeduct = subMemberDeduct,
						ThreeLevelDeduct = threeLevelDeduct,
						ShippingTemplateId = shippingTemplateId,
						SupplierId = 0,
						AuditStatus = ProductAuditStatus.Pass,
						IsCrossborder = this.chkIsCrossborder.Checked,
						ProductType = ((!this.radPhysicalProduct.Checked) ? 1 : 0),
						IsValid = this.valid.Checked,
						ValidStartDate = this.validStartDate.Text.ToDateTime(),
						ValidEndDate = (this.validEndDate.Text + " 23:59:59").ToDateTime(),
						IsRefund = this.IsRefund.Checked,
						IsOverRefund = this.IsOverRefund.Checked,
						IsGenerateMore = this.IsGenerateMore.Checked
					};
					ProductSaleStatus saleStatus = ProductSaleStatus.OnSale;
					if (this.radInStock.Checked)
					{
						saleStatus = ProductSaleStatus.OnStock;
					}
					if (this.radUnSales.Checked)
					{
						saleStatus = ProductSaleStatus.UnSale;
					}
					if (this.radOnSales.Checked)
					{
						saleStatus = ProductSaleStatus.OnSale;
					}
					productInfo.SaleStatus = saleStatus;
					Dictionary<string, SKUItem> dictionary = null;
					Dictionary<int, IList<int>> attrs = null;
					if (this.chkSkuEnabled.Checked)
					{
						productInfo.HasSKU = true;
						dictionary = base.GetSkus(this.txtSkus.Text, weight);
					}
					else
					{
						dictionary = new Dictionary<string, SKUItem>
						{
							{
								"0",
								new SKUItem
								{
									SkuId = "0",
									SKU = Globals.HtmlEncode(Globals.StripScriptTags(this.txtSku.Text.Trim()).Replace("\\", "")),
									SalePrice = num2,
									CostPrice = (nullable.HasValue ? nullable.Value : decimal.Zero),
									Stock = stock,
									WarningStock = warningStock,
									Weight = weight
								}
							}
						};
						if (this.txtMemberPrices.Text.Length > 0)
						{
							base.GetMemberPrices(dictionary["0"], this.txtMemberPrices.Text);
						}
					}
					if (!string.IsNullOrEmpty(this.txtAttributes.Text) && this.txtAttributes.Text.Length > 0)
					{
						attrs = base.GetAttributes(this.txtAttributes.Text);
					}
					ValidationResults validationResults = Validation.Validate(productInfo, "AddProduct");
					if (!validationResults.IsValid)
					{
						this.ShowMsg(validationResults);
					}
					else
					{
						IList<int> list = new List<int>();
						if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
						{
							string text3 = this.txtProductTag.Text.Trim();
							string[] array = null;
							array = ((!text3.Contains(",")) ? new string[1]
							{
								text3
							} : text3.Split(','));
							string[] array2 = array;
							foreach (string value in array2)
							{
								list.Add(Convert.ToInt32(value));
							}
						}
						List<ProductSpecificationImageInfo> attrImgs = this.SaveProductAttributeImages();
						string inputItemJson = "";
						if (this.radServiceProduct.Checked)
						{
							inputItemJson = this.hidJson.Value;
						}
						switch (ProductHelper.AddProduct(productInfo, dictionary, attrs, list, attrImgs, false, inputItemJson))
						{
						case ProductActionStatus.Success:
							this.ShowMsg("添加商品成功", true);
							base.Response.Redirect(Globals.GetAdminAbsolutePath($"/product/AddProductComplete.aspx?categoryId={this.categoryId}&productId={productInfo.ProductId}"), true);
							break;
						case ProductActionStatus.AttributeError:
							this.ShowMsg("添加商品失败，保存商品属性时出错", false);
							break;
						case ProductActionStatus.DuplicateName:
							this.ShowMsg("添加商品失败，商品名称不能重复", false);
							break;
						case ProductActionStatus.DuplicateSKU:
							this.ShowMsg("添加商品失败，商家编码不能重复", false);
							break;
						case ProductActionStatus.SKUError:
							this.ShowMsg("添加商品失败，商家规格错误", false);
							break;
						case ProductActionStatus.ProductTagEroor:
							this.ShowMsg("添加商品失败，保存商品标签时出错", false);
							break;
						case ProductActionStatus.ProductAttrImgsError:
							this.ShowMsg("添加商品失败，保存商品规格图片时出错", false);
							break;
						default:
							this.ShowMsg("添加商品失败，未知错误", false);
							break;
						}
					}
				}
			}
		}

		private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int warningStock, out decimal weight, out decimal? secondLevelDeduct, out decimal? subMemberDeduct, out decimal? threeLevelDeduct, out int shippingTemplateId, out decimal volume, out bool isFreeShipping)
		{
			string text = string.Empty;
			costPrice = null;
			marketPrice = null;
			weight = default(decimal);
			secondLevelDeduct = null;
			subMemberDeduct = null;
			threeLevelDeduct = null;
			volume = default(decimal);
			displaySequence = (stock = (warningStock = 0));
			salePrice = default(decimal);
			shippingTemplateId = 0;
			isFreeShipping = false;
			if (this.txtProductCode.Text.Length > 20)
			{
				text += Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
			}
			if (!string.IsNullOrEmpty(Globals.StripAllTags(this.txtProductCode.Text)) && ProductHelper.IsExistsProductCode(Globals.StripAllTags(this.txtProductCode.Text), 0))
			{
				text += Formatter.FormatErrorMessage("商家编码重复");
			}
			if (!string.IsNullOrEmpty(this.txtMarketPrice.Text))
			{
				decimal value = default(decimal);
				if (decimal.TryParse(this.txtMarketPrice.Text, out value))
				{
					marketPrice = value;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的市场价");
				}
			}
			if (this.radPhysicalProduct.Checked)
			{
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
				if (valuationMethods == ValuationMethods.Weight && !isFreeShipping)
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
				if (valuationMethods == ValuationMethods.Volume && !isFreeShipping)
				{
					decimal num2 = default(decimal);
					if (decimal.TryParse(this.txtVolume.Text, out num2))
					{
						volume = num2;
						weight = volume;
					}
					else
					{
						text += Formatter.FormatErrorMessage("请正确填写商品的体积");
					}
				}
			}
			else if (this.customValid.Checked)
			{
				DateTime now = DateTime.Now;
				DateTime now2 = DateTime.Now;
				if (this.validStartDate.Text == "" || this.validEndDate.Text == "" || !DateTime.TryParse(this.validStartDate.Text, out now) || !DateTime.TryParse(this.validEndDate.Text, out now2))
				{
					text += Formatter.FormatErrorMessage("请选择自定义日期");
				}
				if (now > now2)
				{
					text += Formatter.FormatErrorMessage("自定义时间开始时间必须小于结束时间");
				}
			}
			if (!skuEnabled)
			{
				if (string.IsNullOrEmpty(this.txtSalePrice.Text) || !decimal.TryParse(this.txtSalePrice.Text, out salePrice))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品一口价");
				}
				if (!string.IsNullOrEmpty(this.txtCostPrice.Text))
				{
					decimal value2 = default(decimal);
					if (decimal.TryParse(this.txtCostPrice.Text, out value2))
					{
						costPrice = value2;
					}
					else
					{
						text += Formatter.FormatErrorMessage("请正确填写商品的成本价");
					}
				}
				if (string.IsNullOrEmpty(this.txtStock.Text) || !int.TryParse(this.txtStock.Text, out stock))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的库存数量");
				}
				if (string.IsNullOrEmpty(this.txtWarningStock.Text) || !int.TryParse(this.txtWarningStock.Text, out warningStock))
				{
					text += Formatter.FormatErrorMessage("请正确填写商品的警戒库存数量");
				}
			}
			if (HiContext.Current.SiteSettings.IsOpenSecondLevelCommission && !string.IsNullOrEmpty(this.txtSecondLevelDeduct.Text))
			{
				decimal value3 = default(decimal);
				if (decimal.TryParse(this.txtSecondLevelDeduct.Text, out value3))
				{
					secondLevelDeduct = value3;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写会员上二级抽佣比例");
				}
			}
			if (!string.IsNullOrEmpty(this.txtSubMemberDeduct.Text))
			{
				decimal value4 = default(decimal);
				if (decimal.TryParse(this.txtSubMemberDeduct.Text, out value4))
				{
					subMemberDeduct = value4;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写会员直接上级抽佣比例");
				}
			}
			if (HiContext.Current.SiteSettings.IsOpenThirdLevelCommission && !string.IsNullOrEmpty(this.txtThreeLevelDeduct.Text))
			{
				decimal value5 = default(decimal);
				if (decimal.TryParse(this.txtThreeLevelDeduct.Text, out value5))
				{
					threeLevelDeduct = value5;
				}
				else
				{
					text += Formatter.FormatErrorMessage("请正确填写会员上三级抽佣比例");
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}

		private ProductImagesInfo SaveProductImages()
		{
			string text = HiContext.Current.GetStoragePath() + "product/";
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			ProductImagesInfo productImagesInfo = new ProductImagesInfo();
			string str = HttpContext.Current.Server.MapPath(text + "images/");
			string text2 = this.hidUploadImages.Value.Trim();
			if (text2.Length == 0)
			{
				return productImagesInfo;
			}
			string[] array = text2.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string text3 = array[i];
				text3 = text3.Replace("//", "/");
				if (text3.Length != 0)
				{
					string text4 = (text3.Split('/').Length == 6) ? text3.Split('/')[5] : text3.Split('/')[4];
					string text5 = text + "thumbs40/40_" + text4;
					string text6 = text + "thumbs60/60_" + text4;
					string text7 = text + "thumbs100/100_" + text4;
					string text8 = text + "thumbs160/160_" + text4;
					string text9 = text + "thumbs180/180_" + text4;
					string text10 = text + "thumbs220/220_" + text4;
					string text11 = text + "thumbs310/310_" + text4;
					string text12 = text + "thumbs410/410_" + text4;
					if (File.Exists(str + text4))
					{
						AddProduct.BindProductImageInfo(text, productImagesInfo, i, text4, text5, text6, text7, text8, text9, text10, text11, text12);
					}
					else
					{
						File.Copy(HttpContext.Current.Server.MapPath(text3), str + text4);
						if (File.Exists(HttpContext.Current.Server.MapPath(text3)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(text3));
						}
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text5), 40, 40);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text6), 60, 60);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text7), 100, 100);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text8), 160, 160);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text9), 180, 180);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text10), 220, 220);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text11), 310, 310);
						ResourcesHelper.CreateThumbnail(str + text4, HttpContext.Current.Server.MapPath(text12), 410, 410);
						AddProduct.BindProductImageInfo(text, productImagesInfo, i, text4, text5, text6, text7, text8, text9, text10, text11, text12);
					}
				}
			}
			return productImagesInfo;
		}

		private static void BindProductImageInfo(string uploadPath, ProductImagesInfo imgInfo, int i, string newFilename, string thumbnail40SavePath, string thumbnail60SavePath, string thumbnail100SavePath, string thumbnail160SavePath, string thumbnail180SavePath, string thumbnail220SavePath, string thumbnail310SavePath, string thumbnail410SavePath)
		{
			imgInfo.ImageUrl1 = ((i == 0) ? (uploadPath + "images/" + newFilename) : imgInfo.ImageUrl1);
			imgInfo.ImageUrl2 = ((i == 1) ? (uploadPath + "images/" + newFilename) : imgInfo.ImageUrl2);
			imgInfo.ImageUrl3 = ((i == 2) ? (uploadPath + "images/" + newFilename) : imgInfo.ImageUrl3);
			imgInfo.ImageUrl4 = ((i == 3) ? (uploadPath + "images/" + newFilename) : imgInfo.ImageUrl4);
			imgInfo.ImageUrl5 = ((i == 4) ? (uploadPath + "images/" + newFilename) : imgInfo.ImageUrl5);
			if (i == 0)
			{
				imgInfo.ThumbnailUrl40 = thumbnail40SavePath;
				imgInfo.ThumbnailUrl60 = thumbnail60SavePath;
				imgInfo.ThumbnailUrl100 = thumbnail100SavePath;
				imgInfo.ThumbnailUrl160 = thumbnail160SavePath;
				imgInfo.ThumbnailUrl180 = thumbnail180SavePath;
				imgInfo.ThumbnailUrl220 = thumbnail220SavePath;
				imgInfo.ThumbnailUrl310 = thumbnail310SavePath;
				imgInfo.ThumbnailUrl410 = thumbnail410SavePath;
			}
		}

		private List<ProductSpecificationImageInfo> SaveProductAttributeImages()
		{
			List<ProductSpecificationImageInfo> list = new List<ProductSpecificationImageInfo>();
			string text = HiContext.Current.GetStoragePath() + "sku/";
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string str = HttpContext.Current.Server.MapPath(text + "images/");
			string text2 = this.hidSKUUploadImages.Value.Trim();
			if (text2.Length == 0)
			{
				return list;
			}
			string[] array = text2.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('=');
				string text3 = array2[0];
				if (text3.Length != 0)
				{
					string text4 = array2[1];
					if (text4.Length != 0)
					{
						string text5 = array2[2];
						text5 = text5.Replace("//", "/");
						if (text5.Length != 0)
						{
							string text6 = (text5.Split('/').Length == 6) ? text5.Split('/')[5] : text5.Split('/')[4];
							string text7 = text + "thumbs40/40_" + text6;
							string text8 = text + "thumbs410/410_" + text6;
							ProductSpecificationImageInfo productSpecificationImageInfo = new ProductSpecificationImageInfo();
							productSpecificationImageInfo.AttributeId = int.Parse(text3);
							productSpecificationImageInfo.ValueId = int.Parse(text4);
							productSpecificationImageInfo.ThumbnailUrl40 = text7;
							productSpecificationImageInfo.ThumbnailUrl410 = text8;
							if (File.Exists(str + text6))
							{
								productSpecificationImageInfo.ImageUrl = text + "images/" + text6;
								list.Add(productSpecificationImageInfo);
							}
							else
							{
								Globals.CreatePath(HttpContext.Current.Server.MapPath(text5));
								File.Copy(HttpContext.Current.Server.MapPath(text5), str + text6);
								if (File.Exists(HttpContext.Current.Server.MapPath(text5)))
								{
									File.Delete(HttpContext.Current.Server.MapPath(text5));
								}
								Globals.CreatePath(HttpContext.Current.Server.MapPath(text7));
								Globals.CreatePath(HttpContext.Current.Server.MapPath(text8));
								ResourcesHelper.CreateThumbnail(str + text6, HttpContext.Current.Server.MapPath(text7), 40, 40);
								ResourcesHelper.CreateThumbnail(str + text6, HttpContext.Current.Server.MapPath(text8), 410, 410);
								productSpecificationImageInfo.ImageUrl = text + "images/" + text6;
								list.Add(productSpecificationImageInfo);
							}
						}
					}
				}
			}
			return list;
		}
	}
}
