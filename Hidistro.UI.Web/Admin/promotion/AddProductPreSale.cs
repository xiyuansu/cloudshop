using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.ProductPreSale)]
	public class AddProductPreSale : AdminPage
	{
		protected HiddenField hidProductId;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidSalePrice;

		protected RadioButton radDepositPercent;

		protected TextBox txtDepositPercent;

		protected RadioButton radDeposit;

		protected TextBox txtDeposit;

		protected CalendarPanel PreSaleEndDate;

		protected CalendarPanel PaymentStartDate;

		protected CalendarPanel PaymentEndDate;

		protected RadioButton radDeliveryDays;

		protected TextBox txtDeliveryDays;

		protected RadioButton radDeliveryDate;

		protected CalendarPanel DeliveryDate;

		protected Button btnAddPreSale;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddPreSale.Click += this.btnAddPreSale_Click;
			this.PreSaleEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.PreSaleEndDate.CalendarParameter["minView"] = "0";
			if (base.IsPostBack)
			{
				return;
			}
		}

		private void btnAddPreSale_Click(object sender, EventArgs e)
		{
			this.btnAddPreSale.Enabled = false;
			int num = 0;
			int num2 = 0;
			decimal num3 = default(decimal);
			int num4 = 0;
			int.TryParse(this.hidProductId.Value, out num);
			if (num <= 0)
			{
				this.ShowMsg("请选择预售商品！", false);
				this.btnAddPreSale.Enabled = true;
			}
			else if (!this.PreSaleEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择预售结束时间！", false);
				this.btnAddPreSale.Enabled = true;
			}
			else if (!this.PaymentStartDate.SelectedDate.HasValue || !this.PaymentEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择尾款支付时间！", false);
				this.btnAddPreSale.Enabled = true;
			}
			else
			{
				DateTime dateTime = this.PreSaleEndDate.SelectedDate.Value;
				DateTime now = DateTime.Now;
				if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd HH:mm"))) < 0)
				{
					this.ShowMsg("预售结束时间不能早于当前时间！", false);
					this.btnAddPreSale.Enabled = true;
					dateTime = this.PreSaleEndDate.SelectedDate.Value;
					string str = dateTime.ToString();
					dateTime = DateTime.Now;
					Globals.WriteLog(str + "|" + dateTime.ToString("yyyy-MM-dd HH:mm"), "TimeError.txt");
				}
				else
				{
					dateTime = this.PaymentStartDate.SelectedDate.Value;
					if (dateTime.CompareTo(this.PaymentEndDate.SelectedDate.Value) > 0)
					{
						this.ShowMsg("尾款支付开始时间不能晚于尾款支付结束时间！", false);
						this.btnAddPreSale.Enabled = true;
					}
					else
					{
						dateTime = this.PaymentEndDate.SelectedDate.Value;
						if (dateTime.CompareTo(this.PreSaleEndDate.SelectedDate.Value) < 0)
						{
							this.ShowMsg("尾款支付结束时间不能早于预售结束时间！", false);
							this.btnAddPreSale.Enabled = true;
						}
						else
						{
							dateTime = this.PaymentStartDate.SelectedDate.Value;
							now = DateTime.Now;
							if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd"))) < 0)
							{
								this.ShowMsg("尾款支付开始时间不能早于当前时间！", false);
								this.btnAddPreSale.Enabled = true;
							}
							else
							{
								dateTime = this.PaymentEndDate.SelectedDate.Value;
								now = DateTime.Now;
								if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd"))) < 0)
								{
									this.ShowMsg("尾款支付结束时间不能早于当前时间！", false);
									this.btnAddPreSale.Enabled = true;
								}
								else
								{
									if (this.radDepositPercent.Checked)
									{
										int.TryParse(this.txtDepositPercent.Text, out num2);
										if (num2 <= 0 || num2 > 100)
										{
											this.ShowMsg("定金百分比只能输入数字，限制在1-100之间！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
									}
									else
									{
										decimal.TryParse(this.txtDeposit.Text.Trim(), out num3);
										if (num3 <= decimal.Zero)
										{
											this.ShowMsg("定金固定金额只能输入数字，限制2位小数！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
										decimal d = string.IsNullOrEmpty(this.hidSalePrice.Value) ? decimal.Zero : decimal.Parse(this.hidSalePrice.Value);
										if (num3 > d)
										{
											this.ShowMsg("定金不能大于商品售价！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
										if (num3 > 100000000m)
										{
											this.ShowMsg("您输入的固定金额过大！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
									}
									if (this.radDeliveryDays.Checked)
									{
										int.TryParse(this.txtDeliveryDays.Text, out num4);
										if (num4 <= 0 || num4 > 1000)
										{
											this.ShowMsg("天数只能输入整数，限制在1-1000之间！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
									}
									else
									{
										if (!this.DeliveryDate.SelectedDate.HasValue)
										{
											this.ShowMsg("请选择发货时间！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
										dateTime = this.DeliveryDate.SelectedDate.Value;
										now = DateTime.Now;
										if (dateTime.CompareTo(DateTime.Parse(now.ToString("yyyy-MM-dd"))) < 0)
										{
											this.ShowMsg("发货时间不能早于当前时间！", false);
											this.btnAddPreSale.Enabled = true;
											return;
										}
									}
									ProductPreSaleInfo productPreSaleInfo = new ProductPreSaleInfo();
									productPreSaleInfo.ProductId = num;
									if (this.radDepositPercent.Checked)
									{
										productPreSaleInfo.DepositPercent = num2;
									}
									else
									{
										productPreSaleInfo.Deposit = num3;
									}
									productPreSaleInfo.PreSaleEndDate = this.PreSaleEndDate.SelectedDate.Value;
									ProductPreSaleInfo productPreSaleInfo2 = productPreSaleInfo;
									dateTime = this.PaymentStartDate.SelectedDate.Value;
									productPreSaleInfo2.PaymentStartDate = dateTime.Date;
									ProductPreSaleInfo productPreSaleInfo3 = productPreSaleInfo;
									dateTime = this.PaymentEndDate.SelectedDate.Value;
									dateTime = dateTime.AddDays(1.0);
									productPreSaleInfo3.PaymentEndDate = dateTime.AddSeconds(-1.0);
									if (this.radDeliveryDays.Checked)
									{
										productPreSaleInfo.DeliveryDays = num4;
									}
									else
									{
										productPreSaleInfo.DeliveryDate = this.DeliveryDate.SelectedDate.Value;
									}
									if (ProductPreSaleHelper.CreatePreSale(productPreSaleInfo))
									{
										ProductHelper.UpShelf(num.ToString());
										this.ShowMsg("添加预售活动成功！", true, "ProductPreSale.aspx");
									}
									else
									{
										this.ShowMsg("添加失败！", false);
										this.btnAddPreSale.Enabled = true;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
