using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.UI.Common.Controls;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web
{
	public class DesigHandler : AdminPage, IHttpHandler
	{
		private string pagename = "";

		private string message = "";

		private string modeId = "";

		private string desigtype = "";

		private string elementId = "";

		private string configurl = "";

		private XmlNode currennode = null;

		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public new void ProcessRequest(HttpContext context)
		{
			try
			{
				this.modeId = context.Request.Form["ModelId"];
				string format = "{{\"success\":{0},\"Result\":{1}}}";
				string a = this.modeId;
				if (!(a == "Load"))
				{
					if (!(a == "editedialog"))
					{
						if (a == "editedirection")
						{
							this.message = this.EditeDirection(context);
						}
					}
					else
					{
						this.desigtype = context.Request.Form["Type"];
						if (this.desigtype != "logo")
						{
							string text = context.Request.Form["Elementid"];
							if (this.desigtype != "" && text.Split('_').Length == 2)
							{
								this.elementId = text.Split('_')[1];
								this.configurl = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/" + text.Split('_')[0] + ".xml");
								this.currennode = this.FindNode(text.Split('_')[0]);
								if (this.currennode != null)
								{
									string text2 = JsonConvert.SerializeXmlNode(this.currennode);
									text2 = text2.Remove(0, text2.IndexOf(":") + 1).Remove(text2.Length - (text2.IndexOf(":") + 1) - 1).Replace("@", "");
									this.message = string.Format(format, "true", text2);
								}
							}
						}
						else
						{
							this.message = string.Format(format, "true", "{\"LogoUrl\":\"" + HiContext.Current.SiteSettings.LogoUrl + "\",\"DialogName\":\"DialogTemplates/advert_logo.html\"}");
						}
					}
				}
				else
				{
					this.pagename = context.Request.Form["PageName"];
					DesigAttribute.PageName = this.pagename;
					string text3 = this.LoadFirstHtml();
					if (!string.IsNullOrEmpty(text3))
					{
						this.message = string.Format(format, "true", text3);
					}
				}
			}
			catch (Exception ex)
			{
				this.message = "{\"success\":false,\"Result\":\"未知错误:" + ex.Message + "\"}";
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}

		public HtmlDocument GetHtmlDocument(string url)
		{
			HtmlDocument htmlDocument = null;
			if (url != "")
			{
				htmlDocument = new HtmlDocument();
				htmlDocument.Load(url);
			}
			return htmlDocument;
		}

		public HtmlDocument GetWebHtmlDocument(string weburl)
		{
			HtmlDocument result = null;
			if (weburl != "")
			{
				HtmlWeb htmlWeb = new HtmlWeb();
				result = htmlWeb.Load(weburl);
			}
			return result;
		}

		public string LoadFirstHtml()
		{
			string text = "";
			HtmlDocument htmlDocument = this.GetHtmlDocument(DesigAttribute.DesigPagePath);
			HtmlDocument webHtmlDocument = this.GetWebHtmlDocument(DesigAttribute.SourcePagePath);
			HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@rel=\"desig\"]");
			IList<DesignTempleteInfo> list = new List<DesignTempleteInfo>();
			foreach (HtmlNode item in (IEnumerable<HtmlNode>)htmlNodeCollection)
			{
				HtmlNode elementbyId = webHtmlDocument.GetElementbyId(item.Id);
				if (elementbyId != null)
				{
					DesignTempleteInfo designTempleteInfo = new DesignTempleteInfo();
					designTempleteInfo.TempleteID = item.Id;
					designTempleteInfo.TempleteContent = elementbyId.InnerHtml;
					list.Add(designTempleteInfo);
				}
			}
			if (list.Count > 0)
			{
				return JsonConvert.SerializeObject(list);
			}
			return DesigAttribute.DesigPagePath + "-" + DesigAttribute.SourcePagePath;
		}

		private string EditeDirection(HttpContext context)
		{
			string result = "";
			string format = "{{\"success\":{0},\"Result\":\"{1}\"}}";
			try
			{
				if (!this.CheckDirectionParama(context, ref result))
				{
					return result;
				}
				string text = context.Request.Form["PageName"];
				string type = context.Request.Form["Type"];
				string type2 = context.Request.Form["MoveType"];
				string text2 = context.Request.Form["Elementid"];
				string text3 = context.Request.Form["MoveElementid"];
				Direction direction = (Direction)Enum.Parse(typeof(Direction), context.Request.Form["Direction"]);
				string text4 = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + this.GetPageNameSource(text));
				HtmlDocument htmlDocument = this.GetHtmlDocument(text4);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary = this.SetAttrDic(type, text2.Split('_')[1]);
				HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@" + dictionary.Keys.First() + "='" + dictionary.Values.First() + "']");
				dictionary = this.SetAttrDic(type2, text3.Split('_')[1]);
				HtmlNode htmlNode2 = htmlDocument.DocumentNode.SelectSingleNode("//*[@" + dictionary.Keys.First() + "='" + dictionary.Values.First() + "']");
				HtmlNode newChild = htmlNode.CloneNode(true);
				htmlNode.Remove();
				if (direction == Direction.Up)
				{
					htmlNode2.ParentNode.InsertBefore(newChild, htmlNode2);
				}
				else
				{
					htmlNode2.ParentNode.InsertAfter(newChild, htmlNode2);
				}
				htmlDocument.Save(text4);
				result = string.Format(format, "true", "标签移动更新成功！");
			}
			catch (Exception ex)
			{
				result = string.Format(format, "false", ex.Message);
			}
			return result;
		}

		private bool CheckDirectionParama(HttpContext context, ref string msg)
		{
			bool result = true;
			string format = "{{\"success\":{0},\"Result\":\"{1}\"}}";
			string text = context.Request.Form["PageName"];
			string value = context.Request.Form["Type"];
			string value2 = context.Request.Form["MoveType"];
			string value3 = context.Request.Form["Elementid"];
			string value4 = context.Request.Form["MoveElementid"];
			text = this.GetPageNameSource(text);
			if (string.IsNullOrEmpty(text))
			{
				msg = string.Format(format, "false", "无法找到源文件");
				result = false;
			}
			if (string.IsNullOrEmpty(value))
			{
				result = false;
				msg = string.Format(format, "false", "无法识别移动对象的类型");
			}
			if (string.IsNullOrEmpty(value2))
			{
				result = false;
				msg = string.Format(format, "false", "无法识别移动对象二的类型");
			}
			if (string.IsNullOrEmpty(value3) || string.IsNullOrEmpty(value4))
			{
				result = false;
				msg = string.Format(format, "false", "无法找到移动对象");
			}
			return result;
		}

		public XmlNode FindNode(string configname)
		{
			XmlNode xmlNode = null;
			if (this.elementId != "")
			{
				XmlDocument xmlNode2 = this.GetXmlNode();
				string text = "";
				text = ((!(configname == "products")) ? ((!(configname == "ads")) ? ((!(configname == "comments")) ? $"//Menu[@Id='{this.elementId}']" : $"//Comment[@Id='{this.elementId}']") : $"//Ad[@Id='{this.elementId}']") : $"//Subject[@SubjectId='{this.elementId}']");
				if (text != "")
				{
					xmlNode = xmlNode2.SelectSingleNode(text);
					XmlAttribute xmlAttribute = xmlNode2.CreateAttribute("DialogName");
					xmlAttribute.InnerText = this.GetDialoName();
					xmlNode.Attributes.Append(xmlAttribute);
				}
			}
			return xmlNode;
		}

		public string GetDialoName()
		{
			string str = "error.html";
			if (this.desigtype != "")
			{
				switch (this.desigtype)
				{
				case "floor":
					str = "product_floor_edite.html";
					break;
				case "tab":
					str = "product_tab_edite.html";
					break;
				case "top":
					str = "product_top_edite.html";
					break;
				case "group":
					str = "product_group_edite.html";
					break;
				case "simple":
					str = "simple_edite.html";
					break;
				case "slide":
					str = "advert_slide_edite.html";
					break;
				case "image":
					str = "advert_image_edite.html";
					break;
				case "custom":
					str = "advert_custom_edite.html";
					break;
				case "article":
					str = "comment_article_edite.html";
					break;
				case "category":
					str = "comment_category_edite.html";
					break;
				case "brand":
					str = "comment_brand_edite.html";
					break;
				case "keyword":
					str = "comment_keyword_edite.html";
					break;
				case "attribute":
					str = "comment_attribute_edite.html";
					break;
				case "title":
					str = "comment_title_edite.html";
					break;
				case "morelink":
					str = "comment_morelink_edite.html";
					break;
				default:
					str = "error.html";
					break;
				}
			}
			return "DialogTemplates/" + str;
		}

		private string GetPageNameSource(string pagename)
		{
			string result = "";
			switch (pagename)
			{
			case "default":
				result = "Skin-Default.html";
				break;
			case "login":
				result = "Skin-Login.html";
				break;
			case "brand":
				result = "Skin-Brand.html";
				break;
			case "branddetail":
				result = "Skin-BrandDetails.html";
				break;
			case "product":
				result = "Skin-SubCategory.html";
				break;
			case "productdetail":
				result = "Skin-ProductDetails.html";
				break;
			case "article":
				result = "Skin-Articles.html";
				break;
			case "articledetail":
				result = "Skin-ArticleDetails.html";
				break;
			case "cuountdown":
				result = "Skin-CountDownProducts.html";
				break;
			case "cuountdowndetail":
				result = "Skin-CountDownProductsDetails.html";
				break;
			case "groupbuy":
				result = "Skin-GroupBuyProducts.html";
				break;
			case "groupbuydetail":
				result = "Skin-GroupBuyProductDetails.html";
				break;
			case "help":
				result = "Skin-Helps.html";
				break;
			case "helpdetail":
				result = "Skin-HelpDetails.html";
				break;
			case "gift":
				result = "Skin-OnlineGifts.html";
				break;
			case "giftdetail":
				result = "Skin-GiftDetails.html";
				break;
			case "shopcart":
				result = "Skin-ShoppingCart.html";
				break;
			}
			return result;
		}

		private Dictionary<string, string> SetAttrDic(string type, string Id)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			switch (type.ToLower())
			{
			case "slide":
			case "image":
			case "brand":
			case "custom":
				dictionary.Add("adid", Id);
				break;
			case "article":
			case "category":
			case "keyword":
			case "attribute":
			case "title":
			case "morelink":
				dictionary.Add("commentid", Id);
				break;
			case "floor":
			case "tab":
			case "top":
			case "group":
			case "simple":
				dictionary.Add("subjectid", Id);
				break;
			}
			return dictionary;
		}

		public XmlDocument GetXmlNode()
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(this.configurl))
			{
				xmlDocument.Load(this.configurl);
			}
			return xmlDocument;
		}
	}
}
