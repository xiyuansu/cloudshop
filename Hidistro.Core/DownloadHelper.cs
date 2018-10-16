using System;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.Core
{
	public class DownloadHelper
	{
		private HttpResponse Response = null;

		public DownloadHelper()
		{
			this.Response = HttpContext.Current.Response;
		}

		public void DownLoad(string filePath)
		{
			string text = HttpContext.Current.Server.MapPath(filePath);
			if (File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ClearHeaders();
				HttpContext.Current.Response.Buffer = false;
				HttpContext.Current.Response.ContentType = "application/octet-stream";
				HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(Path.GetFileName(text)));
				HttpContext.Current.Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
				HttpContext.Current.Response.WriteFile(text);
				HttpContext.Current.Response.Flush();
				HttpContext.Current.Response.End();
			}
		}

		public void DownloadByOutputStreamBlock(Stream stream, string fileName)
		{
			using (stream)
			{
				stream.Position = 0L;
				long num = 102400L;
				byte[] buffer = new byte[num];
				long num2 = stream.Length;
				this.Response.ContentType = "application/octet-stream";
				this.Response.AddHeader("Content-Disposition", $"attachment; filename={HttpUtility.UrlPathEncode(fileName)}");
				this.Response.AddHeader("Content-Length", num2.ToString());
				while (num2 > 0 && this.Response.IsClientConnected)
				{
					int num3 = stream.Read(buffer, 0, Convert.ToInt32(num));
					this.Response.OutputStream.Write(buffer, 0, num3);
					this.Response.Flush();
					this.Response.Clear();
					num2 -= num3;
				}
				this.Response.Close();
			}
		}

		public void DownloadByOutputStreamBlock(string filePath)
		{
			this.DownloadByOutputStreamBlock(filePath);
		}

		public void DownloadByOutputStreamBlock(string filePath, string fileName)
		{
			filePath = HttpContext.Current.Server.MapPath(filePath);
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = Path.GetFileName(filePath);
			}
			using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				this.DownloadByOutputStreamBlock(stream, fileName);
			}
		}

		public void DownloadByTransmitFile(string filePath)
		{
			this.DownloadByTransmitFile(filePath, null);
		}

		public void DownloadByTransmitFile(string filePath, string fileName)
		{
			filePath = HttpContext.Current.Server.MapPath(filePath);
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = Path.GetFileName(filePath);
			}
			FileInfo fileInfo = new FileInfo(filePath);
			if (fileInfo != null)
			{
				long length = fileInfo.Length;
				this.Response.Clear();
				this.Response.ContentType = "application/x-zip-compressed";
				this.Response.AddHeader("Content-Disposition", $"attachment;filename={HttpUtility.UrlPathEncode(fileName)}");
				this.Response.AddHeader("Content-Length", length.ToString());
				this.Response.TransmitFile(filePath, 0L, length);
				this.Response.Flush();
				this.Response.Close();
			}
		}

		public void DownloadByWriteFile(string filePath)
		{
			this.DownloadByWriteFile(filePath, null);
		}

		public void DownloadByWriteFile(string filePath, string fileName)
		{
			filePath = HttpContext.Current.Server.MapPath(filePath);
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = Path.GetFileName(filePath);
			}
			FileInfo fileInfo = new FileInfo(filePath);
			if (fileInfo != null)
			{
				long length = fileInfo.Length;
				this.Response.Clear();
				this.Response.ContentType = "application/octet-stream";
				this.Response.AddHeader("Content-Disposition", $"attachment;filename={HttpUtility.UrlPathEncode(fileName)}");
				this.Response.AddHeader("Content-Length", length.ToString());
				this.Response.WriteFile(filePath, 0L, length);
				this.Response.Flush();
				this.Response.Close();
			}
		}

		public void DownloadByBinary(string filePath)
		{
			this.DownloadByBinary(filePath, null);
		}

		public void DownloadByBinary(string filePath, string fileName)
		{
			filePath = HttpContext.Current.Server.MapPath(filePath);
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = Path.GetFileName(filePath);
			}
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				this.Response.ContentType = "application/octet-stream";
				this.Response.AddHeader("Content-Disposition", $"attachment;filename={HttpUtility.UrlPathEncode(fileName)}");
				this.Response.AddHeader("Content-Length", fileStream.Length.ToString());
				byte[] buffer = new byte[fileStream.Length];
				fileStream.Read(buffer, 0, Convert.ToInt32(fileStream.Length));
				this.Response.BinaryWrite(buffer);
				this.Response.Flush();
				this.Response.Close();
			}
		}

		public void DownloadByBinaryBlock(string filePath)
		{
			this.DownloadByBinaryBlock(filePath, null);
		}

		public void DownloadByBinaryBlock(string filePath, string fileName)
		{
			filePath = HttpContext.Current.Server.MapPath(filePath);
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = Path.GetFileName(filePath);
			}
			using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				long num = 102400L;
				byte[] buffer = new byte[num];
				long num2 = fileStream.Length;
				this.Response.ContentType = "application/octet-stream";
				this.Response.AddHeader("Content-Disposition", $"attachment;filename={HttpUtility.UrlPathEncode(fileName)}");
				this.Response.AddHeader("Content-Length", num2.ToString());
				while (num2 > 0 && this.Response.IsClientConnected)
				{
					int num3 = fileStream.Read(buffer, 0, Convert.ToInt32(num));
					this.Response.BinaryWrite(buffer);
					this.Response.Flush();
					this.Response.Clear();
					num2 -= num3;
				}
				this.Response.Close();
			}
		}

		public static void DownloadFile(HttpResponse argResp, StringBuilder argFileStream, string strFileName)
		{
			try
			{
				argFileStream.Insert(0, "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=utf-8\"/>");
				string value = "attachment; filename=" + strFileName;
				argResp.Clear();
				argResp.Buffer = true;
				argResp.AppendHeader("Content-Disposition", value);
				argResp.ContentType = "application/ms-excel";
				argResp.ContentEncoding = Encoding.GetEncoding("utf-8");
				argResp.Charset = "utf-8";
				argResp.Write(argFileStream);
				argResp.Flush();
				argResp.End();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
