using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PictureMange)]
	public class ImageFtp : AdminCallBackPage
	{
		protected ImageDataGradeDropDownList dropImageFtp;

		protected ImageTypeLabel ImageTypeID;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected Button btnSaveImageFtp;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSaveImageFtp.Click += this.btnSaveImageFtp_Click;
			if (!this.Page.IsPostBack)
			{
				this.dropImageFtp.DataBind();
			}
		}

		private void btnSaveImageFtp_Click(object sender, EventArgs e)
		{
			this.UploadImage();
		}

		private void UploadImage()
		{
			string value = this.hidUploadImages.Value;
			if (value.Trim().Length == 0)
			{
				this.ShowMsg("至少需要选择一个图片文件！", false);
			}
			else
			{
				int categoryId = Convert.ToInt32(this.dropImageFtp.SelectedItem.Value);
				int num = 0;
				string[] array = value.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						string text = array[i].Split('/')[4];
						string text2 = DateTime.Now.ToString("yyyyMM").Substring(0, 6);
						string photoPath = "/Storage/master/gallery/" + text2 + "/" + text;
						string str = Globals.GetStoragePath() + "/temp/";
						string text3 = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/gallery/" + text2 + "/");
						if (!Globals.PathExist(text3, false))
						{
							Globals.CreatePath(text3);
						}
						if (!File.Exists(text3 + text))
						{
							File.Copy(HttpContext.Current.Server.MapPath(array[i]), text3 + text);
							FileInfo fileInfo = new FileInfo(text3 + text);
							int fileSize = 0;
							int.TryParse(fileInfo.Length.ToString(), out fileSize);
							if (GalleryHelper.AddPhote(categoryId, text, photoPath, fileSize, 0))
							{
								num++;
							}
							string path = HttpContext.Current.Server.MapPath(str + text);
							if (File.Exists(path))
							{
								File.Delete(path);
							}
							continue;
						}
						return;
					}
				}
				base.CloseWindow(null);
				this.ShowMsg("成功上传了" + num.ToString() + "个文件！", true);
			}
		}
	}
}
