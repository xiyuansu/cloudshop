using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddCombinationBuy : AdminPage
	{
		protected HiddenField hidMainProductId;

		protected HiddenField hidMainProductSkuId;

		protected HiddenField hidOtherProductIds;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidSubmitData;

		protected TextBox txtCombinationPrice;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected Button btnAddCoupons;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddCoupons.Click += this.btnAddCoupons_Click;
		}

		private void btnAddCoupons_Click(object sender, EventArgs e)
		{
			int num = 0;
			int.TryParse(this.hidMainProductId.Value, out num);
			if (num <= 0)
			{
				this.ShowMsg("请选择主商品！", false);
			}
			else if (string.IsNullOrEmpty(this.hidOtherProductIds.Value))
			{
				this.ShowMsg("请至少选择一个组合商品！", false);
			}
			else if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择开始日期！", false);
			}
			else if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择结束日期！", false);
			}
			else
			{
				DateTime dateTime = this.calendarStartDate.SelectedDate.Value;
				if (dateTime.CompareTo(this.calendarEndDate.SelectedDate.Value) > 0)
				{
					this.ShowMsg("开始日期不能晚于结束日期！", false);
				}
				else
				{
					dateTime = this.calendarStartDate.SelectedDate.Value;
					DateTime now = DateTime.Now;
					if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd"))) < 0)
					{
						this.ShowMsg("开始日期不能早于当前时间！", false);
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
							combinationBuyInfo.MainProductId = num;
							combinationBuyInfo.OtherProductIds = this.hidOtherProductIds.Value;
							CombinationBuyInfo combinationBuyInfo2 = combinationBuyInfo;
							dateTime = this.calendarStartDate.SelectedDate.Value;
							combinationBuyInfo2.StartDate = dateTime.Date;
							CombinationBuyInfo combinationBuyInfo3 = combinationBuyInfo;
							dateTime = this.calendarEndDate.SelectedDate.Value;
							dateTime = dateTime.Date;
							dateTime = dateTime.AddDays(1.0);
							combinationBuyInfo3.EndDate = dateTime.AddSeconds(-1.0);
							if (CombinationBuyHelper.AddCombinationBuy(combinationBuyInfo, items))
							{
								this.ShowMsg("添加组合购活动成功！", true, "CombinationBuy.aspx");
							}
							else
							{
								this.ShowMsg("添加失败！", false);
							}
						}
					}
				}
			}
		}
	}
}
