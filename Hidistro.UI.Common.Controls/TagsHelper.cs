using Hidistro.Context;
using Hidistro.Core;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Common.Controls
{
	public static class TagsHelper
	{
		public static XmlNode FindProductNode(int subjectId, string type)
		{
			XmlDocument productDocument = TagsHelper.GetProductDocument();
			return productDocument.SelectSingleNode($"//Subject[@SubjectId='{subjectId}' and @Type='{type}']");
		}

		public static XmlNode FindAdNode(int id, string type)
		{
			XmlDocument adDocument = TagsHelper.GetAdDocument();
			return adDocument.SelectSingleNode($"//Ad[@Id='{id}' and @Type='{type}']");
		}

		public static XmlNode FindCommentNode(int id, string type)
		{
			XmlDocument commentDocument = TagsHelper.GetCommentDocument();
			return commentDocument.SelectSingleNode($"//Comment[@Id='{id}' and @Type='{type}']");
		}

		public static bool UpdateProductNode(int subjectId, string type, Dictionary<string, string> simplenode)
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/Products.xml");
			bool result = false;
			XmlDocument productDocument = TagsHelper.GetProductDocument();
			XmlNode xmlNode = TagsHelper.FindProductNode(subjectId, type);
			if (xmlNode != null)
			{
				foreach (KeyValuePair<string, string> item in simplenode)
				{
					xmlNode.Attributes[item.Key].Value = item.Value;
				}
				productDocument.Save(filename);
				new AspNetCache().Remove("SubjectProductFileCache-Admin");
				result = true;
			}
			return result;
		}

		public static bool UpdateAdNode(int aId, string type, Dictionary<string, string> adnode)
		{
			bool result = false;
			XmlDocument adDocument = TagsHelper.GetAdDocument();
			XmlNode xmlNode = TagsHelper.FindAdNode(aId, type);
			if (xmlNode != null)
			{
				foreach (KeyValuePair<string, string> item in adnode)
				{
					xmlNode.Attributes[item.Key].Value = item.Value;
				}
				string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/Ads.xml");
				adDocument.Save(filename);
				new AspNetCache().Remove("AdFileCache-Admin");
				result = true;
			}
			return result;
		}

		public static bool UpdateCommentNode(int commentId, string type, Dictionary<string, string> commentnode)
		{
			bool result = false;
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/Comments.xml");
			XmlDocument commentDocument = TagsHelper.GetCommentDocument();
			XmlNode xmlNode = TagsHelper.FindCommentNode(commentId, type);
			if (xmlNode != null)
			{
				foreach (KeyValuePair<string, string> item in commentnode)
				{
					xmlNode.Attributes[item.Key].Value = item.Value;
				}
				commentDocument.Save(filename);
				new AspNetCache().Remove("CommentFileCache-Admin");
				result = true;
			}
			return result;
		}

		private static XmlDocument GetProductDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/Products.xml");
			XmlDocument xmlDocument = new AspNetCache().Get<XmlDocument>("SubjectProductFileCache-Admin");
			if (xmlDocument == null)
			{
				HttpContext context = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				new AspNetCache().Insert("SubjectProductFileCache-Admin", xmlDocument, 1800, false);
			}
			return xmlDocument;
		}

		private static XmlDocument GetAdDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/Ads.xml");
			XmlDocument xmlDocument = new AspNetCache().Get<XmlDocument>("AdFileCache-Admin");
			if (xmlDocument == null)
			{
				HttpContext context = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				new AspNetCache().Insert("AdFileCache-Admin", xmlDocument, 1800, false);
			}
			return xmlDocument;
		}

		private static XmlDocument GetCommentDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/Comments.xml");
			XmlDocument xmlDocument = new AspNetCache().Get<XmlDocument>("CommentFileCache-Admin");
			if (xmlDocument == null)
			{
				HttpContext context = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				new AspNetCache().Insert("CommentFileCache-Admin", xmlDocument, 1800, false);
			}
			return xmlDocument;
		}

		private static XmlDocument GetHeadMenuDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new AspNetCache().Get<XmlDocument>("HeadMenuFileCache-Admin");
			if (xmlDocument == null)
			{
				HttpContext context = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				new AspNetCache().Insert("HeadMenuFileCache-Admin", xmlDocument, 1800, false);
			}
			return xmlDocument;
		}

		public static DataTable GetHeaderMune()
		{
			XmlDocument headMenuDocument = TagsHelper.GetHeadMenuDocument();
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Title");
			dataTable.Columns.Add("DisplaySequence", typeof(int));
			dataTable.Columns.Add("Category");
			dataTable.Columns.Add("Url");
			dataTable.Columns.Add("Where");
			dataTable.Columns.Add("Visible");
			XmlNodeList childNodes = headMenuDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				if (item.Attributes["Visible"].Value.ToLower() == "true")
				{
					DataRow dataRow = dataTable.NewRow();
					dataRow["Title"] = item.Attributes["Title"].Value;
					dataRow["DisplaySequence"] = int.Parse(item.Attributes["DisplaySequence"].Value);
					dataRow["Category"] = item.Attributes["Category"].Value;
					dataRow["Url"] = item.Attributes["Url"].Value;
					dataRow["Where"] = item.Attributes["Where"].Value;
					dataRow["Visible"] = item.Attributes["Visible"].Value;
					dataTable.Rows.Add(dataRow);
				}
			}
			return dataTable;
		}
	}
}
