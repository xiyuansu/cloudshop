using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Product
{
	[PrivilegeCheck(Privilege.SupplierEditPd)]
	public class EditProduct : ProductBasePage
	{
		private int productId = 0;

		private int categoryId = 0;

		private ProductInfo OldProductInfo = null;

		protected HiddenField hidListReturnUrl;

		protected HiddenField hidProductId;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected HiddenField txt_SalesCount;

		protected HiddenField txt_ShowSalesCount;

		protected HiddenField txt_VistiCounts;

		protected HiddenField hidSKUUploadImages;

		protected HiddenField hidSKUOldImages;

		protected HiddenField hidHasSku;

		protected HiddenField hidOpenMultReferral;

		protected HiddenField hidHasActivity;

		protected HiddenField HidFromUrl;

		protected HiddenField hidAuditStatus;

		protected Literal litCategoryName;

		protected HyperLink lnkEditCategory;

		protected ProductTypeDownList dropProductTypes;

		protected BrandCategoriesDropDownList dropBrandCategories;

		protected TrimTextBox txtProductName;

		protected TrimTextBox txtShortDescription;

		protected HtmlGenericControl l_tags;

		protected ProductTagsLiteral litralProductTag;

		protected TrimTextBox txtProductTag;

		protected TrimTextBox txtDisplaySequence;

		protected TrimTextBox txtProductCode;

		protected TrimTextBox txtUnit;

		protected TrimTextBox txtAttributes;

		protected RadioButton radOnSales;

		protected RadioButton radUnSales;

		protected RadioButton radInStock;

		protected ShippingTemplatesDropDownList ShippingTemplatesDropDownList;

		protected HtmlGenericControl weightRow;

		protected TrimTextBox txtWeight;

		protected HtmlGenericControl volumeRow;

		protected TrimTextBox txtVolume;

		protected OnOff ChkisfreeShipping;

		protected TrimTextBox txtMarketPrice;

		protected TrimTextBox txtSalePrice;

		protected TrimTextBox txtMemberPrices;

		protected TrimTextBox txtCostPrice;

		protected TrimTextBox txtSku;

		protected TrimTextBox txtStock;

		protected Label lblStock;

		protected TrimTextBox txtWarningStock;

		protected Label lblWarningStock;

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

		protected Literal litSubMemberDeduct;

		protected HtmlGenericControl liReferralDeduct;

		protected TextBox txtSecondLevelDeduct;

		protected Literal litReferralDeduct;

		protected HtmlGenericControl liSubReferralDeduct;

		protected TextBox txtThreeLevelDeduct;

		protected Literal litSubReferralDeduct;

		protected Ueditor fckDescription;

		protected Ueditor fckmobbileDescription;

		protected CheckBox ckbIsDownPic;

		protected Button btnSave;

		protected Button btnSaveAudit;

		protected ImageList ImageList;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += this.btnSave_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			int.TryParse(base.Request.QueryString["productId"], out this.productId);
			int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId);
			string text = (base.Request.UrlReferrer == (Uri)null) ? "" : base.Request.UrlReferrer.PathAndQuery.ToString();
			IList<int> list = null;
			DataTable dataTable = null;
			Dictionary<int, IList<int>> attrs = default(Dictionary<int, IList<int>>);
			ProductInfo productInfo = this.OldProductInfo = ProductHelper.GetProductDetails(this.productId, out attrs, out list, out dataTable);
			this.HasActivitiesByProductId(productInfo);
			this.hidHasSku.Value = (productInfo.HasSKU ? "1" : "0");
			HiddenField hiddenField = this.hidAuditStatus;
			int auditStatus = (int)productInfo.AuditStatus;
			hiddenField.Value = auditStatus.ToString();
			this.ofEnableDeduct.Parameter.Add("onSwitchChange", "fuenableDeduct");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenReferral == 1)
			{
				this.hidOpenMultReferral.Value = (masterSettings.OpenMultReferral ? "1" : "0");
			}
			else
			{
				this.hidOpenMultReferral.Value = "0";
			}
			this.ReferralConfig.Visible = (masterSettings.OpenReferral == 1 && true);
			if (!this.Page.IsPostBack || text.ToLower().IndexOf("selectcategory.aspx") > -1)
			{
				this.HidFromUrl.Value = text;
				if (productInfo == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.hidListReturnUrl.Value = this.Page.Request["returnUrl"].ToNullString();
					this.fckDescription.SupplierId = productInfo.SupplierId;
					this.fckmobbileDescription.SupplierId = productInfo.SupplierId;
					this.hidProductId.Value = this.productId.ToString();
					if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
					{
						this.litCategoryName.Text = CatalogHelper.GetFullCategory(this.categoryId);
						this.ViewState["ProductCategoryId"] = this.categoryId;
						this.lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + this.categoryId.ToString(CultureInfo.InvariantCulture);
					}
					else
					{
						this.litCategoryName.Text = CatalogHelper.GetFullCategory(productInfo.CategoryId);
						this.ViewState["ProductCategoryId"] = productInfo.CategoryId;
						HyperLink hyperLink = this.lnkEditCategory;
						auditStatus = productInfo.CategoryId;
						hyperLink.NavigateUrl = "SelectCategory.aspx?categoryId=" + auditStatus.ToString(CultureInfo.InvariantCulture);
					}
					HyperLink hyperLink2 = this.lnkEditCategory;
					string navigateUrl = hyperLink2.NavigateUrl;
					auditStatus = productInfo.ProductId;
					hyperLink2.NavigateUrl = navigateUrl + "&productId=" + auditStatus.ToString(CultureInfo.InvariantCulture);
					this.litralProductTag.SelectedValue = list;
					if (list.Count > 0)
					{
						foreach (int item in list)
						{
							TrimTextBox trimTextBox = this.txtProductTag;
							trimTextBox.Text = trimTextBox.Text + item.ToString() + ",";
						}
						this.txtProductTag.Text = this.txtProductTag.Text.Substring(0, this.txtProductTag.Text.Length - 1);
					}
					this.dropProductTypes.DataBind();
					this.dropBrandCategories.DataBind();
					this.LoadProduct(productInfo, attrs);
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						string text2 = "";
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							text2 = text2 + dataTable.Rows[i]["ValueId"] + "=" + dataTable.Rows[i]["ImageUrl"] + ",";
						}
						text2 = text2.Substring(0, text2.Length - 1);
						this.hidSKUOldImages.Value = text2;
					}
					SiteSettings siteSettings = HiContext.Current.SiteSettings;
					AttributeCollection attributes = this.txtSubMemberDeduct.Attributes;
					decimal num = siteSettings.SubMemberDeduct;
					attributes.Add("placeholder", "全站统一比例：" + num.ToString() + " %");
					AttributeCollection attributes2 = this.txtSecondLevelDeduct.Attributes;
					num = siteSettings.SecondLevelDeduct;
					attributes2.Add("placeholder", "全站统一比例：" + num.ToString() + " %");
					AttributeCollection attributes3 = this.txtThreeLevelDeduct.Attributes;
					num = siteSettings.ThreeLevelDeduct;
					attributes3.Add("placeholder", "全站统一比例：" + num.ToString() + " %");
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (int.Parse(this.hidAuditStatus.Value) == 2)
			{
				this.SaveProduct(true);
			}
			else
			{
				this.SaveProduct(false);
			}
		}

		private void SaveProduct(bool audit)
		{
			if (this.categoryId == 0)
			{
				this.categoryId = (int)this.ViewState["ProductCategoryId"];
			}
			decimal weight = default(decimal);
			decimal num = default(decimal);
			int shippingTemplateId = 0;
			int showSaleCounts = 0;
			int saleCounts = 0;
			int vistiCounts = 0;
			bool isfreeShipping = false;
			int.TryParse(this.txt_ShowSalesCount.Value, out showSaleCounts);
			int.TryParse(this.txt_SalesCount.Value, out saleCounts);
			int.TryParse(this.txt_VistiCounts.Value, out vistiCounts);
			int displaySequence = default(int);
			decimal num2 = default(decimal);
			decimal? nullable = default(decimal?);
			decimal? marketPrice = default(decimal?);
			int stock = default(int);
			int warningStock = default(int);
			decimal? secondLevelDeduct = default(decimal?);
			decimal? subMemberDeduct = default(decimal?);
			decimal? threeLevelDeduct = default(decimal?);
			if (this.ValidateConverts(this.chkSkuEnabled.Checked, out displaySequence, out num2, out nullable, out marketPrice, out stock, out warningStock, out weight, out secondLevelDeduct, out subMemberDeduct, out threeLevelDeduct, out shippingTemplateId, out num, out isfreeShipping))
			{
				string content = Globals.StripScriptTags(this.txtProductName.Text.Trim());
				content = Globals.StripHtmlXmlTags(content).Replace("\\", "").Replace("'", "");
				if (string.IsNullOrEmpty(content) || content == "")
				{
					this.ShowMsg("产品名称不能为空，且不能包含脚本标签、HTML标签、XML标签、反斜杠(\\)、单引号(')！", false);
				}
				else
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
					string text = this.fckDescription.Text;
					string text2 = this.fckmobbileDescription.Text;
					if (this.ckbIsDownPic.Checked)
					{
						text = base.DownRemotePic(text);
						text2 = base.DownRemotePic(text2);
					}
					List<string> list = this.DeleteProductImages();
					ProductImagesInfo productImagesInfo = this.SaveProductImages();
					List<string> list2 = this.DeleteProductSkuImages();
					List<ProductSpecificationImageInfo> attrImgs = this.SaveProductAttributeImages();
					Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
					ProductInfo productById = ProductHelper.GetProductById(this.productId);
					if (SupplierHelper.GetSupplierById(productById.SupplierId).Status == 2)
					{
						this.ShowMsg("供应商已被冻结，商品已不可编辑！", false);
					}
					else
					{
						productById.ShowSaleCounts = showSaleCounts;
						productById.SaleCounts = saleCounts;
						productById.VistiCounts = vistiCounts;
						productById.ProductId = this.productId;
						productById.CategoryId = this.categoryId;
						productById.TypeId = this.dropProductTypes.SelectedValue;
						productById.ProductName = content;
						productById.ProductCode = this.txtProductCode.Text;
						productById.DisplaySequence = displaySequence;
						productById.MarketPrice = marketPrice;
						productById.Unit = this.txtUnit.Text;
						productById.ImageUrl1 = ((this.OldProductInfo != null && this.OldProductInfo.ImageUrl1 != null && this.OldProductInfo.ImageUrl1.Contains("http:") && !list.Contains(this.OldProductInfo.ImageUrl1)) ? this.OldProductInfo.ImageUrl1 : productImagesInfo.ImageUrl1);
						productById.ImageUrl2 = ((this.OldProductInfo != null && this.OldProductInfo.ImageUrl2 != null && this.OldProductInfo.ImageUrl2.Contains("http:") && !list.Contains(this.OldProductInfo.ImageUrl2)) ? this.OldProductInfo.ImageUrl2 : productImagesInfo.ImageUrl2);
						productById.ImageUrl3 = ((this.OldProductInfo != null && this.OldProductInfo.ImageUrl3 != null && this.OldProductInfo.ImageUrl3.Contains("http:") && !list.Contains(this.OldProductInfo.ImageUrl3)) ? this.OldProductInfo.ImageUrl3 : productImagesInfo.ImageUrl3);
						productById.ImageUrl4 = ((this.OldProductInfo != null && this.OldProductInfo.ImageUrl4 != null && this.OldProductInfo.ImageUrl4.Contains("http:") && !list.Contains(this.OldProductInfo.ImageUrl4)) ? this.OldProductInfo.ImageUrl4 : productImagesInfo.ImageUrl4);
						productById.ImageUrl5 = ((this.OldProductInfo != null && this.OldProductInfo.ImageUrl5 != null && this.OldProductInfo.ImageUrl5.Contains("http:") && !list.Contains(this.OldProductInfo.ImageUrl5)) ? this.OldProductInfo.ImageUrl5 : productImagesInfo.ImageUrl5);
						productById.ShortDescription = this.txtShortDescription.Text;
						productById.IsfreeShipping = isfreeShipping;
						productById.Description = ((!string.IsNullOrEmpty(text) && text.Length > 0) ? regex.Replace(text, "") : null);
						productById.MobbileDescription = ((!string.IsNullOrEmpty(text2) && text2.Length > 0) ? regex.Replace(text2, "") : null);
						productById.Title = this.txtTitle.Text;
						productById.Meta_Description = this.txtMetaDescription.Text;
						productById.Meta_Keywords = this.txtMetaKeywords.Text;
						productById.BrandId = this.dropBrandCategories.SelectedValue;
						productById.SecondLevelDeduct = secondLevelDeduct;
						productById.SubMemberDeduct = subMemberDeduct;
						productById.ThreeLevelDeduct = threeLevelDeduct;
						productById.ShippingTemplateId = shippingTemplateId;
						productById.UpdateDate = DateTime.Now;
						productById.ThumbnailUrl40 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl40 : productImagesInfo.ThumbnailUrl40);
						productById.ThumbnailUrl60 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl60 : productImagesInfo.ThumbnailUrl60);
						productById.ThumbnailUrl100 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl100 : productImagesInfo.ThumbnailUrl100);
						productById.ThumbnailUrl160 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl160 : productImagesInfo.ThumbnailUrl160);
						productById.ThumbnailUrl180 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl180 : productImagesInfo.ThumbnailUrl180);
						productById.ThumbnailUrl220 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl220 : productImagesInfo.ThumbnailUrl220);
						productById.ThumbnailUrl310 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl310 : productImagesInfo.ThumbnailUrl310);
						productById.ThumbnailUrl410 = ((this.OldProductInfo != null && productById.ImageUrl1 != null && productById.ImageUrl1.Contains("http:")) ? this.OldProductInfo.ThumbnailUrl410 : productImagesInfo.ThumbnailUrl410);
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
						productById.SaleStatus = saleStatus;
						CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
						if (category != null)
						{
							productById.MainCategoryPath = category.Path + "|";
						}
						Dictionary<string, SKUItem> dictionary = null;
						Dictionary<int, IList<int>> attrs = null;
						if (this.chkSkuEnabled.Checked)
						{
							productById.HasSKU = true;
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
						ValidationResults validationResults = Validation.Validate(productById);
						if (!validationResults.IsValid)
						{
							this.ShowMsg(validationResults);
						}
						else
						{
							IList<int> list3 = new List<int>();
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
									list3.Add(Convert.ToInt32(value));
								}
							}
							switch (ProductHelper.UpdateProduct(productById, dictionary, attrs, list3, attrImgs, ""))
							{
							case ProductActionStatus.Success:
							{
								if (productById.AuditStatus == ProductAuditStatus.Apply & audit)
								{
									if (!ProductHelper.AuditProducts(this.productId.ToString(), "", true))
									{
										this.ShowMsg("审核商品失败", true);
										base.ClientScript.RegisterClientScriptBlock(base.GetType(), "RedirectForAdmin", "<script>ShowSecondMenuLeft('供应商','Supplier/Product/AuditProductList.aspx','Supplier/Product/AuditProductList.aspx');</script>");
										break;
									}
									productById.SaleStatus = ProductSaleStatus.OnSale;
								}
								this.litralProductTag.SelectedValue = list3;
								this.ShowMsg("修改商品成功", true);
								string text4 = this.hidListReturnUrl.Value.ToNullString();
								if (this.HidFromUrl.Value.IndexOf("AuditProductList") > -1)
								{
									if (string.IsNullOrEmpty(text4))
									{
										text4 = "Supplier/Product/AuditProductList.aspx";
									}
									base.ClientScript.RegisterClientScriptBlock(base.GetType(), "RedirectForAdmin", "<script>ShowSecondMenuLeft('供应商','Supplier/Product/AuditProductList.aspx','" + text4 + "');</script>");
								}
								else
								{
									if (string.IsNullOrEmpty(text4))
									{
										text4 = "Supplier/Product/ProductList.aspx";
									}
									base.ClientScript.RegisterClientScriptBlock(base.GetType(), "RedirectForAdmin", "<script>ShowSecondMenuLeft('供应商','Supplier/Product/ProductList.aspx','" + text4 + "');</script>");
								}
								break;
							}
							case ProductActionStatus.AttributeError:
								this.ShowMsg("修改商品失败，保存商品属性时出错", false);
								break;
							case ProductActionStatus.DuplicateName:
								this.ShowMsg("修改商品失败，商品名称不能重复", false);
								break;
							case ProductActionStatus.DuplicateSKU:
								this.ShowMsg("修改商品失败，货号不能重复", false);
								break;
							case ProductActionStatus.SKUError:
								this.ShowMsg("修改商品失败，商家编码不能重复", false);
								break;
							case ProductActionStatus.ProductTagEroor:
								this.ShowMsg("修改商品失败，保存商品标签时出错", false);
								break;
							default:
								this.ShowMsg("修改商品失败，未知错误", false);
								break;
							}
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
			displaySequence = (stock = (warningStock = 0));
			salePrice = default(decimal);
			volume = default(decimal);
			shippingTemplateId = 0;
			isFreeShipping = false;
			if (string.IsNullOrEmpty(this.txtDisplaySequence.Text) || !int.TryParse(this.txtDisplaySequence.Text, out displaySequence))
			{
				text += Formatter.FormatErrorMessage("请正确填写商品排序");
			}
			if (this.txtProductCode.Text.Length > 20)
			{
				text += Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
			}
			if (!string.IsNullOrEmpty(Globals.StripAllTags(this.txtProductCode.Text)) && ProductHelper.IsExistsProductCode(Globals.StripAllTags(this.txtProductCode.Text), this.productId))
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
			if (!string.IsNullOrEmpty(this.txtSecondLevelDeduct.Text))
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
			if (!string.IsNullOrEmpty(this.txtThreeLevelDeduct.Text))
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

		private void LoadProduct(ProductInfo product, Dictionary<int, IList<int>> attrs)
		{
			HiddenField hiddenField = this.txt_SalesCount;
			int num = product.SaleCounts;
			hiddenField.Value = num.ToString();
			HiddenField hiddenField2 = this.txt_ShowSalesCount;
			num = product.ShowSaleCounts;
			hiddenField2.Value = num.ToString();
			HiddenField hiddenField3 = this.txt_VistiCounts;
			num = product.VistiCounts;
			hiddenField3.Value = num.ToString();
			this.dropProductTypes.SelectedValue = product.TypeId;
			this.dropBrandCategories.SelectedValue = product.BrandId;
			this.dropProductTypes.Enabled = false;
			TrimTextBox trimTextBox = this.txtDisplaySequence;
			num = product.DisplaySequence;
			trimTextBox.Text = num.ToString();
			this.txtProductName.Text = Globals.HtmlDecode(product.ProductName);
			this.txtProductCode.Text = product.ProductCode;
			this.txtUnit.Text = product.Unit;
			if (product.MarketPrice.HasValue)
			{
				this.txtMarketPrice.Text = product.MarketPrice.Value.F2ToString("f2");
			}
			if (product.SecondLevelDeduct.HasValue)
			{
				this.txtSecondLevelDeduct.Text = product.SecondLevelDeduct.Value.F2ToString("f2");
			}
			if (product.SubMemberDeduct.HasValue)
			{
				this.txtSubMemberDeduct.Text = product.SubMemberDeduct.Value.F2ToString("f2");
			}
			if (product.ThreeLevelDeduct.HasValue)
			{
				this.txtThreeLevelDeduct.Text = product.ThreeLevelDeduct.Value.F2ToString("f2");
			}
			int shippingTemplateId = product.ShippingTemplateId;
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
			this.txtVolume.Text = product.Weight.F2ToString("f2");
			this.ShippingTemplatesDropDownList.DataBind();
			if (shippingTemplate != null)
			{
				this.ShippingTemplatesDropDownList.SelectedValue = shippingTemplateId;
			}
			if (this.ShippingTemplatesDropDownList.SelectedValue.HasValue)
			{
				this.ShippingTemplatesDropDownList.Enabled = false;
			}
			if (!product.SecondLevelDeduct.HasValue || product.SecondLevelDeduct.Value != decimal.Zero || !product.SubMemberDeduct.HasValue || product.SubMemberDeduct.Value != decimal.Zero || !product.ThreeLevelDeduct.HasValue || product.ThreeLevelDeduct.Value != decimal.Zero)
			{
				this.liSubMemberDeduct.Style.Add("display", "");
				if (HiContext.Current.SiteSettings.OpenMultReferral)
				{
					this.liReferralDeduct.Style.Add("display", "");
					this.liSubReferralDeduct.Style.Add("display", "");
				}
				this.ofEnableDeduct.SelectedValue = true;
			}
			else
			{
				this.ofEnableDeduct.SelectedValue = false;
			}
			this.txtShortDescription.Text = product.ShortDescription;
			this.ChkisfreeShipping.SelectedValue = product.IsfreeShipping;
			this.fckDescription.Text = product.Description;
			this.fckmobbileDescription.Text = product.MobbileDescription;
			this.txtTitle.Text = product.Title;
			this.txtMetaDescription.Text = product.Meta_Description;
			this.txtMetaKeywords.Text = product.Meta_Keywords;
			if (product.SaleStatus == ProductSaleStatus.OnSale)
			{
				this.radOnSales.Checked = true;
			}
			else if (product.SaleStatus == ProductSaleStatus.UnSale)
			{
				this.radUnSales.Checked = true;
			}
			else
			{
				this.radInStock.Checked = true;
			}
			string empty = string.Empty;
			empty = empty + product.ImageUrl1 + ",";
			empty = empty + product.ImageUrl2 + ",";
			empty = empty + product.ImageUrl3 + ",";
			empty = empty + product.ImageUrl4 + ",";
			empty += product.ImageUrl5;
			this.hidOldImages.Value = empty;
			if (attrs != null && attrs.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<xml><attributes>");
				foreach (int key in attrs.Keys)
				{
					StringBuilder stringBuilder2 = stringBuilder.Append("<item attributeId=\"").Append(key.ToString(CultureInfo.InvariantCulture)).Append("\" usageMode=\"");
					num = (int)ProductTypeHelper.GetAttribute(key).UsageMode;
					stringBuilder2.Append(num.ToString()).Append("\" >");
					foreach (int item in attrs[key])
					{
						stringBuilder.Append("<attValue valueId=\"").Append(item.ToString(CultureInfo.InvariantCulture)).Append("\" />");
					}
					stringBuilder.Append("</item>");
				}
				stringBuilder.Append("</attributes></xml>");
				this.txtAttributes.Text = stringBuilder.ToString();
			}
			this.chkSkuEnabled.Checked = product.HasSKU;
			if (product.HasSKU)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append("<xml><productSkus>");
				foreach (string key2 in product.Skus.Keys)
				{
					SKUItem sKUItem = product.Skus[key2];
					string[] obj = new string[13]
					{
						"<item skuCode=\"",
						sKUItem.SKU,
						"\" salePrice=\"",
						sKUItem.SalePrice.F2ToString("f2"),
						"\" costPrice=\"",
						(sKUItem.CostPrice > decimal.Zero) ? sKUItem.CostPrice.F2ToString("f2") : "",
						"\" qty=\"",
						null,
						null,
						null,
						null,
						null,
						null
					};
					num = sKUItem.Stock;
					obj[7] = num.ToString(CultureInfo.InvariantCulture);
					obj[8] = "\" warningQty=\"";
					num = sKUItem.WarningStock;
					obj[9] = num.ToString(CultureInfo.InvariantCulture);
					obj[10] = "\" weight=\"";
					obj[11] = ((sKUItem.Weight > decimal.Zero) ? sKUItem.Weight.F2ToString("f2") : "");
					obj[12] = "\">";
					string str = string.Concat(obj);
					str += "<skuFields>";
					foreach (int key3 in sKUItem.SkuItems.Keys)
					{
						string[] obj2 = new string[5]
						{
							"<sku attributeId=\"",
							key3.ToString(CultureInfo.InvariantCulture),
							"\" valueId=\"",
							null,
							null
						};
						num = sKUItem.SkuItems[key3];
						obj2[3] = num.ToString(CultureInfo.InvariantCulture);
						obj2[4] = "\" />";
						string str2 = string.Concat(obj2);
						str += str2;
					}
					str += "</skuFields>";
					if (sKUItem.MemberPrices.Count > 0)
					{
						str += "<memberPrices>";
						foreach (int key4 in sKUItem.MemberPrices.Keys)
						{
							str += string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", key4.ToString(CultureInfo.InvariantCulture), sKUItem.MemberPrices[key4].F2ToString("f2"));
						}
						str += "</memberPrices>";
					}
					str += "</item>";
					stringBuilder3.Append(str);
				}
				stringBuilder3.Append("</productSkus></xml>");
				this.txtSkus.Text = stringBuilder3.ToString();
			}
			SKUItem defaultSku = product.DefaultSku;
			this.txtSku.Text = product.SKU;
			this.txtSalePrice.Text = defaultSku.SalePrice.F2ToString("f2");
			this.txtCostPrice.Text = ((defaultSku.CostPrice > decimal.Zero) ? defaultSku.CostPrice.F2ToString("f2") : "");
			TrimTextBox trimTextBox2 = this.txtStock;
			num = defaultSku.Stock;
			trimTextBox2.Text = num.ToString(CultureInfo.InvariantCulture);
			TrimTextBox trimTextBox3 = this.txtWarningStock;
			num = defaultSku.WarningStock;
			trimTextBox3.Text = num.ToString(CultureInfo.InvariantCulture);
			this.txtWeight.Text = ((defaultSku.Weight > decimal.Zero) ? defaultSku.Weight.F2ToString("f2") : "0");
			if (product.ReferralDeduct.HasValue || product.SubMemberDeduct.HasValue || product.SubReferralDeduct.HasValue)
			{
				this.liSubMemberDeduct.Style.Add("display", "");
				if (HiContext.Current.SiteSettings.OpenMultReferral)
				{
					this.liReferralDeduct.Style.Add("display", "");
					this.liSubReferralDeduct.Style.Add("display", "");
				}
			}
			if (defaultSku.MemberPrices.Count > 0)
			{
				this.txtMemberPrices.Text = "<xml><gradePrices>";
				foreach (int key5 in defaultSku.MemberPrices.Keys)
				{
					TrimTextBox trimTextBox4 = this.txtMemberPrices;
					trimTextBox4.Text += string.Format("<grande id=\"{0}\" price=\"{1}\" />", key5.ToString(CultureInfo.InvariantCulture), defaultSku.MemberPrices[key5].F2ToString("f2"));
				}
				TrimTextBox trimTextBox5 = this.txtMemberPrices;
				trimTextBox5.Text += "</gradePrices></xml>";
			}
		}

		private List<string> DeleteProductImages()
		{
			try
			{
				string uploadPath = HiContext.Current.GetStoragePath() + "product/";
				string tempPath = HiContext.Current.GetStoragePath() + "temp/";
				string originalSavePath = HttpContext.Current.Server.MapPath(uploadPath + "images/");
				string[] source = this.hidOldImages.Value.Trim().Split(',');
				string[] imgSrcs = this.hidUploadImages.Value.Trim().Split(',');
				List<string> list = (from a in source
				where !imgSrcs.Contains(a) && a.Length > 0
				select a).ToList();
				list.ForEach(delegate(string c)
				{
					c = c.Replace("//", "/");
					if (c.Length > 0 && !c.Contains("http:"))
					{
						string text = c.Split('/')[5];
						string path = uploadPath + "thumbs40/40_" + text;
						string path2 = uploadPath + "thumbs60/60_" + text;
						string path3 = uploadPath + "thumbs100/100_" + text;
						string path4 = uploadPath + "thumbs160/160_" + text;
						string path5 = uploadPath + "thumbs180/180_" + text;
						string path6 = uploadPath + "thumbs220/220_" + text;
						string path7 = uploadPath + "thumbs310/310_" + text;
						string path8 = uploadPath + "thumbs410/410_" + text;
						string path9 = originalSavePath + text;
						if (File.Exists(path9))
						{
							File.Delete(path9);
						}
						string path10 = HttpContext.Current.Server.MapPath(tempPath + text);
						if (File.Exists(path10))
						{
							File.Delete(path10);
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path2)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path2));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path3)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path3));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path4)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path4));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path5)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path5));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path6)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path6));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path7)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path7));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path8)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path8));
						}
					}
				});
				return list;
			}
			catch
			{
				return null;
			}
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
				if (text3.Length != 0 && !text3.Contains("http:"))
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
						EditProduct.BindProductImageInfo(text, productImagesInfo, i, text4, text5, text6, text7, text8, text9, text10, text11, text12);
					}
					else if (File.Exists(HttpContext.Current.Server.MapPath(text3)))
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
						EditProduct.BindProductImageInfo(text, productImagesInfo, i, text4, text5, text6, text7, text8, text9, text10, text11, text12);
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

		private List<string> DeleteProductSkuImages()
		{
			try
			{
				string uploadPath = HiContext.Current.GetStoragePath() + "sku/";
				string tempPath = HiContext.Current.GetStoragePath() + "temp/";
				string originalSavePath = HttpContext.Current.Server.MapPath(uploadPath + "images/");
				string[] source = this.hidOldImages.Value.Trim().Split(',');
				string[] imgSrcs = this.hidUploadImages.Value.Trim().Split(',');
				List<string> list = (from a in source
				where !imgSrcs.Contains(a) && a.Length > 0
				select a).ToList();
				list.ForEach(delegate(string m)
				{
					string text = m.Split('=')[1];
					text = text.Replace("//", "/");
					if (text.Length > 0 && !text.Contains("http:"))
					{
						string text2 = text.Split('/')[5];
						string path = uploadPath + "thumbs40/40_" + text2;
						string path2 = uploadPath + "thumbs410/410_" + text2;
						string path3 = originalSavePath + text2;
						if (File.Exists(path3))
						{
							File.Delete(path3);
						}
						string path4 = HttpContext.Current.Server.MapPath(tempPath + text2);
						if (File.Exists(path4))
						{
							File.Delete(path4);
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path));
						}
						if (File.Exists(HttpContext.Current.Server.MapPath(path2)))
						{
							File.Delete(HttpContext.Current.Server.MapPath(path2));
						}
					}
				});
				return list;
			}
			catch
			{
				return null;
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
							productSpecificationImageInfo.ValueId = int.Parse(text4.ToString());
							productSpecificationImageInfo.ThumbnailUrl40 = text7;
							productSpecificationImageInfo.ThumbnailUrl410 = text8;
							if (File.Exists(str + text6))
							{
								productSpecificationImageInfo.ImageUrl = text + "images/" + text6;
								list.Add(productSpecificationImageInfo);
							}
							else if (File.Exists(HttpContext.Current.Server.MapPath(text5)))
							{
								File.Copy(HttpContext.Current.Server.MapPath(text5), str + text6);
								if (File.Exists(HttpContext.Current.Server.MapPath(text5)))
								{
									File.Delete(HttpContext.Current.Server.MapPath(text5));
								}
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

		private void HasActivitiesByProductId(ProductInfo product)
		{
			if (PromoteHelper.ProductCountDownExist(product.ProductId))
			{
				this.ShowMsg("商品正在参加限时购不能编辑规格、价格、库存以及销售状态", true);
				this.setControlEnable(product.HasSKU);
			}
			else if (PromoteHelper.ProductGroupBuyExist(product.ProductId))
			{
				this.ShowMsg("商品正在参加团购不能编辑规格、价格、库存以及销售状态等", true);
				this.setControlEnable(product.HasSKU);
			}
			else if (VShopHelper.ExistEffectiveFightGroupInfo(product.ProductId))
			{
				this.ShowMsg("商品正在参加火拼团不能编辑规格、价格、库存以及销售状态等", true);
				this.setControlEnable(product.HasSKU);
			}
			else if (CombinationBuyHelper.ExistEffectiveCombinationBuyInfo(product.ProductId))
			{
				this.ShowMsg("商品正在参加组合购不能编辑规格、价格、库存以及销售状态等", true);
				this.setControlEnable(product.HasSKU);
			}
		}

		protected void btnSaveAudit_Click(object sender, EventArgs e)
		{
			this.SaveProduct(true);
		}

		private void setControlEnable(bool hassku)
		{
			this.hidHasActivity.Value = "1";
			if (hassku)
			{
				this.hidHasActivity.Value = "2";
			}
			this.txtSalePrice.Enabled = false;
			this.txtStock.Enabled = false;
			this.radInStock.Enabled = false;
			this.radOnSales.Enabled = false;
			this.radUnSales.Enabled = false;
			this.dropProductTypes.Enabled = false;
		}
	}
}
