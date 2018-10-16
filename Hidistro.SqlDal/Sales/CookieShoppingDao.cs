using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Hidistro.SqlDal.Sales
{
	public class CookieShoppingDao : BaseDao
	{
		private const string CartDataCookieName = "Hid_Hishop_ShoppingCart_Data_New";

		public bool IsExistSkuId(string skuId)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand("SELECT COUNT(SkuId) FROM Hishop_SKUs WHERE SkuId=@SkuId;");
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			object obj = base.database.ExecuteScalar(sqlStringCommand);
			if (obj == null || obj == DBNull.Value || (int)obj == 0)
			{
				return false;
			}
			return true;
		}

		public AddCartItemStatus AddLineItem(string skuId, int quantity, int storeId = 0)
		{
			if (this.IsExistSkuId(skuId))
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@s='" + skuId + "|" + storeId + "']");
				if (xmlNode2 == null)
				{
					xmlNode2 = CookieShoppingDao.CreateLineItemNode(shoppingCartData, skuId + "|" + storeId, quantity);
					xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
				}
				else
				{
					xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
				}
				this.SaveShoppingCartData(shoppingCartData);
				return AddCartItemStatus.Successed;
			}
			return AddCartItemStatus.ProductNotExists;
		}

		public void ClearShoppingCart()
		{
			HttpContext.Current.Response.Cookies["Hid_Hishop_ShoppingCart_Data_New"].Value = null;
			HttpContext.Current.Response.Cookies["Hid_Hishop_ShoppingCart_Data_New"].Expires = new DateTime(1999, 10, 12);
			HttpContext.Current.Response.Cookies["Hid_Hishop_ShoppingCart_Data_New"].Path = "/";
		}

		public ShoppingCartInfo GetShoppingCart(string currentBuyProductckIds = null)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			ShoppingCartInfo shoppingCartInfo = null;
			XmlNodeList xmlNodeList = shoppingCartData.SelectNodes("//sc/lis/l");
			XmlNodeList xmlNodeList2 = shoppingCartData.SelectNodes("//sc/gf/l");
			if ((xmlNodeList != null && xmlNodeList.Count > 0) || (xmlNodeList2 != null && xmlNodeList2.Count > 0))
			{
				shoppingCartInfo = new ShoppingCartInfo();
			}
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				IList<string> list = new List<string>();
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				if (!string.IsNullOrEmpty(currentBuyProductckIds))
				{
					string[] source = currentBuyProductckIds.Split(',');
					foreach (XmlNode item in xmlNodeList)
					{
						if (source.Contains(item.Attributes["s"].Value) || source.Contains(item.Attributes["s"].Value.Split('|')[0]))
						{
							list.Add(item.Attributes["s"].Value);
							dictionary.Add(item.Attributes["s"].Value, int.Parse(item.Attributes["q"].Value));
						}
					}
				}
				else
				{
					foreach (XmlNode item2 in xmlNodeList)
					{
						list.Add(item2.Attributes["s"].Value);
						dictionary.Add(item2.Attributes["s"].Value, int.Parse(item2.Attributes["q"].Value));
					}
				}
				this.LoadCartProduct(shoppingCartInfo, dictionary, list);
			}
			if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
				foreach (XmlNode item3 in xmlNodeList2)
				{
					stringBuilder.AppendFormat("{0},", int.Parse(item3.Attributes["g"].Value));
					dictionary2.Add(int.Parse(item3.Attributes["g"].Value), int.Parse(item3.Attributes["q"].Value));
				}
				this.LoadCartGift(shoppingCartInfo, dictionary2, stringBuilder.ToString());
			}
			return shoppingCartInfo;
		}

		public int GetCartQuantity()
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNodeList xmlNodeList = shoppingCartData.SelectNodes("//sc/lis/l");
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (XmlNode item in xmlNodeList)
			{
				num += int.Parse(item.Attributes["q"].Value);
			}
			return num;
		}

		public void RemoveLineItem(string skuId, int storeId = 0)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@s='" + skuId + "|" + storeId + "']");
			if (xmlNode2 != null)
			{
				xmlNode.RemoveChild(xmlNode2);
				this.SaveShoppingCartData(shoppingCartData);
			}
		}

		public void UpdateLineItemQuantity(string skuId, int quantity, int storeId = 0)
		{
			if (quantity <= 0)
			{
				this.RemoveLineItem(skuId, storeId);
			}
			else
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//lis");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@s='" + skuId + "|" + storeId + "']");
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
					this.SaveShoppingCartData(shoppingCartData);
				}
			}
		}

		public void UpdateShopCartProductGiftsQuantity(string giftIds, int quantity)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			foreach (string item in giftIds.Split(',').ToList())
			{
				if (quantity <= 0)
				{
					this.RemoveGiftItem(item.ToInt(0));
					return;
				}
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g='" + item + "']");
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
				}
			}
			this.SaveShoppingCartData(shoppingCartData);
		}

		public bool AddGiftItem(int giftId, int quantity)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g=" + giftId + "]");
			if (xmlNode2 == null)
			{
				xmlNode2 = CookieShoppingDao.CreateGiftLineItemNode(shoppingCartData, giftId, quantity);
				xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
			}
			else
			{
				xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
			}
			this.SaveShoppingCartData(shoppingCartData);
			return true;
		}

		public bool AddProductPromotionGiftItems(List<int> giftIds)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			foreach (int giftId in giftIds)
			{
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g=" + giftId + "]");
				if (xmlNode2 == null)
				{
					xmlNode2 = CookieShoppingDao.CreateGiftLineItemNode(shoppingCartData, giftId, 1);
					xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
				}
				else
				{
					xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + 1).ToString(CultureInfo.InvariantCulture);
				}
			}
			this.SaveShoppingCartData(shoppingCartData);
			return true;
		}

		public void UpdateGiftItemQuantity(int giftId, int quantity)
		{
			if (quantity <= 0)
			{
				this.RemoveGiftItem(giftId);
			}
			else
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g='" + giftId + "']");
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
					this.SaveShoppingCartData(shoppingCartData);
				}
			}
		}

		public void RemoveGiftItem(int giftId)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("l[@g='" + giftId + "']");
			if (xmlNode2 != null)
			{
				xmlNode.RemoveChild(xmlNode2);
				this.SaveShoppingCartData(shoppingCartData);
			}
		}

		private ShoppingCartItemInfo GetCartItemInfo(string skuId, int quantity)
		{
			int num = 0;
			string[] array = skuId.Split('|');
			if (array.Length == 2)
			{
				skuId = array[0];
				num = array[1].ToInt(0);
			}
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			string empty = string.Empty;
			empty = ((num <= 0) ? "SELECT p.ProductId, SaleStatus, SKU, Stock,  ProductName, CategoryId, [Weight], SalePrice, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,CASE WHEN (p.SaleStatus<>1 OR s.SKU is NULL) THEN 0 ELSE 1 END as IsValid ,isnull(p.SupplierId,0) as SupplierId,isnull(su.SupplierName,'平台') as SupplierName,0 as StoreId,'' as StoreName,0 as IsAboveSelf  FROM Hishop_Products p JOIN Hishop_Skus s ON p.ProductId = s.ProductId left join Hishop_Supplier as su on p.SupplierId=su.SupplierId  WHERE SkuId = @SkuId SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr,(SELECT ISNULL(MAX(Stock),s.Stock) FROM Hishop_StoreSKUs WHERE s.SkuId=Hishop_StoreSKUs.SkuID) as StoreStock FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)" : "SELECT p.ProductId,B.CostPrice, P.SaleStatus, SKU, K.Stock,  ProductName, CategoryId, [Weight],HasSku\r\n                ,(case K.StoreSalePrice when 0 then B.SalePrice else K.StoreSalePrice end)  AS SalePrice,ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220,IsfreeShipping,ShippingTemplateId,CASE WHEN (P.SaleStatus<>1 OR B.SKU is NULL OR K.SkuId is NULL) THEN 0 ELSE 1 END AS IsValid,P.StoreId,StoreName,IsAboveSelf,0 as SupplierId,'' as SupplierName\r\n                FROM Hishop_StoreProducts P \r\n                inner join Hishop_Products A on P.ProductId=A.ProductId\r\n                inner join Hishop_Skus B ON A.ProductId = B.ProductId \r\n                inner join Hishop_StoreSKUs K on B.SkuId=K.SkuId and P.StoreId=K.StoreId\r\n                inner join Hishop_Stores S on P.StoreId=S.StoreId\r\n                WHERE P.StoreId=@StoreId AND K.SkuId = @SkuId; SELECT s.SkuId, s.SKU, s.ProductId, k.Stock, AttributeName, ValueStr\r\n                    FROM Hishop_SKUs s inner join Hishop_StoreSKUs k on s.SkuId=k.SkuId left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId\r\n                    WHERE k.SkuId = @SkuId and StoreId=@StoreId AND k.ProductId IN (SELECT ProductId FROM Hishop_StoreProducts WHERE SaleStatus=1)");
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand(empty);
			base.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
			base.database.AddInParameter(sqlStringCommand, "StoreId", DbType.Int32, num);
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					shoppingCartItemInfo.ProductId = (int)((IDataRecord)dataReader)["ProductId"];
					shoppingCartItemInfo.SupplierId = ((IDataRecord)dataReader)["SupplierId"].ToInt(0);
					shoppingCartItemInfo.SupplierName = ((IDataRecord)dataReader)["SupplierName"].ToString();
					shoppingCartItemInfo.StoreId = ((IDataRecord)dataReader)["StoreId"].ToInt(0);
					shoppingCartItemInfo.StoreName = ((IDataRecord)dataReader)["StoreName"].ToString();
					shoppingCartItemInfo.HasStore = ((IDataRecord)dataReader)["IsAboveSelf"].ToBool();
					shoppingCartItemInfo.Name = ((IDataRecord)dataReader)["ProductName"].ToString();
					if (DBNull.Value != ((IDataRecord)dataReader)["Weight"])
					{
						shoppingCartItemInfo.Weight = (decimal)((IDataRecord)dataReader)["Weight"];
					}
					else
					{
						shoppingCartItemInfo.Weight = decimal.Zero;
					}
					ShoppingCartItemInfo shoppingCartItemInfo2 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo3 = shoppingCartItemInfo;
					decimal num4 = shoppingCartItemInfo2.MemberPrice = (shoppingCartItemInfo3.AdjustedPrice = (decimal)((IDataRecord)dataReader)["SalePrice"]);
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = ((IDataRecord)dataReader)["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = ((IDataRecord)dataReader)["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = ((IDataRecord)dataReader)["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != ((IDataRecord)dataReader)["ThumbnailUrl180"])
					{
						shoppingCartItemInfo.ThumbnailUrl180 = ((IDataRecord)dataReader)["ThumbnailUrl180"].ToString();
					}
					if (((IDataRecord)dataReader)["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)((IDataRecord)dataReader)["SKU"];
					}
					ShoppingCartItemInfo shoppingCartItemInfo4 = shoppingCartItemInfo;
					ShoppingCartItemInfo shoppingCartItemInfo5 = shoppingCartItemInfo;
					int num7 = shoppingCartItemInfo4.Quantity = (shoppingCartItemInfo5.ShippQuantity = quantity);
					if (DBNull.Value != ((IDataRecord)dataReader)["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(((IDataRecord)dataReader)["IsfreeShipping"]);
					}
					shoppingCartItemInfo.ShippingTemplateId = ((IDataRecord)dataReader)["ShippingTemplateId"].ToInt(0);
					shoppingCartItemInfo.IsValid = (!(((IDataRecord)dataReader)["IsValid"].ToString() == "0") && true);
					int num8 = ((IDataRecord)dataReader)["Stock"].ToInt(0);
					shoppingCartItemInfo.HasEnoughStock = (num8 > 0 && num8 >= quantity && true);
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (((IDataRecord)dataReader)["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["AttributeName"]) && ((IDataRecord)dataReader)["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)((IDataRecord)dataReader)["ValueStr"]))
							{
								text = text + ((IDataRecord)dataReader)["AttributeName"] + "：" + ((IDataRecord)dataReader)["ValueStr"] + "; ";
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
				}
			}
			return shoppingCartItemInfo;
		}

		private void LoadCartGift(ShoppingCartInfo cartInfo, Dictionary<int, int> giftQuantityList, string giftIds)
		{
			DbCommand sqlStringCommand = base.database.GetSqlStringCommand($"SELECT * FROM Hishop_Gifts WHERE GiftId in {giftIds.TrimEnd(',')}");
			using (IDataReader dataReader = base.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
					shoppingCartGiftInfo.Quantity = giftQuantityList[shoppingCartGiftInfo.GiftId];
					cartInfo.LineGifts.Add(shoppingCartGiftInfo);
				}
			}
		}

		private void LoadCartProduct(ShoppingCartInfo cartInfo, Dictionary<string, int> productQuantityList, IList<string> skuIds)
		{
			foreach (string skuId in skuIds)
			{
				ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(skuId, productQuantityList[skuId]);
				if (cartItemInfo != null)
				{
					cartInfo.LineItems.Add(cartItemInfo);
				}
			}
		}

		private int GetShoppingProductQuantity(string skuId, int ProductId)
		{
			int result = 0;
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis/l[SkuId='" + skuId + "' AND p=" + ProductId + "]");
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.Attributes["q"].Value, out result);
			}
			return result;
		}

		private XmlDocument GetShoppingCartData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Hid_Hishop_ShoppingCart_Data_New"];
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				xmlDocument = CookieShoppingDao.CreateEmptySchema();
			}
			else
			{
				try
				{
					xmlDocument.LoadXml(Globals.UrlDecode(httpCookie.Value));
				}
				catch
				{
					this.ClearShoppingCart();
					xmlDocument = CookieShoppingDao.CreateEmptySchema();
				}
			}
			return xmlDocument;
		}

		private void SaveShoppingCartData(XmlDocument doc)
		{
			if (doc == null)
			{
				this.ClearShoppingCart();
			}
			else
			{
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Hid_Hishop_ShoppingCart_Data_New"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("Hid_Hishop_ShoppingCart_Data_New");
				}
				httpCookie.HttpOnly = true;
				httpCookie.Value = Globals.UrlEncode(doc.OuterXml);
				httpCookie.Expires = DateTime.Now.AddDays(3.0);
				HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
		}

		private static XmlDocument CreateEmptySchema()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<sc><lis></lis><gf></gf></sc>");
			return xmlDocument;
		}

		private static XmlNode CreateLineItemNode(XmlDocument doc, string skuId, int quantity)
		{
			XmlNode xmlNode = doc.CreateElement("l");
			XmlNode xmlNode2 = doc.SelectSingleNode("//lis");
			XmlAttribute xmlAttribute = doc.CreateAttribute("s");
			xmlAttribute.Value = skuId;
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("q");
			xmlAttribute2.Value = quantity.ToString(CultureInfo.InvariantCulture);
			xmlNode.Attributes.Append(xmlAttribute);
			xmlNode.Attributes.Append(xmlAttribute2);
			return xmlNode;
		}

		private static XmlNode CreateGiftLineItemNode(XmlDocument doc, int giftId, int quantity)
		{
			XmlNode xmlNode = doc.CreateElement("l");
			XmlNode xmlNode2 = doc.SelectSingleNode("//gf");
			XmlAttribute xmlAttribute = doc.CreateAttribute("q");
			xmlAttribute.Value = quantity.ToString(CultureInfo.InvariantCulture);
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("g");
			xmlAttribute2.Value = giftId.ToString();
			xmlNode.Attributes.Append(xmlAttribute);
			xmlNode.Attributes.Append(xmlAttribute2);
			return xmlNode;
		}

		private static int GenerateLastItemId(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("/sc");
			XmlAttribute xmlAttribute = xmlNode.Attributes["lid"];
			int result;
			if (xmlAttribute == null)
			{
				xmlAttribute = doc.CreateAttribute("lid");
				xmlNode.Attributes.Append(xmlAttribute);
				result = 1;
			}
			else
			{
				result = int.Parse(xmlAttribute.Value) + 1;
			}
			xmlAttribute.Value = result.ToString(CultureInfo.InvariantCulture);
			return result;
		}
	}
}
