using Hidistro.Core;
using Hidistro.UI.Web.Admin.store.models;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class SetHeaderMenu : AdminBaseHandler
	{
		private string themName;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			this.themName = base.CurrentSiteSetting.Theme;
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setshow":
				this.SetShow(context);
				break;
			case "setshownum":
				this.SetShowNum(context);
				break;
			case "sort":
				this.Sort(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			SetHeaderMenuGetList dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private SetHeaderMenuGetList GetDataList()
		{
			SetHeaderMenuGetList setHeaderMenuGetList = new SetHeaderMenuGetList();
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Id", typeof(int));
			dataTable.Columns.Add("Title");
			dataTable.Columns.Add("DisplaySequence", typeof(int));
			dataTable.Columns.Add("Url");
			dataTable.Columns.Add("Category");
			dataTable.Columns.Add("Visible");
			XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			setHeaderMenuGetList.CategoryNum = xmlNode.Attributes["CategoryNum"].Value.ToInt(0);
			XmlNodeList childNodes = xmlNode.ChildNodes;
			string empty = string.Empty;
			foreach (XmlNode item in childNodes)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["Id"] = int.Parse(item.Attributes["Id"].Value);
				dataRow["Title"] = item.Attributes["Title"].Value;
				dataRow["DisplaySequence"] = int.Parse(item.Attributes["DisplaySequence"].Value);
				dataRow["Category"] = item.Attributes["Category"].Value;
				dataRow["Url"] = item.Attributes["Url"].Value;
				dataRow["Visible"] = item.Attributes["Visible"].Value;
				dataTable.Rows.Add(dataRow);
			}
			dataTable.DefaultView.Sort = "DisplaySequence Desc";
			setHeaderMenuGetList.rows = DataHelper.DataTableToDictionary(dataTable.DefaultView.ToTable());
			foreach (Dictionary<string, object> row in setHeaderMenuGetList.rows)
			{
				string a = row["Category"].ToString();
				a = ((!(a == "1")) ? ((!(a == "2")) ? "自定义链接" : "商品搜索链接") : "系统页面");
				row.Add("CategoryName", a);
			}
			setHeaderMenuGetList.total = setHeaderMenuGetList.rows.Count;
			return setHeaderMenuGetList;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			XmlNodeList childNodes = xmlNode.ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				if (item.Attributes["Id"].Value == value.ToString())
				{
					xmlNode.RemoveChild(item);
					break;
				}
			}
			xmlDocument.Save(filename);
			new AspNetCache().Remove("HeadMenuFileCache-Admin");
			base.ReturnSuccessResult(context, "删除成功", 0, true);
		}

		public void SetShow(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				if (item.Attributes["Id"].Value == value.ToString())
				{
					if (item.Attributes["Visible"].Value == "true")
					{
						item.Attributes["Visible"].Value = "false";
					}
					else
					{
						item.Attributes["Visible"].Value = "true";
					}
					break;
				}
			}
			xmlDocument.Save(filename);
			new AspNetCache().Remove("HeadMenuFileCache-Admin");
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}

		public void Sort(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "sort", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				if (item.Attributes["Id"].Value == value.ToString())
				{
					item.Attributes["DisplaySequence"].Value = value2.ToString();
					break;
				}
			}
			xmlDocument.Save(filename);
			base.ReturnSuccessResult(context, "更新排序成功", 0, true);
		}

		public void SetShowNum(HttpContext context)
		{
			int value = base.GetIntParam(context, "num", false).Value;
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			xmlNode.Attributes["CategoryNum"].Value = value.ToString();
			xmlDocument.Save(filename);
			new AspNetCache().Remove("HeadMenuFileCache-Admin");
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}
	}
}
