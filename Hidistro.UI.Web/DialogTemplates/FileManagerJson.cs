using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.DialogTemplates
{
	public class FileManagerJson : IHttpHandler
	{
		private class NameSorter : IComparer
		{
			private bool ascend;

			public NameSorter(bool isAscend)
			{
				this.ascend = isAscend;
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return (!this.ascend) ? 1 : (-1);
				}
				if (y == null)
				{
					return this.ascend ? 1 : (-1);
				}
				FileInfo fileInfo = new FileInfo(x.ToString());
				FileInfo fileInfo2 = new FileInfo(y.ToString());
				return this.ascend ? fileInfo.FullName.CompareTo(fileInfo2.FullName) : fileInfo2.FullName.CompareTo(fileInfo.FullName);
			}
		}

		private class SizeSorter : IComparer
		{
			private bool ascend;

			public SizeSorter(bool isAscend)
			{
				this.ascend = isAscend;
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return (!this.ascend) ? 1 : (-1);
				}
				if (y == null)
				{
					return this.ascend ? 1 : (-1);
				}
				FileInfo fileInfo = new FileInfo(x.ToString());
				FileInfo fileInfo2 = new FileInfo(y.ToString());
				long length;
				int result;
				if (!this.ascend)
				{
					length = fileInfo2.Length;
					result = length.CompareTo(fileInfo.Length);
				}
				else
				{
					length = fileInfo.Length;
					result = length.CompareTo(fileInfo2.Length);
				}
				return result;
			}
		}

		private class DateTimeSorter : IComparer
		{
			private bool ascend;

			private int type;

			public DateTimeSorter(int sortType, bool isAscend)
			{
				this.ascend = isAscend;
				this.type = sortType;
			}

			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return (!this.ascend) ? 1 : (-1);
				}
				if (y == null)
				{
					return this.ascend ? 1 : (-1);
				}
				FileInfo fileInfo = new FileInfo(x.ToString());
				FileInfo fileInfo2 = new FileInfo(y.ToString());
				DateTime dateTime;
				if (this.type == 0)
				{
					int result;
					if (!this.ascend)
					{
						dateTime = fileInfo2.CreationTime;
						result = dateTime.CompareTo(fileInfo.CreationTime);
					}
					else
					{
						dateTime = fileInfo.CreationTime;
						result = dateTime.CompareTo(fileInfo2.CreationTime);
					}
					return result;
				}
				int result2;
				if (!this.ascend)
				{
					dateTime = fileInfo2.LastWriteTime;
					result2 = dateTime.CompareTo(fileInfo.LastWriteTime);
				}
				else
				{
					dateTime = fileInfo.LastWriteTime;
					result2 = dateTime.CompareTo(fileInfo2.LastWriteTime);
				}
				return result2;
			}
		}

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
			Hashtable hashtable = new Hashtable();
			if (manager == null)
			{
				this.message = "没有权限";
			}
			else
			{
				string text = "";
				string text2 = "";
				string text3 = context.Request.QueryString["path"];
				if (text3 == null || text3 == "-1")
				{
					text3 = "AdvertImg";
				}
				text = string.Format("{0}/UploadImage/" + text3 + "/", HiContext.Current.GetPCHomePageSkinPath());
				text2 = ((!(context.Request.ApplicationPath != "/")) ? text : text.Substring(context.Request.ApplicationPath.Length));
				string text4 = context.Request.QueryString["order"];
				text4 = (string.IsNullOrEmpty(text4) ? "uploadtime" : text4.ToLower());
				this.message = "未知错误";
				if (this.FillTableForPath(text, text2, text4, hashtable, text3))
				{
					string text5 = context.Request.Url.ToString();
					text5 = text5.Substring(0, text5.IndexOf("/", 7));
					text5 += context.Request.ApplicationPath;
					if (text5.EndsWith("/"))
					{
						text5 = text5.Substring(0, text5.Length - 1);
					}
					hashtable["domain"] = text5;
					this.message = JsonMapper.ToJson(hashtable);
				}
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}

		private bool FillTableForPath(string path, string url, string order, Hashtable table, string cid)
		{
			string text = "";
			text = Globals.MapPath(path);
			string text2 = "";
			string text3 = "";
			string text4 = "";
			path = (string.IsNullOrEmpty(path) ? "" : path);
			if (Regex.IsMatch(path, "\\.\\."))
			{
				this.message = "Access is not allowed.";
				return false;
			}
			if (path != "" && !path.EndsWith("/"))
			{
				path += "/";
			}
			text = path;
			text3 = "";
			text4 = "";
			text = HttpContext.Current.Server.MapPath(text);
			if (!Directory.Exists(text))
			{
				this.message = "此目录不存在";
				return false;
			}
			string[] files = Directory.GetFiles(text);
			switch (order)
			{
			case "uploadtime":
				Array.Sort(files, new DateTimeSorter(0, true));
				break;
			case "uploadtime desc":
				Array.Sort(files, new DateTimeSorter(0, false));
				break;
			case "lastupdatetime":
				Array.Sort(files, new DateTimeSorter(1, true));
				break;
			case "lastupdatetime desc":
				Array.Sort(files, new DateTimeSorter(1, false));
				break;
			case "photoname":
				Array.Sort(files, new NameSorter(true));
				break;
			case "photoname desc":
				Array.Sort(files, new NameSorter(false));
				break;
			case "filesize":
				Array.Sort(files, new SizeSorter(true));
				break;
			case "filesize desc":
				Array.Sort(files, new SizeSorter(false));
				break;
			default:
				Array.Sort(files, new NameSorter(true));
				break;
			}
			List<Hashtable> list = new List<Hashtable>();
			table["moveup_dir_path"] = text4;
			table["current_dir_path"] = cid;
			table["current_url"] = url;
			table["file_list"] = list;
			DateTime dateTime;
			if (cid != "")
			{
				Hashtable hashtable = new Hashtable();
				hashtable["is_dir"] = true;
				hashtable["has_file"] = true;
				hashtable["filesize"] = 0;
				hashtable["is_photo"] = false;
				hashtable["filetype"] = "";
				hashtable["filename"] = "上级目录";
				hashtable["path"] = "-1";
				Hashtable hashtable2 = hashtable;
				dateTime = DateTime.Now;
				hashtable2["datetime"] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				list.Add(hashtable);
			}
			else
			{
				foreach (Hashtable item in this.BindFileCategory())
				{
					list.Add(item);
				}
			}
			table["total_count"] = files.Length;
			table["current_cateogry"] = cid;
			for (int i = 0; i < files.Length; i++)
			{
				FileInfo fileInfo = new FileInfo(files[i]);
				Hashtable hashtable3 = new Hashtable();
				hashtable3["cid"] = "-1";
				hashtable3["name"] = fileInfo.Name;
				hashtable3["path"] = url + fileInfo.Name;
				hashtable3["filename"] = fileInfo.Name;
				hashtable3["filesize"] = fileInfo.Length;
				Hashtable hashtable4 = hashtable3;
				dateTime = fileInfo.CreationTime;
				hashtable4["addedtime"] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				Hashtable hashtable5 = hashtable3;
				dateTime = fileInfo.LastWriteTime;
				hashtable5["updatetime"] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				Hashtable hashtable6 = hashtable3;
				dateTime = fileInfo.LastWriteTime;
				hashtable6["datetime"] = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				hashtable3["filetype"] = fileInfo.Extension.Substring(1);
				list.Add(hashtable3);
			}
			return true;
		}

		private List<Hashtable> BindFileCategory()
		{
			List<Hashtable> list = new List<Hashtable>();
			string str = $"{HiContext.Current.GetPCHomePageSkinPath()}/UploadImage/";
			string path = HttpContext.Current.Request.MapPath(str + "AdvertImg/");
			Hashtable hashtable = new Hashtable();
			DateTime now;
			if (!Directory.Exists(path))
			{
				hashtable["has_file"] = false;
				hashtable["filesize"] = 0;
				Hashtable hashtable2 = hashtable;
				now = DateTime.Now;
				hashtable2["datetime"] = now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				hashtable["has_file"] = (Directory.GetFiles(path).Length != 0);
				hashtable["filesize"] = 0;
				hashtable["datetime"] = Directory.GetLastWriteTime(path);
			}
			hashtable["is_dir"] = true;
			hashtable["is_photo"] = false;
			hashtable["filetype"] = "";
			hashtable["filename"] = "广告位图片";
			hashtable["cid"] = "AdvertImg";
			hashtable["path"] = "AdvertImg";
			list.Add(hashtable);
			hashtable = new Hashtable();
			string path2 = HttpContext.Current.Request.MapPath(str + "AdvertImg/");
			if (!Directory.Exists(path2))
			{
				hashtable["has_file"] = false;
				hashtable["filesize"] = 0;
				Hashtable hashtable3 = hashtable;
				now = DateTime.Now;
				hashtable3["datetime"] = now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				hashtable["has_file"] = (Directory.GetFiles(path2).Length != 0);
				hashtable["filesize"] = 0;
				hashtable["datetime"] = Directory.GetLastWriteTime(path2);
			}
			hashtable["is_dir"] = true;
			hashtable["is_photo"] = false;
			hashtable["filetype"] = "";
			hashtable["filename"] = "标题图片";
			hashtable["cid"] = "TitleImg";
			hashtable["path"] = "TitleImg";
			list.Add(hashtable);
			return list;
		}
	}
}
