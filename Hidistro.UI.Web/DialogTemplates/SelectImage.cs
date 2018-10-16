using Hidistro.Context;
using Hidistro.Core;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.DialogTemplates
{
	public class SelectImage : Page
	{
		private string type = "";

		private int pagesize = 30;

		public int pagetotal = 0;

		public int pageindex = 1;

		public int sum = 0;

		protected HtmlForm form1;

		protected HtmlSelect slsbannerposition;

		protected HtmlGenericControl imagesize;

		protected Repeater rp_img;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.loadParamQuery();
			if (!string.IsNullOrEmpty(base.Request.QueryString["iscallback"]) && Convert.ToBoolean(base.Request.QueryString["iscallback"]))
			{
				this.UploadImage();
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["del"]))
			{
				string path = base.Request.QueryString["del"];
				string path2 = Globals.PhysicalPath(path);
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
			if (!base.IsPostBack)
			{
				this.DataBindImages();
			}
		}

		private void loadParamQuery()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["size"]))
			{
				this.imagesize.InnerText = Globals.HtmlEncode(this.Page.Request.QueryString["size"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["type"]))
			{
				this.slsbannerposition.Value = this.Page.Request.QueryString["type"];
				this.slsbannerposition.Disabled = true;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageindex"]) && Convert.ToInt32(this.Page.Request.QueryString["pageindex"]) > 0)
			{
				this.pageindex = Convert.ToInt32(this.Page.Request.QueryString["pageindex"]);
			}
		}

		private void UploadImage()
		{
			System.Drawing.Image image = null;
			System.Drawing.Image image2 = null;
			Bitmap bitmap = null;
			Graphics graphics = null;
			MemoryStream memoryStream = null;
			try
			{
				HttpPostedFile httpPostedFile = base.Request.Files["Filedata"];
				string str = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
				string str2 = HiContext.Current.GetPCHomePageSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/";
				string text = str + Path.GetExtension(httpPostedFile.FileName);
				httpPostedFile.SaveAs(Globals.MapPath(str2 + text));
				base.Response.StatusCode = 200;
				base.Response.Write(HiContext.Current.GetPCHomePageSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/" + text);
			}
			catch (Exception)
			{
				base.Response.StatusCode = 500;
				base.Response.Write("服务器错误");
				base.Response.End();
			}
			finally
			{
				bitmap?.Dispose();
				graphics?.Dispose();
				image2?.Dispose();
				image?.Dispose();
				memoryStream?.Close();
				base.Response.End();
			}
		}

		private void DataBindImages()
		{
			string path = Globals.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/UploadImage/" + this.slsbannerposition.Value);
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			IOrderedEnumerable<FileInfo> source = from file in directoryInfo.GetFiles()
			orderby file.CreationTime descending
			select file;
			this.sum = source.Count();
			this.pagetotal = this.sum / this.pagesize;
			if (this.sum % this.pagesize != 0)
			{
				this.pagetotal++;
			}
			if (this.pageindex < 1 || this.pageindex > this.pagetotal)
			{
				this.pageindex = 1;
			}
			this.rp_img.DataSource = source.Skip((this.pageindex - 1) * this.pagesize).Take(this.pagesize);
			this.rp_img.DataBind();
		}

		protected string ShowImage(string filename)
		{
			filename = HiContext.Current.GetPCHomePageSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/" + filename;
			return filename;
		}
	}
}
