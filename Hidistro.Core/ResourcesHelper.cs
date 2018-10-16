using Hidistro.Core.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;

namespace Hidistro.Core
{
	public static class ResourcesHelper
	{
		private static FileClass[] imageFileClass = new FileClass[4]
		{
			FileClass.jpg,
			FileClass.gif,
			FileClass.bmp,
			FileClass.png
		};

		private static FileClass[] accessoryFileClass = new FileClass[8]
		{
			FileClass.rar,
			FileClass.txt,
			FileClass.doc,
			FileClass.doc,
			FileClass.doc,
			FileClass.htm,
			FileClass.html,
			FileClass.zip
		};

		private static FileClass[] mediaFileClass = new FileClass[6]
		{
			FileClass.wmv,
			FileClass.mid,
			FileClass.mp3,
			FileClass.mpg,
			FileClass.rmvb,
			FileClass.xv
		};

		private static FileClass[] flasFileClass = new FileClass[2]
		{
			FileClass.swf,
			FileClass.f4v
		};

		private static FileClass[] certFileClass = new FileClass[7]
		{
			FileClass.cer,
			FileClass.p12,
			FileClass.p12,
			FileClass.pem,
			FileClass.p12,
			FileClass.p12,
			FileClass.cer
		};

		public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
		{
			Image image = Image.FromFile(sourceFilename);
			if (image.Width <= width && image.Height <= height)
			{
				File.Copy(sourceFilename, destFilename, true);
				image.Dispose();
			}
			else
			{
				int width2 = image.Width;
				int height2 = image.Height;
				float num = (float)height / (float)height2;
				if ((float)width / (float)width2 < num)
				{
					num = (float)width / (float)width2;
				}
				width = (int)((float)width2 * num);
				height = (int)((float)height2 * num);
				Image image2 = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(image2);
				graphics.Clear(Color.White);
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, width2, height2), GraphicsUnit.Pixel);
				EncoderParameters encoderParameters = new EncoderParameters();
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 100L);
				encoderParameters.Param[0] = encoderParameter;
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo encoder = null;
				int num2 = 0;
				while (num2 < imageEncoders.Length)
				{
					if (!imageEncoders[num2].FormatDescription.Equals("JPEG"))
					{
						num2++;
						continue;
					}
					encoder = imageEncoders[num2];
					break;
				}
				image2.Save(destFilename, encoder, encoderParameters);
				encoderParameters.Dispose();
				encoderParameter.Dispose();
				image.Dispose();
				image2.Dispose();
				graphics.Dispose();
			}
		}

		public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
		{
			ImageFormat rawFormat = b.RawFormat;
			int num = 0;
			int num2 = 0;
			int width = b.Width;
			int height = b.Height;
			if (height > destHeight || width > destWidth)
			{
				if (width * destHeight < height * destWidth)
				{
					num = destWidth;
					num2 = destWidth * height / width;
				}
				else
				{
					num2 = destHeight;
					num = width * destHeight / height;
				}
			}
			else
			{
				num = destWidth;
				num2 = destHeight;
			}
			Bitmap bitmap = new Bitmap(destWidth, destHeight);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.Transparent);
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(b, new Rectangle((destWidth - num) / 2, (destHeight - num2) / 2, num, num2), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel);
			graphics.Dispose();
			EncoderParameters encoderParameters = new EncoderParameters();
			long[] value = new long[1]
			{
				100L
			};
			EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, value);
			encoderParameters.Param[0] = encoderParameter;
			b.Dispose();
			return bitmap;
		}

		public static bool CheckPostedFile(HttpPostedFile postedFile, string fileType = "image", FileClass[] customFileClass = null)
		{
			if (postedFile == null || postedFile.ContentLength == 0)
			{
				return false;
			}
			int num = 0;
			int.TryParse(ResourcesHelper.GetFileClassCode(postedFile), out num);
			FileClass[] array = ResourcesHelper.imageFileClass;
			switch (fileType)
			{
			case "image":
				array = ResourcesHelper.imageFileClass;
				goto default;
			case "file":
				array = ResourcesHelper.accessoryFileClass;
				goto default;
			case "media":
				array = ResourcesHelper.mediaFileClass;
				goto default;
			case "flash":
				array = ResourcesHelper.flasFileClass;
				goto default;
			case "cert":
				array = ResourcesHelper.certFileClass;
				goto default;
			case "custom":
				if (customFileClass == null)
				{
					return false;
				}
				goto default;
			default:
			{
				FileClass[] array2 = array;
				foreach (FileClass fileClass in array2)
				{
					if (fileClass == (FileClass)num)
					{
						return true;
					}
				}
				return false;
			}
			}
		}

		public static bool CheckFileType(string filePath, string fileType = "image", FileClass[] customFileClass = null)
		{
			int num = 0;
			int.TryParse(ResourcesHelper.GetFileClassCode(filePath), out num);
			FileClass[] array = ResourcesHelper.imageFileClass;
			switch (fileType)
			{
			case "image":
				array = ResourcesHelper.imageFileClass;
				goto default;
			case "file":
				array = ResourcesHelper.accessoryFileClass;
				goto default;
			case "media":
				array = ResourcesHelper.mediaFileClass;
				goto default;
			case "flash":
				array = ResourcesHelper.flasFileClass;
				goto default;
			case "cert":
				array = ResourcesHelper.certFileClass;
				goto default;
			case "custom":
				if (customFileClass == null)
				{
					return false;
				}
				goto default;
			default:
			{
				FileClass[] array2 = array;
				foreach (FileClass fileClass in array2)
				{
					if (fileClass == (FileClass)num)
					{
						return true;
					}
				}
				return false;
			}
			}
		}

		public static string GetFileClassCode(string filePath)
		{
			filePath = Globals.GetphysicsPath(filePath);
			FileStream fileStream = new FileStream(filePath, FileMode.Open);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, 2);
			fileStream.Close();
			fileStream.Dispose();
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			string text = "";
			try
			{
				byte b = binaryReader.ReadByte();
				text = b.ToString();
				b = binaryReader.ReadByte();
				text += b.ToString();
			}
			catch
			{
			}
			binaryReader.Close();
			memoryStream.Close();
			return text;
		}

		private static string GetFileClassCode(HttpPostedFile postedFile)
		{
			int contentLength = postedFile.ContentLength;
			byte[] buffer = new byte[contentLength];
			postedFile.InputStream.Read(buffer, 0, contentLength);
			MemoryStream memoryStream = new MemoryStream(buffer);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			string text = "";
			try
			{
				byte b = binaryReader.ReadByte();
				text = b.ToString();
				b = binaryReader.ReadByte();
				text += b.ToString();
			}
			catch
			{
			}
			binaryReader.Close();
			memoryStream.Close();
			return text;
		}

		public static string GenerateFilename(string extension)
		{
			return Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + extension;
		}

		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HttpContext.Current.Request.MapPath(imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}

		public static Bitmap GetNetImg(string imgUrl)
		{
			try
			{
				Random random = new Random();
				imgUrl = ((!imgUrl.Contains("?")) ? (imgUrl + "?aid=" + random.NextDouble()) : (imgUrl + "&aid=" + random.NextDouble()));
				WebRequest webRequest = WebRequest.Create(imgUrl);
				WebResponse response = webRequest.GetResponse();
				Stream responseStream = response.GetResponseStream();
				Image image = Image.FromStream(responseStream);
				responseStream.Close();
				responseStream.Dispose();
				webRequest = null;
				response = null;
				return (Bitmap)image;
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("NetImgUrl", imgUrl);
				Globals.WriteExceptionLog(ex, dictionary, "SplittinRuleGetNetImg");
				return new Bitmap(100, 100);
			}
		}

		public static Bitmap CombinImage(Bitmap QRimg, Image Logoimg, int logoW)
		{
			Bitmap bitmap = new Bitmap(QRimg.Width + 20, QRimg.Height + 20);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.White);
			graphics.DrawImage(QRimg, 10, 10, QRimg.Width, QRimg.Height);
			graphics.DrawImage(Logoimg, (bitmap.Width - logoW) / 2, (bitmap.Height - logoW) / 2, logoW, logoW);
			return bitmap;
		}

		public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180f, 90f);
			graphicsPath.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
			graphicsPath.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270f, 90f);
			graphicsPath.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
			graphicsPath.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0f, 90f);
			graphicsPath.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
			graphicsPath.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90f, 90f);
			graphicsPath.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
	}
}
