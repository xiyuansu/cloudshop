using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.UI.Common.Controls;
using System;
using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Senparc.Weixin.MP.CommonAPIs;
using Hidistro.UI.SaleSystem.CodeBehind.BaseClasses;

namespace Hidistro.UI.Web.Applet
{
    public partial class SplittinRule : AppletTemplatedWebControl
    {
        string SetJson = "";
        Newtonsoft.Json.Linq.JObject resultObj = null;
        string UserHead = "";
        string CodeUrl = "";
        string StoreName = "";
        string UserName = "";
        int ReferralId = 0;
        int userId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings setting = HiContext.Current.SiteSettings;
            CheckLogin();
            MemberInfo member = Users.GetUser(HiContext.Current.UserId);
            repeledPanel.Visible = false;
            if (member != null && member.UserId > 0 && member.IsReferral())
            {
                if (member.Referral.IsRepeled)
                {
                    posterPanel.Visible = false;
                    noreferralPanel.Visible = false;
                    repeledPanel.Visible = true;
                    repeledTime.Text = member.Referral.RepelTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    if (!string.IsNullOrEmpty(member.Referral.RepelReason.ToNullString()))
                    {
                        repeledReason.Text = member.Referral.RepelReason.ToNullString() + "<br>";
                        divRepeledReason.Visible = true;
                    }
                }
                else
                {

                    imghtmlfxvshop.Visible = false;
                    string ReferralUrl = Globals.HostPath(HttpContext.Current.Request.Url) + "/WapShop/ReferralAgreement?ReferralUserId=" + HiContext.Current.User.UserId;
                    //userReferralQRCode.ImageUrl = "/api/VshopProcess.ashx?action=GenerateTwoDimensionalImage&url=" + ReferralUrl;
                    CodeUrl = Globals.CreateQRCode(ReferralUrl, "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.Replace(".", "").Replace("https://", "").Replace("http://", "").Replace("/", "").Replace("\\", "") + "_Referral_" + HiContext.Current.User.UserId);
                    SetJson = System.IO.File.ReadAllText(HttpRuntime.AppDomainAppPath.ToString() + "Storage/data/Utility/ReferralPoster.js"); //获取配置文件
                    resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject(SetJson) as Newtonsoft.Json.Linq.JObject; //转化成JSON对象
                    bool rs = false;

                    if (resultObj != null && resultObj["writeDate"] != null && resultObj["posList"] != null
                        && resultObj["DefaultHead"] != null && resultObj["myusername"] != null && resultObj["shopname"] != null)
                    {
                        StoreName = HiContext.Current.SiteSettings.SiteName;
                        UserHead = member.Picture;
                        UserName = string.IsNullOrEmpty(member.NickName) ? member.RealName : member.NickName;
                        ReferralId = HiContext.Current.User.UserId;
                        userId = HiContext.Current.UserId;
                        string posterUrl;
                        CreatePoster(out posterUrl);
                        userReferralQRCode.ImageUrl = posterUrl;
                    }
                    else
                    {
                        userReferralQRCode.ImageUrl = "SET Failed";
                    }

                    string url = Page.Request.Url.AbsoluteUri;
                    this.hdAppId.Value = setting.WeixinAppId;

                    hidExtendShareTitle.Value = setting.ExtendShareTitle.ToNullString();
                    hidExtendShareDetail.Value = setting.ExtendShareDetail.ToNullString();

                    hidExtendSharePic.Value = string.IsNullOrEmpty(setting.ExtendSharePic) ? userReferralQRCode.ImageUrl : setting.ExtendSharePic.ToNullString().Contains("http://") ? setting.ExtendSharePic.ToNullString() : "http://" + HttpContext.Current.Request.Url.Host + setting.ExtendSharePic.ToNullString();
                    hidExtendShareUrl.Value = Globals.HostPath(HttpContext.Current.Request.Url) + "/vshop/ReferralAgreement.aspx?ReferralUserId=" + HiContext.Current.User.UserId;

                    //if (!site.OpenMultReferral)
                    //{
                    //    FindControl("secondDeduct").Visible = false;
                    //}
                    posterPanel.Visible = true;
                    noreferralPanel.Visible = false;
                }
            }
            else
            {
                posterPanel.Visible = false;
                noreferralPanel.Visible = true;

            }
        }


        public bool CreatePoster(out string imgUrl)
        {
            bool result = false;

            if (resultObj == null || resultObj["BgImg"] == null)
            {
                imgUrl = "掌柜名片模板未设置，无法生成名片！";
                return result;
            }

            imgUrl = "生成失败";




            //生成二维码图片
            Bitmap Qrimage = null;
            if (CodeUrl.Contains("weixin.qq.com"))
            {
                Qrimage = ResourcesHelper.GetNetImg(CodeUrl);
            }
            else
            {
                Qrimage = (Bitmap)System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(CodeUrl));
            }
            int DefaultHead = int.Parse(resultObj["DefaultHead"].ToString());

            if (string.IsNullOrEmpty(UserHead) || (!UserHead.ToLower().StartsWith("http") && !UserHead.ToLower().StartsWith("https") && !File.Exists(Globals.MapPath(UserHead))))
                UserHead = "/Utility/pics/imgnopic.jpg";


            if (DefaultHead == 2)
            {
                UserHead = "";
            }


            System.Drawing.Image logoimg;
            if (UserHead.ToLower().StartsWith("http") || UserHead.ToLower().StartsWith("https"))
            {
                logoimg = ResourcesHelper.GetNetImg(UserHead); //获取网络图片 ;// new Bitmap(UserHead);
            }
            else
            {
                if (!string.IsNullOrEmpty(UserHead) && File.Exists(Globals.MapPath(UserHead)))
                {
                    logoimg = System.Drawing.Image.FromFile(Globals.MapPath(UserHead));
                }
                else
                {
                    logoimg = new Bitmap(100, 100);
                };
            }


            //转换成圆形图片
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(new Rectangle(0, 0, logoimg.Width, logoimg.Width));
            Bitmap Tlogoimg = new Bitmap(logoimg.Width, logoimg.Width);
            using (Graphics gl = Graphics.FromImage(Tlogoimg))
            { //假设bm就是你要绘制的正方形位图，已创建好
                gl.SetClip(gp);
                gl.DrawImage(logoimg, 0, 0, logoimg.Width, logoimg.Width);
            }
            logoimg.Dispose();

            //合成二维码图像
            //Qrimage = CombinImage(Qrimage, Tlogoimg, 80); //二维码图片

            Bitmap Cardbmp = new Bitmap(480, 735);

            Graphics g = Graphics.FromImage(Cardbmp);
            g.SmoothingMode = SmoothingMode.HighQuality; ; //抗锯齿
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.High;
            g.Clear(System.Drawing.Color.White); //白色填充

            Bitmap Bgimg = new Bitmap(100, 100);

            if (resultObj["BgImg"] != null && File.Exists(Globals.MapPath(resultObj["BgImg"].ToString())))
            {
                //如果背景图片存在，
                Bgimg = (Bitmap)System.Drawing.Image.FromFile(Globals.MapPath(resultObj["BgImg"].ToString())); //如果存在，读取背景图片
                Bgimg =ResourcesHelper.GetThumbnail(Bgimg, 735, 480); //处理成对应尺寸图片
            }

            //绘制背景图
            g.DrawImage(Bgimg, 0, 0, 480, 735);


            Font usernameFont = new Font("微软雅黑", (int)(resultObj["myusernameSize"].ToInt() * 6 / 5));

            Font shopnameFont = new Font("微软雅黑", (int)(resultObj["shopnameSize"].ToInt() * 6 / 5));


            //加入用户头像
            g.DrawImage(Tlogoimg, (int)(((decimal)resultObj["posList"][0]["left"]) * 480),
                (int)resultObj["posList"][0]["top"] * 735 / 490,
                (int)(((decimal)resultObj["posList"][0]["width"]) * 480),
                (int)(((decimal)resultObj["posList"][0]["width"]) * 480)
                );

            StringFormat StringFormat = new StringFormat(StringFormatFlags.DisplayFormatControl);



            string myusername = resultObj["myusername"].ToString().Replace(@"{{昵称}}", "$");
            string shopname = resultObj["shopname"].ToString().Replace(@"{{商城名称}}", "$");

            string[] myusernameArray = myusername.Split('$');
            string[] shopnameArray = shopname.Split('$');

            //写昵称
            g.DrawString(myusernameArray[0], usernameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["myusernameColor"].ToString())),
                 (int)(((decimal)resultObj["posList"][1]["left"]) * 480),
                 (int)resultObj["posList"][1]["top"] * 735 / 490,
                StringFormat);

            if (myusernameArray.Length > 1)
            {
                var spcSize1 = g.MeasureString(" ", usernameFont);
                var myusernameSize = g.MeasureString(myusernameArray[0], usernameFont);
                g.DrawString(UserName, usernameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["nickNameColor"].ToString())),
               (int)(((decimal)resultObj["posList"][1]["left"]) * 480) + myusernameSize.Width - spcSize1.Width,
                (int)resultObj["posList"][1]["top"] * 735 / 490,
               StringFormat);

                var usernameSize = g.MeasureString(UserName, usernameFont);
                g.DrawString(myusernameArray[1], usernameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["myusernameColor"].ToString())),
               (int)(((decimal)resultObj["posList"][1]["left"]) * 480) + myusernameSize.Width - spcSize1.Width * 2 + usernameSize.Width,
                (int)resultObj["posList"][1]["top"] * 735 / 490,
               StringFormat);

            }


            //写店铺名
            var lineWidth = 660 - (int)(((decimal)resultObj["posList"][2]["left"]) * 480);
            float fontWidth = 0;
            int index = 0;
            int lineIndex = 0;

            for (int i = 0; i < shopnameArray[0].Length; i++)
            {
                if (i < shopnameArray[0].Length)
                {
                    string str = shopnameArray[0].Substring(i, 1);
                    var spcSize2 = g.MeasureString(str, shopnameFont);
                    fontWidth += spcSize2.Width;
                    if (fontWidth > (lineWidth - spcSize2.Width) && fontWidth <= lineWidth)
                    {
                        g.DrawString(shopnameArray[0].Substring(index, i - index), shopnameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["shopnameColor"].ToString())),
             (int)(((decimal)resultObj["posList"][2]["left"]) * 480),
             (int)resultObj["posList"][2]["top"] * 735 / 490 + lineIndex * (int)spcSize2.Height);
                        index = i;
                        lineIndex++;
                        fontWidth = 0;
                    }
                }
            }

            if (index < shopnameArray[0].Length)
            {
                string str = shopnameArray[0].Substring(index, 1);
                var spcSize2 = g.MeasureString(str, shopnameFont);
                g.DrawString(shopnameArray[0].Substring(index, shopnameArray[0].Length - index), shopnameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["shopnameColor"].ToString())),
            (int)(((decimal)resultObj["posList"][2]["left"]) * 480),
            (int)resultObj["posList"][2]["top"] * 735 / 490 + lineIndex * (int)spcSize2.Height);
            }

            if (shopnameArray.Length > 1)
            {
                var spcSize = g.MeasureString(" ", shopnameFont);

                var shopnameSize = g.MeasureString(shopnameArray[0], shopnameFont);
                g.DrawString(StoreName, shopnameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["storeNameColor"].ToString())),
               (int)(((decimal)resultObj["posList"][2]["left"]) * 480) + shopnameSize.Width - spcSize.Width,
                (int)resultObj["posList"][2]["top"] * 735 / 490,
               StringFormat);

                var StorenameSize = g.MeasureString(StoreName, shopnameFont);
                g.DrawString(shopnameArray[1], shopnameFont, new SolidBrush(System.Drawing.ColorTranslator.FromHtml(resultObj["shopnameColor"].ToString())),
               (int)(((decimal)resultObj["posList"][2]["left"]) * 480) + shopnameSize.Width - spcSize.Width * 2 + StorenameSize.Width,
                (int)resultObj["posList"][2]["top"] * 735 / 490,
               StringFormat);

            }


            //加入二维码
            //g.DrawImage(Qrimage,
            //    (int)(((decimal)resultObj["posList"][3]["left"]) * 480),
            //    ((int)resultObj["posList"][3]["top"] * 735 / 490),
            //    (int)(((decimal)resultObj["posList"][3]["width"]) * 480),
            //    (int)(((decimal)resultObj["posList"][3]["width"]) * 480)
            //    );
            //Qrimage.Dispose();

            if (!Directory.Exists(Globals.MapPath(Globals.GetStoragePath() + @"/ReferralPoster")))
                Directory.CreateDirectory(Globals.MapPath(Globals.GetStoragePath() + @"/ReferralPoster"));

            Cardbmp.Save(Globals.MapPath(Globals.GetStoragePath() + @"/ReferralPoster/Poster_Applet_" + ReferralId + ".jpg"), ImageFormat.Jpeg);
            Random rd = new Random();
            imgUrl = Globals.GetStoragePath() + @"/ReferralPoster/Poster_Applet_" + ReferralId + ".jpg" + "?rnd=" + rd.Next();

            Cardbmp.Dispose();
            result = true;

            return result;
        }



    }
}