using Hidistro.Context;
using Hidistro.Core;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[PersistChildren(true)]
	[ParseChildren(false)]
	public class HeadContainer : Control
	{
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string domainName = Globals.DomainName;
			string userAgent = HttpContext.Current.Request.UserAgent;
			bool flag = false;
			flag = (masterSettings.OpenWap == 1 && true);
			bool flag2 = masterSettings.OpenVstore == 1 && true;
			if (string.IsNullOrEmpty(userAgent))
			{
				userAgent = "";
			}
			HiContext current = HiContext.Current;
			bool flag3 = false;
			if (masterSettings.OpenMultStore)
			{
				flag3 = true;
			}
			string imageServerUrl = Globals.GetImageServerUrl();
			writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n            var skinPath = \"{0}\";\r\n            var HasWapRight = {1};\r\n            var IsOpenStores = {2};\r\n            var IsOpenReferral = {3};\r\n            var HasVshopRight = {4};\r\n            var ImageServerUrl=\"{5}\";\r\n  var ImageUploadPath=\"{6}\";\r\n        </script>", current.GetSkinPath(), flag ? "true" : "false", flag3.ToString().ToLower(), current.SiteSettings.OpenReferral.ToString().ToLower(), flag2 ? "true" : "false", imageServerUrl, string.IsNullOrEmpty(imageServerUrl) ? "/admin/UploadHandler.ashx?action=newupload" : "/admin/UploadHandler.ashx?action=remoteupdateimages");
			writer.WriteLine();
			this.RenderMetaCharset(writer);
			this.RenderMetaLanguage(writer);
			this.RenderFavicon(writer);
			this.RenderMetaAuthor(writer);
			this.RenderMetaGenerator(writer);
			if (!HttpContext.Current.Request.Url.ToString().ToLower().Contains("/desig_templete"))
			{
				this.LoadVideo(writer);
			}
		}

		private void RenderMetaGenerator(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"GENERATOR\" content=\"" + HiContext.Current.Config.Version + "\" />");
		}

		private void RenderFavicon(HtmlTextWriter writer)
		{
			string arg = Globals.FullPath("/Favicon.ico");
			writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
			writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", arg);
		}

		private void RenderMetaAuthor(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta name=\"author\" content=\"Hishop development team\" />");
		}

		private void RenderMetaCharset(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />");
		}

		private void RenderMetaLanguage(HtmlTextWriter writer)
		{
			writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
		}

		private void LoadVideo(HtmlTextWriter writer)
		{
			string value = "\r\n<script language='javascript' type='text/javascript'>\r\n$(document).ready(function(){\r\n$('embed').each(function () {\r\n            if (!$(this).attr('src')) {\r\n                $('embed').each(function () {\r\n                    if (!$(this).attr('src')) {\r\n                        $(this).attr('src', $(this).attr('data-url'));\r\n                        var newembed = $(this).clone();\r\n                        $(this).parent().append(newembed);\r\n                        $(this).remove();\r\n                    }\r\n                });\r\n\r\n                $('iframe').each(function () {\r\n                    if (!$(this).attr('src')) {\r\n                        $('iframe').each(function () {\r\n                            if (!$(this).attr('src')) {\r\n                                $(this).attr('src', $(this).attr('data-url'));\r\n                            }\r\n                        });\r\n                    }\r\n                });\r\n            }\r\n        });\r\n})            \r\n</script>\r\n";
			writer.WriteLine(value);
		}

		private static string ToNullString(object obj)
		{
			return (obj == null && obj != DBNull.Value) ? string.Empty : obj.ToString().Trim();
		}
	}
}
