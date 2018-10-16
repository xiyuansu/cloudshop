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
	public class EditVote : AdminPage
	{
		private long voteId;

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

		protected HiddenField txtOldKeys;

		protected HtmlGenericControl liimg;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected Button btnEditVote;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["VoteId"], out this.voteId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditVote.Click += this.btnEditVote_Click;
				if (!this.Page.IsPostBack)
				{
					VoteInfo voteById = StoreHelper.GetVoteById(this.voteId);
					IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(this.voteId);
					if (voteById == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.txtAddVoteName.Text = Globals.HtmlDecode(voteById.VoteName);
						this.txtMaxCheck.Text = voteById.MaxCheck.ToString();
						if (string.IsNullOrEmpty(voteById.ImageUrl))
						{
							this.hidOldImages.Value = "/Storage/master/vote/default.jpg";
						}
						else
						{
							this.hidOldImages.Value = voteById.ImageUrl;
						}
						string text = "";
						foreach (VoteItemInfo item in voteItems)
						{
							text = text + Globals.HtmlDecode(item.VoteItemName) + "\r\n";
						}
						this.txtValues.Text = text;
						this.calendarStartDate.SelectedDate = voteById.StartDate;
						this.calendarEndDate.SelectedDate = voteById.EndDate;
						this.txtKeys.Text = voteById.Keys;
						this.txtOldKeys.Value = voteById.Keys;
						this.chkDisplayWeixin.Checked = voteById.IsDisplayAtWX;
						this.chkDisplayWeixinStatic.Checked = voteById.IsDisplayAtWX;
						HtmlGenericControl htmlGenericControl = this.likey;
						HtmlGenericControl htmlGenericControl2 = this.liimg;
						bool visible = htmlGenericControl2.Visible = voteById.IsDisplayAtWX;
						htmlGenericControl.Visible = visible;
						SiteSettings masterSettings = SettingsManager.GetMasterSettings();
						if (masterSettings.OpenVstore != 1)
						{
							HtmlGenericControl htmlGenericControl3 = this.liimg;
							HtmlGenericControl htmlGenericControl4 = this.likey;
							CheckBox checkBox = this.chkDisplayWeixin;
							bool flag2 = checkBox.Visible = false;
							visible = (htmlGenericControl4.Visible = flag2);
							htmlGenericControl3.Visible = visible;
						}
					}
				}
			}
		}

		private void btnEditVote_Click(object sender, EventArgs e)
		{
			if (StoreHelper.GetVoteCounts(this.voteId) > 0)
			{
				this.ShowMsg("投票已经开始，不能再对投票选项进行任何操作", false);
			}
			else
			{
				VoteInfo voteInfo = new VoteInfo();
				voteInfo.VoteName = Globals.HtmlEncode(this.txtAddVoteName.Text.Trim());
				voteInfo.VoteId = this.voteId;
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
				voteInfo.IsBackup = true;
				if (this.chkDisplayWeixin.Checked)
				{
					if (string.IsNullOrEmpty(this.txtKeys.Text.Trim()))
					{
						this.ShowMsg("关键字不能为空!", false);
						return;
					}
					ReplyHelper.DeleteReplyKey(this.txtOldKeys.Value.Trim());
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
						this.hidOldImages.Value = voteInfo.ImageUrl;
					}
					catch
					{
						this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
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
						if (StoreHelper.UpdateVote(voteInfo))
						{
							if (this.txtKeys.Text.Trim().Length > 0 && !string.IsNullOrEmpty(this.hidUploadImages.Value))
							{
								this.CreateVshopKeyReply(voteInfo.VoteId, voteInfo.ImageUrl);
							}
							this.ShowMsg("修改投票成功", true);
						}
						else
						{
							this.ShowMsg("修改投票失败", false);
						}
					}
				}
				else
				{
					this.ShowMsg("投票选项不能为空", false);
				}
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
	}
}
