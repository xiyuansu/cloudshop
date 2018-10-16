using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class EidVLotteryActivity : AdminPage
	{
		private int type;

		private int activityid;

		protected HtmlAnchor alist;

		protected Literal LitListTitle;

		protected Literal LitTitle;

		protected TextBox txtActiveName;

		protected TextBox calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected TextBox txtKeyword;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected OnOff ooOpen;

		protected TextBox txtMaxNum;

		protected TextBox txtUsePoints;

		protected TextBox txtdesc;

		protected HtmlInputHidden hidJson;

		protected Button btnAddActivity;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.SetDateControl();
			if (int.TryParse(base.Request.QueryString["typeid"], out this.type) && int.TryParse(base.Request.QueryString["id"], out this.activityid))
			{
				this.alist.HRef = "NewLotteryActivity.aspx?type=" + this.type;
				switch (this.type)
				{
				default:
					if (!this.Page.IsPostBack)
					{
						this.RestoreActivity();
					}
					break;
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}

		private void SetDateControl()
		{
			this.calendarEndDate.CalendarParameter.Add("startDate ", DateTime.Now.ToString("yyyy-MM-dd"));
			this.calendarEndDate.FunctionNameForChangeDate = "fuChangeEndDate";
			this.calendarEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii";
			this.calendarEndDate.CalendarParameter["minView"] = "0";
		}

		private void RestoreActivity()
		{
			ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(this.activityid);
			this.txtActiveName.Text = activityInfo.ActivityName;
			this.txtKeyword.Text = activityInfo.ShareDetail;
			this.txtdesc.Text = activityInfo.Description;
			this.hidOldImages.Value = activityInfo.SharePic;
			this.calendarStartDate.Text = activityInfo.StartDate.ToString();
			this.calendarEndDate.SelectedDate = activityInfo.EndDate;
			TextBox textBox = this.txtMaxNum;
			int num = activityInfo.FreeTimes;
			textBox.Text = num.ToString();
			TextBox textBox2 = this.txtUsePoints;
			num = activityInfo.ConsumptionIntegral;
			textBox2.Text = num.ToString();
			this.calendarStartDate.Enabled = false;
			if (activityInfo.ResetType == 2)
			{
				this.ooOpen.SelectedValue = true;
			}
			else
			{
				this.ooOpen.SelectedValue = false;
			}
			this.ooOpen.Enabled = false;
			List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(activityInfo.ActivityId);
			string json = JsonHelper.GetJson(activityItemList);
			this.hidJson.Value = json;
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/topic/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/topic/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/topic/" + text2;
		}

		protected void btnAddActivity_Click(object sender, EventArgs e)
		{
			string sharePic = string.Empty;
			if (this.hidUploadImages.Value.Trim().Length > 0)
			{
				try
				{
					sharePic = this.UploadImage();
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动结束时间", false);
			}
			else
			{
				DateTime value = this.calendarEndDate.SelectedDate.Value;
				DateTime? t = this.calendarStartDate.Text.ToDateTime();
				int freeTimes = default(int);
				int consumptionIntegral = default(int);
				if ((DateTime?)value < t)
				{
					this.ShowMsg("活动开始时间不能晚于活动结束时间", false);
				}
				else if (!int.TryParse(this.txtMaxNum.Text, out freeTimes) || freeTimes.ToString() != this.txtMaxNum.Text)
				{
					this.ShowMsg("可抽奖次数必须是整数", false);
				}
				else if (!int.TryParse(this.txtUsePoints.Text, out consumptionIntegral))
				{
					this.ShowMsg("每次参与消耗的积分不能为空，且是整数", false);
				}
				else
				{
					ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(this.activityid);
					activityInfo.ActivityName = Globals.StripHtmlXmlTags(this.txtActiveName.Text);
					activityInfo.Description = this.txtdesc.Text;
					DateTime value2 = this.calendarEndDate.SelectedDate.Value;
					if (value2.Second == 0)
					{
						value2.AddSeconds(59.0);
					}
					activityInfo.EndDate = value2;
					activityInfo.ShareDetail = this.txtKeyword.Text;
					activityInfo.SharePic = sharePic;
					activityInfo.FreeTimes = freeTimes;
					activityInfo.ConsumptionIntegral = consumptionIntegral;
					Database database = DatabaseFactory.CreateDatabase();
					using (DbConnection dbConnection = database.CreateConnection())
					{
						dbConnection.Open();
						DbTransaction dbTransaction = dbConnection.BeginTransaction();
						try
						{
							if (ActivityHelper.UpdateActivityInfo(activityInfo, dbTransaction))
							{
								string value3 = this.hidJson.Value;
								if (string.IsNullOrEmpty(value3))
								{
									dbTransaction.Rollback();
									this.ShowMsg("获得奖项不能为空！", false);
								}
								else
								{
									decimal d = default(decimal);
									bool flag = true;
									List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(activityInfo.ActivityId);
									List<ActivityAwardItemInfo> list = JsonHelper.ParseFormJson<List<ActivityAwardItemInfo>>(value3);
									int[] array = new int[activityItemList.Count];
									int num = 0;
									string text = "";
									foreach (ActivityAwardItemInfo item in list)
									{
										if (item.AwardId > 0)
										{
											text = text + item.AwardId + ",";
										}
									}
									text = text.Remove(text.Length - 1);
									ActivityHelper.DeleteEidAwardItem(this.activityid, text, dbTransaction);
									foreach (ActivityAwardItemInfo item2 in activityItemList)
									{
										array[num] = item2.AwardId;
										num++;
									}
									if (list.Any())
									{
										foreach (ActivityAwardItemInfo item3 in list)
										{
											if (item3.PrizeType == 2)
											{
												int couponSurplus = CouponHelper.GetCouponSurplus(item3.PrizeValue);
												if (item3.AwardNum > couponSurplus)
												{
													dbTransaction.Rollback();
													this.ShowMsg("优惠券奖项数量不能大于总数！", false);
													return;
												}
											}
											d += item3.HitRate;
											item3.ActivityId = this.activityid;
											bool flag2 = false;
											if (array.Contains(item3.AwardId))
											{
												if (item3.AwardId > 0)
												{
													ActivityAwardItemInfo newItemInfo = this.GetNewItemInfo(item3);
													if (!ActivityHelper.UpdateActivityAwardItemInfo(newItemInfo, dbTransaction))
													{
														dbTransaction.Rollback();
														this.ShowMsg("编辑失败！", false);
														return;
													}
												}
												else if (ActivityHelper.AddActivityAwardItemInfo(item3, dbTransaction) <= 0)
												{
													dbTransaction.Rollback();
													this.ShowMsg("编辑失败！", false);
													return;
												}
												continue;
											}
											if (item3.AwardId > 0)
											{
												flag = ActivityHelper.DeleteActivityAwardItemByActivityId(item3.AwardId, dbTransaction);
												continue;
											}
											int num2 = ActivityHelper.AddActivityAwardItemInfo(item3, dbTransaction);
											if (num2 > 0)
											{
												flag = true;
												continue;
											}
											dbTransaction.Rollback();
											this.ShowMsg("编辑失败！", false);
											return;
										}
									}
									if (d > 100m)
									{
										dbTransaction.Rollback();
										this.ShowMsg("奖项概率总和不能大于100%！", false);
									}
									else if (flag)
									{
										dbTransaction.Commit();
										this.ShowMsg("编辑成功！", true);
										base.Response.Redirect("NewLotteryActivity.aspx?type=" + this.type, true);
										base.Response.End();
									}
									else
									{
										dbTransaction.Rollback();
										this.ShowMsg("编辑失败！", false);
									}
								}
							}
						}
						catch (Exception ex)
						{
							dbTransaction.Rollback();
							Globals.WriteLog("ActivityLog.txt", "Methed:Edit , Id：" + activityInfo.ActivityId + " , Msg：" + ex.Message);
							this.ShowMsg(ex.Message, false);
						}
						finally
						{
							dbConnection.Close();
						}
					}
				}
			}
		}

		public ActivityAwardItemInfo GetNewItemInfo(ActivityAwardItemInfo model)
		{
			ActivityAwardItemInfo awardItemInfoById = ActivityHelper.GetAwardItemInfoById(model.AwardId);
			if (model.PrizeType != awardItemInfoById.PrizeType || model.PrizeValue != awardItemInfoById.PrizeValue)
			{
				awardItemInfoById.WinningNum = 0;
			}
			awardItemInfoById.AwardGrade = model.AwardGrade;
			awardItemInfoById.PrizeType = model.PrizeType;
			awardItemInfoById.HitRate = model.HitRate;
			awardItemInfoById.AwardNum = model.AwardNum;
			awardItemInfoById.PrizeValue = model.PrizeValue;
			return awardItemInfoById;
		}
	}
}
