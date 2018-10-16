using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class AddRedEnvelope : AdminPage
	{
		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtName;

		protected TextBox txtMaxNumber;

		protected RadioButton rdbTypeRandom;

		protected RadioButton rdbTypeFixed;

		protected TextBox txtMinAmount;

		protected TextBox txtMaxAmount;

		protected TextBox txtAmountFixed;

		protected RadioButton rdbUnlimited;

		protected RadioButton rdbSatisfy;

		protected TextBox txtEnableUseMinAmount;

		protected TextBox txtEnableIssueMinAmount;

		protected CalendarPanel txtActiveStartTime;

		protected CalendarPanel txtActiveEndTime;

		protected CalendarPanel txtEffectivePeriodStartTime;

		protected CalendarPanel txtEffectivePeriodEndTime;

		protected TextBox txtShareTitle;

		protected TextBox txtShareDetails;

		protected Button Submit;

		protected void Page_Load(object sender, EventArgs e)
		{
			Dictionary<string, object> calendarParameter = this.txtActiveStartTime.CalendarParameter;
			DateTime now = DateTime.Now;
			calendarParameter.Add("startDate", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter2 = this.txtActiveEndTime.CalendarParameter;
			now = DateTime.Now;
			calendarParameter2.Add("startDate", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter3 = this.txtEffectivePeriodStartTime.CalendarParameter;
			now = DateTime.Now;
			calendarParameter3.Add("startDate", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter4 = this.txtEffectivePeriodEndTime.CalendarParameter;
			now = DateTime.Now;
			calendarParameter4.Add("startDate", now.ToString("yyyy-MM-dd"));
		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			WeiXinRedEnvelopeInfo weiXinRedEnvelopeInfo = new WeiXinRedEnvelopeInfo();
			if (this.CheckeGetWeiXinRedEnvelope(weiXinRedEnvelopeInfo))
			{
				weiXinRedEnvelopeInfo.ShareIcon = this.UploadImage();
				WeiXinRedEnvelopeProcessor.AddWeiXinRedEnvelope(weiXinRedEnvelopeInfo);
				this.txtName.Text = string.Empty;
				this.ShowMsg("代金红包促销添加成功！", true);
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/Vshop/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			if (value.Trim().Contains("http:"))
			{
				return value.Trim();
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/Vshop/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/Vshop/" + text2;
		}

		public bool CheckeGetWeiXinRedEnvelope(WeiXinRedEnvelopeInfo weiXinRedEnvelope)
		{
			WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
			if (openedWeiXinRedEnvelope != null)
			{
				this.ShowMsg("已经存在正在进行中的活动，不能再次添加！", false);
				return false;
			}
			Regex regex = new Regex("^([1-9]\\d*\\.\\d*|0\\.\\d*[1-9]\\d*)|([1-9]\\d*)$");
			weiXinRedEnvelope.Name = this.txtName.Text.Trim();
			if (string.IsNullOrEmpty(weiXinRedEnvelope.Name))
			{
				this.ShowMsg("代金红包名称不能为空", false);
				return false;
			}
			if (weiXinRedEnvelope.Name.Length > 64)
			{
				this.ShowMsg("代金红包名称不能超过64个字", false);
				return false;
			}
			Regex regex2 = new Regex("^[1-9]\\d*$");
			if (!regex2.IsMatch(this.txtMaxNumber.Text.Trim()))
			{
				this.ShowMsg("单次分享可领个数必须是大于零的整数", false);
				return false;
			}
			weiXinRedEnvelope.MaxNumber = Convert.ToInt32(this.txtMaxNumber.Text.Trim());
			if (weiXinRedEnvelope.MaxNumber <= 0)
			{
				this.ShowMsg("单次分享可领个数必须大于零", false);
				return false;
			}
			if (weiXinRedEnvelope.MaxNumber > 888)
			{
				this.ShowMsg("单次分享可领个数不能大于888个", false);
				return false;
			}
			weiXinRedEnvelope.ActualNumber = 0;
			if (this.rdbTypeRandom.Checked)
			{
				weiXinRedEnvelope.Type = 0;
				if (string.IsNullOrEmpty(this.txtMinAmount.Text))
				{
					this.ShowMsg("代金红包随机最少金额不能为空", false);
					return false;
				}
				if (!regex.IsMatch(this.txtMinAmount.Text))
				{
					this.ShowMsg("代金红包随机最少金额必须是有效的正数", false);
					return false;
				}
				if (string.IsNullOrEmpty(this.txtMaxAmount.Text))
				{
					this.ShowMsg("代金红包随机最大金额不能为空", false);
					return false;
				}
				if (!regex.IsMatch(this.txtMaxAmount.Text))
				{
					this.ShowMsg("代金红包随机最大金额必须是有效的正数", false);
					return false;
				}
				weiXinRedEnvelope.MinAmount = Convert.ToDecimal(this.txtMinAmount.Text);
				weiXinRedEnvelope.MaxAmount = Convert.ToDecimal(this.txtMaxAmount.Text);
				if (weiXinRedEnvelope.MinAmount <= decimal.Zero)
				{
					this.ShowMsg("代金红包随机最少金额必须大于零", false);
					return false;
				}
				if (weiXinRedEnvelope.MaxAmount < weiXinRedEnvelope.MinAmount)
				{
					this.ShowMsg("代金红包随机最大金额必须大于最少金额", false);
					return false;
				}
			}
			else if (this.rdbTypeFixed.Checked)
			{
				weiXinRedEnvelope.Type = 1;
				if (!regex.IsMatch(this.txtAmountFixed.Text))
				{
					this.ShowMsg("代金红包固定金额必须是有效的正数", false);
					return false;
				}
				decimal num3 = weiXinRedEnvelope.MinAmount = (weiXinRedEnvelope.MaxAmount = Convert.ToDecimal(this.txtAmountFixed.Text.Trim()));
				if (weiXinRedEnvelope.MinAmount <= decimal.Zero)
				{
					this.ShowMsg("代金红包固定金额必须大于零", false);
					return false;
				}
			}
			if (this.rdbUnlimited.Checked)
			{
				weiXinRedEnvelope.EnableUseMinAmount = decimal.Zero;
			}
			else if (this.rdbSatisfy.Checked)
			{
				if (!regex.IsMatch(this.txtEnableUseMinAmount.Text))
				{
					this.ShowMsg("使用条件金额必须是有效的正数", false);
					return false;
				}
				weiXinRedEnvelope.EnableUseMinAmount = Convert.ToDecimal(this.txtEnableUseMinAmount.Text.Trim());
				if (weiXinRedEnvelope.EnableUseMinAmount <= decimal.Zero)
				{
					this.ShowMsg("使用条件金额不能少于零", false);
					return false;
				}
			}
			if (!regex.IsMatch(this.txtEnableIssueMinAmount.Text))
			{
				this.ShowMsg("代金红包发放条件金额必须是有效的正数", false);
				return false;
			}
			weiXinRedEnvelope.EnableIssueMinAmount = Convert.ToDecimal(this.txtEnableIssueMinAmount.Text.Trim());
			if (weiXinRedEnvelope.EnableIssueMinAmount <= decimal.Zero)
			{
				this.ShowMsg("代金红包发放条件必须大于零", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtActiveStartTime.Text.Trim()))
			{
				this.ShowMsg("代金红包活动开始时间不能为空", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtActiveEndTime.Text.Trim()))
			{
				this.ShowMsg("代金红包活动结束时间不能为空", false);
				return false;
			}
			weiXinRedEnvelope.ActiveStartTime = DateTime.Parse(this.txtActiveStartTime.Text.Trim() + " 00:00:00");
			weiXinRedEnvelope.ActiveEndTime = DateTime.Parse(this.txtActiveEndTime.Text.Trim() + " 23:59:59");
			if (weiXinRedEnvelope.ActiveStartTime > weiXinRedEnvelope.ActiveEndTime)
			{
				this.ShowMsg("代金红包活动开始时间不能大于代金红包活动结束时间", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtEffectivePeriodStartTime.Text.Trim()))
			{
				this.ShowMsg("代金红包有效期的起始时间不能为空", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.txtEffectivePeriodEndTime.Text.Trim()))
			{
				this.ShowMsg("代金红包有效期的结束时间不能为空", false);
				return false;
			}
			weiXinRedEnvelope.EffectivePeriodStartTime = DateTime.Parse(this.txtEffectivePeriodStartTime.Text.Trim() + " 00:00:00");
			weiXinRedEnvelope.EffectivePeriodEndTime = DateTime.Parse(this.txtEffectivePeriodEndTime.Text.Trim() + " 23:59:59");
			if (weiXinRedEnvelope.EffectivePeriodStartTime > weiXinRedEnvelope.EffectivePeriodEndTime)
			{
				this.ShowMsg("代金红包有效期的起始时间不能大于代金红包有效期的结束时间", false);
				return false;
			}
			if (string.IsNullOrEmpty(this.hidUploadImages.Value))
			{
				this.ShowMsg("分享图标不能为空", false);
				return false;
			}
			weiXinRedEnvelope.ShareIcon = this.hidUploadImages.Value;
			weiXinRedEnvelope.ShareTitle = this.txtShareTitle.Text.Trim();
			if (string.IsNullOrEmpty(weiXinRedEnvelope.ShareTitle))
			{
				this.ShowMsg("代金红包在微信中的分享标题不能为空", false);
				return false;
			}
			if (weiXinRedEnvelope.ShareTitle.Length > 256)
			{
				this.ShowMsg("代金红包分享详情不能超过256个字", false);
				return false;
			}
			weiXinRedEnvelope.ShareDetails = this.txtShareDetails.Text.Trim();
			if (string.IsNullOrEmpty(weiXinRedEnvelope.ShareDetails))
			{
				this.ShowMsg("代金红包在微信中的分享详情不能为空", false);
				return false;
			}
			if (weiXinRedEnvelope.ShareDetails.Length > 1024)
			{
				this.ShowMsg("代金红包分享详情不能超过1024个字", false);
				return false;
			}
			weiXinRedEnvelope.CreateTime = DateTime.Now;
			weiXinRedEnvelope.State = 1;
			return true;
		}
	}
}
