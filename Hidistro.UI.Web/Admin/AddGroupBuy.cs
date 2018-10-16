using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.GroupBuy)]
	public class AddGroupBuy : AdminPage
	{
		private int productId;

		protected Literal ltProductName;

		protected HtmlGenericControl li_price;

		protected Label lblPrice;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected TextBox txtNeedPrice;

		protected TextBox txtMaxCount;

		protected TextBox txtCount;

		protected TextBox txtPrice;

		protected TextBox txtContent;

		protected Button btnAddGroupBuy;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productId = this.Page.Request["productId"].ToInt(0);
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				this.DoCallback();
			}
			this.SetDateControl();
			this.btnAddGroupBuy.Click += this.btnAddGroupBuy_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		private void SetDateControl()
		{
			Dictionary<string, object> calendarParameter = this.calendarStartDate.CalendarParameter;
			DateTime now = DateTime.Now;
			calendarParameter.Add("startDate ", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter2 = this.calendarEndDate.CalendarParameter;
			now = DateTime.Now;
			calendarParameter2.Add("startDate ", now.ToString("yyyy-MM-dd"));
			this.calendarStartDate.FunctionNameForChangeDate = "fuChangeStartDate";
			this.calendarEndDate.FunctionNameForChangeDate = "fuChangeEndDate";
			this.calendarStartDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarStartDate.CalendarParameter["minView"] = "0";
			this.calendarEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarEndDate.CalendarParameter["minView"] = "0";
		}

		private void BindProduct()
		{
			if (this.productId != 0)
			{
				IList<int> list = null;
				Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out dictionary, out list);
				if (productDetails != null)
				{
					this.ltProductName.Text = productDetails.ProductName;
					this.lblPrice.Text = productDetails.MinSalePrice.F2ToString("f2");
					this.li_price.Style.Add("display", "block");
				}
			}
		}

		private void btnAddGroupBuy_Click(object sender, EventArgs e)
		{
			GroupBuyInfo groupBuyInfo = new GroupBuyInfo();
			string text = string.Empty;
			if (this.productId > 0)
			{
				if (PromoteHelper.ProductGroupBuyExist(this.productId))
				{
					text = "已经存在此商品的团购活动，并且活动正在进行中";
				}
				groupBuyInfo.ProductId = this.productId;
			}
			else
			{
				text += Formatter.FormatErrorMessage("请选择团购商品");
			}
			if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择开始日期");
			}
			else
			{
				groupBuyInfo.StartDate = this.calendarStartDate.SelectedDate.Value;
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择结束日期");
			}
			else
			{
				groupBuyInfo.EndDate = this.calendarEndDate.SelectedDate.Value;
				if (groupBuyInfo.StartDate >= groupBuyInfo.EndDate)
				{
					text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
				}
			}
			if (!string.IsNullOrEmpty(this.txtNeedPrice.Text))
			{
				decimal needPrice = default(decimal);
				if (decimal.TryParse(this.txtNeedPrice.Text.Trim(), out needPrice))
				{
					groupBuyInfo.NeedPrice = needPrice;
				}
				else
				{
					text += Formatter.FormatErrorMessage("违约金填写格式不正确");
				}
			}
			int maxCount = default(int);
			if (int.TryParse(this.txtMaxCount.Text.Trim(), out maxCount))
			{
				groupBuyInfo.MaxCount = maxCount;
			}
			else
			{
				text += Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
			}
			int count = default(int);
			if (int.TryParse(this.txtCount.Text.Trim(), out count))
			{
				groupBuyInfo.Count = count;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
			}
			decimal price = default(decimal);
			if (decimal.TryParse(this.txtPrice.Text.Trim(), out price))
			{
				groupBuyInfo.Price = price;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
			}
			if (groupBuyInfo.MaxCount < groupBuyInfo.Count)
			{
				text += Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
			}
			else
			{
				groupBuyInfo.Content = this.txtContent.Text;
				groupBuyInfo.Status = GroupBuyStatus.UnderWay;
				if (PromoteHelper.AddGroupBuy(groupBuyInfo))
				{
					this.ShowMsg("添加团购活动成功", true, "GroupBuys.aspx");
				}
				else
				{
					this.ShowMsg("添加团购活动失败", true);
				}
			}
		}

		private void DoCallback()
		{
			base.Response.Clear();
			base.Response.ContentType = "application/json";
			string text = base.Request.QueryString["action"];
			if (text.Equals("getGroupBuyProducts"))
			{
				ProductQuery productQuery = new ProductQuery();
				if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
				{
					int num = default(int);
					int.TryParse(base.Request.QueryString["categoryId"], out num);
					if (num > 0)
					{
						productQuery.CategoryId = num;
						productQuery.MaiCategoryPath = CatalogHelper.GetCategory(num).Path;
					}
				}
				string productCode = base.Request.QueryString["sku"];
				string text3 = productQuery.Keywords = base.Request.QueryString["productName"];
				productQuery.ProductCode = productCode;
				productQuery.SaleStatus = ProductSaleStatus.OnSale;
				DataTable groupBuyProducts = ProductHelper.GetGroupBuyProducts(productQuery);
				if (groupBuyProducts == null || groupBuyProducts.Rows.Count == 0)
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{\"Status\":\"OK\",");
					stringBuilder.AppendFormat("\"Product\":[{0}]", this.GenerateBrandString(groupBuyProducts));
					stringBuilder.Append("}");
					base.Response.Write(stringBuilder.ToString());
				}
			}
			base.Response.End();
		}

		private string GenerateBrandString(DataTable tb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow row in tb.Rows)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"ProductId\":\"{0}\",\"ProductName\":\"{1}\"", row["ProductId"], Uri.EscapeDataString(row["ProductName"].ToString()));
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
	}
}
