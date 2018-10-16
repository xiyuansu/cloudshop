using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SlideAd : WebControl
	{
		public bool IsDelayedLoading
		{
			get;
			set;
		}

		public int AdId
		{
			get;
			set;
		}

		public bool IsFullScreen
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}

		public string RendHtml()
		{
			XmlNode xmlNode = TagsHelper.FindAdNode(this.AdId, "slide");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				if (this.IsFullScreen)
				{
					stringBuilder.AppendFormat("<div class=\"ad_slide cssEdite\" type=\"slide\" id=\"ads_{0}\" >", this.AdId).AppendLine();
					stringBuilder.AppendLine("<div class=\"focusWarp\">");
					stringBuilder.AppendLine("<ul class=\"imgList\">");
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image1"].Value))
					{
						if (xmlNode.Attributes["Url1"].Value.Length == 0 || xmlNode.Attributes["Url1"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image1"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url1"].Value.Length == 0) ? "" : xmlNode.Attributes["Url1"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image1"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image2"].Value))
					{
						if (xmlNode.Attributes["Url2"].Value.Length == 0 || xmlNode.Attributes["Url2"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image2"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url2"].Value.Length == 0) ? "" : xmlNode.Attributes["Url2"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image2"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image3"].Value))
					{
						if (xmlNode.Attributes["Url3"].Value.Length == 0 || xmlNode.Attributes["Url3"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image3"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url3"].Value.Length == 0) ? "" : xmlNode.Attributes["Url3"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image3"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image4"].Value))
					{
						if (xmlNode.Attributes["Url4"].Value.Length == 0 || xmlNode.Attributes["Url4"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image4"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url4"].Value.Length == 0) ? "" : xmlNode.Attributes["Url4"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image4"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image5"].Value))
					{
						if (xmlNode.Attributes["Url5"].Value.Length == 0 || xmlNode.Attributes["Url5"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image5"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url5"].Value.Length == 0) ? "" : xmlNode.Attributes["Url5"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image5"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image6"].Value))
					{
						if (xmlNode.Attributes["Url6"].Value.Length == 0 || xmlNode.Attributes["Url6"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image6"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url6"].Value.Length == 0) ? "" : xmlNode.Attributes["Url6"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image6"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image7"].Value))
					{
						if (xmlNode.Attributes["Url7"].Value.Length == 0 || xmlNode.Attributes["Url7"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image7"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url7"].Value.Length == 0) ? "" : xmlNode.Attributes["Url7"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image7"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image8"].Value))
					{
						if (xmlNode.Attributes["Url8"].Value.Length == 0 || xmlNode.Attributes["Url8"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image8"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url8"].Value.Length == 0) ? "" : xmlNode.Attributes["Url8"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image8"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image9"].Value))
					{
						if (xmlNode.Attributes["Url9"].Value.Length == 0 || xmlNode.Attributes["Url9"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image9"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url9"].Value.Length == 0) ? "" : xmlNode.Attributes["Url9"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image9"].Value)).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image10"].Value))
					{
						if (xmlNode.Attributes["Url10"].Value.Length == 0 || xmlNode.Attributes["Url10"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><div style=\"background:url({0}) no-repeat center;\"></div></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image10"].Value)).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><div style=\"background:url({1}) no-repeat center;\"></div></a></li>", (xmlNode.Attributes["Url10"].Value.Length == 0) ? "" : xmlNode.Attributes["Url10"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image10"].Value)).AppendLine();
						}
					}
					stringBuilder.AppendLine("</ul>");
					stringBuilder.AppendLine("</div>");
					stringBuilder.AppendLine("</div>");
				}
				else
				{
					string text = "600";
					string text2 = "300";
					if (!string.IsNullOrEmpty(xmlNode.Attributes["AdImageSize"].Value) && xmlNode.Attributes["AdImageSize"].Value.Contains("*"))
					{
						text = xmlNode.Attributes["AdImageSize"].Value.Split('*')[0];
						text2 = xmlNode.Attributes["AdImageSize"].Value.Split('*')[1];
					}
					stringBuilder.AppendFormat("<div class=\"ad_slide cssEdite\" type=\"slide\" id=\"ads_{0}\" >", this.AdId).AppendLine();
					stringBuilder.AppendLine("<div class=\"focusWarp\">");
					stringBuilder.AppendLine("<ul class=\"imgList\">");
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image1"].Value))
					{
						if (xmlNode.Attributes["Url1"].Value.Length == 0 || xmlNode.Attributes["Url1"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image1"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url1"].Value.Length == 0) ? "" : xmlNode.Attributes["Url1"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image1"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image2"].Value))
					{
						if (xmlNode.Attributes["Url2"].Value.Length == 0 || xmlNode.Attributes["Url2"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image2"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url2"].Value.Length == 0) ? "" : xmlNode.Attributes["Url2"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image2"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image3"].Value))
					{
						if (xmlNode.Attributes["Url3"].Value.Length == 0 || xmlNode.Attributes["Url3"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image3"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url3"].Value.Length == 0) ? "" : xmlNode.Attributes["Url3"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image3"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image4"].Value))
					{
						if (xmlNode.Attributes["Url4"].Value.Length == 0 || xmlNode.Attributes["Url4"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image4"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url4"].Value.Length == 0) ? "" : xmlNode.Attributes["Url4"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image4"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image5"].Value))
					{
						if (xmlNode.Attributes["Url5"].Value.Length == 0 || xmlNode.Attributes["Url5"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image5"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url5"].Value.Length == 0) ? "" : xmlNode.Attributes["Url5"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image5"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image6"].Value))
					{
						if (xmlNode.Attributes["Url6"].Value.Length == 0 || xmlNode.Attributes["Url6"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image6"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url6"].Value.Length == 0) ? "" : xmlNode.Attributes["Url6"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image6"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image7"].Value))
					{
						if (xmlNode.Attributes["Url7"].Value.Length == 0 || xmlNode.Attributes["Url7"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image7"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url7"].Value.Length == 0) ? "" : xmlNode.Attributes["Url7"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image7"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image8"].Value))
					{
						if (xmlNode.Attributes["Url8"].Value.Length == 0 || xmlNode.Attributes["Url8"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image8"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url8"].Value.Length == 0) ? "" : xmlNode.Attributes["Url8"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image8"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image9"].Value))
					{
						if (xmlNode.Attributes["Url9"].Value.Length == 0 || xmlNode.Attributes["Url9"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image9"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url9"].Value.Length == 0) ? "" : xmlNode.Attributes["Url9"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image9"].Value), text, text2).AppendLine();
						}
					}
					if (!string.IsNullOrEmpty(xmlNode.Attributes["Image10"].Value))
					{
						if (xmlNode.Attributes["Url10"].Value.Length == 0 || xmlNode.Attributes["Url10"].Value.Equals("http://"))
						{
							stringBuilder.AppendFormat("<li><a><img src=\"{0}\" width=\"{1}\" height=\"{2}\" /></a></li>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image10"].Value), text, text2).AppendLine();
						}
						else
						{
							stringBuilder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", (xmlNode.Attributes["Url10"].Value.Length == 0) ? "" : xmlNode.Attributes["Url110"].Value, Globals.GetImageServerUrl("http://", xmlNode.Attributes["Image10"].Value), text, text2).AppendLine();
						}
					}
					stringBuilder.AppendLine("</ul>");
					stringBuilder.AppendLine("</div>");
					stringBuilder.AppendLine("</div>");
					stringBuilder.AppendLine("<script type=\"text/javascript\">");
					stringBuilder.Append("$(function(){");
					stringBuilder.AppendFormat("$(\"#ads_{0}\").mogFocus({{scrollWidth:" + text + "}}); ", this.AdId).AppendLine();
					stringBuilder.Append("});");
					stringBuilder.AppendLine("</script>");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
