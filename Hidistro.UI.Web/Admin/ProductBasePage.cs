using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Sales;
using Hidistro.SqlDal.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	public class ProductBasePage : AdminPage
	{
		protected void DoCallback()
		{
			base.Response.Clear();
			base.Response.ContentType = "application/json";
			string text = base.Request.QueryString["action"];
			if (text.Equals("getPrepareData"))
			{
				int typeId = int.Parse(base.Request.QueryString["typeId"]);
				IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId);
				DataTable dataTable = ProductTypeHelper.GetBrandCategoriesByTypeId(typeId);
				if (dataTable.Rows.Count == 0)
				{
					dataTable = CatalogHelper.GetBrandCategories(0);
				}
				base.Response.Write(this.GenerateJsonString(attributes, dataTable));
				attributes.Clear();
			}
			else if (text.Equals("getMemberGradeList"))
			{
				IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
				if (memberGrades == null || memberGrades.Count == 0)
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{\"Status\":\"OK\",\"MemberGrades\":[");
					foreach (MemberGradeInfo item in memberGrades)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"GradeId\":\"{0}\",", item.GradeId);
						stringBuilder.AppendFormat("\"Name\":\"{0}\",", item.Name);
						stringBuilder.AppendFormat("\"Discount\":\"{0}\"", item.Discount);
						stringBuilder.Append("},");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("]}");
					base.Response.Write(stringBuilder.ToString());
				}
			}
			else if (text.Equals("checkProductCode"))
			{
				string productCode = Globals.StripAllTags(base.Request.QueryString["productCode"].ToNullString());
				int productId = 0;
				int.TryParse(base.Request.QueryString["productId"], out productId);
				ProductDao productDao = new ProductDao();
				if (productDao.IsExistsProductCode(productCode, productId))
				{
					base.Response.Write(true);
				}
				else
				{
					base.Response.Write(false);
				}
			}
			else if (text.Equals("checkSkuCode"))
			{
				string skuCode = Globals.StripAllTags(base.Request.QueryString["skuCode"].ToNullString());
				int productId2 = 0;
				int.TryParse(base.Request.QueryString["productId"], out productId2);
				ProductDao productDao2 = new ProductDao();
				if (productDao2.IsExistsSkuCode(skuCode, productId2))
				{
					base.Response.Write(true);
				}
				else
				{
					base.Response.Write(false);
				}
			}
			else if (text.Equals("getShippingTemplatesValuationMethod"))
			{
				int num = base.Request.QueryString["shippingTemplatesId"].ToInt(0);
				if (num > 0)
				{
					ShippingTemplateInfo shippingTemplate = SalesHelper.GetShippingTemplate(num, false);
					if (shippingTemplate != null)
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.Append("{\"Status\":\"OK\",\"ValuationMethod\":\"" + (int)shippingTemplate.ValuationMethod + "\"}");
						base.Response.Write(stringBuilder2.ToString());
						base.Response.End();
					}
				}
				base.Response.Write("{\"Status\":\"FAIL\"}");
				base.Response.End();
			}
			base.Response.End();
		}

		protected void GetMemberPrices(SKUItem sku, string xml)
		{
			if (!string.IsNullOrEmpty(xml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				foreach (XmlNode item in xmlDocument.DocumentElement.SelectNodes("//grande"))
				{
					if (!string.IsNullOrEmpty(item.Attributes["price"].Value) && item.Attributes["price"].Value.Trim().Length != 0)
					{
						sku.MemberPrices.Add(int.Parse(item.Attributes["id"].Value), decimal.Parse(item.Attributes["price"].Value.Trim()));
					}
				}
			}
		}

		protected Dictionary<int, IList<int>> GetAttributes(string attributesXml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			Dictionary<int, IList<int>> dictionary = null;
			try
			{
				xmlDocument.LoadXml(attributesXml);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList == null || xmlNodeList.Count == 0)
				{
					return null;
				}
				dictionary = new Dictionary<int, IList<int>>();
				foreach (XmlNode item in xmlNodeList)
				{
					int key = int.Parse(item.Attributes["attributeId"].Value);
					IList<int> list = new List<int>();
					foreach (XmlNode childNode in item.ChildNodes)
					{
						if (childNode.Attributes["valueId"].Value != "")
						{
							list.Add(int.Parse(childNode.Attributes["valueId"].Value));
						}
					}
					if (list.Count > 0)
					{
						dictionary.Add(key, list);
					}
				}
			}
			catch
			{
			}
			return dictionary;
		}

		protected Dictionary<string, SKUItem> GetSkus(string skusXml, decimal weight = default(decimal))
		{
			XmlDocument xmlDocument = new XmlDocument();
			Dictionary<string, SKUItem> dictionary = null;
			try
			{
				xmlDocument.LoadXml(skusXml);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList == null || xmlNodeList.Count == 0)
				{
					return null;
				}
				dictionary = new Dictionary<string, SKUItem>();
				foreach (XmlNode item in xmlNodeList)
				{
					SKUItem sKUItem = new SKUItem
					{
						SKU = Globals.HtmlEncode(Globals.StripScriptTags(item.Attributes["skuCode"].Value).Replace("\\", "")),
						SalePrice = ((item.Attributes["salePrice"].Value.Length > 0) ? decimal.Parse(item.Attributes["salePrice"].Value) : decimal.Zero),
						CostPrice = ((item.Attributes["costPrice"].Value.Length > 0) ? decimal.Parse(item.Attributes["costPrice"].Value) : decimal.Zero),
						Stock = ((item.Attributes["qty"].Value.Length > 0) ? int.Parse(item.Attributes["qty"].Value) : 0),
						WarningStock = ((item.Attributes["warningQty"].Value.Length > 0) ? int.Parse(item.Attributes["warningQty"].Value) : 0),
						Weight = ((item.Attributes["weight"].Value.Length > 0 && item.Attributes["weight"].Value != "undefined") ? decimal.Parse(item.Attributes["weight"].Value) : weight)
					};
					string text = "";
					foreach (XmlNode childNode in item.SelectSingleNode("skuFields").ChildNodes)
					{
						text = text + childNode.Attributes["valueId"].Value + "_";
						sKUItem.SkuItems.Add(int.Parse(childNode.Attributes["attributeId"].Value), int.Parse(childNode.Attributes["valueId"].Value));
					}
					XmlNode xmlNode3 = item.SelectSingleNode("memberPrices");
					if (xmlNode3 != null && xmlNode3.ChildNodes.Count > 0)
					{
						foreach (XmlNode childNode2 in xmlNode3.ChildNodes)
						{
							if (!string.IsNullOrEmpty(childNode2.Attributes["price"].Value) && childNode2.Attributes["price"].Value.Trim().Length != 0)
							{
								sKUItem.MemberPrices.Add(int.Parse(childNode2.Attributes["id"].Value), decimal.Parse(childNode2.Attributes["price"].Value.Trim()));
							}
						}
					}
					if (text.Length >= 2)
					{
						sKUItem.SkuId = text.Substring(0, text.Length - 1);
						dictionary.Add(sKUItem.SkuId, sKUItem);
					}
				}
				return dictionary;
			}
			catch (Exception ex)
			{
				base.Response.Write(ex.Message);
				base.Response.End();
				return null;
			}
		}

		private string GenerateJsonString(IList<AttributeInfo> attributes, DataTable tbBrandCategories)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			string str = "";
			int num = 0;
			if (attributes != null && attributes.Count > 0)
			{
				stringBuilder2.Append("\"Attributes\":[");
				stringBuilder3.Append("\"SKUs\":[");
				foreach (AttributeInfo attribute in attributes)
				{
					int num2;
					if (attribute.UsageMode == AttributeUseageMode.Choose)
					{
						flag2 = true;
						if (attribute.UseAttributeImage)
						{
							flag4 = true;
							str = attribute.AttributeName;
							num = attribute.AttributeId;
						}
						stringBuilder3.Append("{");
						stringBuilder3.AppendFormat("\"Name\":\"{0}\",", attribute.AttributeName);
						StringBuilder stringBuilder5 = stringBuilder3;
						num2 = attribute.AttributeId;
						stringBuilder5.AppendFormat("\"AttributeId\":\"{0}\",", num2.ToString(CultureInfo.InvariantCulture));
						stringBuilder3.AppendFormat("\"UseAttributeImage\":\"{0}\",", attribute.UseAttributeImage ? 1 : 0);
						stringBuilder3.AppendFormat("\"SKUValues\":[{0}]", this.GenerateValueItems(attribute.AttributeValues));
						stringBuilder3.Append("},");
					}
					else if (attribute.UsageMode == AttributeUseageMode.View || attribute.UsageMode == AttributeUseageMode.MultiView)
					{
						flag = true;
						stringBuilder2.Append("{");
						stringBuilder2.AppendFormat("\"Name\":\"{0}\",", attribute.AttributeName);
						StringBuilder stringBuilder6 = stringBuilder2;
						num2 = attribute.AttributeId;
						stringBuilder6.AppendFormat("\"AttributeId\":\"{0}\",", num2.ToString(CultureInfo.InvariantCulture));
						StringBuilder stringBuilder7 = stringBuilder2;
						num2 = (int)attribute.UsageMode;
						stringBuilder7.AppendFormat("\"UsageMode\":\"{0}\",", num2.ToString());
						stringBuilder2.AppendFormat("\"AttributeValues\":[{0}]", this.GenerateValueItems(attribute.AttributeValues));
						stringBuilder2.Append("},");
					}
				}
				if (stringBuilder2.Length > 14)
				{
					stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
				}
				if (stringBuilder3.Length > 8)
				{
					stringBuilder3.Remove(stringBuilder3.Length - 1, 1);
				}
				stringBuilder2.Append("]");
				stringBuilder3.Append("]");
			}
			if (tbBrandCategories != null && tbBrandCategories.Rows.Count > 0)
			{
				flag3 = true;
				stringBuilder4.AppendFormat("\"BrandCategories\":[{0}]", this.GenerateBrandString(tbBrandCategories));
			}
			stringBuilder.Append("{\"HasAttribute\":\"" + flag.ToString() + "\",");
			stringBuilder.Append("\"HasSKU\":\"" + flag2.ToString() + "\",");
			stringBuilder.Append("\"HasBrandCategory\":\"" + flag3.ToString() + "\",");
			stringBuilder.Append("\"HasAttributeImage\":\"" + flag4.ToString() + "\",");
			stringBuilder.Append("\"ImageAttributeText\":\"" + str + "\",");
			stringBuilder.Append("\"ImageAttributeId\":\"" + num + "\",");
			if (flag)
			{
				stringBuilder.Append(stringBuilder2.ToString()).Append(",");
			}
			if (flag2)
			{
				stringBuilder.Append(stringBuilder3.ToString()).Append(",");
			}
			if (flag3)
			{
				stringBuilder.Append(stringBuilder4.ToString()).Append(",");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		private string GenerateValueItems(IList<AttributeValueInfo> values)
		{
			if (values == null || values.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (AttributeValueInfo value in values)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"ValueId\":\"{0}\",\"ValueStr\":\"{1}\"", value.ValueId.ToString(CultureInfo.InvariantCulture), HttpUtility.UrlEncode(value.ValueStr.Replace("\\", "/")));
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		private string GenerateBrandString(DataTable tb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow row in tb.Rows)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"BrandId\":\"{0}\",\"BrandName\":\"{1}\"", row["BrandID"], row["BrandName"]);
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		protected string DownRemotePic(string productDescrip)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			DateTime now = DateTime.Now;
			int num = now.Year;
			string str = num.ToString();
			now = DateTime.Now;
			num = now.Month;
			string text = $"/Storage/master/gallery/{str + num.ToString()}/";
			string text2 = HttpContext.Current.Request.MapPath(text);
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			IList<string> outsiteLinkImgs = this.GetOutsiteLinkImgs(productDescrip);
			if (outsiteLinkImgs.Count > 0)
			{
				foreach (string item in outsiteLinkImgs)
				{
					WebClient webClient = new WebClient();
					string str2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
					string str3 = item.Substring(item.LastIndexOf('.'));
					try
					{
						webClient.DownloadFile(item, text2 + str2 + str3);
						productDescrip = productDescrip.Replace(item, text + str2 + str3);
					}
					catch
					{
					}
				}
			}
			return productDescrip;
		}

		private IList<string> GetOutsiteLinkImgs(string html)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			IList<string> list = new List<string>();
			Regex regex = new Regex("(src)[^>]*[^/].(?:jpg|bmp|gif|png)(?:\"|') ", RegexOptions.IgnoreCase);
			MatchCollection matchCollection = regex.Matches(html);
			string text = "";
			int num = 0;
			for (int i = 0; i < matchCollection.Count; i++)
			{
				text = matchCollection[i].Value.Replace("\\", "").Replace("\"", "").Replace("'", "")
					.Trim();
				text = text.Substring(4);
				if (text.ToLower(CultureInfo.InvariantCulture).IndexOf(siteSettings.SiteUrl.ToLower(CultureInfo.InvariantCulture)) == -1 && text.ToLower(CultureInfo.InvariantCulture).IndexOf(masterSettings.SiteUrl.ToLower(CultureInfo.InvariantCulture)) == -1)
				{
					list.Add(text);
				}
			}
			return list;
		}
	}
}
