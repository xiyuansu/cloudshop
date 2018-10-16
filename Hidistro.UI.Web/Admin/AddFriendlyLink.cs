using Hidistro.Core;
using Hidistro.Entities.Store;
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
	public class AddFriendlyLink : AdminPage
	{
		protected TextBox txtaddTitle;

		protected HtmlGenericControl txtaddTitleTip;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtaddLinkUrl;

		protected OnOff ooShowLinks;

		protected Button btnSubmitLinks;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.ooShowLinks.SelectedValue = true;
			}
			this.btnSubmitLinks.Click += this.btnSubmitLinks_Click;
		}

		private void btnSubmitLinks_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			if (this.hidUploadImages.Value.Trim().Length == 0 && string.IsNullOrEmpty(this.txtaddTitle.Text.Trim()))
			{
				this.ShowMsg("友情链接Logo和网站名称不能同时为空", false);
			}
			else
			{
				FriendlyLinksInfo friendlyLinksInfo = new FriendlyLinksInfo();
				try
				{
					empty = this.UploadImage();
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
				friendlyLinksInfo.ImageUrl = empty;
				this.hidOldImages.Value = empty;
				friendlyLinksInfo.LinkUrl = this.txtaddLinkUrl.Text;
				friendlyLinksInfo.Title = Globals.HtmlEncode(this.txtaddTitle.Text.Trim());
				friendlyLinksInfo.Visible = this.ooShowLinks.SelectedValue;
				ValidationResults validationResults = Validation.Validate(friendlyLinksInfo, "ValFriendlyLinksInfo");
				string text = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						text += Formatter.FormatErrorMessage(item.Message);
					}
					this.ShowMsg(text, false);
				}
				else
				{
					this.AddNewFriendlyLink(friendlyLinksInfo);
					this.Reset();
				}
			}
		}

		private string UploadImage()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/link/");
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
				return Globals.GetStoragePath() + "/link/" + text2;
			}
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), text + text2);
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/link/" + text2;
		}

		private void AddNewFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			if (StoreHelper.CreateFriendlyLink(friendlyLink))
			{
				this.ShowMsg("成功添加了一个友情链接", true);
			}
			else
			{
				this.ShowMsg("未知错误", false);
			}
		}

		private void Reset()
		{
			this.txtaddTitle.Text = string.Empty;
			this.ooShowLinks.SelectedValue = true;
			this.txtaddLinkUrl.Text = string.Empty;
			this.hidOldImages.Value = string.Empty;
			this.hidUploadImages.Value = string.Empty;
		}
	}
}
