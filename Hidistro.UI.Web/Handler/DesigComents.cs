using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
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
	public class DesigComents : IHttpHandler
	{
		private string message = "";

		private string modeId = "";

		private string elementId = "";

		private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = "";
				this.modeId = context.Request.Form["ModelId"];
				switch (this.modeId)
				{
				case "commentarticleview":
					text = this.GetMainArticleCategories();
					this.message = string.Format(this.resultformat, "true", text);
					break;
				case "commentCategory":
					text = this.GetCategorys();
					this.message = string.Format(this.resultformat, "true", text);
					break;
				case "editecommentarticle":
				{
					string value7 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value7))
					{
						JObject articleobject = (JObject)JsonConvert.DeserializeObject(value7);
						if (this.CheckCommentArticle(articleobject) && this.UpdateCommentArticle(articleobject))
						{
							Common_SubjectArticle common_SubjectArticle = new Common_SubjectArticle();
							common_SubjectArticle.CommentId = Convert.ToInt32(this.elementId);
							var value8 = new
							{
								ComArticle = common_SubjectArticle.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value8));
						}
					}
					break;
				}
				case "editecommentcategory":
				{
					string value3 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value3))
					{
						JObject categoryobject = (JObject)JsonConvert.DeserializeObject(value3);
						if (this.CheckCommentCategory(categoryobject) && this.UpdateCommentCategory(categoryobject))
						{
							Common_SubjectCategory common_SubjectCategory = new Common_SubjectCategory();
							common_SubjectCategory.CommentId = Convert.ToInt32(this.elementId);
							var value4 = new
							{
								ComCategory = common_SubjectCategory.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value4));
						}
					}
					break;
				}
				case "editecommentbrand":
				{
					string value5 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value5))
					{
						JObject jObject = (JObject)JsonConvert.DeserializeObject(value5);
						if (this.CheckCommentBrand(jObject) && this.UpdateCommentBrand(jObject))
						{
							Common_SubjectBrand common_SubjectBrand = new Common_SubjectBrand();
							common_SubjectBrand.CommentId = Convert.ToInt32(this.elementId);
							var value6 = new
							{
								ComBrand = common_SubjectBrand.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value6));
						}
					}
					break;
				}
				case "editecommentkeyword":
				{
					string value11 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value11))
					{
						JObject keywordobject = (JObject)JsonConvert.DeserializeObject(value11);
						if (this.CheckCommentKeyWord(keywordobject) && this.UpdateCommentKeyWord(keywordobject))
						{
							Common_SubjectKeyword common_SubjectKeyword = new Common_SubjectKeyword();
							common_SubjectKeyword.CommentId = Convert.ToInt32(this.elementId);
							var value12 = new
							{
								ComCategory = common_SubjectKeyword.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value12));
						}
					}
					break;
				}
				case "editecommentattribute":
				{
					string value13 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value13))
					{
						JObject attributeobject = (JObject)JsonConvert.DeserializeObject(value13);
						if (this.CheckCommentAttribute(attributeobject) && this.UpdateCommentAttribute(attributeobject))
						{
							Common_SubjectAttribute common_SubjectAttribute = new Common_SubjectAttribute();
							common_SubjectAttribute.CommentId = Convert.ToInt32(this.elementId);
							var value14 = new
							{
								ComAttribute = common_SubjectAttribute.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value14));
						}
					}
					break;
				}
				case "editecommenttitle":
				{
					string value9 = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value9))
					{
						JObject titleobject = (JObject)JsonConvert.DeserializeObject(value9);
						if (this.CheckCommentTitle(titleobject) && this.UpdateCommentTitle(titleobject))
						{
							Common_SubjectTitle common_SubjectTitle = new Common_SubjectTitle();
							common_SubjectTitle.CommentId = Convert.ToInt32(this.elementId);
							var value10 = new
							{
								ComTitle = common_SubjectTitle.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value10));
						}
					}
					break;
				}
				case "editecommentmorelink":
				{
					string value = context.Request.Form["Param"];
					if (!string.IsNullOrEmpty(value))
					{
						JObject morelinkobject = (JObject)JsonConvert.DeserializeObject(value);
						if (this.CheckCommentMorelink(morelinkobject) && this.UpdateMorelink(morelinkobject))
						{
							Common_SubjectMoreLink common_SubjectMoreLink = new Common_SubjectMoreLink();
							common_SubjectMoreLink.CommentId = Convert.ToInt32(this.elementId);
							var value2 = new
							{
								ComMoreLink = common_SubjectMoreLink.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value2));
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
			Dictionary<string, string> dictionary = scriptobject.ToObject<Dictionary<string, string>>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				dictionary2.Add(item.Key, Globals.HtmlEncode(item.Value.ToString()));
			}
			return dictionary2;
		}

		private bool CheckCommentArticle(JObject articleobject)
		{
			if (string.IsNullOrEmpty(articleobject["Title"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入文字标题!\"");
				return false;
			}
			if (string.IsNullOrEmpty(articleobject["MaxNum"].ToString()) || Convert.ToInt16(articleobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(articleobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为1~100的正整数！\"");
				return false;
			}
			return true;
		}

		private bool UpdateCommentArticle(JObject articleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改文章标签列表失败\"");
			this.elementId = articleobject["Id"].ToString().Split('_')[1];
			articleobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(articleobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "article", xmlNodeString);
		}

		private bool CheckCommentCategory(JObject categoryobject)
		{
			if (string.IsNullOrEmpty(categoryobject["CategoryId"].ToString()) || Convert.ToInt16(categoryobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
				return false;
			}
			if (string.IsNullOrEmpty(categoryobject["MaxNum"].ToString()) || Convert.ToInt16(categoryobject["MaxNum"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
				return false;
			}
			return true;
		}

		private bool UpdateCommentCategory(JObject categoryobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改分类标签列表失败\"");
			this.elementId = categoryobject["Id"].ToString().Split('_')[1];
			categoryobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(categoryobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "category", xmlNodeString);
		}

		private bool CheckCommentAttribute(JObject attributeobject)
		{
			if (string.IsNullOrEmpty(attributeobject["CategoryId"].ToString()) || Convert.ToInt16(attributeobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类!\"");
				return false;
			}
			if (string.IsNullOrEmpty(attributeobject["MaxNum"].ToString()) || Convert.ToInt16(attributeobject["MaxNum"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"商品数量必须为正整数！\"");
				return false;
			}
			return true;
		}

		private bool UpdateCommentAttribute(JObject attributeobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改属性标签列表失败\"");
			this.elementId = attributeobject["Id"].ToString().Split('_')[1];
			attributeobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "attribute", xmlNodeString);
		}

		private bool CheckCommentBrand(JObject brandobject)
		{
			if (!string.IsNullOrEmpty(brandobject["CategoryId"].ToString()) && Convert.ToInt16(brandobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(brandobject["IsShowLogo"].ToString()) || string.IsNullOrEmpty(brandobject["IsShowTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"参数格式不正确!\"");
				return false;
			}
			if (string.IsNullOrEmpty(brandobject["MaxNum"].ToString()) || Convert.ToInt16(brandobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(brandobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"显示数量必须为0~100的正整数！\"");
				return false;
			}
			return true;
		}

		private bool UpdateCommentBrand(JObject attributeobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = attributeobject["Id"].ToString().Split('_')[1];
			attributeobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(attributeobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "brand", xmlNodeString);
		}

		private bool CheckCommentKeyWord(JObject keywordobject)
		{
			if (!string.IsNullOrEmpty(keywordobject["CategoryId"].ToString()) && Convert.ToInt16(keywordobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(keywordobject["MaxNum"].ToString()) || Convert.ToInt16(keywordobject["MaxNum"].ToString()) <= 0 || Convert.ToInt16(keywordobject["MaxNum"].ToString()) > 100)
			{
				this.message = string.Format(this.resultformat, "false", "\"显示数量必须为1~100的正整数！\"");
				return false;
			}
			return true;
		}

		private bool UpdateCommentKeyWord(JObject keywordobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = keywordobject["Id"].ToString().Split('_')[1];
			keywordobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(keywordobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "keyword", xmlNodeString);
		}

		private bool CheckCommentMorelink(JObject morelinkobject)
		{
			if (!string.IsNullOrEmpty(morelinkobject["CategoryId"].ToString()) && Convert.ToInt16(morelinkobject["CategoryId"].ToString()) <= 0)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择商品分类！\"");
				return false;
			}
			if (string.IsNullOrEmpty(morelinkobject["Title"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入链接标题！\"");
				return false;
			}
			return true;
		}

		private bool UpdateMorelink(JObject morelinkobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = morelinkobject["Id"].ToString().Split('_')[1];
			morelinkobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(morelinkobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "morelink", xmlNodeString);
		}

		private bool CheckCommentTitle(JObject titleobject)
		{
			if (string.IsNullOrEmpty(titleobject["Title"].ToString()) && string.IsNullOrEmpty(titleobject["ImageTitle"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请输入标题或上传图片！\"");
				return false;
			}
			return true;
		}

		private bool UpdateCommentTitle(JObject titleobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改品牌标签列表失败\"");
			this.elementId = titleobject["Id"].ToString().Split('_')[1];
			titleobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(titleobject);
			return TagsHelper.UpdateCommentNode(Convert.ToInt16(this.elementId), "title", xmlNodeString);
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

		private string GetMainArticleCategories()
		{
			IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
			return JsonConvert.SerializeObject(articleMainCategories);
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
	}
}
