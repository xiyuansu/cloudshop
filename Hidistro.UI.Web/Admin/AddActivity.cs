using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AddActivity : AdminPage
	{
		protected TextBox txtName;

		protected CalendarPanel txtStartDate;

		protected CalendarPanel txtEndDate;

		protected TextBox txtMaxValue;

		protected TextBox txtDescription;

		protected TextBox txtItem1;

		protected TextBox txtItem2;

		protected TextBox txtItem3;

		protected TextBox txtItem4;

		protected TextBox txtItem5;

		protected TextBox txtCloseRemark;

		protected TextBox txtKeys;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected Button btnAddActivity;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddActivity.Click += this.btnAddActivity_Click;
		}

		private void btnAddActivity_Click(object sender, EventArgs e)
		{
			if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
			{
				this.ShowMsg("关键字重复!", false);
			}
			else
			{
				int maxValue = 0;
				if (!this.txtStartDate.SelectedDate.HasValue)
				{
					this.ShowMsg("请选择开始日期！", false);
				}
				else if (!this.txtEndDate.SelectedDate.HasValue)
				{
					this.ShowMsg("请选择结束日期！", false);
				}
				else
				{
					DateTime dateTime = this.txtStartDate.SelectedDate.Value;
					if (dateTime.CompareTo(this.txtEndDate.SelectedDate.Value) >= 0)
					{
						this.ShowMsg("开始日期不能晚于结束日期！", false);
					}
					else if (this.txtMaxValue.Text != "" && !int.TryParse(this.txtMaxValue.Text, out maxValue))
					{
						this.ShowMsg("人数上限格式错误！", false);
					}
					else
					{
						VActivityInfo vActivityInfo = new VActivityInfo();
						vActivityInfo.CloseRemark = this.txtCloseRemark.Text.Trim();
						vActivityInfo.Description = this.txtDescription.Text.Trim();
						VActivityInfo vActivityInfo2 = vActivityInfo;
						dateTime = this.txtEndDate.SelectedDate.Value;
						dateTime = dateTime.AddMinutes(59.0);
						vActivityInfo2.EndDate = dateTime.AddSeconds(59.0);
						vActivityInfo.Item1 = this.txtItem1.Text.Trim();
						vActivityInfo.Item2 = this.txtItem2.Text.Trim();
						vActivityInfo.Item3 = this.txtItem3.Text.Trim();
						vActivityInfo.Item4 = this.txtItem4.Text.Trim();
						vActivityInfo.Item5 = this.txtItem5.Text.Trim();
						vActivityInfo.Keys = this.txtKeys.Text.Trim();
						vActivityInfo.MaxValue = maxValue;
						vActivityInfo.Name = this.txtName.Text.Trim();
						vActivityInfo.PicUrl = this.UploadImage();
						vActivityInfo.StartDate = this.txtStartDate.SelectedDate.Value;
						if (VShopHelper.SaveActivity(vActivityInfo))
						{
							base.Response.Redirect("ManageActivity.aspx");
						}
						else
						{
							this.ShowMsg("添加失败", false);
						}
					}
				}
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
	}
}
