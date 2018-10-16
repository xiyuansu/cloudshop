using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Web.ashxBase;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	public class EditSpecificationValues : AdminBaseHandler
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
			case "editvalue":
				this.EditValue(context);
				break;
			case "addvalue":
				this.AddValue(context);
				break;
			case "setorder":
				this.SetOrder(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			int value = base.GetIntParam(context, "attributeId", false).Value;
			DataGridViewModel<AttributeValueInfo> dataList = this.GetDataList(value);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<AttributeValueInfo> GetDataList(int attributeId)
		{
			DataGridViewModel<AttributeValueInfo> dataGridViewModel = new DataGridViewModel<AttributeValueInfo>();
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
			dataGridViewModel.rows = attribute.AttributeValues.ToList();
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			foreach (AttributeValueInfo row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "ids", false).Value;
			if (ProductTypeHelper.DeleteAttributeValue(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！未知错误！");
		}

		private void EditValue(HttpContext context)
		{
			int value = base.GetIntParam(context, "valueId", false).Value;
			string parameter = base.GetParameter(context, "OldValue", false);
			string text = Globals.StripHtmlXmlTags(Globals.StripScriptTags(parameter.Trim().Replace("+", "").Replace("\\", "")));
			if (text.Length > 15 || text.Length == 0)
			{
				throw new HidistroAshxException("属性值必须小于15个字符，不能为空,且不能包含脚本标签、HTML标签、XML标签以及\\+！");
			}
			AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(value);
			attributeValueInfo.ValueStr = text;
			if (ProductTypeHelper.UpdateAttributeValue(attributeValueInfo))
			{
				base.ReturnSuccessResult(context, "修改成功！", 0, true);
			}
		}

		private void AddValue(HttpContext context)
		{
			int value = base.GetIntParam(context, "attributeId", false).Value;
			string parameter = base.GetParameter(context, "Value", false);
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			string text = Globals.StripHtmlXmlTags(Globals.StripScriptTags(parameter.Trim().Replace("+", "").Replace("\\", "")));
			if (text.Length > 15 || text.Length == 0)
			{
				throw new HidistroAshxException("属性值必须小于15个字符，不能为空,且不能包含脚本标签、HTML标签、XML标签以及\\+！");
			}
			attributeValueInfo.ValueStr = text;
			attributeValueInfo.AttributeId = value;
			if (ProductTypeHelper.AddAttributeValue(attributeValueInfo) > 0)
			{
				base.ReturnSuccessResult(context, "添加成功！", 0, true);
			}
		}

		private void SetOrder(HttpContext context)
		{
			string parameter = base.GetParameter(context, "Type", false);
			int num = base.GetIntParam(context, "DisplaySequence", false).Value;
			int value = base.GetIntParam(context, "AttributeId", false).Value;
			int value2 = base.GetIntParam(context, "replaceAttributeId", false).Value;
			AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(value2);
			int num2 = attributeValueInfo.DisplaySequence;
			if (parameter == "Fall")
			{
				if (num2 == num)
				{
					num++;
				}
			}
			else if (num2 == num)
			{
				num2++;
			}
			ProductTypeHelper.SwapAttributeValueSequence(value, value2, num, num2);
			base.ReturnSuccessResult(context, "", 0, true);
		}
	}
}
