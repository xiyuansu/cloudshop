using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AddLotteryTicket : AdminPage
	{
		protected TextBox txtActiveName;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarOpenDate;

		protected CalendarPanel calendarEndDate;

		protected CheckBoxList cbList;

		protected TextBox txtMinValue;

		protected TextBox txtCode;

		protected TextBox txtKeyword;

		protected TextBox txtdesc;

		protected TextBox txtPrize1;

		protected TextBox txtPrize1Num;

		protected TextBox txtPrize2;

		protected TextBox txtPrize2Num;

		protected TextBox txtPrize3;

		protected TextBox txtPrize3Num;

		protected OnOff ooOpen;

		protected TextBox txtPrize4;

		protected TextBox txtPrize4Num;

		protected TextBox txtPrize5;

		protected TextBox txtPrize5Num;

		protected TextBox txtPrize6;

		protected TextBox txtPrize6Num;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected Button btnAddActivity;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.SetDateControl();
			if (!base.IsPostBack)
			{
				this.cbList.DataSource = MemberHelper.GetMemberGrades();
				this.cbList.DataTextField = "Name";
				this.cbList.DataValueField = "GradeId";
				this.cbList.DataBind();
			}
		}

		private void SetDateControl()
		{
			this.calendarOpenDate.CalendarParameter.Add("startDate ", DateTime.Now.ToString("yyyy-MM-dd"));
			this.calendarOpenDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarOpenDate.CalendarParameter["minView"] = "0";
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
			if (ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
			}
			else if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动开始时间", false);
			}
			else if (!this.calendarOpenDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择抽奖开始时间", false);
			}
			else if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择活动结束时间", false);
			}
			else if (this.calendarEndDate.SelectedDate.Value < this.calendarStartDate.SelectedDate.Value)
			{
				this.ShowMsg("活动开始时间不能晚于活动结束时间", false);
			}
			else
			{
				string activityPic = this.UploadImage();
				string text = string.Empty;
				for (int i = 0; i < this.cbList.Items.Count; i++)
				{
					if (this.cbList.Items[i].Selected)
					{
						text = text + "," + this.cbList.Items[i].Value;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					this.ShowMsg("请选择活动会员等级", false);
				}
				else
				{
					LotteryTicketInfo lotteryTicketInfo = new LotteryTicketInfo();
					lotteryTicketInfo.GradeIds = text;
					lotteryTicketInfo.MinValue = Convert.ToInt32(this.txtMinValue.Text);
					lotteryTicketInfo.InvitationCode = this.txtCode.Text.Trim();
					lotteryTicketInfo.ActivityName = this.txtActiveName.Text;
					lotteryTicketInfo.ActivityKey = this.txtKeyword.Text;
					lotteryTicketInfo.ActivityDesc = this.txtdesc.Text;
					lotteryTicketInfo.ActivityPic = activityPic;
					lotteryTicketInfo.ActivityType = 4;
					lotteryTicketInfo.StartTime = this.calendarStartDate.SelectedDate.Value;
					lotteryTicketInfo.OpenTime = this.calendarOpenDate.SelectedDate.Value;
					lotteryTicketInfo.EndTime = this.calendarEndDate.SelectedDate.Value;
					lotteryTicketInfo.PrizeSettingList = new List<PrizeSetting>();
					try
					{
						lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
						{
							PrizeName = this.txtPrize1.Text,
							PrizeNum = Convert.ToInt32(this.txtPrize1Num.Text),
							PrizeLevel = "一等奖"
						});
						lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
						{
							PrizeName = this.txtPrize2.Text,
							PrizeNum = Convert.ToInt32(this.txtPrize2Num.Text),
							PrizeLevel = "二等奖"
						});
						lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
						{
							PrizeName = this.txtPrize3.Text,
							PrizeNum = Convert.ToInt32(this.txtPrize3Num.Text),
							PrizeLevel = "三等奖"
						});
					}
					catch (FormatException)
					{
						this.ShowMsg("奖品数量格式错误", false);
						return;
					}
					if (this.ooOpen.SelectedValue)
					{
						try
						{
							lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
							{
								PrizeName = this.txtPrize4.Text,
								PrizeNum = Convert.ToInt32(this.txtPrize4Num.Text),
								PrizeLevel = "四等奖"
							});
							lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
							{
								PrizeName = this.txtPrize5.Text,
								PrizeNum = Convert.ToInt32(this.txtPrize5Num.Text),
								PrizeLevel = "五等奖"
							});
							lotteryTicketInfo.PrizeSettingList.Add(new PrizeSetting
							{
								PrizeName = this.txtPrize6.Text,
								PrizeNum = Convert.ToInt32(this.txtPrize6Num.Text),
								PrizeLevel = "六等奖"
							});
						}
						catch (FormatException)
						{
							this.ShowMsg("奖品数量格式错误", false);
							return;
						}
					}
					int num = VShopHelper.SaveLotteryTicket(lotteryTicketInfo);
					if (num > 0)
					{
						ReplyInfo replyInfo = new TextReplyInfo();
						replyInfo.Keys = lotteryTicketInfo.ActivityKey;
						replyInfo.MatchType = MatchType.Equal;
						replyInfo.MessageType = MessageType.Text;
						replyInfo.ReplyType = ReplyType.Ticket;
						replyInfo.ActivityId = num;
						ReplyHelper.SaveReply(replyInfo);
						base.Response.Redirect("ManageLotteryTicket.aspx");
					}
				}
			}
		}
	}
}
