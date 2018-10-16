using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppSplittinRule : AppshopMemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Image userReferralQRCode;

		private HtmlImage imghtmlfxvshop;

		private HtmlImage imghtmlfxwapshop;

		private HtmlInputHidden hidExtendShareTitle;

		private HtmlInputHidden hidExtendShareDetail;

		private HtmlInputHidden hidExtendSharePic;

		private HtmlInputHidden hidExtendShareUrl;

		private HtmlGenericControl posterPanel;

		private HtmlGenericControl noreferralPanel;

		private HtmlGenericControl repeledPanel;

		private HtmlGenericControl divRepeledReason;

		private Literal repeledTime;

		private Literal repeledReason;

		private string SetJson = "";

		private JObject resultObj = null;

		private string UserHead = "";

		private string CodeUrl = "";

		private string StoreName = "";

		private string UserName = "";

		private int ReferralId = 0;

		private int userId = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinRule.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.repeledPanel = (HtmlGenericControl)this.FindControl("repeledPanel");
			this.repeledTime = (Literal)this.FindControl("repeledTime");
			this.repeledReason = (Literal)this.FindControl("repeledReason");
			this.userReferralQRCode = (System.Web.UI.WebControls.Image)this.FindControl("userReferralQRCode");
			this.divRepeledReason = (HtmlGenericControl)this.FindControl("divRepeledReason");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.imghtmlfxvshop = (HtmlImage)this.FindControl("imghtmlfxvshop");
			this.imghtmlfxwapshop = (HtmlImage)this.FindControl("imghtmlfxwapshop");
			string str = Globals.HostPath(HttpContext.Current.Request.Url) + "/WapShop/ReferralAgreement?ReferralUserId=" + HiContext.Current.User.UserId;
			this.userReferralQRCode.ImageUrl = "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + str;
			this.hidExtendShareTitle = (HtmlInputHidden)this.FindControl("hidExtendShareTitle");
			this.hidExtendShareDetail = (HtmlInputHidden)this.FindControl("hidExtendShareDetail");
			this.hidExtendSharePic = (HtmlInputHidden)this.FindControl("hidExtendSharePic");
			this.hidExtendShareUrl = (HtmlInputHidden)this.FindControl("hidExtendShareUrl");
			this.posterPanel = (HtmlGenericControl)this.FindControl("posterPanel");
			this.noreferralPanel = (HtmlGenericControl)this.FindControl("noreferralPanel");
			string text = HttpContext.Current.Request["SessionId"].ToNullString();
			MemberInfo user = HiContext.Current.User;
			this.repeledPanel.Visible = false;
			if (user.UserId > 0 && user.IsReferral())
			{
				if (user.Referral.IsRepeled)
				{
					this.posterPanel.Visible = false;
					this.noreferralPanel.Visible = false;
					this.repeledPanel.Visible = true;
					this.repeledTime.Text = user.Referral.RepelTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
					if (!string.IsNullOrEmpty(user.Referral.RepelReason.ToNullString()))
					{
						this.repeledReason.Text = user.Referral.RepelReason.ToNullString() + "<br>";
						this.divRepeledReason.Visible = true;
					}
				}
				else
				{
					this.imghtmlfxvshop.Visible = false;
					str = Globals.HostPath(HttpContext.Current.Request.Url) + "/ReferralAgreement.aspx?ReferralUserId=" + HiContext.Current.User.UserId;
					this.CodeUrl = Globals.CreateQRCode(str, "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.Replace(".", "").Replace("https://", "").Replace("http://", "")
						.Replace("/", "")
						.Replace("\\", "") + "_Referral_" + HiContext.Current.User.UserId, false, ImageFormats.Png);
					this.SetJson = File.ReadAllText(HttpRuntime.AppDomainAppPath.ToString() + "Storage/data/Utility/ReferralPoster.js");
					this.resultObj = (JsonConvert.DeserializeObject(this.SetJson) as JObject);
					FileInfo fileInfo = new FileInfo(HttpRuntime.AppDomainAppPath.ToString() + "Storage/data/Utility/ReferralPoster.js");
					DateTime lastWriteTime = fileInfo.LastWriteTime;
					FileInfo fileInfo2 = new FileInfo(HttpRuntime.AppDomainAppPath.ToString() + Globals.GetStoragePath() + "/ReferralPoster/Poster_Wap_" + HiContext.Current.User.UserId + ".jpg");
					bool flag = true;
					if (fileInfo.Exists && fileInfo2.Exists && fileInfo2.LastWriteTime > lastWriteTime)
					{
						this.userReferralQRCode.ImageUrl = Globals.GetStoragePath() + "/ReferralPoster/Poster_Wap_" + HiContext.Current.User.UserId + ".jpg";
						flag = false;
					}
					if (flag)
					{
						if (this.resultObj != null && this.resultObj["writeDate"] != null && this.resultObj["posList"] != null && this.resultObj["DefaultHead"] != null && this.resultObj["myusername"] != null && this.resultObj["shopname"] != null)
						{
							this.StoreName = HiContext.Current.SiteSettings.SiteName;
							this.UserHead = user.Picture;
							this.UserName = (string.IsNullOrEmpty(user.NickName) ? user.RealName : user.NickName);
							this.ReferralId = HiContext.Current.User.UserId;
							this.userId = HiContext.Current.UserId;
							string imageUrl = default(string);
							this.CreatePoster(out imageUrl);
							this.userReferralQRCode.ImageUrl = imageUrl;
						}
						else
						{
							this.userReferralQRCode.ImageUrl = "SET Failed";
						}
					}
					this.hidExtendShareTitle.Value = masterSettings.ExtendShareTitle.ToNullString();
					this.hidExtendShareDetail.Value = masterSettings.ExtendShareDetail.ToNullString();
					this.hidExtendSharePic.Value = (string.IsNullOrEmpty(masterSettings.ExtendSharePic) ? Globals.FullPath(this.userReferralQRCode.ImageUrl) : (masterSettings.ExtendSharePic.ToNullString().Contains("http://") ? masterSettings.ExtendSharePic.ToNullString() : ("http://" + HttpContext.Current.Request.Url.Host + masterSettings.ExtendSharePic.ToNullString())));
					this.hidExtendShareUrl.Value = Globals.HostPath(HttpContext.Current.Request.Url) + "/wapshop/ReferralAgreement.aspx?ReferralUserId=" + HiContext.Current.User.UserId;
					this.posterPanel.Visible = true;
					this.noreferralPanel.Visible = false;
				}
			}
			else
			{
				this.posterPanel.Visible = false;
				this.noreferralPanel.Visible = true;
			}
		}

		public bool CreatePoster(out string imgUrl)
		{
			bool result = false;
			if (this.resultObj == null || this.resultObj["BgImg"] == null)
			{
				imgUrl = "掌柜名片模板未设置，无法生成名片！";
				return result;
			}
			imgUrl = "生成失败";
			Bitmap bitmap = null;
			bitmap = ((!this.CodeUrl.Contains("weixin.qq.com")) ? ((Bitmap)System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(this.CodeUrl))) : ResourcesHelper.GetNetImg(this.CodeUrl));
			int num = int.Parse(this.resultObj["DefaultHead"].ToString());
			if (string.IsNullOrEmpty(this.UserHead) || (!this.UserHead.ToLower().StartsWith("http") && !this.UserHead.ToLower().StartsWith("https") && !File.Exists(Globals.MapPath(this.UserHead))))
			{
				this.UserHead = "/Utility/pics/imgnopic.jpg";
			}
			if (num == 2)
			{
				this.UserHead = "";
			}
			System.Drawing.Image image = (!this.UserHead.ToLower().StartsWith("http") && !this.UserHead.ToLower().StartsWith("https")) ? ((string.IsNullOrEmpty(this.UserHead) || !File.Exists(Globals.MapPath(this.UserHead))) ? new Bitmap(100, 100) : System.Drawing.Image.FromFile(Globals.MapPath(this.UserHead))) : ResourcesHelper.GetNetImg(this.UserHead);
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddEllipse(new Rectangle(0, 0, image.Width, image.Width));
			Bitmap bitmap2 = new Bitmap(image.Width, image.Width);
			using (Graphics graphics = Graphics.FromImage(bitmap2))
			{
				graphics.SetClip(graphicsPath);
				graphics.DrawImage(image, 0, 0, image.Width, image.Width);
			}
			image.Dispose();
			bitmap = ResourcesHelper.CombinImage(bitmap, bitmap2, 80);
			Bitmap bitmap3 = new Bitmap(480, 735);
			Graphics graphics2 = Graphics.FromImage(bitmap3);
			graphics2.SmoothingMode = SmoothingMode.HighQuality;
			graphics2.CompositingQuality = CompositingQuality.HighQuality;
			graphics2.InterpolationMode = InterpolationMode.High;
			graphics2.Clear(Color.White);
			Bitmap image2 = new Bitmap(100, 100);
			if (this.resultObj["BgImg"] != null && File.Exists(Globals.MapPath(this.resultObj["BgImg"].ToString())))
			{
				image2 = (Bitmap)System.Drawing.Image.FromFile(Globals.MapPath(this.resultObj["BgImg"].ToString()));
				image2 = ResourcesHelper.GetThumbnail(image2, 735, 480);
			}
			graphics2.DrawImage(image2, 0, 0, 480, 735);
			Font font = new Font("微软雅黑", (float)(this.resultObj["myusernameSize"].ToInt(0) * 6 / 5));
			Font font2 = new Font("微软雅黑", (float)(this.resultObj["shopnameSize"].ToInt(0) * 6 / 5));
			graphics2.DrawImage(bitmap2, (int)((decimal)this.resultObj["posList"][0]["left"] * 480m), (int)this.resultObj["posList"][0]["top"] * 735 / 490, (int)((decimal)this.resultObj["posList"][0]["width"] * 480m), (int)((decimal)this.resultObj["posList"][0]["width"] * 480m));
			StringFormat format = new StringFormat(StringFormatFlags.DisplayFormatControl);
			string text = this.resultObj["myusername"].ToString().Replace("{{昵称}}", "$");
			string text2 = this.resultObj["shopname"].ToString().Replace("{{商城名称}}", "$");
			string[] array = text.Split('$');
			string[] array2 = text2.Split('$');
			graphics2.DrawString(array[0], font, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["myusernameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][1]["left"] * 480m), (float)((int)this.resultObj["posList"][1]["top"] * 735 / 490), format);
			if (array.Length > 1)
			{
				SizeF sizeF = graphics2.MeasureString(" ", font);
				SizeF sizeF2 = graphics2.MeasureString(array[0], font);
				graphics2.DrawString(this.UserName, font, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["nickNameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][1]["left"] * 480m) + sizeF2.Width - sizeF.Width, (float)((int)this.resultObj["posList"][1]["top"] * 735 / 490), format);
				SizeF sizeF3 = graphics2.MeasureString(this.UserName, font);
				graphics2.DrawString(array[1], font, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["myusernameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][1]["left"] * 480m) + sizeF2.Width - sizeF.Width * 2f + sizeF3.Width, (float)((int)this.resultObj["posList"][1]["top"] * 735 / 490), format);
			}
			int num2 = 660 - (int)((decimal)this.resultObj["posList"][2]["left"] * 480m);
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < array2[0].Length; i++)
			{
				if (i < array2[0].Length)
				{
					string text3 = array2[0].Substring(i, 1);
					SizeF sizeF4 = graphics2.MeasureString(text3, font2);
					num3 += sizeF4.Width;
					if (num3 > (float)num2 - sizeF4.Width && num3 <= (float)num2)
					{
						graphics2.DrawString(array2[0].Substring(num4, i - num4), font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][2]["left"] * 480m), (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490 + num5 * (int)sizeF4.Height));
						num4 = i;
						num5++;
						num3 = 0f;
					}
				}
			}
			if (num4 < array2[0].Length)
			{
				string text4 = array2[0].Substring(num4, 1);
				SizeF sizeF5 = graphics2.MeasureString(text4, font2);
				graphics2.DrawString(array2[0].Substring(num4, array2[0].Length - num4), font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][2]["left"] * 480m), (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490 + num5 * (int)sizeF5.Height));
			}
			if (array2.Length > 1)
			{
				SizeF sizeF6 = graphics2.MeasureString(" ", font2);
				SizeF sizeF7 = graphics2.MeasureString(array2[0], font2);
				graphics2.DrawString(this.StoreName, font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["storeNameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][2]["left"] * 480m) + sizeF7.Width - sizeF6.Width, (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490), format);
				SizeF sizeF8 = graphics2.MeasureString(this.StoreName, font2);
				graphics2.DrawString(array2[1], font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), (float)(int)((decimal)this.resultObj["posList"][2]["left"] * 480m) + sizeF7.Width - sizeF6.Width * 2f + sizeF8.Width, (float)((int)this.resultObj["posList"][2]["top"] * 735 / 490), format);
			}
			graphics2.DrawImage(bitmap, (int)((decimal)this.resultObj["posList"][3]["left"] * 480m), (int)this.resultObj["posList"][3]["top"] * 735 / 490, (int)((decimal)this.resultObj["posList"][3]["width"] * 480m), (int)((decimal)this.resultObj["posList"][3]["width"] * 480m));
			bitmap.Dispose();
			if (!Directory.Exists(Globals.MapPath(Globals.GetStoragePath() + "/ReferralPoster")))
			{
				Directory.CreateDirectory(Globals.MapPath(Globals.GetStoragePath() + "/ReferralPoster"));
			}
			bitmap3.Save(Globals.MapPath(Globals.GetStoragePath() + "/ReferralPoster/Poster_Wap_" + this.ReferralId + ".jpg"), ImageFormat.Jpeg);
			Random random = new Random();
			imgUrl = Globals.GetStoragePath() + "/ReferralPoster/Poster_Wap_" + this.ReferralId + ".jpg?rnd=" + random.Next();
			bitmap3.Dispose();
			return true;
		}
	}
}
