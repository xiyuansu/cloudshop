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
	[PrivilegeCheck(Privilege.FriendlyLinks)]
	public class EditFriendlyLink : AdminPage
	{
		private int linkId;

		protected TextBox txtaddTitle;

		protected HtmlGenericControl txtaddTitleTip;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtaddLinkUrl;

		protected OnOff ooShowLinks;

		protected Button btnSubmitLinks;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitLinks.Click += this.btnSubmitLinks_Click;
			if (!int.TryParse(base.Request.QueryString["linkId"], out this.linkId))
			{
				base.GotoResourceNotFound();
			}
			else if (!base.IsPostBack)
			{
				FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(this.linkId);
				if (friendlyLink == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.txtaddTitle.Text = Globals.HtmlDecode(friendlyLink.Title);
					this.txtaddLinkUrl.Text = friendlyLink.LinkUrl;
					this.ooShowLinks.SelectedValue = friendlyLink.Visible;
					this.hidOldImages.Value = friendlyLink.ImageUrl;
				}
			}
		}

		private void btnSubmitLinks_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			try
			{
				empty = this.UploadImage();
			}
			catch
			{
				this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
				return;
			}
			FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(this.linkId);
			friendlyLink.ImageUrl = empty;
			this.hidOldImages.Value = empty;
			friendlyLink.LinkUrl = this.txtaddLinkUrl.Text;
			friendlyLink.Title = Globals.HtmlEncode(this.txtaddTitle.Text.Trim());
			friendlyLink.Visible = this.ooShowLinks.SelectedValue;
			if (string.IsNullOrEmpty(friendlyLink.ImageUrl) && string.IsNullOrEmpty(friendlyLink.Title))
			{
				this.ShowMsg("友情链接Logo和网站名称不能同时为空", false);
			}
			else
			{
				ValidationResults validationResults = Validation.Validate(friendlyLink, "ValFriendlyLinksInfo");
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
					this.UpdateFriendlyLink(friendlyLink);
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

		private void UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			if (StoreHelper.UpdateFriendlyLink(friendlyLink))
			{
				this.ShowMsg("修改友情链接信息成功", true);
			}
			else
			{
				this.ShowMsg("修改友情链接信息失败", false);
			}
		}
	}
}
