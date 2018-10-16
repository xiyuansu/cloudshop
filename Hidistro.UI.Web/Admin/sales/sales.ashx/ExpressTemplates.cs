using Hidistro.Core;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class ExpressTemplates : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setyesorno":
				this.SetYesOrNo(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
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
			DataTable expressTemplates = SalesHelper.GetExpressTemplates();
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(expressTemplates);
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			string parameter = base.GetParameter(context, "name", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (SalesHelper.DeleteExpressTemplate(value))
			{
				string text = HttpContext.Current.Request.MapPath($"/Storage/master/flex/{parameter}");
				if (File.Exists(text))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(text);
					XmlNode xmlNode = xmlDocument.SelectSingleNode("printer/pic");
					string path = HttpContext.Current.Request.MapPath($"/Storage/master/flex/{xmlNode.InnerText}");
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					File.Delete(text);
				}
				base.ReturnSuccessResult(context, "已经成功删除选择的快递单模板", 0, true);
				return;
			}
			throw new HidistroAshxException("删除快递单模板失败");
		}

		public void SetYesOrNo(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			SalesHelper.SetExpressIsUse(value);
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}
	}
}
