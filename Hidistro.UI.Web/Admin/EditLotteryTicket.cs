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
	public class EditLotteryTicket : AdminPage
	{
		private int activityid;

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

		protected Button btnUpdateActivity;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.activityid = base.GetUrlIntParam("id");
			this.SetDateControl();
			if (!this.Page.IsPostBack)
			{
				this.cbList.DataSource = MemberHelper.GetMemberGrades();
				this.cbList.DataTextField = "Name";
				this.cbList.DataValueField = "GradeId";
				this.cbList.DataBind();
				this.RestoreLottery();
			}
		}

		private void SetDateControl()
		{
			this.calendarOpenDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarOpenDate.CalendarParameter["minView"] = "0";
		}

		public void RestoreLottery()
		{
			LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(this.activityid);
			if (lotteryTicket != null)
			{
				TextBox textBox = this.txtMinValue;
				int num = lotteryTicket.MinValue;
				textBox.Text = num.ToString();
				this.txtCode.Text = lotteryTicket.InvitationCode;
				this.txtActiveName.Text = lotteryTicket.ActivityName;
				this.txtKeyword.Text = lotteryTicket.ActivityKey;
				this.txtdesc.Text = lotteryTicket.ActivityDesc;
				this.hidOldImages.Value = lotteryTicket.ActivityPic;
				this.calendarStartDate.SelectedDate = lotteryTicket.StartTime;
				this.calendarOpenDate.SelectedDate = lotteryTicket.OpenTime;
				this.calendarEndDate.SelectedDate = lotteryTicket.EndTime;
				this.txtPrize1.Text = lotteryTicket.PrizeSettingList[0].PrizeName;
				TextBox textBox2 = this.txtPrize1Num;
				num = lotteryTicket.PrizeSettingList[0].PrizeNum;
				textBox2.Text = num.ToString();
				this.txtPrize2.Text = lotteryTicket.PrizeSettingList[1].PrizeName;
				TextBox textBox3 = this.txtPrize2Num;
				num = lotteryTicket.PrizeSettingList[1].PrizeNum;
				textBox3.Text = num.ToString();
				this.txtPrize3.Text = lotteryTicket.PrizeSettingList[2].PrizeName;
				TextBox textBox4 = this.txtPrize3Num;
				num = lotteryTicket.PrizeSettingList[2].PrizeNum;
				textBox4.Text = num.ToString();
				if (lotteryTicket.PrizeSettingList.Count > 3)
				{
					this.ooOpen.SelectedValue = true;
					this.txtPrize4.Text = lotteryTicket.PrizeSettingList[3].PrizeName;
					TextBox textBox5 = this.txtPrize4Num;
					num = lotteryTicket.PrizeSettingList[3].PrizeNum;
					textBox5.Text = num.ToString();
					this.txtPrize5.Text = lotteryTicket.PrizeSettingList[4].PrizeName;
					TextBox textBox6 = this.txtPrize5Num;
					num = lotteryTicket.PrizeSettingList[4].PrizeNum;
					textBox6.Text = num.ToString();
					this.txtPrize6.Text = lotteryTicket.PrizeSettingList[5].PrizeName;
					TextBox textBox7 = this.txtPrize6Num;
					num = lotteryTicket.PrizeSettingList[5].PrizeNum;
					textBox7.Text = num.ToString();
				}
				if (!string.IsNullOrEmpty(lotteryTicket.GradeIds) && lotteryTicket.GradeIds.Length > 1)
				{
					string[] collection = lotteryTicket.GradeIds.Split(',');
					List<string> list = new List<string>(collection);
					for (int i = 0; i < this.cbList.Items.Count; i++)
					{
						if (list.Contains(this.cbList.Items[i].Value))
						{
							this.cbList.Items[i].Selected = true;
						}
					}
				}
			}
		}

		private string UploadImage()
		{
			if (this.hidUploadImages.Value.IndexOf("/temp/") < 0)
			{
				return this.hidUploadImages.Value;
			}
			if (!File.Exists(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value)))
			{
				return this.hidUploadImages.Value;
			}
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

		protected void btnUpdateActivity_Click(object sender, EventArgs e)
		{
			LotteryTicketInfo lotteryTicket;
			if (!this.calendarStartDate.SelectedDate.HasValue)
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
					lotteryTicket = VShopHelper.GetLotteryTicket(this.activityid);
					if (lotteryTicket.ActivityKey != this.txtKeyword.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeyword.Text.Trim()))
					{
						this.ShowMsg("关键字重复!", false);
					}
					else
					{
						lotteryTicket.GradeIds = text;
						lotteryTicket.MinValue = Convert.ToInt32(this.txtMinValue.Text);
						lotteryTicket.InvitationCode = this.txtCode.Text.Trim();
						lotteryTicket.ActivityName = this.txtActiveName.Text;
						lotteryTicket.ActivityKey = this.txtKeyword.Text;
						lotteryTicket.ActivityDesc = this.txtdesc.Text;
						lotteryTicket.ActivityPic = activityPic;
						lotteryTicket.StartTime = this.calendarStartDate.SelectedDate.Value;
						lotteryTicket.OpenTime = this.calendarOpenDate.SelectedDate.Value;
						lotteryTicket.EndTime = this.calendarEndDate.SelectedDate.Value;
						int num = default(int);
						if (int.TryParse(this.txtPrize1Num.Text, out num) && int.TryParse(this.txtPrize2Num.Text, out num) && int.TryParse(this.txtPrize3Num.Text, out num))
						{
							lotteryTicket.PrizeSettingList.Clear();
							lotteryTicket.PrizeSettingList.Add(new PrizeSetting
							{
								PrizeName = this.txtPrize1.Text,
								PrizeNum = Convert.ToInt32(this.txtPrize1Num.Text),
								PrizeLevel = "一等奖"
							});
							lotteryTicket.PrizeSettingList.Add(new PrizeSetting
							{
								PrizeName = this.txtPrize2.Text,
								PrizeNum = Convert.ToInt32(this.txtPrize2Num.Text),
								PrizeLevel = "二等奖"
							});
							lotteryTicket.PrizeSettingList.Add(new PrizeSetting
							{
								PrizeName = this.txtPrize3.Text,
								PrizeNum = Convert.ToInt32(this.txtPrize3Num.Text),
								PrizeLevel = "三等奖"
							});
							if (this.ooOpen.SelectedValue)
							{
								if (string.IsNullOrEmpty(this.txtPrize4.Text) || string.IsNullOrEmpty(this.txtPrize5.Text) || string.IsNullOrEmpty(this.txtPrize6.Text))
								{
									this.ShowMsg("开启四五六名必须填写", false);
								}
								else
								{
									if (int.TryParse(this.txtPrize4Num.Text, out num) && int.TryParse(this.txtPrize5Num.Text, out num) && int.TryParse(this.txtPrize6Num.Text, out num))
									{
										lotteryTicket.PrizeSettingList.Add(new PrizeSetting
										{
											PrizeName = this.txtPrize4.Text,
											PrizeNum = Convert.ToInt32(this.txtPrize4Num.Text),
											PrizeLevel = "四等奖"
										});
										lotteryTicket.PrizeSettingList.Add(new PrizeSetting
										{
											PrizeName = this.txtPrize5.Text,
											PrizeNum = Convert.ToInt32(this.txtPrize5Num.Text),
											PrizeLevel = "五等奖"
										});
										lotteryTicket.PrizeSettingList.Add(new PrizeSetting
										{
											PrizeName = this.txtPrize6.Text,
											PrizeNum = Convert.ToInt32(this.txtPrize6Num.Text),
											PrizeLevel = "六等奖"
										});
										goto IL_0559;
									}
									this.ShowMsg("奖品数量必须为数字！", false);
								}
								return;
							}
							goto IL_0559;
						}
						this.ShowMsg("奖品数量必须为数字！", false);
					}
				}
			}
			return;
			IL_0559:
			if (VShopHelper.UpdateLotteryTicket(lotteryTicket))
			{
				base.Response.Redirect("ManageLotteryTicket.aspx");
			}
		}
	}
}
