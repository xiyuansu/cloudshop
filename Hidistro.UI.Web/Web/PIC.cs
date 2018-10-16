using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Hidistro.UI.Web
{
	public class PIC
	{
		private Bitmap _outBmp = null;

		public Bitmap OutBMP
		{
			get
			{
				return this._outBmp;
			}
		}

		private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = Convert.ToDouble(width);
			double num4 = Convert.ToDouble(height);
			double num5 = Convert.ToDouble(maxWidth);
			double num6 = Convert.ToDouble(maxHeight);
			if (num3 < num5 && num4 < num6)
			{
				num = num3;
				num2 = num4;
			}
			else if (num3 / num4 > num5 / num6)
			{
				num = (double)maxWidth;
				num2 = num * num4 / num3;
			}
			else
			{
				num2 = (double)maxHeight;
				num = num2 * num3 / num4;
			}
			return new Size(Convert.ToInt32(num), Convert.ToInt32(num2));
		}

		public static void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth)
		{
			Image image = null;
			Bitmap bitmap = null;
			Graphics graphics = null;
			try
			{
				image = Image.FromFile(fileName);
				ImageFormat rawFormat = image.RawFormat;
				Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				bitmap = new Bitmap(size.Width, size.Height);
				graphics = Graphics.FromImage(bitmap);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
				graphics?.Dispose();
				EncoderParameters encoderParameters = new EncoderParameters();
				long[] value = new long[1]
				{
					100L
				};
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, value);
				encoderParameters.Param[0] = encoderParameter;
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo imageCodecInfo = null;
				int num = 0;
				while (num < imageEncoders.Length)
				{
					if (!imageEncoders[num].FormatDescription.Equals("JPEG"))
					{
						num++;
						continue;
					}
					imageCodecInfo = imageEncoders[num];
					break;
				}
				if (imageCodecInfo != null)
				{
					bitmap.Save(newFile, imageCodecInfo, encoderParameters);
				}
				else
				{
					bitmap.Save(newFile, rawFormat);
				}
			}
			catch
			{
			}
			finally
			{
				graphics?.Dispose();
				image?.Dispose();
				bitmap?.Dispose();
			}
		}

		public void Dispose()
		{
			if (this._outBmp != null)
			{
				this._outBmp.Dispose();
				this._outBmp = null;
			}
		}

		public void SendSmallImage(string fileName, int maxHeight, int maxWidth)
		{
			Image image = null;
			this._outBmp = null;
			Graphics graphics = null;
			try
			{
				image = Image.FromFile(fileName);
				ImageFormat rawFormat = image.RawFormat;
				Size size = PIC.NewSize(maxWidth, maxHeight, image.Width, image.Height);
				this._outBmp = new Bitmap(size.Width, size.Height);
				graphics = Graphics.FromImage(this._outBmp);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
				graphics?.Dispose();
			}
			catch
			{
			}
			finally
			{
				graphics?.Dispose();
				image?.Dispose();
			}
		}

		public MemoryStream AddImageSignPic(Image img, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
		{
			Graphics graphics = null;
			Image image = null;
			ImageAttributes imageAttributes = null;
			MemoryStream memoryStream = null;
			try
			{
				graphics = Graphics.FromImage(img);
				if (File.Exists(watermarkFilename))
				{
					image = new Bitmap(watermarkFilename);
				}
				imageAttributes = new ImageAttributes();
				ColorMap colorMap = new ColorMap();
				colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
				colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
				ColorMap[] map = new ColorMap[1]
				{
					colorMap
				};
				imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);
				float num = 0.5f;
				if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
				{
					num = (float)watermarkTransparency / 10f;
				}
				float[][] newColorMatrix = new float[5][]
				{
					new float[5]
					{
						1f,
						0f,
						0f,
						0f,
						0f
					},
					new float[5]
					{
						0f,
						1f,
						0f,
						0f,
						0f
					},
					new float[5]
					{
						0f,
						0f,
						1f,
						0f,
						0f
					},
					new float[5]
					{
						0f,
						0f,
						0f,
						num,
						0f
					},
					new float[5]
					{
						0f,
						0f,
						0f,
						0f,
						1f
					}
				};
				ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
				imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
				int x = 0;
				int y = 0;
				if (image == null)
				{
					watermarkStatus = 1;
				}
				switch (watermarkStatus)
				{
				case 1:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.01f);
					break;
				case 2:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.01f);
					break;
				case 3:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.01f);
					break;
				case 4:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 5:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 6:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.5f - (float)(image.Height / 2));
					break;
				case 7:
					x = (int)((float)img.Width * 0.01f);
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				case 8:
					x = (int)((float)img.Width * 0.5f - (float)(image.Width / 2));
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				case 9:
					x = (int)((float)img.Width * 0.99f - (float)image.Width);
					y = (int)((float)img.Height * 0.99f - (float)image.Height);
					break;
				}
				if (image != null)
				{
					graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
				}
				ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
				ImageCodecInfo imageCodecInfo = null;
				ImageCodecInfo[] array = imageEncoders;
				foreach (ImageCodecInfo imageCodecInfo2 in array)
				{
					if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
					{
						imageCodecInfo = imageCodecInfo2;
					}
				}
				EncoderParameters encoderParameters = new EncoderParameters();
				long[] array2 = new long[1];
				if (quality < 0 || quality > 100)
				{
					quality = 80;
				}
				array2[0] = quality;
				EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, array2);
				encoderParameters.Param[0] = encoderParameter;
				memoryStream = new MemoryStream();
				if (imageCodecInfo != null)
				{
					img.Save(memoryStream, imageCodecInfo, encoderParameters);
				}
				return memoryStream;
			}
			catch
			{
				return null;
			}
			finally
			{
				graphics?.Dispose();
				img?.Dispose();
				image?.Dispose();
				imageAttributes?.Dispose();
			}
		}
	}
}
