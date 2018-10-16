using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class EditCombinationBuy : AdminPage
	{
		private int combinationId;

		protected HiddenField hidMainProductId;

		protected HiddenField hidMainProductSkuId;

		protected HiddenField hidOtherProductIds;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidSubmitData;

		protected HiddenField hidCombinationId;

		protected HiddenField hidEditType;

		protected TextBox txtCombinationPrice;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected Button btnAddCoupons;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddCoupons.Click += this.btnAddCoupons_Click;
			if (!base.IsPostBack)
			{
				if (!int.TryParse(this.Page.Request.QueryString["combinationId"], out this.combinationId) || string.IsNullOrEmpty(this.Page.Request.QueryString["editType"]))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					CombinationBuyInfo combinationBuyById = CombinationBuyHelper.GetCombinationBuyById(this.combinationId);
					if (combinationBuyById != null)
					{
						HiddenField hiddenField = this.hidMainProductId;
						int mainProductId = combinationBuyById.MainProductId;
						hiddenField.Value = mainProductId.ToString();
						this.hidOtherProductIds.Value = combinationBuyById.OtherProductIds;
						HiddenField hiddenField2 = this.hidCombinationId;
						mainProductId = combinationBuyById.CombinationId;
						hiddenField2.Value = mainProductId.ToString();
						this.calendarStartDate.SelectedDate = combinationBuyById.StartDate;
						this.calendarEndDate.SelectedDate = combinationBuyById.EndDate;
						this.hidEditType.Value = this.Page.Request.QueryString["editType"];
					}
					else
					{
						base.GotoResourceNotFound();
					}
				}
			}
		}

		private void btnAddCoupons_Click(object sender, EventArgs e)
		{
			int num = 0;
			int.TryParse(this.hidCombinationId.Value, out num);
			if (num == 0)
			{
				this.ShowMsg("请重新编辑！", false);
			}
			else
			{
				int num2 = 0;
				int.TryParse(this.hidMainProductId.Value, out num2);
				if (num2 <= 0)
				{
					this.ShowMsg("请选择主商品！", false);
				}
				else if (string.IsNullOrEmpty(this.hidOtherProductIds.Value))
				{
					this.ShowMsg("请至少选择一个组合商品！", false);
				}
				else
				{
					DateTime dateTime;
					DateTime now;
					if (this.hidEditType.Value != "2")
					{
						if (!this.calendarStartDate.SelectedDate.HasValue)
						{
							this.ShowMsg("请选择开始日期！", false);
							return;
						}
						dateTime = this.calendarStartDate.SelectedDate.Value;
						now = DateTime.Now;
						if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd"))) < 0)
						{
							this.ShowMsg("开始日期不能早于当前时间！", false);
							return;
						}
					}
					if (!this.calendarEndDate.SelectedDate.HasValue)
					{
						this.ShowMsg("请选择结束日期！", false);
					}
					else
					{
						dateTime = this.calendarStartDate.SelectedDate.Value;
						if (dateTime.CompareTo(this.calendarEndDate.SelectedDate.Value) > 0)
						{
							this.ShowMsg("开始日期不能晚于结束日期！", false);
						}
						else
						{
							dateTime = this.calendarEndDate.SelectedDate.Value;
							now = DateTime.Now;
							if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd"))) < 0)
							{
								this.ShowMsg("结束日期不能早于当前时间！", false);
							}
							else
							{
								List<CombinationBuySKUInfo> items = JsonHelper.ParseFormJson<List<CombinationBuySKUInfo>>(this.hidSubmitData.Value);
								CombinationBuyInfo combinationBuyInfo = new CombinationBuyInfo();
								combinationBuyInfo.CombinationId = num;
								combinationBuyInfo.OtherProductIds = this.hidOtherProductIds.Value;
								CombinationBuyInfo combinationBuyInfo2 = combinationBuyInfo;
								dateTime = this.calendarStartDate.SelectedDate.Value;
								combinationBuyInfo2.StartDate = dateTime.Date;
								CombinationBuyInfo combinationBuyInfo3 = combinationBuyInfo;
								dateTime = this.calendarEndDate.SelectedDate.Value;
								dateTime = dateTime.Date;
								dateTime = dateTime.AddDays(1.0);
								combinationBuyInfo3.EndDate = dateTime.AddSeconds(-1.0);
								combinationBuyInfo.MainProductId = num2;
								if (CombinationBuyHelper.UpdateCombinationBuy(combinationBuyInfo, items))
								{
									this.ShowMsg("编辑组合购活动成功！", true, "CombinationBuy.aspx");
								}
								else
								{
									this.ShowMsg("编辑失败！", false);
								}
							}
						}
					}
				}
			}
		}
	}
}
