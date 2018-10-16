using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class DesigProduct : AdminPage, IHttpHandler
	{
		private string message = "";

		private string modeId = "";

		private string elementId = "";

		private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";

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
				string text = "";
				string text2 = "";
				string text3 = "";
				switch (this.modeId)
				{
				case "simpleview":
					this.message = string.Format(this.resultformat, "true", this.GetSimpleProductView());
					break;
				case "editesimple":
				{
					string text7 = context.Request.Form["Param"];
					if (text7 != "")
					{
						JObject simpleobject = (JObject)JsonConvert.DeserializeObject(text7);
						if (this.CheckSimpleProduct(simpleobject) && this.UpdateSimpleProduct(simpleobject))
						{
							Common_SubjectProduct_Simple common_SubjectProduct_Simple = new Common_SubjectProduct_Simple();
							common_SubjectProduct_Simple.SubjectId = Convert.ToInt32(this.elementId);
							var value4 = new
							{
								Simple = common_SubjectProduct_Simple.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value4));
						}
					}
					break;
				}
				case "producttabview":
				{
					text = this.GetCategorys();
					text2 = this.GetProductBrand();
					text3 = this.GetProductTags();
					string arg3 = "{\"Categorys\":" + text + ",\"Brands\":" + text2 + ",\"Tags\":" + text3 + "}";
					this.message = string.Format(this.resultformat, "true", arg3);
					break;
				}
				case "editeproducttab":
				{
					string text5 = context.Request.Form["Param"];
					if (text5 != "")
					{
						JObject jObject = (JObject)JsonConvert.DeserializeObject(text5);
						if (this.CheckProductTab(jObject) && this.UpdateProductTab(jObject))
						{
							Common_SubjectProduct_Tab common_SubjectProduct_Tab = new Common_SubjectProduct_Tab();
							common_SubjectProduct_Tab.SubjectId = Convert.ToInt32(this.elementId);
							var value2 = new
							{
								ProductTab = common_SubjectProduct_Tab.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value2));
						}
					}
					break;
				}
				case "productfloorview":
				{
					text = this.GetCategorys();
					text2 = this.GetProductBrand();
					text3 = this.GetProductTags();
					string arg = "{\"Categorys\":" + text + ",\"Brands\":" + text2 + ",\"Tags\":" + text3 + "}";
					this.message = string.Format(this.resultformat, "true", arg);
					break;
				}
				case "editeproductfloor":
				{
					string text6 = context.Request.Form["Param"];
					if (text6 != "")
					{
						JObject floorobject = (JObject)JsonConvert.DeserializeObject(text6);
						if (this.CheckProductFloor(floorobject) && this.UpdateProductFloor(floorobject))
						{
							Common_SubjectProduct_Floor common_SubjectProduct_Floor = new Common_SubjectProduct_Floor();
							common_SubjectProduct_Floor.SubjectId = Convert.ToInt32(this.elementId);
							var value3 = new
							{
								ProductFloor = common_SubjectProduct_Floor.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value3));
						}
					}
					break;
				}
				case "producttopview":
				{
					text = this.GetCategorys();
					string arg2 = "{\"Categorys\":" + text + "}";
					this.message = string.Format(this.resultformat, "true", arg2);
					break;
				}
				case "editeproducttop":
				{
					string text4 = context.Request.Form["Param"];
					if (text4 != "")
					{
						JObject topobject = (JObject)JsonConvert.DeserializeObject(text4);
						if (this.CheckProductTop(topobject) && this.UpdateProductTop(topobject))
						{
							Common_SubjectProduct_Top common_SubjectProduct_Top = new Common_SubjectProduct_Top();
							common_SubjectProduct_Top.SubjectId = Convert.ToInt32(this.elementId);
							var value = new
							{
								ProductTop = common_SubjectProduct_Top.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value));
						}
					}
					break;
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

		public Dictionary<string, string> GetXmlNodeString(JObject scriptobject)
		{
			return scriptobject.ToObject<Dictionary<string, string>>().ToDictionary((KeyValuePair<string, string> s) => s.Key, (KeyValuePair<string, string> s) => Globals.HtmlEncode(DataHelper.CleanSearchString(s.Value.ToString())));
		}

		private string GetSimpleProductView()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			IList<ProductTypeInfo> list = null;
			string text4 = "[]";
			IList<AttributeInfo> list2 = null;
			string text5 = "[]";
			text = this.GetProductBrand();
			text2 = this.GetCategorys();
			text3 = this.GetProductTags();
			list = this.GetProductTypeList();
			list2 = ProductTypeHelper.GetAttributes(AttributeUseageMode.MultiView);
			if (list != null)
			{
				text4 = JsonConvert.SerializeObject(list);
			}
			if (list2.Count > 0)
			{
				text5 = JsonConvert.SerializeObject(list2);
			}
			return "{\"Categorys\":" + text2 + ",\"Brands\":" + text + ",\"Tags\":" + text3 + ",\"ProductTypes\":" + text4 + ",\"Attributes\":" + text5 + "}";
		}

		private bool CheckSimpleProduct(JObject simpleobject)
		{
			if (string.IsNullOrEmpty(simpleobject["MaxNum"].ToString()) || Convert.ToInt16(simpleobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(simpleobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(simpleobject["ImageSize"].ToString()) || Convert.ToInt16(simpleobject["ImageSize"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
				return false;
			}
			if (string.IsNullOrEmpty(simpleobject["SubjectId"].ToString()) || simpleobject["SubjectId"].ToString().Split('_').Length != 2)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			return true;
		}

		private bool UpdateSimpleProduct(JObject simpleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改商品列表失败\"");
			this.elementId = simpleobject["SubjectId"].ToString().Split('_')[1];
			simpleobject["SubjectId"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(simpleobject);
			return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "simple", xmlNodeString);
		}

		private bool CheckProductTab(JObject tabobject)
		{
			if (string.IsNullOrEmpty(tabobject["MaxNum"].ToString()) || Convert.ToInt16(tabobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(tabobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(tabobject["ImageSize"].ToString()) || Convert.ToInt16(tabobject["ImageSize"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
				return false;
			}
			if (string.IsNullOrEmpty(tabobject["SubjectId"].ToString()) || tabobject["SubjectId"].ToString().Split('_').Length != 2)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			return true;
		}

		private bool UpdateProductTab(JObject producttabobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改商品选项卡失败\"");
			this.elementId = producttabobject["SubjectId"].ToString().Split('_')[1];
			producttabobject["SubjectId"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(producttabobject);
			return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "tab", xmlNodeString);
		}

		private bool CheckProductFloor(JObject floorobject)
		{
			if (string.IsNullOrEmpty(floorobject["MaxNum"].ToString()) || Convert.ToInt16(floorobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(floorobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (!string.IsNullOrEmpty(floorobject["SubCategoryNum"].ToString()) && (Convert.ToInt16(floorobject["SubCategoryNum"].ToString()) < 0 || Convert.ToInt16(floorobject["MaxNum"].ToString()) > 100))
			{
				this.message = string.Format(this.resultformat, "false", "\"子类显示数量必须为0~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(floorobject["ImageSize"].ToString()) || Convert.ToInt16(floorobject["ImageSize"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"图片规格不允许为空！\"");
				return false;
			}
			if (string.IsNullOrEmpty(floorobject["SubjectId"].ToString()) || floorobject["SubjectId"].ToString().Split('_').Length != 2)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			if (string.IsNullOrEmpty(floorobject["Title"].ToString()) && string.IsNullOrEmpty(floorobject["ImageTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请上传标题图片或输入楼层标题\"");
				return false;
			}
			return true;
		}

		private bool UpdateProductFloor(JObject floorobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改商品楼层失败\"");
			this.elementId = floorobject["SubjectId"].ToString().Split('_')[1];
			floorobject["SubjectId"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(floorobject);
			return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "floor", xmlNodeString);
		}

		private bool CheckProductTop(JObject topobject)
		{
			if (string.IsNullOrEmpty(topobject["MaxNum"].ToString()) || Convert.ToInt16(topobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(topobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(topobject["ImageNum"].ToString()) || Convert.ToInt16(topobject["ImageNum"].ToString()) < 0 || Convert.ToInt16(topobject["ImageNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"图片显示数量必须为1~100的正整数！\"");
				return false;
			}
			if (string.IsNullOrEmpty(topobject["ImageSize"].ToString()) || Convert.ToInt16(topobject["ImageSize"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品图片规格不允许为空！\"");
				return false;
			}
			return true;
		}

		private bool UpdateProductTop(JObject topobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改销售排行榜失败\"");
			this.elementId = topobject["SubjectId"].ToString().Split('_')[1];
			topobject["SubjectId"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(topobject);
			return TagsHelper.UpdateProductNode(Convert.ToInt16(this.elementId), "top", xmlNodeString);
		}

		private string GetProductBrand()
		{
			DataTable dataTable = null;
			string result = "";
			int index = 2;
			dataTable = CatalogHelper.GetBrandCategories(0).Copy();
			if (dataTable != null)
			{
				do
				{
					dataTable.Columns.RemoveAt(index);
				}
				while (dataTable.Columns.Count > 2);
			}
			if (dataTable != null)
			{
				result = JsonConvert.SerializeObject(dataTable, new ConvertTojson());
			}
			return result;
		}

		private string GetCategorys()
		{
			DataTable dataTable = null;
			string result = "";
			string[] propertyName = new string[3]
			{
				"CategoryId",
				"Name",
				"Depth"
			};
			dataTable = this.ConvertListToDataTable(CatalogHelper.GetSequenceCategories("").ToList(), propertyName);
			if (dataTable != null)
			{
				result = JsonConvert.SerializeObject(dataTable, new ConvertTojson());
			}
			return result;
		}

		private DataTable ConvertListToDataTable<T>(IList<T> list, params string[] propertyName)
		{
			List<string> list2 = new List<string>();
			if (propertyName != null)
			{
				list2.AddRange((IEnumerable<string>)propertyName);
			}
			DataTable dataTable = new DataTable();
			if (((ICollection<T>)list).Count > 0)
			{
				PropertyInfo[] properties = list[0].GetType().GetProperties();
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					if (list2.Count == 0)
					{
						dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
					}
					else if (list2.Contains(propertyInfo.Name))
					{
						dataTable.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
					}
				}
				for (int j = 0; j < ((ICollection<T>)list).Count; j++)
				{
					ArrayList arrayList = new ArrayList();
					PropertyInfo[] array2 = properties;
					foreach (PropertyInfo propertyInfo2 in array2)
					{
						if (list2.Count == 0)
						{
							object value = propertyInfo2.GetValue(list[j], null);
							arrayList.Add(value);
						}
						else if (list2.Contains(propertyInfo2.Name))
						{
							object value2 = propertyInfo2.GetValue(list[j], null);
							arrayList.Add(value2);
						}
					}
					object[] values = arrayList.ToArray();
					dataTable.LoadDataRow(values, true);
				}
			}
			return dataTable;
		}

		private string GetProductTags()
		{
			DataTable dataTable = null;
			string[] propertyName = new string[2]
			{
				"TagID",
				"TagName"
			};
			IList<TagInfo> tags = CatalogHelper.GetTags();
			dataTable = this.ConvertListToDataTable(tags, propertyName);
			string result = "";
			if (dataTable != null)
			{
				result = JsonConvert.SerializeObject(dataTable, new ConvertTojson());
			}
			return result;
		}

		private IList<ProductTypeInfo> GetProductTypeList()
		{
			return ProductTypeHelper.GetProductTypes();
		}

		public string GetXmlPath(string xmlname)
		{
			if (xmlname != "")
			{
				return Globals.PhysicalPath(HiContext.Current.GetSkinPath() + "/config/" + xmlname + ".xml");
			}
			return null;
		}
	}
}
