using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
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
	public class FightGroupActivitiy : AdminPage
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

			public string Icon
			{
				get;
				set;
			}

			public string MaxCount
			{
				get;
				set;
			}

			public string JoinNumber
			{
				get;
				set;
			}

			public string LimitedHour
			{
				get;
				set;
			}
		}

		private string formData;

		private int productId;

		public int fightGroupActivityId;

		protected Literal ltProductName;

		protected HtmlGenericControl liSkus;

		protected Repeater rptProductSkus;

		protected HiddenField hidUploadLogo;

		protected RadioButtonList rbtlTitle;

		protected TextBox txtFightGroupShareTitle;

		protected TextBox txtFightGroupShareDetails;

		protected HtmlGenericControl liSalePrice;

		protected Label lblPrice;

		protected HtmlGenericControl liDefaultPrice;

		protected TextBox txtPrice;

		protected HtmlGenericControl liDefaultStock;

		protected Literal ltStock;

		protected HtmlGenericControl liDefaultTotalCount;

		protected TextBox txtTotalCount;

		protected CalendarPanel CPStartTime;

		protected CalendarPanel CPEndDate;

		protected Literal ltBoughtCount;

		protected TextBox txtJoinNumber;

		protected TextBox txtLimitedHour;

		protected TextBox txtMaxCount;

		protected Button btnSaveFightGroupActivitiy;

		protected HiddenField hfFightGroupActivityId;

		protected HiddenField hfProductId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.fightGroupActivityId = this.Page.Request["fightGroupActivityId"].ToInt(0);
			this.formData = this.Page.Request["formData"].ToNullString();
			this.SetDateControl();
			this.btnSaveFightGroupActivitiy.Click += this.btnSaveFightGroupActivitiy_Click;
			this.productId = this.Page.Request["productId"].ToInt(0);
			if (!this.Page.IsPostBack)
			{
				this.BindFightGroupActivitiy();
				this.BindProduct();
				this.ShowSKUOrDefault();
				this.BindFormData();
			}
			this.hfFightGroupActivityId.Value = this.fightGroupActivityId.ToString();
		}

		private void BindFightGroupActivitiy()
		{
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.fightGroupActivityId);
			if (fightGroupActivitieInfo != null)
			{
				IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(this.fightGroupActivityId);
				int num = 0;
				if (fightGroupSkus.Count() == 1)
				{
					this.txtPrice.Text = fightGroupSkus[0].SalePrice.F2ToString("f2");
					this.txtTotalCount.Text = fightGroupSkus[0].TotalCount.ToNullString();
					num = fightGroupSkus[0].BoughtCount;
				}
				else
				{
					for (int i = 0; i < fightGroupSkus.Count(); i++)
					{
						num += fightGroupSkus[i].BoughtCount;
					}
				}
				this.rbtlTitle.SelectedIndex = ((!string.IsNullOrEmpty(fightGroupActivitieInfo.ShareTitle)) ? 1 : 0);
				this.txtFightGroupShareTitle.Text = fightGroupActivitieInfo.ShareTitle;
				this.txtFightGroupShareDetails.Text = fightGroupActivitieInfo.ShareContent;
				this.txtMaxCount.Text = Convert.ToString(fightGroupActivitieInfo.MaxCount);
				this.ltBoughtCount.Text = num.ToString();
				this.CPEndDate.SelectedDate = fightGroupActivitieInfo.EndDate;
				this.CPStartTime.SelectedDate = fightGroupActivitieInfo.StartDate;
				this.hidUploadLogo.Value = fightGroupActivitieInfo.Icon;
				TextBox textBox = this.txtJoinNumber;
				int num2 = fightGroupActivitieInfo.JoinNumber;
				textBox.Text = num2.ToString();
				TextBox textBox2 = this.txtLimitedHour;
				num2 = fightGroupActivitieInfo.LimitedHour;
				textBox2.Text = num2.ToString();
			}
		}

		private void SetDateControl()
		{
			this.CPStartTime.FunctionNameForChangeDate = "fuChangeStartDate";
			this.CPEndDate.FunctionNameForChangeDate = "fuChangeEndDate";
			this.CPStartTime.CalendarParameter["format"] = "yyyy-mm-dd hh:ii";
			this.CPStartTime.CalendarParameter["minView"] = "0";
			this.CPEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii";
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
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.fightGroupActivityId);
			this.productId = ((this.productId == 0) ? fightGroupActivitieInfo.ProductId : this.productId);
			this.rptProductSkus.DataSource = VShopHelper.GetFightGroupSkus(this.fightGroupActivityId, this.productId);
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
			this.hfProductId.Value = this.productId.ToString();
		}

		private void btnSaveFightGroupActivitiy_Click(object sender, EventArgs e)
		{
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.fightGroupActivityId);
			List<FightGroupSkuInfo> list = new List<FightGroupSkuInfo>();
			if (fightGroupActivitieInfo == null)
			{
				this.ShowMsg("火拼团不存在", false, "FightGroupActivitiyList.aspx");
				return;
			}
			if (this.productId > 0)
			{
				fightGroupActivitieInfo.ProductId = this.productId;
			}
			else
			{
				this.productId = fightGroupActivitieInfo.ProductId;
			}
			string text = Globals.StripAllTags(this.txtFightGroupShareTitle.Text.Trim());
			string text2 = Globals.StripAllTags(this.txtFightGroupShareDetails.Text.Trim());
			int num;
			if ((this.rbtlTitle.SelectedIndex != 1 || (!string.IsNullOrEmpty(text) && text.Length <= 60)) && !string.IsNullOrEmpty(text2))
			{
				num = ((text2.Length > 60) ? 1 : 0);
				goto IL_00c4;
			}
			num = 1;
			goto IL_00c4;
			IL_00c4:
			if (num != 0)
			{
				this.ShowMsg("请按要求输入分享标题和详情", false);
			}
			else
			{
				if (this.rbtlTitle.SelectedIndex == 0)
				{
					fightGroupActivitieInfo.ShareTitle = "";
				}
				else
				{
					fightGroupActivitieInfo.ShareTitle = text;
				}
				fightGroupActivitieInfo.ShareContent = text2;
				if (this.rptProductSkus.Items.Count > 0)
				{
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
						FightGroupSkuInfo item = new FightGroupSkuInfo
						{
							SalePrice = textBox2.Text.Trim().ToDecimal(0),
							TotalCount = textBox.Text.Trim().ToInt(0),
							SkuId = hiddenField.Value,
							FightGroupActivityId = fightGroupActivitieInfo.FightGroupActivityId
						};
						list.Add(item);
					}
					if (this.rptProductSkus.Items.Count == 0)
					{
						if (this.txtPrice.Text.Trim().ToDecimal(0) == decimal.Zero)
						{
							this.ShowMsg("请填写火拼价", false);
							return;
						}
						if (this.txtTotalCount.Text.Trim().ToInt(0) == 0)
						{
							this.ShowMsg("请填写活动库存", false);
							return;
						}
					}
				}
				if (!this.CPStartTime.SelectedDate.HasValue)
				{
					this.ShowMsg(Formatter.FormatErrorMessage("请填写开始时间"), false);
				}
				else
				{
					fightGroupActivitieInfo.StartDate = this.CPStartTime.SelectedDate.Value;
					if (!this.CPEndDate.SelectedDate.HasValue)
					{
						this.ShowMsg(Formatter.FormatErrorMessage("请填写结束时间"), false);
					}
					else
					{
						fightGroupActivitieInfo.EndDate = this.CPEndDate.SelectedDate.Value;
						if (fightGroupActivitieInfo.StartDate >= fightGroupActivitieInfo.EndDate)
						{
							this.ShowMsg(Formatter.FormatErrorMessage("结束时间要大于开始时间"), false);
						}
						else if (fightGroupActivitieInfo.EndDate <= DateTime.Now)
						{
							this.ShowMsg(Formatter.FormatErrorMessage("结束时间要大于当前系统时间"), false);
						}
						else if (string.IsNullOrEmpty(this.txtJoinNumber.Text.Trim()))
						{
							this.ShowMsg("请填写参团人数", false);
						}
						else
						{
							fightGroupActivitieInfo.JoinNumber = this.txtJoinNumber.Text.ToInt(0);
							if (fightGroupActivitieInfo.JoinNumber.ToInt(0) <= 1)
							{
								this.ShowMsg("参团人数要大于1人0", false);
							}
							else if (string.IsNullOrEmpty(this.txtLimitedHour.Text.Trim()))
							{
								this.ShowMsg("请填写成团时限", false);
							}
							else
							{
								fightGroupActivitieInfo.LimitedHour = this.txtLimitedHour.Text.ToInt(0);
								if (fightGroupActivitieInfo.LimitedHour.ToInt(0) <= 0)
								{
									this.ShowMsg("成团时限要大于0", false);
								}
								else if (this.txtMaxCount.Text.Trim().ToInt(0) == 0)
								{
									this.ShowMsg("请填写每人限购数量", false);
								}
								else
								{
									fightGroupActivitieInfo.MaxCount = this.txtMaxCount.Text.ToInt(0);
									if (VShopHelper.ProductFightGroupActivitiyExist(fightGroupActivitieInfo.ProductId, this.fightGroupActivityId, fightGroupActivitieInfo.EndDate))
									{
										this.ShowMsg("已经存在此商品的火拼团活动", false);
									}
									else
									{
										int num2 = 0;
										num2 = ((this.rptProductSkus.Items.Count == 0) ? this.txtTotalCount.Text.Trim().ToInt(0) : list.Sum((FightGroupSkuInfo c) => c.TotalCount));
										if (fightGroupActivitieInfo.MaxCount > num2)
										{
											this.ShowMsg(Formatter.FormatErrorMessage("每人限购数量不能大于活动库存"), false);
										}
										else if (list.Count > 0 && fightGroupActivitieInfo.MaxCount > (from c in list
										where c.TotalCount > 0
										select c).Min((FightGroupSkuInfo c) => c.TotalCount))
										{
											this.ShowMsg(Formatter.FormatErrorMessage("每人限购数量不能大于规格的最小活动库存"), false);
										}
										else
										{
											fightGroupActivitieInfo.Icon = Globals.SaveFile("GroupActivitie", this.hidUploadLogo.Value, "/Storage/master/", true, false, "");
											IList<int> list2 = null;
											Dictionary<int, IList<int>> dictionary = default(Dictionary<int, IList<int>>);
											ProductInfo productDetails = ProductHelper.GetProductDetails(fightGroupActivitieInfo.ProductId, out dictionary, out list2);
											if (productDetails != null)
											{
												fightGroupActivitieInfo.ProductName = productDetails.ProductName;
												if (productDetails.Stock < fightGroupActivitieInfo.MaxCount)
												{
													this.ShowMsg("库存小于每人限购数量", false);
													return;
												}
												if (productDetails.Stock < num2)
												{
													this.ShowMsg($"当前活动库存为 {num2}，商品库存小于活动库存", false);
													return;
												}
												if (list.Count > 0)
												{
													foreach (KeyValuePair<string, SKUItem> sku in productDetails.Skus)
													{
														FightGroupSkuInfo fightGroupSkuInfo = (from s in list
														where s.SkuId == sku.Value.SkuId
														select s).FirstOrDefault();
														if (fightGroupSkuInfo == null)
														{
															this.ShowMsg("规格不存在，请重新刷新页面", false);
															return;
														}
														if (fightGroupSkuInfo.TotalCount > sku.Value.Stock)
														{
															this.ShowMsg($"当前规格活动库存为 {fightGroupSkuInfo.TotalCount}，商品库存小于活动库存", false);
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
													FightGroupSkuInfo item2 = new FightGroupSkuInfo
													{
														SalePrice = this.txtPrice.Text.Trim().ToDecimal(0),
														TotalCount = num2,
														SkuId = dataRow["SkuId"].ToNullString(),
														FightGroupActivityId = fightGroupActivitieInfo.FightGroupActivityId
													};
													list.Add(item2);
												}
											}
											if (VShopHelper.CanAddFightGroupActivitiy(fightGroupActivitieInfo.ProductId, productDetails.ProductName, fightGroupActivitieInfo.FightGroupActivityId))
											{
												VShopHelper.EditFightGroupActivitie(fightGroupActivitieInfo, list);
												this.ShowMsg("保存火拼团活动成功", true, "FightGroupActivitiyList.aspx");
											}
											else
											{
												this.ShowMsg("该商品正在参加其他活动，无法同时参加拼团活动", false);
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

		private void BindFormData()
		{
			if (!string.IsNullOrEmpty(this.formData))
			{
				DataForm dataForm = JsonHelper.ParseFormJson<DataForm>(this.formData);
				this.txtMaxCount.Text = dataForm.MaxCount;
				this.txtPrice.Text = dataForm.Price;
				this.txtTotalCount.Text = dataForm.TotalCount;
				this.CPStartTime.SelectedDate = dataForm.StartTime.ToDateTime();
				this.CPEndDate.SelectedDate = dataForm.EndTime.ToDateTime();
				this.hidUploadLogo.Value = dataForm.Icon;
				this.txtJoinNumber.Text = dataForm.JoinNumber;
				this.txtLimitedHour.Text = dataForm.LimitedHour;
			}
		}
	}
}
