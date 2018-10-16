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
	public class AddVLotteryActivity : AdminPage
	{
		private int type;

		protected HtmlAnchor alist;

		protected Literal LitListTitle;

		protected Literal LitTitle;

		protected TextBox txtActiveName;

		protected CalendarPanel calendarStartDate;

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
			this.ooOpen.Parameter.Add("onSwitchChange", "fuenableDeduct");
			this.SetDateControl();
			if (int.TryParse(base.Request.QueryString["type"], out this.type))
			{
				this.alist.HRef = "NewLotteryActivity.aspx?type=" + this.type;
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
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

		private void SetDateControl()
		{
			Dictionary<string, object> calendarParameter = this.calendarStartDate.CalendarParameter;
			DateTime now = DateTime.Now;
			calendarParameter.Add("startDate ", now.ToString("yyyy-MM-dd"));
			this.calendarStartDate.FunctionNameForChangeDate = "fuChangeStartDate";
			this.calendarStartDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii";
			this.calendarStartDate.CalendarParameter["minView"] = "0";
			Dictionary<string, object> calendarParameter2 = this.calendarEndDate.CalendarParameter;
			now = DateTime.Now;
			calendarParameter2.Add("startDate ", now.ToString("yyyy-MM-dd"));
			this.calendarEndDate.FunctionNameForChangeDate = "fuChangeEndDate";
			this.calendarEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii";
			this.calendarEndDate.CalendarParameter["minView"] = "0";
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
			int freeTimes = default(int);
			int consumptionIntegral = default(int);
			if (string.IsNullOrEmpty(Globals.StripHtmlXmlTags(this.txtActiveName.Text)))
			{
				this.ShowMsg("标题不能为空，或者危险字符！", false);
			}
			else if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动开始时间", false);
			}
			else if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动结束时间", false);
			}
			else if (this.calendarEndDate.SelectedDate.Value < this.calendarStartDate.SelectedDate.Value)
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
				ActivityInfo activityInfo = new ActivityInfo();
				activityInfo.ActivityName = Globals.StripHtmlXmlTags(this.txtActiveName.Text);
				activityInfo.ActivityType = this.type;
				activityInfo.Description = this.txtdesc.Text;
				activityInfo.StartDate = this.calendarStartDate.SelectedDate.Value;
				activityInfo.EndDate = this.calendarEndDate.SelectedDate.Value.AddSeconds(59.0);
				activityInfo.ShareDetail = this.txtKeyword.Text;
				activityInfo.SharePic = sharePic;
				activityInfo.ResetType = ((!this.ooOpen.SelectedValue) ? 1 : 2);
				activityInfo.FreeTimes = freeTimes;
				activityInfo.ConsumptionIntegral = consumptionIntegral;
				activityInfo.CreateDate = DateTime.Now;
				Database database = DatabaseFactory.CreateDatabase();
				using (DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						long num = ActivityHelper.AddActivityInfo(activityInfo, dbTransaction);
						if (num > 0)
						{
							string value = this.hidJson.Value;
							if (string.IsNullOrEmpty(value))
							{
								dbTransaction.Rollback();
								this.ShowMsg("获得奖项不能为空！", false);
							}
							else
							{
								bool flag = true;
								decimal d = default(decimal);
								List<ActivityAwardItemInfo> list = JsonHelper.ParseFormJson<List<ActivityAwardItemInfo>>(value);
								if (list.Any())
								{
									foreach (ActivityAwardItemInfo item in list)
									{
										if (item.PrizeType == 2)
										{
											int couponSurplus = CouponHelper.GetCouponSurplus(item.PrizeValue);
											if (item.AwardNum > couponSurplus)
											{
												dbTransaction.Rollback();
												this.ShowMsg("优惠券奖项数量不能大于总数！", false);
												return;
											}
										}
										d += item.HitRate;
										if (d > 100m)
										{
											dbTransaction.Rollback();
											this.ShowMsg("奖项概率总和不能大于100%！", false);
											return;
										}
										item.ActivityId = num.ToInt(0);
										int num2 = ActivityHelper.AddActivityAwardItemInfo(item, dbTransaction);
										if (num2 <= 0)
										{
											flag = false;
											dbTransaction.Rollback();
										}
									}
								}
								if (flag)
								{
									dbTransaction.Commit();
									this.ShowMsg("添加成功", true, "NewLotteryActivity.aspx?type=" + this.type);
								}
							}
						}
						else
						{
							dbTransaction.Rollback();
							this.ShowMsg("添加失败！", false);
						}
					}
					catch (Exception ex)
					{
						dbTransaction.Rollback();
						Globals.WriteLog("ActivityLog.txt", "Methed:btnAddActivity_Click , Id：" + activityInfo.ActivityId + " , Msg：" + ex.Message);
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
}
