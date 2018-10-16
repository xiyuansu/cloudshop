using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	public class SearchPromotionProduct : AdminCallBackPage
	{
		protected string productName;

		private int? categoryId;

		private int? brandId;

		private int? tagId;

		private int activityId;

		private bool IsMobileExclusive = false;

		protected HtmlInputHidden hdIsMobileExclusive;

		protected HtmlInputHidden hdActivityId;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected ProductTagsDropDownList dropTagList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected ImageLinkButton btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				this.MobileExclusiveHadler();
			}
			else if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.LoadParameters();
				this.hdActivityId.Value = this.activityId.ToString();
				bool.TryParse(this.Page.Request.QueryString["IsMobileExclusive"], out this.IsMobileExclusive);
				if (this.IsMobileExclusive)
				{
					this.hdIsMobileExclusive.Value = "1";
				}
				this.btnAdd.Click += this.btnAdd_Click;
			}
		}

		public void MobileExclusiveHadler()
		{
			try
			{
				this.Page.Response.Clear();
				this.Page.Response.ContentType = "application/json";
				string productIds = this.Page.Request["productIds"];
				int num = 0;
				if (!int.TryParse(this.Page.Request["activityId"], out num))
				{
					this.Page.Response.Write("{\"Status\":\"001\"}");
				}
				else
				{
					DataTable productSalepriceInfo = ProductHelper.GetProductSalepriceInfo(productIds);
					if (productSalepriceInfo != null && productSalepriceInfo.Rows.Count > 0)
					{
						PromotionInfo promotion = PromoteHelper.GetPromotion(num);
						if (promotion == null)
						{
							this.Page.Response.Write("{\"Status\":\"002\"}");
						}
						else
						{
							bool flag = false;
							foreach (DataRow row in productSalepriceInfo.Rows)
							{
								decimal d = Convert.ToDecimal(row["MinSalePrice"]);
								if (d - promotion.DiscountValue <= decimal.Zero)
								{
									flag = true;
									break;
								}
							}
							this.Page.Response.Write("{\"Status\":\"" + (flag ? "004" : "0") + "\"}");
						}
					}
					else
					{
						this.Page.Response.Write("{\"Status\":\"003\"}");
					}
				}
			}
			catch (Exception)
			{
				this.Page.Response.Write("{\"Status\":\"001\"}");
			}
			finally
			{
				base.Response.End();
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择一件商品！", false);
			}
			else if (PromoteHelper.AddPromotionProducts(this.activityId, text, this.IsMobileExclusive))
			{
				base.CloseWindow(null);
			}
			else
			{
				this.ShowMsg("选择的商品没有添加到此促销活动中！", false);
			}
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = value;
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.brandId = value2;
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value2))
			{
				this.tagId = value3;
			}
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropBrandList.DataBind();
			this.dropBrandList.SelectedValue = value2;
			this.dropTagList.DataBind();
			this.dropBrandList.SelectedValue = value3;
		}
	}
}
