using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class ImageReplace : AdminPage
	{
		protected HiddenField RePlaceImg;

		protected HiddenField RePlaceId;

		protected FileUpload FileUpload1;

		protected HtmlInputHidden Hidden1;

		protected Button btnSaveImageData;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack && !string.IsNullOrEmpty(this.Page.Request.QueryString["imgsrc"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["imgId"]))
			{
				string value = Globals.HtmlDecode(this.Page.Request.QueryString["imgsrc"]);
				string value2 = Globals.HtmlDecode(this.Page.Request.QueryString["imgId"]);
				this.RePlaceImg.Value = value;
				this.RePlaceId.Value = value2;
			}
			this.btnSaveImageData.Click += this.btnSaveImageData_Click;
		}

		protected void btnSaveImageData_Click(object sender, EventArgs e)
		{
			string value = this.RePlaceImg.Value;
			int photoId = Convert.ToInt32(this.RePlaceId.Value);
			string photoPath = GalleryHelper.GetPhotoPath(photoId);
			string a = photoPath.Substring(photoPath.LastIndexOf("."));
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				HttpFileCollection files = base.Request.Files;
				HttpPostedFile httpPostedFile = files[0];
				empty = Path.GetExtension(httpPostedFile.FileName);
				if (a != empty)
				{
					this.ShowMsg("上传图片类型与原文件类型不一致！", false);
				}
				else
				{
					string str = HiContext.Current.GetStoragePath() + "/gallery";
					empty2 = photoPath.Substring(photoPath.LastIndexOf("/") + 1);
					string text = value.Substring(value.LastIndexOf("/") - 6, 6);
					string text2 = str + "/" + text + "/";
					int contentLength = httpPostedFile.ContentLength;
					string path = base.Request.MapPath(text2);
					string text3 = text + "/" + empty2;
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					if (!ResourcesHelper.CheckPostedFile(httpPostedFile, "image", null))
					{
						this.ShowMsg("文件上传的类型不正确！", false);
					}
					else if (contentLength >= 2048000)
					{
						this.ShowMsg("图片文件已超过网站限制大小！", false);
					}
					else
					{
						httpPostedFile.SaveAs(base.Request.MapPath(text2 + empty2));
						GalleryHelper.ReplacePhoto(photoId, contentLength);
						this.CloseWindow();
					}
				}
			}
			catch
			{
				this.ShowMsg("替换文件错误!", false);
			}
		}
	}
}
