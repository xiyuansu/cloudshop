using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web
{
	public class VerifyCodeImage : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
				string text = HiContext.Current.CreateVerifyCode(4, VerifyCodeType.Digital, base.Request.QueryString["openId"].ToNullString());
				int num = 45;
				int num2 = text.Length * 20;
				Bitmap bitmap = new Bitmap(num2 - 3, 27);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.Clear(Color.AliceBlue);
				graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, bitmap.Width - 1, bitmap.Height - 3);
				Random random = new Random();
				Pen pen = new Pen(Color.LightGray, 0f);
				for (int i = 0; i < 50; i++)
				{
					int x = random.Next(0, bitmap.Width);
					int y = random.Next(0, bitmap.Height);
					graphics.DrawRectangle(pen, x, y, 1, 1);
				}
				char[] array = text.ToCharArray();
				StringFormat stringFormat = new StringFormat(StringFormatFlags.NoClip);
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				Color[] array2 = new Color[8]
				{
					Color.Black,
					Color.Red,
					Color.DarkBlue,
					Color.Green,
					Color.Brown,
					Color.DarkCyan,
					Color.Purple,
					Color.DarkGreen
				};
				for (int j = 0; j < array.Length; j++)
				{
					int num3 = random.Next(7);
					int num4 = random.Next(4);
					Font font = new Font("Microsoft Sans Serif", 17f, FontStyle.Bold);
					Brush brush = new SolidBrush(array2[num3]);
					Point point = new Point(14, 11);
					float num5 = (float)random.Next(-num, num);
					graphics.TranslateTransform((float)point.X, (float)point.Y);
					graphics.RotateTransform(num5);
					graphics.DrawString(array[j].ToString(), font, brush, 1f, 1f, stringFormat);
					graphics.RotateTransform(0f - num5);
					graphics.TranslateTransform(2f, (float)(-point.Y));
				}
				MemoryStream memoryStream = new MemoryStream();
				bitmap.Save(memoryStream, ImageFormat.Gif);
				base.Response.ClearContent();
				base.Response.ContentType = "image/gif";
				base.Response.BinaryWrite(memoryStream.ToArray());
				graphics.Dispose();
				bitmap.Dispose();
			}
			catch
			{
			}
		}
	}
}
