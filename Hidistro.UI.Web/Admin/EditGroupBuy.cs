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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.GroupBuy)]
	public class EditGroupBuy : AdminPage
	{
		private int groupBuyId;

		private int productId;

		protected Literal ltProductName;

		protected HtmlAnchor selectProductA;

		protected Label lblPrice;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected TextBox txtNeedPrice;

		protected TextBox txtMaxCount;

		protected TextBox txtCount;

		protected TextBox txtPrice;

		protected TextBox txtContent;

		protected Button btnUpdateGroupBuy;

		protected Button btnFinish;

		protected Button btnSuccess;

		protected Button btnFail;

		protected HiddenField hfGroupId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productId = this.Page.Request["productId"].ToInt(0);
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				if (int.TryParse(base.Request["productId"], out this.productId))
				{
					string priceByProductId = PromoteHelper.GetPriceByProductId(this.productId);
					if (priceByProductId.Length > 0)
					{
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						base.Response.Write("{ ");
						base.Response.Write("\"Status\":\"OK\",");
						base.Response.Write(string.Format("\"Price\":\"{0}\"", decimal.Parse(priceByProductId).F2ToString("f2")));
						base.Response.Write("}");
						base.Response.End();
					}
				}
			}
			else if (!int.TryParse(base.Request.QueryString["groupBuyId"], out this.groupBuyId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.hfGroupId.Value = base.Request.QueryString["groupBuyId"].ToString();
				this.btnUpdateGroupBuy.Click += this.btnUpdateGroupBuy_Click;
				this.btnFail.Click += this.btnFail_Click;
				this.btnSuccess.Click += this.btnSuccess_Click;
				this.btnFinish.Click += this.btnFinish_Click;
				this.BindProduct();
				this.SetDateControl();
				if (!base.IsPostBack)
				{
					GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(this.groupBuyId);
					if (PromoteHelper.GetOrderCount(this.groupBuyId) > 0)
					{
						this.selectProductA.Disabled = true;
					}
					if (groupBuy == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						if (groupBuy.Status == GroupBuyStatus.EndUntreated)
						{
							this.btnFail.Visible = true;
							this.btnSuccess.Visible = true;
							this.selectProductA.Disabled = true;
							this.calendarStartDate.Enabled = false;
							this.calendarEndDate.Enabled = false;
							this.txtNeedPrice.Enabled = false;
							this.txtMaxCount.Enabled = false;
							this.txtCount.Enabled = false;
							this.txtPrice.Enabled = false;
							this.txtContent.Enabled = false;
							this.btnUpdateGroupBuy.Enabled = false;
						}
						if (groupBuy.Status == GroupBuyStatus.UnderWay)
						{
							this.selectProductA.Disabled = true;
							this.btnFinish.Visible = true;
						}
						this.LoadGroupBuy(groupBuy);
					}
				}
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
			GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(this.groupBuyId);
			this.productId = ((this.productId == 0) ? groupBuy.ProductId : this.productId);
			IList<int> list = null;
			Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
			ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out dictionary, out list);
			if (productDetails != null)
			{
				this.ltProductName.Text = productDetails.ProductName;
				this.lblPrice.Text = productDetails.MinSalePrice.F2ToString("f2");
			}
		}

		private void btnFinish_Click(object sender, EventArgs e)
		{
			if (PromoteHelper.SetGroupBuyEndUntreated(this.groupBuyId))
			{
				this.btnFail.Visible = true;
				this.btnSuccess.Visible = true;
				this.btnFinish.Visible = false;
				this.ShowMsg("成功设置团购活动为结束状态", true);
			}
			else
			{
				this.ShowMsg("设置团购活动状态失败", true);
			}
		}

		private void btnFail_Click(object sender, EventArgs e)
		{
			if (PromoteHelper.SetGroupBuyStatus(this.groupBuyId, GroupBuyStatus.Failed))
			{
				this.btnFail.Visible = false;
				this.btnSuccess.Visible = false;
				this.ShowMsg("成功设置团购活动为失败状态", true);
			}
			else
			{
				this.ShowMsg("设置团购活动状态失败", true);
			}
		}

		private void btnSuccess_Click(object sender, EventArgs e)
		{
			if (PromoteHelper.SetGroupBuyStatus(this.groupBuyId, GroupBuyStatus.Success))
			{
				this.btnFail.Visible = false;
				this.btnSuccess.Visible = false;
				this.ShowMsg("成功设置团购活动为成功状态", true);
			}
			else
			{
				this.ShowMsg("设置团购活动状态失败", true);
			}
		}

		private void LoadGroupBuy(GroupBuyInfo groupBuy)
		{
			TextBox textBox = this.txtPrice;
			decimal num = groupBuy.Price;
			textBox.Text = num.ToString("F");
			this.txtContent.Text = Globals.HtmlDecode(groupBuy.Content);
			TextBox textBox2 = this.txtMaxCount;
			int num2 = groupBuy.MaxCount;
			textBox2.Text = num2.ToString();
			TextBox textBox3 = this.txtCount;
			num2 = groupBuy.Count;
			textBox3.Text = num2.ToString();
			TextBox textBox4 = this.txtNeedPrice;
			num = groupBuy.NeedPrice;
			textBox4.Text = num.ToString("F");
			this.calendarEndDate.SelectedDate = groupBuy.EndDate;
			this.calendarStartDate.SelectedDate = groupBuy.StartDate;
		}

		private void btnUpdateGroupBuy_Click(object sender, EventArgs e)
		{
			GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(this.groupBuyId);
			string text = string.Empty;
			if (this.productId > 0)
			{
				if (groupBuy.ProductId != this.productId && PromoteHelper.ProductGroupBuyExist(this.productId))
				{
					this.ShowMsg("已经存在此商品的团购活动，并且活动正在进行中", false);
					return;
				}
				groupBuy.ProductId = this.productId;
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
				groupBuy.StartDate = this.calendarStartDate.SelectedDate.Value;
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择结束日期");
			}
			else
			{
				groupBuy.EndDate = this.calendarEndDate.SelectedDate.Value;
				if (groupBuy.StartDate >= groupBuy.EndDate)
				{
					text += Formatter.FormatErrorMessage("开始日期必须要早于结束日期");
				}
			}
			if (!string.IsNullOrEmpty(this.txtNeedPrice.Text))
			{
				decimal needPrice = default(decimal);
				if (decimal.TryParse(this.txtNeedPrice.Text.Trim(), out needPrice))
				{
					groupBuy.NeedPrice = needPrice;
				}
				else
				{
					text += Formatter.FormatErrorMessage("违约金填写格式不正确");
				}
			}
			int maxCount = default(int);
			if (int.TryParse(this.txtMaxCount.Text.Trim(), out maxCount))
			{
				groupBuy.MaxCount = maxCount;
			}
			else
			{
				text += Formatter.FormatErrorMessage("限购数量不能为空，只能为整数");
			}
			groupBuy.Content = this.txtContent.Text;
			int count = default(int);
			if (int.TryParse(this.txtCount.Text.Trim(), out count))
			{
				groupBuy.Count = count;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购满足数量不能为空，只能为整数");
			}
			decimal price = default(decimal);
			if (decimal.TryParse(this.txtPrice.Text.Trim(), out price))
			{
				groupBuy.Price = price;
			}
			else
			{
				text += Formatter.FormatErrorMessage("团购价格不能为空，只能为数值类型");
			}
			if (groupBuy.MaxCount < groupBuy.Count)
			{
				text += Formatter.FormatErrorMessage("限购数量必须大于等于满足数量 ");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
			}
			else if (PromoteHelper.UpdateGroupBuy(groupBuy))
			{
				this.ShowMsg("编辑团购活动成功", true, "GroupBuys.aspx");
			}
			else
			{
				this.ShowMsg("编辑团购活动失败", true);
			}
		}
	}
}
