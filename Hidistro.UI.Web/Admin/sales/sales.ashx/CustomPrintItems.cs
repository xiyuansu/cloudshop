using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class CustomPrintItems : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "delete")
				{
					this.Delete(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/DataItems/Item");
			foreach (XmlNode item in xmlNodeList)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("Name", item.ChildNodes[0].InnerText.Trim());
				dictionary.Add("Content", item.ChildNodes[1].InnerText.Trim());
				dataGridViewModel.rows.Add(dictionary);
			}
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string parameter = base.GetParameter(context, "name", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("错误的参数");
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/DataItems/Item");
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/DataItems");
			foreach (XmlNode item in xmlNodeList)
			{
				string text = item.ChildNodes[0].InnerText.Trim();
				if (text.Equals(parameter.Trim()))
				{
					xmlNode.RemoveChild(item);
				}
			}
			xmlDocument.Save(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
			base.ReturnSuccessResult(context, "删除成功", 0, true);
		}
	}
}
