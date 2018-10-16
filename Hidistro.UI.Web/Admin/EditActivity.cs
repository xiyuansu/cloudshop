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
	public class EditActivity : AdminPage
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

		protected Button btnEditActivity;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnEditActivity.Click += this.btnEditActivity_Click;
			if (!this.Page.IsPostBack)
			{
				int urlIntParam = base.GetUrlIntParam("id");
				VActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
				this.txtCloseRemark.Text = activity.CloseRemark;
				this.txtDescription.Text = activity.Description;
				this.txtEndDate.SelectedDate = activity.EndDate;
				this.txtKeys.Text = activity.Keys;
				if (activity.MaxValue != 0)
				{
					this.txtMaxValue.Text = activity.MaxValue.ToString();
				}
				this.txtName.Text = activity.Name;
				this.txtStartDate.SelectedDate = activity.StartDate;
				this.hidOldImages.Value = activity.PicUrl;
				this.txtItem1.Text = activity.Item1;
				this.txtItem2.Text = activity.Item2;
				this.txtItem3.Text = activity.Item3;
				this.txtItem4.Text = activity.Item4;
				this.txtItem5.Text = activity.Item5;
			}
		}

		private void btnEditActivity_Click(object sender, EventArgs e)
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
					int urlIntParam = base.GetUrlIntParam("id");
					VActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
					if (activity.Keys != this.txtKeys.Text.Trim() && ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
					{
						this.ShowMsg("关键字重复!", false);
					}
					else
					{
						activity.CloseRemark = this.txtCloseRemark.Text.Trim();
						activity.Description = this.txtDescription.Text.Trim();
						VActivityInfo vActivityInfo = activity;
						dateTime = this.txtEndDate.SelectedDate.Value;
						dateTime = dateTime.AddMinutes(59.0);
						vActivityInfo.EndDate = dateTime.AddSeconds(59.0);
						activity.Item1 = this.txtItem1.Text.Trim();
						activity.Item2 = this.txtItem2.Text.Trim();
						activity.Item3 = this.txtItem3.Text.Trim();
						activity.Item4 = this.txtItem4.Text.Trim();
						activity.Item5 = this.txtItem5.Text.Trim();
						activity.Keys = this.txtKeys.Text.Trim();
						activity.MaxValue = maxValue;
						activity.Name = this.txtName.Text.Trim();
						activity.PicUrl = this.UploadImage();
						activity.StartDate = this.txtStartDate.SelectedDate.Value;
						if (VShopHelper.UpdateActivity(activity))
						{
							base.Response.Redirect("ManageActivity.aspx");
						}
						else
						{
							this.ShowMsg("更新失败", false);
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
	}
}
