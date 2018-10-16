using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class UploadHandler : IHttpHandler
	{
		private string uploaderId;

		private string uploadType;

		private string action;

		private string fileType = "Image";

		private string uploadPaths = string.Empty;

		private string ControlID = "";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.AddHeader("Access-Control-Allow-Origin", "*");
			context.Response.ContentType = "text/plain";
			HttpRequest request = context.Request;
			string text = request["action"];
			switch (text)
			{
			case "upload":
				this.UploadImage();
				break;
			case "newupload":
				this.NewUploadImage();
				break;
			case "remoteupdateimages":
				UploadHandler.RemoteUpdateImages();
				break;
			case "oldupload":
				this.OldUpload(context);
				break;
			case "delete":
				this.DeleteImage();
				break;
			case "olddelete":
				this.DoDelete(context);
				break;
			default:
				context.Response.Write("false");
				break;
			}
		}

		private void OldUpload(HttpContext context)
		{
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			context.Response.ContentType = "text/html";
			this.uploaderId = context.Request["uploaderId"].ToNullString();
			this.uploadType = context.Request["uploadType"].ToNullString();
			this.uploadPaths = context.Request["uploadPath"].ToNullString();
			this.ControlID = context.Request["ControlID"].ToNullString();
			if (string.IsNullOrEmpty(this.uploadType))
			{
				this.uploadType = "product";
			}
			else
			{
				this.uploadType = this.uploadType.ToLower();
			}
			this.action = context.Request.QueryString["action"];
			if (context.Request.QueryString["filetype"] != null)
			{
				this.fileType = context.Request.QueryString["filetype"];
			}
			try
			{
				this.DoUpload(context);
			}
			catch (Exception ex)
			{
				this.WriteBackError(context, ex.Message);
			}
		}

		private void UploadImage()
		{
			try
			{
				HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files["Filedata"];
				string str = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
				string text = HttpContext.Current.Request["uploadpath"];
				string str2 = str + Path.GetExtension(httpPostedFile.FileName);
				if (string.IsNullOrEmpty(text))
				{
					ClientType clientType = ClientType.VShop;
					string text2 = HttpContext.Current.Request["clientType"];
					if (!string.IsNullOrWhiteSpace(text2))
					{
						clientType = (ClientType)int.Parse(text2);
					}
					text = HiContext.Current.GetCommonSkinPath() + "/images/ad/";
					str2 = "imgCustomBg" + Path.GetExtension(httpPostedFile.FileName);
					string[] files = Directory.GetFiles(Globals.MapPath(text), "imgCustomBg.*");
					string[] array = files;
					foreach (string path in array)
					{
						File.Delete(path);
					}
				}
				if (!Directory.Exists(Globals.MapPath(text)))
				{
					Directory.CreateDirectory(Globals.MapPath(text));
				}
				httpPostedFile.SaveAs(Globals.MapPath(text + str2));
				HttpContext.Current.Response.Write(text + str2);
			}
			catch (Exception ex)
			{
				HttpContext.Current.Response.Write("服务器错误" + ex.Message);
				HttpContext.Current.Response.End();
			}
		}

		private void NewUploadImage()
		{
			string text = "";
			string text2 = "";
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				decimal num = HttpContext.Current.Request["hidFileMaxSize"].ToDecimal(0);
				string text3 = string.IsNullOrEmpty(HttpContext.Current.Request["foldName"].ToNullString()) ? "temp" : HttpContext.Current.Request["foldName"].ToNullString();
				if (num <= decimal.Zero)
				{
					num = 2m;
				}
				for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
				{
					HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files[i];
					if (!httpPostedFile.ContentType.ToLower().StartsWith("image/") || !Regex.IsMatch(Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(jpg|gif|png|bmp|jpeg)$", RegexOptions.Compiled))
					{
						HttpContext.Current.Response.Write("error:上传格式为gif、jpeg、jpg、png、bmp");
						return;
					}
					text2 = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + i + Path.GetExtension(httpPostedFile.FileName);
					string text4 = HttpContext.Current.Server.MapPath("~/Storage/master/" + text3 + "/");
					if (!Directory.Exists(text4))
					{
						Directory.CreateDirectory(text4);
					}
					try
					{
						if ((decimal)httpPostedFile.ContentLength > 1048576m * num)
						{
							Bitmap bitmap = new Bitmap(100, 100);
							bitmap = (Bitmap)Image.FromStream(httpPostedFile.InputStream);
							bitmap = ResourcesHelper.GetThumbnail(bitmap, 735, 480);
							switch (HttpContext.Current.Request["hidOrientation"].ToInt(0))
							{
							case 3:
								bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
								break;
							case 6:
								bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
								break;
							case 8:
								bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
								break;
							}
							bitmap.Save(Path.Combine(text4, text2));
						}
						else
						{
							httpPostedFile.SaveAs(Path.Combine(text4, text2));
						}
						stringBuilder.AppendFormat("/Storage/master/" + text3 + "/" + text2 + ",");
					}
					catch (Exception ex)
					{
						Globals.WriteExceptionLog(ex, null, "NewUploadImage");
					}
				}
			}
			catch (Exception ex2)
			{
				Globals.WriteExceptionLog(ex2, null, "NewUploadImage1");
			}
			HttpContext.Current.Response.Write(stringBuilder.ToString().TrimEnd(','));
		}

		private static void RemoteUpdateImages()
		{
			decimal d = HttpContext.Current.Request["hidFileMaxSize"].ToDecimal(0);
			StringBuilder stringBuilder = new StringBuilder();
			string url = Globals.GetImageServerUrl() + "/admin/UploadHandler.ashx?action=newupload";
			if (d <= decimal.Zero)
			{
				d = 2m;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("foldName", HttpContext.Current.Request["foldName"].ToNullString());
			for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
			{
				HttpPostedFile postedFile = HttpContext.Current.Request.Files[i];
				string empty = string.Empty;
				UploadHandler.HttpPostFile(url, postedFile, dictionary, ref empty);
				stringBuilder.Append(empty + ",");
			}
			HttpContext.Current.Response.Write(stringBuilder.ToString().TrimEnd(','));
		}

		private static void HttpPostFile(string url, HttpPostedFile postedFile, Dictionary<string, object> parameters, ref string output)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.Timeout = 20000;
			httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
			httpWebRequest.KeepAlive = true;
			string str = "----------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
			httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
			string format = "\r\n--" + str + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
			byte[] array = new byte[postedFile.ContentLength];
			postedFile.InputStream.Read(array, 0, array.Length);
			string format2 = "Content-Disposition:application/x-www-form-urlencoded; name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
			format2 = string.Format(format2, "filedata", postedFile.FileName, postedFile.ContentType);
			byte[] bytes2 = Encoding.ASCII.GetBytes(format2);
			try
			{
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					if (parameters != null)
					{
						foreach (KeyValuePair<string, object> parameter in parameters)
						{
							stream.Write(bytes, 0, bytes.Length);
							byte[] bytes3 = Encoding.UTF8.GetBytes(string.Format(format, parameter.Key, parameter.Value));
							stream.Write(bytes3, 0, bytes3.Length);
						}
					}
					stream.Write(bytes, 0, bytes.Length);
					stream.Write(bytes2, 0, bytes2.Length);
					stream.Write(array, 0, array.Length);
					byte[] bytes4 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
					stream.Write(bytes4, 0, bytes4.Length);
					stream.Close();
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
				{
					output = streamReader.ReadToEnd();
				}
				httpWebResponse.Close();
			}
			catch (Exception ex)
			{
				Globals.AppendLog("上传文件时远程服务器发生异常:" + ex.StackTrace, "", "", "");
			}
		}

		private void DeleteImage()
		{
			string path = HttpContext.Current.Request.Form["del"];
			string path2 = Globals.PhysicalPath(path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
				HttpContext.Current.Response.Write("true");
			}
			catch (Exception)
			{
				HttpContext.Current.Response.Write("false");
			}
		}

		private void DoUpload(HttpContext context)
		{
			if (context.Request.Files.Count == 0)
			{
				this.WriteBackError(context, "没有检测到任何文件");
			}
			else
			{
				HttpPostedFile httpPostedFile = context.Request.Files[0];
				int num = 1;
				while (httpPostedFile.ContentLength == 0 && num < context.Request.Files.Count)
				{
					httpPostedFile = context.Request.Files[num];
					num++;
				}
				if (httpPostedFile.ContentLength == 0)
				{
					this.WriteBackError(context, "当前文件没有任何内容");
				}
				else if ((this.uploadType == "product" || this.uploadType == "gift" || this.uploadType == "nothumimage") && (!httpPostedFile.ContentType.ToLower().StartsWith("image/") || !Regex.IsMatch(Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(jpg|gif|png|bmp|jpeg)$", RegexOptions.Compiled)))
				{
					this.WriteBackError(context, "文件类型错误，请选择有效的图片文件");
				}
				else if (this.uploadType == "compressed" && !Regex.IsMatch(Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(rar|zip|7z)$", RegexOptions.Compiled))
				{
					this.WriteBackError(context, "文件类型错误，请选择正确的压缩包文件。");
				}
				else if (this.uploadType == "cert" && !Regex.IsMatch(Path.GetExtension(httpPostedFile.FileName.ToLower()), "\\.(crt|cer|p12|p7b|p7c|spc|key|der|pem|pfx)$", RegexOptions.Compiled))
				{
					this.WriteBackError(context, "控件已不支持图片上传，请选择新的控件");
				}
				else if (this.uploadType == "product" || this.uploadType == "gift" || this.uploadType == "nothumimage")
				{
					this.WriteBackError(context, "文件类型错误，请选择有效的证书文件");
				}
				else
				{
					this.UploadFile(context, httpPostedFile);
				}
			}
		}

		private void DoDelete(HttpContext context)
		{
			this.uploaderId = context.Request["uploaderId"].ToNullString();
			this.uploadType = context.Request["uploadType"].ToNullString();
			this.uploadPaths = context.Request["uploadPath"].ToNullString();
			this.ControlID = context.Request["ControlID"].ToNullString();
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ClearContent();
			context.Response.Expires = -1;
			context.Response.ContentType = "text/html";
			string text = "";
			try
			{
				text = ((!(this.uploadType != "product") || !(this.uploadType != "gift")) ? context.Request.Form[this.uploaderId + "_uploadedImageUrl"].ToNullString() : context.Request.Form[this.uploaderId + "_uploadedFileUrl"].ToNullString());
				string text2 = context.Request.MapPath(text);
				if ((this.uploadType == "product" || this.uploadType.Equals("gift")) && this.CheckFileFormatOrPath(text2))
				{
					string path = text2.Replace("\\images\\", "\\thumbs40\\40_");
					string path2 = text2.Replace("\\images\\", "\\thumbs60\\60_");
					string path3 = text2.Replace("\\images\\", "\\thumbs100\\100_");
					string path4 = text2.Replace("\\images\\", "\\thumbs160\\160_");
					string path5 = text2.Replace("\\images\\", "\\thumbs180\\180_");
					string path6 = text2.Replace("\\images\\", "\\thumbs220\\220_");
					string path7 = text2.Replace("\\images\\", "\\thumbs310\\310_");
					string path8 = text2.Replace("\\images\\", "\\thumbs410\\410_");
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					if (File.Exists(path2))
					{
						File.Delete(path2);
					}
					if (File.Exists(path3))
					{
						File.Delete(path3);
					}
					if (File.Exists(path4))
					{
						File.Delete(path4);
					}
					if (File.Exists(path5))
					{
						File.Delete(path5);
					}
					if (File.Exists(path6))
					{
						File.Delete(path6);
					}
					if (File.Exists(path7))
					{
						File.Delete(path7);
					}
					if (File.Exists(path8))
					{
						File.Delete(path8);
					}
				}
				else if (this.CheckFileFormatOrPath(text2) && File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog_Page(ex, new NameValueCollection
				{
					context.Request.QueryString,
					context.Request.Form
				}, "DeleteFile");
			}
			if (this.uploadType == "product" || this.uploadType == "gift")
			{
				context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + this.uploaderId + "','" + this.uploadType + "');</script>");
			}
			else
			{
				context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteFileCallback('" + this.uploaderId + "','" + this.uploadType + "');</script>");
			}
		}

		private void UploadFile(HttpContext context, HttpPostedFile file)
		{
			string storagePath = HiContext.Current.GetStoragePath();
			string text = storagePath + (storagePath.EndsWith("/") ? "" : "/") + this.uploadType;
			if (!string.IsNullOrEmpty(this.uploadPaths))
			{
				text = this.uploadPaths;
			}
			try
			{
				if (!Directory.Exists(HttpContext.Current.Server.MapPath(text)))
				{
					Directory.CreateDirectory(HttpContext.Current.Server.MapPath(text));
				}
			}
			catch
			{
			}
			string text2 = "";
			Random random = new Random();
			text2 = random.Next(10000, 99999).ToString();
			string str = DateTime.Now.ToString("MMddHHmmss") + text2 + Path.GetExtension(file.FileName);
			string text3 = text + (text.EndsWith("/") ? "" : "/") + str;
			file.SaveAs(context.Request.MapPath(text3));
			string text4 = context.Request.MapPath(text3);
			string[] value = new string[3]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + text3 + "'"
			};
			context.Response.Write("<script type=\"text/javascript\">window.parent.UploadFileCallback(" + string.Join(",", value) + ");</script>");
		}

		private void WriteBackError(HttpContext context, string error)
		{
			if (!string.IsNullOrEmpty(error))
			{
				error = error.Replace("'", "").Replace("\"", "").Replace("“", "")
					.Replace("\\", "\\\\");
			}
			string[] value = new string[3]
			{
				"'" + this.uploadType + "'",
				"'" + this.uploaderId + "'",
				"'" + error + "'"
			};
			if (this.uploadType == "product" || this.uploadType == "gift")
			{
				context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorCallback(" + string.Join(",", value) + ");</script>");
			}
			else
			{
				context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorFileCallback(" + string.Join(",", value) + ");</script>");
			}
		}

		private bool CheckFileFormatOrPath(string imageUrl)
		{
			string text = imageUrl.ToUpper();
			if (this.uploadType == "product" || this.uploadType == "gift" || this.uploadType == "nothumimage")
			{
				if (!string.IsNullOrEmpty(imageUrl) && (text.Contains(".JPG") || text.Contains(".GIF") || text.Contains(".PNG") || text.Contains(".BMP") || text.Contains(".JPEG")) && (imageUrl.Contains(HiContext.Current.Context.Server.MapPath(HiContext.Current.GetStoragePath() + "/" + this.uploadType)) || imageUrl.Contains(HiContext.Current.Context.Server.MapPath("/utility/pics/none.gif")) || imageUrl.Contains(HiContext.Current.Context.Server.MapPath(HiContext.Current.GetStoragePath()))))
				{
					return true;
				}
			}
			else if (this.uploadType == "compressed")
			{
				if ((text.Contains(".ZIP") || text.Contains(".RAR") || text.Contains(".7Z")) && imageUrl.Contains(HiContext.Current.Context.Server.MapPath(HiContext.Current.GetStoragePath() + "/" + this.uploadType)))
				{
					return true;
				}
			}
			else if (this.uploadType == "cert" && (text.Contains(".CRT") || text.Contains(".CER") || text.Contains(".P12") || text.Contains(".P7B") || text.Contains(".P7C") || text.Contains(".SPC") || text.Contains(".KEY") || text.Contains(".DER") || text.Contains(".PEM") || text.Contains(".PFX")))
			{
				return true;
			}
			return false;
		}
	}
}
