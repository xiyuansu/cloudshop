using Hidistro.Context;
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
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.CountDown)]
	public class AddCountDown : AdminPage
	{
		public class DataForm
		{
			public string Price
			{
				get;
				set;
			}

			public string TotalCount
			{
				get;
				set;
			}

			public string StartTime
			{
				get;
				set;
			}

			public string EndTime
			{
				get;
				set;
			}

			public string Content
			{
				get;
				set;
			}

			public string ShareTitle
			{
				get;
				set;
			}

			public string ShareDetails
			{
				get;
				set;
			}

			public string ShareIcon
			{
				get;
				set;
			}

			public string MaxCount
			{
				get;
				set;
			}
		}

		private string formData;

		private int productId;

		public string filterProductIds;

		public SiteSettings site = SettingsManager.GetMasterSettings();

		protected HiddenField hidOpenMultStore;

		protected HiddenField hidStoreIds;

		protected Panel pnlReMark;

		protected Literal ltProductName;

		protected HtmlGenericControl liSkus;

		protected Repeater rptProductSkus;

		protected HtmlGenericControl liSalePrice;

		protected Label lblPrice;

		protected HtmlGenericControl liDefaultPrice;

		protected TextBox txtPrice;

		protected HtmlGenericControl liDefaultStock;

		protected Literal ltStock;

		protected HtmlGenericControl liDefaultTotalCount;

		protected TextBox txtTotalCount;

		protected TextBox txtMaxCount;

		protected CalendarPanel CPStartTime;

		protected CalendarPanel CPEndDate;

		protected TextBox txtContent;

		protected TextBox txtShareTitle;

		protected TextBox txtShareDetails;

		protected HiddenField hidUploadLogo;

		protected Button btnAddCountDown;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.formData = this.Page.Request["formData"].ToNullString();
			this.btnAddCountDown.Click += this.btnAddCountDown_Click;
			this.productId = this.Page.Request["productId"].ToInt(0);
			this.SetDateControl();
			if (!this.Page.IsPostBack)
			{
				if (!this.site.OpenMultStore)
				{
					this.pnlReMark.Visible = false;
					this.hidOpenMultStore.Value = "0";
				}
				else
				{
					this.hidOpenMultStore.Value = "1";
				}
				this.BindProduct();
				this.ShowSKUOrDefault();
				this.BindFormData();
			}
			this.filterProductIds = (this.site.OpenMultStore ? "" : PromoteHelper.GetCountDownActiveProducts());
			if (this.productId > 0 && this.filterProductIds.Length > 0)
			{
				this.filterProductIds = this.filterProductIds + "," + this.productId;
			}
			else if (this.filterProductIds.Length == 0 && this.productId > 0)
			{
				this.filterProductIds = this.productId.ToString();
			}
		}

		private void SetDateControl()
		{
			Dictionary<string, object> calendarParameter = this.CPStartTime.CalendarParameter;
			DateTime now = DateTime.Now;
			calendarParameter.Add("startDate ", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter2 = this.CPEndDate.CalendarParameter;
			now = DateTime.Now;
			calendarParameter2.Add("startDate ", now.ToString("yyyy-MM-dd"));
			this.CPStartTime.FunctionNameForChangeDate = "fuChangeStartDate";
			this.CPEndDate.FunctionNameForChangeDate = "fuChangeEndDate";
			this.CPStartTime.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.CPStartTime.CalendarParameter["minView"] = "0";
			this.CPEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.CPEndDate.CalendarParameter["minView"] = "0";
		}

		private void ShowSKUOrDefault()
		{
			if (this.rptProductSkus.Items.Count == 0)
			{
				this.liSkus.Style.Add("display", "none");
				if (this.productId > 0)
				{
					this.liSalePrice.Style.Add("display", "block");
					this.liDefaultStock.Style.Add("display", "block");
				}
			}
			else
			{
				this.liDefaultPrice.Style.Add("display", "none");
				this.liDefaultStock.Style.Add("display", "none");
				this.liDefaultTotalCount.Style.Add("display", "none");
			}
		}

		private void BindProduct()
		{
			if (this.productId != 0)
			{
				this.rptProductSkus.DataSource = ProductHelper.GetSkusByProductIdNew(this.productId);
				this.rptProductSkus.DataBind();
				IList<int> list = null;
				Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(this.productId, out dictionary, out list);
				if (productDetails != null)
				{
					this.ltProductName.Text = productDetails.ProductName;
					this.ltStock.Text = productDetails.Stock.ToString();
					this.lblPrice.Text = productDetails.MinSalePrice.F2ToString("f2");
				}
			}
		}

		private void btnAddCountDown_Click(object sender, EventArgs e)
		{
			CountDownInfo countDownInfo = new CountDownInfo();
			List<CountDownSkuInfo> list = new List<CountDownSkuInfo>();
			if (this.productId == 0)
			{
				this.ShowMsg(Formatter.FormatErrorMessage("请选择商品"), false);
			}
			else
			{
				countDownInfo.ProductId = this.productId;
				for (int i = 0; i < this.rptProductSkus.Items.Count; i++)
				{
					RepeaterItem repeaterItem = this.rptProductSkus.Items[i];
					HiddenField hiddenField = repeaterItem.FindControl("hfSkuId") as HiddenField;
					TextBox textBox = repeaterItem.FindControl("txtActivityStock") as TextBox;
					TextBox textBox2 = repeaterItem.FindControl("txtActivitySalePrice") as TextBox;
					if (textBox2.Text.Trim().ToDecimal(0) == decimal.Zero)
					{
						this.ShowMsg("请完整填写商品规格", false);
						return;
					}
					CountDownSkuInfo item = new CountDownSkuInfo
					{
						SalePrice = textBox2.Text.Trim().ToDecimal(0),
						TotalCount = textBox.Text.Trim().ToInt(0),
						SkuId = hiddenField.Value
					};
					list.Add(item);
				}
				if (this.rptProductSkus.Items.Count == 0)
				{
					if (this.txtPrice.Text.Trim().ToDecimal(0) == decimal.Zero)
					{
						this.ShowMsg("请填写限时抢购价格", false);
						return;
					}
					if (this.txtTotalCount.Text.Trim().ToInt(0) == 0)
					{
						this.ShowMsg("请填写商品活动库存", false);
						return;
					}
				}
				if (!this.CPStartTime.SelectedDate.HasValue)
				{
					this.ShowMsg(Formatter.FormatErrorMessage("请填写活动开始时间"), false);
				}
				else
				{
					countDownInfo.StartDate = this.CPStartTime.SelectedDate.Value;
					if (!this.CPEndDate.SelectedDate.HasValue)
					{
						this.ShowMsg(Formatter.FormatErrorMessage("请填写活动结束时间"), false);
					}
					else
					{
						countDownInfo.EndDate = this.CPEndDate.SelectedDate.Value;
						if (countDownInfo.StartDate >= countDownInfo.EndDate)
						{
							this.ShowMsg(Formatter.FormatErrorMessage("活动结束时间要大于活动开始时间"), false);
						}
						else if (countDownInfo.EndDate <= DateTime.Now)
						{
							this.ShowMsg(Formatter.FormatErrorMessage("活动结束时间要大于当前系统时间"), false);
						}
						else if (this.txtMaxCount.Text.Trim().ToInt(0) == 0)
						{
							this.ShowMsg("请填写每人限购数量", false);
						}
						else
						{
							countDownInfo.MaxCount = this.txtMaxCount.Text.ToInt(0);
							if (PromoteHelper.ProductCountDownExist(countDownInfo.ProductId))
							{
								this.ShowMsg("已经存在此商品的限时抢购活动", false);
							}
							else
							{
								int num = 0;
								num = ((this.rptProductSkus.Items.Count == 0) ? this.txtTotalCount.Text.Trim().ToInt(0) : list.Sum((CountDownSkuInfo c) => c.TotalCount));
								if (!this.site.OpenMultStore && countDownInfo.MaxCount > num)
								{
									this.ShowMsg(Formatter.FormatErrorMessage("每人限购数量不能大于活动库存"), false);
								}
								else if (!this.site.OpenMultStore && list.Count > 0 && countDownInfo.MaxCount > (from c in list
								where c.TotalCount > 0
								select c).Min((CountDownSkuInfo c) => c.TotalCount))
								{
									this.ShowMsg(Formatter.FormatErrorMessage("每人限购数量不能大于规格的最小活动库存"), false);
								}
								else
								{
									countDownInfo.Content = Globals.HtmlEncode(this.txtContent.Text);
									countDownInfo.ShareTitle = Globals.HtmlEncode(this.txtShareTitle.Text);
									countDownInfo.ShareDetails = Globals.HtmlEncode(this.txtShareDetails.Text);
									countDownInfo.ShareIcon = Globals.SaveFile("countDown", this.hidUploadLogo.Value, "/Storage/master/", true, false, "");
									IList<int> list2 = null;
									Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
									ProductInfo productDetails = ProductHelper.GetProductDetails(countDownInfo.ProductId, out dictionary, out list2);
									if (productDetails != null)
									{
										if (!this.site.OpenMultStore && productDetails.Stock < countDownInfo.MaxCount)
										{
											this.ShowMsg("库存小于每人限购数量", false);
											return;
										}
										if (!this.site.OpenMultStore && productDetails.Stock < num)
										{
											this.ShowMsg("库存小于活动库存", false);
											return;
										}
										if (list.Count > 0)
										{
											foreach (KeyValuePair<string, SKUItem> sku in productDetails.Skus)
											{
												CountDownSkuInfo countDownSkuInfo = (from s in list
												where s.SkuId == sku.Value.SkuId
												select s).FirstOrDefault();
												if (countDownSkuInfo == null)
												{
													this.ShowMsg("规格不存在，请重新刷新页面", false);
													return;
												}
												if (!this.site.OpenMultStore && countDownSkuInfo.TotalCount > sku.Value.Stock)
												{
													this.ShowMsg("商品规格库存不足", false);
													return;
												}
											}
										}
									}
									if (list.Count == 0)
									{
										DataTable skusByProductId = ProductHelper.GetSkusByProductId(this.productId);
										if (skusByProductId.Rows.Count > 0)
										{
											DataRow dataRow = skusByProductId.Rows[0];
											CountDownSkuInfo item2 = new CountDownSkuInfo
											{
												SalePrice = this.txtPrice.Text.Trim().ToDecimal(0),
												TotalCount = num,
												SkuId = dataRow["SkuId"].ToNullString()
											};
											list.Add(item2);
										}
									}
									if (this.site.OpenMultStore)
									{
										if (string.IsNullOrEmpty(this.hidStoreIds.Value))
										{
											this.ShowMsg("请选择门店范围", false);
											return;
										}
										countDownInfo.StoreType = 2;
										countDownInfo.StoreIds = this.hidStoreIds.Value;
									}
									else
									{
										countDownInfo.StoreType = 0;
									}
									PromoteHelper.AddCountDown(countDownInfo, list);
									if (countDownInfo.StartDate < DateTime.Now && countDownInfo.EndDate > DateTime.Now)
									{
										this.ShowMsg("添加限时抢购活动成功", true, "countdowns.aspx");
									}
									else
									{
										this.ShowMsg("添加限时抢购活动成功", true, "countdowns.aspx?State=1");
									}
								}
							}
						}
					}
				}
			}
		}

		private void BindFormData()
		{
			if (!string.IsNullOrEmpty(this.formData))
			{
				DataForm dataForm = JsonHelper.ParseFormJson<DataForm>(this.formData);
				this.txtContent.Text = dataForm.Content;
				this.txtMaxCount.Text = dataForm.MaxCount;
				this.txtPrice.Text = dataForm.Price;
				this.txtShareDetails.Text = dataForm.ShareDetails;
				this.txtShareTitle.Text = dataForm.ShareTitle;
				this.txtTotalCount.Text = dataForm.TotalCount;
				this.CPStartTime.SelectedDate = dataForm.StartTime.ToDateTime();
				this.CPEndDate.SelectedDate = dataForm.EndTime.ToDateTime();
				this.hidUploadLogo.Value = dataForm.ShareIcon;
			}
		}
	}
}
