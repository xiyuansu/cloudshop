using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.API
{
	public class SiteMapJobHandler : IHttpHandler
	{
		private SiteSettings siteSettings;

		private Database database;

		private List<string> sitemaps = new List<string>();

		private string webroot;

		private string weburl;

		private string indexxml;

		private string prourl;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				this.siteSettings = SettingsManager.GetMasterSettings();
				this.database = DatabaseFactory.CreateDatabase();
				string text = "";
				text = ((!(text == "/")) ? ("/" + text.Replace("/", "")) : "");
				this.prourl = "http://" + this.siteSettings.SiteUrl;
				this.weburl = "http://" + this.siteSettings.SiteUrl + text;
				this.sitemaps.Add(this.weburl + "/sitemap1.xml");
				this.sitemaps.Add(this.weburl + "/sitemap2.xml");
				this.sitemaps.Add(this.weburl + "/sitemap3.xml");
				this.indexxml = this.weburl + "/sitemapindex.xml";
				this.webroot = Globals.MapPath("/");
				this.CreateSiteMapxml();
			}
			catch (Exception ex)
			{
				Globals.WriteLog("SiteMapJob.txt", DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.Source + "\r\n");
			}
			context.Response.Write("OK");
		}

		public void CreateSiteMapxml()
		{
			try
			{
				this.CreateCateXml();
				this.CreateProductXml();
				this.CreateArticleXml();
				this.CreateIndexXml();
			}
			catch (Exception ex)
			{
				Globals.WriteLog("SiteMapJob.txt", DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n");
			}
		}

		public void CreateCateXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select CategoryId,RewriteName from Hishop_Categories");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("url", xmlElement.NamespaceURI);
					XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
					object obj = ((IDataRecord)dataReader)["RewriteName"];
					string text = (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString())) ? (this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "subCategory_Rewrite", new
					{
						rewrite = obj,
						categoryId = Convert.ToInt32(((IDataRecord)dataReader)["CategoryId"])
					})) : (this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "subCategory", new
					{
						categoryId = Convert.ToInt32(((IDataRecord)dataReader)["CategoryId"])
					}));
					XmlText newChild2 = xmlDocument.CreateTextNode(text);
					xmlElement3.AppendChild(newChild2);
					XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
					XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
					xmlElement4.AppendChild(newChild3);
					XmlElement xmlElement5 = xmlDocument.CreateElement("changefreq", xmlElement2.NamespaceURI);
					xmlElement5.InnerText = "daily";
					XmlElement xmlElement6 = xmlDocument.CreateElement("priority", xmlElement2.NamespaceURI);
					xmlElement6.InnerText = "1.0";
					XmlElement xmlElement7 = xmlDocument.CreateElement("data", xmlElement2.NamespaceURI);
					XmlElement xmlElement8 = xmlDocument.CreateElement("display", xmlElement7.NamespaceURI);
					XmlElement xmlElement9 = xmlDocument.CreateElement("html5_url", xmlElement8.NamespaceURI);
					xmlElement9.InnerText = text;
					XmlElement xmlElement10 = xmlDocument.CreateElement("wml_url", xmlElement8.NamespaceURI);
					xmlElement10.InnerText = text;
					XmlElement xmlElement11 = xmlDocument.CreateElement("xhtml_url", xmlElement8.NamespaceURI);
					xmlElement11.InnerText = text;
					xmlElement7.AppendChild(xmlElement8);
					xmlElement8.AppendChild(xmlElement9);
					xmlElement8.AppendChild(xmlElement10);
					xmlElement8.AppendChild(xmlElement11);
					xmlElement2.AppendChild(xmlElement3);
					xmlElement2.AppendChild(xmlElement4);
					xmlElement2.AppendChild(xmlElement5);
					xmlElement2.AppendChild(xmlElement6);
					xmlElement2.AppendChild(xmlElement7);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (File.Exists(this.webroot + "/sitemap1.xml"))
			{
				File.Delete(this.webroot + "/sitemap1.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemap1.xml");
		}

		public void CreateArticleXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ArticleId from dbo.Hishop_Articles where IsRelease='1'");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("url", xmlElement.NamespaceURI);
					XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
					XmlText newChild2 = xmlDocument.CreateTextNode(this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "ArticleDetails", new
					{
						articleId = Convert.ToInt32(((IDataRecord)dataReader)["ArticleId"])
					}));
					xmlElement3.AppendChild(newChild2);
					XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
					XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
					xmlElement4.AppendChild(newChild3);
					XmlElement xmlElement5 = xmlDocument.CreateElement("changefreq", xmlElement2.NamespaceURI);
					xmlElement5.InnerText = "daily";
					XmlElement xmlElement6 = xmlDocument.CreateElement("priority", xmlElement2.NamespaceURI);
					xmlElement6.InnerText = "1.0";
					XmlElement xmlElement7 = xmlDocument.CreateElement("data", xmlElement2.NamespaceURI);
					XmlElement xmlElement8 = xmlDocument.CreateElement("display", xmlElement7.NamespaceURI);
					XmlElement xmlElement9 = xmlDocument.CreateElement("html5_url", xmlElement8.NamespaceURI);
					xmlElement9.InnerText = this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "ArticleDetails", new
					{
						articleId = Convert.ToInt32(((IDataRecord)dataReader)["ArticleId"])
					});
					XmlElement xmlElement10 = xmlDocument.CreateElement("wml_url", xmlElement8.NamespaceURI);
					xmlElement10.InnerText = this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "ArticleDetails", new
					{
						articleId = Convert.ToInt32(((IDataRecord)dataReader)["ArticleId"])
					});
					XmlElement xmlElement11 = xmlDocument.CreateElement("xhtml_url", xmlElement8.NamespaceURI);
					xmlElement11.InnerText = this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "ArticleDetails", new
					{
						articleId = Convert.ToInt32(((IDataRecord)dataReader)["ArticleId"])
					});
					xmlElement7.AppendChild(xmlElement8);
					xmlElement8.AppendChild(xmlElement9);
					xmlElement8.AppendChild(xmlElement10);
					xmlElement8.AppendChild(xmlElement11);
					xmlElement2.AppendChild(xmlElement3);
					xmlElement2.AppendChild(xmlElement4);
					xmlElement2.AppendChild(xmlElement5);
					xmlElement2.AppendChild(xmlElement6);
					xmlElement2.AppendChild(xmlElement7);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (File.Exists(this.webroot + "/sitemap3.xml"))
			{
				File.Delete(this.webroot + "/sitemap3.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemap3.xml");
		}

		public void CreateProductXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			int num = default(int);
			DbCommand command = (!int.TryParse(this.siteSettings.SiteMapNum, out num) || num <= 0) ? this.database.GetSqlStringCommand("select top  1000 productid from dbo.Hishop_Products where salestatus=1 order by productid desc") : this.database.GetSqlStringCommand("select top " + num + " productid from dbo.Hishop_Products where salestatus=1  order by productid desc");
			using (IDataReader dataReader = this.database.ExecuteReader(command))
			{
				while (dataReader.Read())
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("url", xmlElement.NamespaceURI);
					XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
					XmlText newChild2 = xmlDocument.CreateTextNode(this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "productDetails", new
					{
						ProductId = Convert.ToInt32(((IDataRecord)dataReader)["productid"])
					}));
					xmlElement3.AppendChild(newChild2);
					XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
					XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
					xmlElement4.AppendChild(newChild3);
					XmlElement xmlElement5 = xmlDocument.CreateElement("changefreq", xmlElement2.NamespaceURI);
					xmlElement5.InnerText = "daily";
					XmlElement xmlElement6 = xmlDocument.CreateElement("priority", xmlElement2.NamespaceURI);
					xmlElement6.InnerText = "1.0";
					XmlElement xmlElement7 = xmlDocument.CreateElement("data", xmlElement2.NamespaceURI);
					XmlElement xmlElement8 = xmlDocument.CreateElement("display", xmlElement7.NamespaceURI);
					XmlElement xmlElement9 = xmlDocument.CreateElement("html5_url", xmlElement8.NamespaceURI);
					xmlElement9.InnerText = this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "productDetails", new
					{
						ProductId = Convert.ToInt32(((IDataRecord)dataReader)["productid"])
					});
					XmlElement xmlElement10 = xmlDocument.CreateElement("wml_url", xmlElement8.NamespaceURI);
					xmlElement10.InnerText = this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "productDetails", new
					{
						ProductId = Convert.ToInt32(((IDataRecord)dataReader)["productid"])
					});
					XmlElement xmlElement11 = xmlDocument.CreateElement("xhtml_url", xmlElement8.NamespaceURI);
					xmlElement11.InnerText = this.prourl + RouteConfig.GetRouteUrl(HttpContext.Current, "productDetails", new
					{
						ProductId = Convert.ToInt32(((IDataRecord)dataReader)["productid"])
					});
					xmlElement7.AppendChild(xmlElement8);
					xmlElement8.AppendChild(xmlElement9);
					xmlElement8.AppendChild(xmlElement10);
					xmlElement8.AppendChild(xmlElement11);
					xmlElement2.AppendChild(xmlElement3);
					xmlElement2.AppendChild(xmlElement4);
					xmlElement2.AppendChild(xmlElement5);
					xmlElement2.AppendChild(xmlElement6);
					xmlElement2.AppendChild(xmlElement7);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (File.Exists(this.webroot + "/sitemap2.xml"))
			{
				File.Delete(this.webroot + "/sitemap2.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemap2.xml");
		}

		public void CreateIndexXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			foreach (string sitemap in this.sitemaps)
			{
				XmlElement xmlElement2 = xmlDocument.CreateElement("sitemap", xmlElement.NamespaceURI);
				XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
				XmlText newChild2 = xmlDocument.CreateTextNode(sitemap);
				xmlElement3.AppendChild(newChild2);
				XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
				XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
				xmlElement4.AppendChild(newChild3);
				xmlElement2.AppendChild(xmlElement3);
				xmlElement2.AppendChild(xmlElement4);
				xmlElement.AppendChild(xmlElement2);
			}
			if (File.Exists(this.webroot + "/sitemapindex.xml"))
			{
				File.Delete(this.webroot + "/sitemapindex.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemapindex.xml");
		}
	}
}
