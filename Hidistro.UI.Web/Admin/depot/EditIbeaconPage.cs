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
	public class EditIbeaconPage : AdminPage
	{
		private long page_id = 0L;

		protected TextBox txtTitle;

		protected TextBox txtSubtitle;

		protected HiddenField hficon_url;

		protected DropDownList ddlSystemUrl;

		protected TextBox txtURL;

		protected TextBox txtRemark;

		protected Button btnEdit;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnEdit.Click += this.btnEdit_Click;
			this.page_id = base.Request.QueryString["page_id"].ToLong(0);
			if (!base.IsPostBack)
			{
				this.BindPage();
			}
		}

		private string GetFullURL(string shortURL)
		{
			return string.Format("{0}{1}", base.Request.Url.ToString().Substring(0, base.Request.Url.ToString().ToLower().IndexOf("admin")), shortURL);
		}

		private void BindPage()
		{
			SearchPagesResultJson searchPagesResultJson = WXStoreHelper.SearchPagesByPageId(new long[1]
			{
				this.page_id
			});
			if (searchPagesResultJson.errcode.Equals(ReturnCode.请求成功) && searchPagesResultJson.data.pages.Count > 0)
			{
				SearchPages_Data_Page searchPages_Data_Page = searchPagesResultJson.data.pages[0];
				this.txtRemark.Text = searchPages_Data_Page.comment;
				this.txtSubtitle.Text = searchPages_Data_Page.description;
				this.txtTitle.Text = searchPages_Data_Page.title;
				this.txtURL.Text = searchPages_Data_Page.page_url;
				this.hficon_url.Value = searchPages_Data_Page.icon_url;
			}
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			string text = this.txtTitle.Text.Trim();
			string text2 = this.txtSubtitle.Text.Trim();
			string text3 = this.txtURL.Text.Trim();
			string text4 = this.txtRemark.Text.Trim();
			string text5 = string.Empty;
			string empty = string.Empty;
			string iconUrl = string.Empty;
			if (!string.IsNullOrEmpty(this.hficon_url.Value))
			{
				text5 = this.hficon_url.Value.Replace("//", "/");
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
				if (this.hficon_url.Value.IndexOf("http://") < 0)
				{
					System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(this.hficon_url.Value));
					if (!image.Width.Equals(image.Height) || image.Height == 0 || image.Width == 0)
					{
						this.ShowMsg("图片大小建议120px*120 px，限制不超过200 px *200 px，图片需为正方形。", false);
						return;
					}
					image.Dispose();
					empty = Globals.SaveFile("images", text5, "/Storage/master/store/", true, true, "");
					UploadImageResultJson uploadImageResultJson = WXStoreHelper.UploadImageToWXShakeAShake(empty);
					if (uploadImageResultJson.errcode.Equals(ReturnCode.请求成功))
					{
						iconUrl = uploadImageResultJson.data.pic_url;
					}
				}
				else
				{
					iconUrl = this.hficon_url.Value.ToString();
				}
				UpdatePageResultJson updatePageResultJson = WXStoreHelper.UpdatePage(this.page_id, text, text2, text3, iconUrl, text4);
				if (updatePageResultJson.errcode == ReturnCode.请求成功)
				{
					base.Response.Redirect("IbeaconPageList.aspx");
				}
			}
		}
	}
}
