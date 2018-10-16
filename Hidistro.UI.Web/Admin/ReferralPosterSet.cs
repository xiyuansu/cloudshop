using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Referrals)]
	public class ReferralPosterSet : AdminPage
	{
		protected TextBox linkUrl;

		protected HiddenField hidUploadLogo;

		protected ReferralPosterSet()
		{
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = HiContext.Current.SiteSettings.SiteUrl.ToLower();
			text = ((text.StartsWith("http://") || text.StartsWith("https://")) ? text : Globals.FullPath(text));
			this.linkUrl.Text = text + "/wapshop/SplittinRule";
			string a = base.Request.Form["action"];
			if (a == "Edit")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"success\":\"false\",\"Desciption\":\"保存设置失败!\"}";
				string path = base.Server.MapPath("~/Storage/data/Utility/ReferralPoster.js");
				string text2 = base.Request.Form["SotreCardJson"].ToNullString();
				string fileURL = base.Request.Form["tempImage"].ToNullString();
				fileURL = Globals.SaveFile("ReferralPoster/Posterbg", fileURL, "/Storage/master/", true, false, "");
				if (!string.IsNullOrEmpty(fileURL) && fileURL.ToLower().StartsWith("/storage/master/temp/"))
				{
					fileURL = Globals.SaveFile("ReferralPoster/Posterbg", fileURL, "/Storage/master/", true, false, "");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text2 = text2.Replace("--BigImagePlaceholder--", fileURL);
					JObject jObject = JsonConvert.DeserializeObject(text2) as JObject;
					if (jObject["posList"] != null && jObject["DefaultHead"] != null && jObject["myusername"] != null && jObject["shopname"] != null)
					{
						try
						{
							jObject["writeDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
							File.WriteAllText(path, JsonConvert.SerializeObject(jObject));
							DirectoryInfo directoryInfo = new DirectoryInfo(base.Server.MapPath("/Storage/master/ReferralPoster/Posterbg/"));
							if (directoryInfo != null)
							{
								FileInfo[] files = directoryInfo.GetFiles();
								FileInfo[] array = files;
								foreach (FileInfo fileInfo in array)
								{
									if (fileInfo.Name.StartsWith("Poster_"))
									{
										File.Delete(fileInfo.FullName);
									}
								}
							}
							s = "{\"success\":\"true\",\"Desciption\":\"保存设置成功!\"}";
						}
						catch (Exception ex)
						{
							s = "{\"success\":\"false\",\"Desciption\":\"保存设置失败!" + ex.Message + "\"}";
						}
					}
				}
				base.Response.Write(s);
				base.Response.End();
			}
		}
	}
}
