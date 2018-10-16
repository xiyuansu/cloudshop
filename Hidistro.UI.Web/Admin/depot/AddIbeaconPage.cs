using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.ShakeAround;
using System;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[WeiXinCheck(true)]
	public class AddIbeaconPage : AdminPage
	{
		protected TextBox txtTitle;

		protected TextBox txtSubtitle;

		protected HiddenField hidUploadLogo;

		protected DropDownList ddlSystemUrl;

		protected TextBox txtURL;

		protected TextBox txtRemark;

		protected Button btnAdd;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (base.IsPostBack)
			{
				return;
			}
		}

		private string GetFullURL(string shortURL)
		{
			return string.Format("{0}{1}", base.Request.Url.ToString().Substring(0, base.Request.Url.ToString().ToLower().IndexOf("admin")), shortURL);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			string text = this.txtTitle.Text.Trim();
			string text2 = this.txtSubtitle.Text.Trim();
			string text3 = this.txtURL.Text.Trim();
			string text4 = this.txtRemark.Text.Trim();
			string text5 = string.Empty;
			string empty = string.Empty;
			string iconUrl = string.Empty;
			if (!string.IsNullOrEmpty(this.hidUploadLogo.Value))
			{
				text5 = this.hidUploadLogo.Value.Replace("//", "/");
			}
			if (string.IsNullOrEmpty(text) || text.Length > 100)
			{
				this.ShowMsg("主标题不能为空，长度必须小于或等于100个字符", false);
			}
			else if (string.IsNullOrEmpty(text2) || text2.Length > 100)
			{
				this.ShowMsg("副标题不能为空，长度必须小于或等于100个字符", false);
			}
			else if (string.IsNullOrEmpty(text3) || text3.Length > 300)
			{
				this.ShowMsg("跳转URL不能为空，长度必须小于或等于300个字符", false);
			}
			else if (string.IsNullOrEmpty(text4) || text4.Length > 100)
			{
				this.ShowMsg("备注不能为空，长度必须小于或等于100个字符", false);
			}
			else if (string.IsNullOrEmpty(text5))
			{
				this.ShowMsg("请上传缩略图", false);
			}
			else
			{
				System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(this.hidUploadLogo.Value));
				if (!image.Width.Equals(image.Height) || image.Height == 0 || image.Width == 0)
				{
					this.ShowMsg("图片大小建议120px*120 px，限制不超过200 px *200 px，图片需为正方形。", false);
				}
				else
				{
					image.Dispose();
					empty = Globals.SaveFile("images", text5, "/Storage/master/store/", true, true, "");
					UploadImageResultJson uploadImageResultJson = WXStoreHelper.UploadImageToWXShakeAShake(empty);
					if (uploadImageResultJson.errcode.Equals(ReturnCode.请求成功))
					{
						iconUrl = uploadImageResultJson.data.pic_url;
					}
					AddPageResultJson addPageResultJson = WXStoreHelper.AddPage(text, text2, text3, iconUrl, text4);
					if (addPageResultJson.errcode == ReturnCode.请求成功)
					{
						base.Response.Redirect("IbeaconPageList.aspx");
					}
				}
			}
		}
	}
}
