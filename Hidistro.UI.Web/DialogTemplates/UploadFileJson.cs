using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.DialogTemplates
{
	public class UploadFileJson : IHttpHandler
	{
		private string savePath;

		private string saveUrl;

		private string message = "";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			string str = "AdvertImg";
			if (manager == null)
			{
				this.showError("您没有权限执行此操作");
			}
			else
			{
				if (context.Request.Form["fileCategory"] != null)
				{
					str = context.Request.Form["fileCategory"];
				}
				string text = string.Empty;
				if (context.Request.Form["imgTitle"] != null)
				{
					text = context.Request.Form["imgTitle"];
				}
				this.savePath = string.Format("{0}/UploadImage/" + str + "/", HiContext.Current.GetPCHomePageSkinPath());
				if (context.Request.ApplicationPath != "/")
				{
					this.saveUrl = this.savePath.Substring(context.Request.ApplicationPath.Length);
				}
				else
				{
					this.saveUrl = this.savePath;
				}
				HttpPostedFile httpPostedFile = context.Request.Files["imgFile"];
				string text2 = "";
				if (this.CheckUploadFile(httpPostedFile, ref text2))
				{
					if (!Directory.Exists(text2))
					{
						Directory.CreateDirectory(text2);
					}
					string fileName = httpPostedFile.FileName;
					if (text.Length == 0)
					{
						text = fileName;
					}
					string str2 = Path.GetExtension(fileName).ToLower();
					string str3 = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + str2;
					string filename = text2 + str3;
					string value = this.saveUrl + str3;
					try
					{
						httpPostedFile.SaveAs(filename);
						Hashtable hashtable = new Hashtable();
						hashtable["error"] = 0;
						hashtable["url"] = value;
						this.message = JsonMapper.ToJson(hashtable);
					}
					catch
					{
						this.showError("保存文件出错");
					}
				}
			}
			context.Response.ContentType = "text/html";
			context.Response.Write(this.message);
		}

		private bool CheckUploadFile(HttpPostedFile imgFile, ref string dirPath)
		{
			if (imgFile == null)
			{
				this.showError("请选择上传文件");
				return false;
			}
			if (!ResourcesHelper.CheckPostedFile(imgFile, "image", null))
			{
				this.showError("不能上传空文件，且必须是有效的图片文件！");
				return false;
			}
			dirPath = Globals.MapPath(this.savePath);
			if (!Directory.Exists(dirPath))
			{
				this.showError("上传目录不存在。");
				return false;
			}
			return true;
		}

		private void showError(string message)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["error"] = 1;
			hashtable["message"] = message;
			message = JsonConvert.SerializeObject(hashtable);
		}
	}
}
