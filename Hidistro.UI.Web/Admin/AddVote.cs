using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Votes)]
	public class AddVote : AdminPage
	{
		protected TextBox txtAddVoteName;

		protected CheckBox checkIsBackup;

		protected TextBox txtMaxCheck;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected TextBox txtValues;

		protected CheckBox chkDisplayWeixinStatic;

		protected CheckBox chkDisplayWeixin;

		protected HtmlGenericControl likey;

		protected TextBox txtKeys;

		protected HtmlGenericControl liimg;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected Button btnAddVote;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddVote.Click += this.btnAddVote_Click;
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenVstore != 1)
				{
					HtmlGenericControl htmlGenericControl = this.liimg;
					HtmlGenericControl htmlGenericControl2 = this.likey;
					CheckBox checkBox = this.chkDisplayWeixin;
					bool flag2 = checkBox.Visible = false;
					bool visible = htmlGenericControl2.Visible = flag2;
					htmlGenericControl.Visible = visible;
				}
			}
		}

		private void btnAddVote_Click(object sender, EventArgs e)
		{
			VoteInfo voteInfo = new VoteInfo();
			voteInfo.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
			voteInfo.IsBackup = true;
			int maxCheck = default(int);
			if (int.TryParse(this.txtMaxCheck.Text.Trim(), out maxCheck))
			{
				voteInfo.MaxCheck = maxCheck;
			}
			else
			{
				voteInfo.MaxCheck = -2147483648;
			}
			voteInfo.StartDate = this.calendarStartDate.SelectedDate;
			voteInfo.EndDate = this.calendarEndDate.SelectedDate;
			voteInfo.IsDisplayAtWX = this.chkDisplayWeixin.Checked;
			voteInfo.Keys = string.Empty;
			if (this.chkDisplayWeixin.Checked)
			{
				if (string.IsNullOrEmpty(this.txtKeys.Text.Trim()))
				{
					this.ShowMsg("关键字不能为空!", false);
					return;
				}
				if (ReplyHelper.HasReplyKey(this.txtKeys.Text.Trim()))
				{
					this.ShowMsg("关键字重复!", false);
					return;
				}
				voteInfo.Keys = this.txtKeys.Text.Trim();
				string empty = string.Empty;
				try
				{
					string text2 = voteInfo.ImageUrl = this.UploadImage();
				}
				catch (Exception ex)
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限" + ex.Message, false);
					return;
				}
			}
			IList<VoteItemInfo> list = null;
			if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
			{
				list = new List<VoteItemInfo>();
				string text3 = this.txtValues.Text.Trim().Replace("\r\n", "\n");
				string[] array = text3.Replace("\n", "*").Split('*');
				for (int i = 0; i < array.Length; i++)
				{
					VoteItemInfo voteItemInfo = new VoteItemInfo();
					if (array[i].Length > 60)
					{
						this.ShowMsg("投票选项长度限制在60个字符以内", false);
						return;
					}
					voteItemInfo.VoteItemName = Globals.HtmlEncode(array[i]);
					list.Add(voteItemInfo);
				}
				voteInfo.VoteItems = list;
				if (this.ValidationVote(voteInfo))
				{
					if (StoreHelper.CreateVote(voteInfo) > 0)
					{
						if (this.txtKeys.Text.Trim().Length > 0)
						{
							if (string.IsNullOrEmpty(voteInfo.ImageUrl))
							{
								voteInfo.ImageUrl = "/Storage/master/vote/default.jpg";
							}
							this.CreateVshopKeyReply(voteInfo.VoteId, voteInfo.ImageUrl);
						}
						this.ShowMsg("成功的添加了一个投票", true);
						this.txtAddVoteName.Text = string.Empty;
						this.checkIsBackup.Checked = false;
						this.txtMaxCheck.Text = string.Empty;
						this.txtValues.Text = string.Empty;
						this.chkDisplayWeixin.Checked = false;
						this.txtKeys.Text = string.Empty;
						this.liimg.Visible = false;
						this.likey.Visible = false;
					}
					else
					{
						this.ShowMsg("添加投票失败", false);
					}
				}
			}
			else
			{
				this.ShowMsg("投票选项不能为空", false);
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/vote/");
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
				return Globals.GetStoragePath() + "/vote/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/vote/" + text2;
		}

		private bool ValidationVote(VoteInfo vote)
		{
			ValidationResults validationResults = Validation.Validate(vote, "ValVote");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}

		protected void chkDisplayWeixin_CheckedChanged(object sender, EventArgs e)
		{
			HtmlGenericControl htmlGenericControl = this.likey;
			HtmlGenericControl htmlGenericControl2 = this.liimg;
			bool visible = htmlGenericControl2.Visible = this.chkDisplayWeixin.Checked;
			htmlGenericControl.Visible = visible;
		}

		private bool CreateVshopKeyReply(long voteId, string picUrl)
		{
			NewsReplyInfo newsReplyInfo = new NewsReplyInfo();
			newsReplyInfo.IsDisable = false;
			if (!string.IsNullOrWhiteSpace(this.txtKeys.Text))
			{
				newsReplyInfo.Keys = this.txtKeys.Text.Trim();
			}
			newsReplyInfo.MatchType = MatchType.Like;
			newsReplyInfo.MessageType = MessageType.News;
			newsReplyInfo.ReplyType = ReplyType.Keys;
			NewsMsgInfo item = new NewsMsgInfo
			{
				Reply = newsReplyInfo,
				Content = this.txtAddVoteName.Text,
				Description = this.txtAddVoteName.Text,
				Title = this.txtAddVoteName.Text,
				Url = base.Request.Url.Authority + "/vshop/Vote.aspx?voteId=" + voteId,
				PicUrl = picUrl
			};
			newsReplyInfo.NewsMsg = new List<NewsMsgInfo>();
			newsReplyInfo.NewsMsg.Add(item);
			return ReplyHelper.SaveReply(newsReplyInfo);
		}
	}
}
