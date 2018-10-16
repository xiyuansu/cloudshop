using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.ProductPreSale)]
	public class EditProductPreSale : AdminPage
	{
		private int preSaleId;

		protected HiddenField hidProductId;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidPreSaleId;

		protected HiddenField hidProductName;

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
			if (!base.IsPostBack)
			{
				if (!int.TryParse(this.Page.Request.QueryString["preSaleId"], out this.preSaleId))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					if (ProductPreSaleHelper.IsPreSaleHasOrder(this.preSaleId))
					{
						this.ShowMsg("该预售已产生订单，不能修改！", false, "ProductPreSale.aspx");
					}
					ProductPreSaleInfo preSaleInfoWithNameAndPrice = ProductPreSaleHelper.GetPreSaleInfoWithNameAndPrice(this.preSaleId);
					if (preSaleInfoWithNameAndPrice != null)
					{
						HiddenField hiddenField = this.hidPreSaleId;
						int num = preSaleInfoWithNameAndPrice.PreSaleId;
						hiddenField.Value = num.ToString();
						HiddenField hiddenField2 = this.hidProductId;
						num = preSaleInfoWithNameAndPrice.ProductId;
						hiddenField2.Value = num.ToString();
						this.hidProductName.Value = preSaleInfoWithNameAndPrice.ProductName;
						this.hidSalePrice.Value = preSaleInfoWithNameAndPrice.SalePrice.F2ToString("f2");
						if (preSaleInfoWithNameAndPrice.DepositPercent > 0)
						{
							this.radDepositPercent.Checked = true;
							TextBox textBox = this.txtDepositPercent;
							num = preSaleInfoWithNameAndPrice.DepositPercent;
							textBox.Text = num.ToString();
						}
						else if (preSaleInfoWithNameAndPrice.Deposit > decimal.Zero)
						{
							this.radDeposit.Checked = true;
							this.txtDeposit.Text = preSaleInfoWithNameAndPrice.Deposit.ToString();
						}
						this.PreSaleEndDate.SelectedDate = preSaleInfoWithNameAndPrice.PreSaleEndDate;
						this.PaymentStartDate.SelectedDate = preSaleInfoWithNameAndPrice.PaymentStartDate;
						this.PaymentEndDate.SelectedDate = preSaleInfoWithNameAndPrice.PaymentEndDate;
						if (preSaleInfoWithNameAndPrice.DeliveryDays > 0)
						{
							this.radDeliveryDays.Checked = true;
							TextBox textBox2 = this.txtDeliveryDays;
							num = preSaleInfoWithNameAndPrice.DeliveryDays;
							textBox2.Text = num.ToString();
						}
						else if (preSaleInfoWithNameAndPrice.DeliveryDate.HasValue)
						{
							this.radDeliveryDate.Checked = true;
							this.DeliveryDate.SelectedDate = preSaleInfoWithNameAndPrice.DeliveryDate;
						}
					}
					else
					{
						base.GotoResourceNotFound();
					}
				}
			}
		}

		private void btnAddPreSale_Click(object sender, EventArgs e)
		{
			this.btnAddPreSale.Enabled = false;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			decimal num4 = default(decimal);
			int num5 = 0;
			int.TryParse(this.hidPreSaleId.Value, out num);
			int.TryParse(this.hidProductId.Value, out num2);
			if (num2 <= 0)
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
			else if (this.PreSaleEndDate.SelectedDate.Value < DateTime.Now)
			{
				this.ShowMsg("预售结束时间不能早于当前时间！", false);
				this.btnAddPreSale.Enabled = true;
			}
			else
			{
				DateTime dateTime = this.PaymentStartDate.SelectedDate.Value;
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
						DateTime now = DateTime.Now;
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
									int.TryParse(this.txtDepositPercent.Text, out num3);
									if (num3 <= 0 || num3 > 100)
									{
										this.ShowMsg("定金百分比只能输入数字，限制在1-100之间！", false);
										this.btnAddPreSale.Enabled = true;
										return;
									}
								}
								else
								{
									decimal.TryParse(this.txtDeposit.Text.Trim(), out num4);
									if (num4 <= decimal.Zero)
									{
										this.ShowMsg("定金固定金额只能输入数字，限制2位小数！", false);
										this.btnAddPreSale.Enabled = true;
										return;
									}
									decimal d = string.IsNullOrEmpty(this.hidSalePrice.Value) ? decimal.Zero : decimal.Parse(this.hidSalePrice.Value);
									if (num4 > d)
									{
										this.ShowMsg("定金不能大于商品售价！", false);
										this.btnAddPreSale.Enabled = true;
										return;
									}
									if (num4 > 100000000m)
									{
										this.ShowMsg("您输入的固定金额过大！", false);
										this.btnAddPreSale.Enabled = true;
										return;
									}
								}
								if (this.radDeliveryDays.Checked)
								{
									int.TryParse(this.txtDeliveryDays.Text, out num5);
									if (num5 <= 0 || num5 > 1000)
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
								productPreSaleInfo.PreSaleId = num;
								productPreSaleInfo.ProductId = num2;
								if (this.radDepositPercent.Checked)
								{
									productPreSaleInfo.DepositPercent = num3;
									productPreSaleInfo.Deposit = decimal.Zero;
								}
								else
								{
									productPreSaleInfo.Deposit = num4;
									productPreSaleInfo.DepositPercent = 0;
								}
								productPreSaleInfo.PreSaleEndDate = this.PreSaleEndDate.SelectedDate.Value;
								productPreSaleInfo.PaymentStartDate = this.PaymentStartDate.SelectedDate.Value;
								ProductPreSaleInfo productPreSaleInfo2 = productPreSaleInfo;
								dateTime = this.PaymentEndDate.SelectedDate.Value;
								dateTime = dateTime.AddDays(1.0);
								productPreSaleInfo2.PaymentEndDate = dateTime.AddSeconds(-1.0);
								if (this.radDeliveryDays.Checked)
								{
									productPreSaleInfo.DeliveryDays = num5;
									productPreSaleInfo.DeliveryDate = null;
								}
								else
								{
									productPreSaleInfo.DeliveryDays = 0;
									productPreSaleInfo.DeliveryDate = this.DeliveryDate.SelectedDate.Value;
								}
								if (ProductPreSaleHelper.UpdatePreSale(productPreSaleInfo))
								{
									this.ShowMsg("编辑预售活动成功！", true, "ProductPreSale.aspx");
								}
								else
								{
									this.ShowMsg("编辑失败！", false);
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
