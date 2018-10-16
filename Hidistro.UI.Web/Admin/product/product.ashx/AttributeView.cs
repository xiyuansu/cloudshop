using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Hishop.Components.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AttributeView : AdminBaseHandler
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
			case "addsku":
				this.AddSku(context);
				break;
			case "addvalue":
				this.AddValue(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "deletevalue":
				this.DeleteValue(context);
				break;
			case "ismulti":
				this.IsMulti(context);
				break;
			case "editname":
				this.EditName(context);
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
			int value = base.GetIntParam(context, "typeId", false).Value;
			DataGridViewModel<AttributeInfo> dataList = this.GetDataList(value);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<AttributeInfo> GetDataList(int typeId)
		{
			DataGridViewModel<AttributeInfo> dataGridViewModel = new DataGridViewModel<AttributeInfo>();
			IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.View);
			dataGridViewModel.rows = attributes.ToList();
			dataGridViewModel.total = attributes.Count;
			foreach (AttributeInfo row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void AddSku(HttpContext context)
		{
			int value = base.GetIntParam(context, "typeId", false).Value;
			bool value2 = base.GetBoolParam(context, "UseAttributeImage", false).Value;
			string parameter = base.GetParameter(context, "SkuName", false);
			AttributeInfo attributeInfo = new AttributeInfo();
			attributeInfo.TypeId = value;
			attributeInfo.AttributeName = Globals.StripHtmlXmlTags(Globals.StripScriptTags(parameter).Replace("，", ",").Replace("\\", "")).Trim();
			if (string.IsNullOrEmpty(attributeInfo.AttributeName))
			{
				throw new HidistroAshxException("规格名称限制在1-30个字符以内，不允许包含脚本标签、HTML标签和\\\\(反斜杠)，系统会自动过滤！");
			}
			IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(attributeInfo.TypeId);
			foreach (AttributeInfo item in attributes)
			{
				if (item.AttributeName.Trim() == attributeInfo.AttributeName)
				{
					throw new HidistroAshxException("同一个类型中不能存相同的规格/属性名称！");
				}
			}
			attributeInfo.UsageMode = AttributeUseageMode.Choose;
			attributeInfo.UseAttributeImage = value2;
			ValidationResults validationResults = Validation.Validate(attributeInfo, "ValAttribute");
			string str = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item2 in (IEnumerable<ValidationResult>)validationResults)
				{
					str += item2.Message;
				}
			}
			else
			{
				IList<AttributeInfo> attributes2 = ProductTypeHelper.GetAttributes(value, AttributeUseageMode.Choose);
				if (ProductTypeHelper.AddAttributeName(attributeInfo))
				{
					base.ReturnSuccessResult(context, "添加成功！", 0, true);
				}
			}
		}

		private void AddValue(HttpContext context)
		{
			IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
			int value = base.GetIntParam(context, "typeId", false).Value;
			int value2 = base.GetIntParam(context, "id", false).Value;
			string parameter = base.GetParameter(context, "contents", false);
			parameter = Globals.StripHtmlXmlTags(Globals.StripScriptTags(parameter.Trim()).Replace("，", ",").Replace("\\", "")
				.Replace("+", ""));
			string[] array = parameter.Split(',');
			for (int i = 0; i < array.Length && array[i].Trim().Length <= 100; i++)
			{
				AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
				if (array[i].Trim().Length > 15)
				{
					throw new HidistroAshxException("属性值不合规范");
				}
				attributeValueInfo.ValueStr = array[i].Trim();
				attributeValueInfo.ValueStr = attributeValueInfo.ValueStr.Replace("+", "").Replace("\\", "").Replace("/", "");
				attributeValueInfo.AttributeId = value2;
				list.Add(attributeValueInfo);
			}
			foreach (AttributeValueInfo item in list)
			{
				ProductTypeHelper.AddAttributeValue(item);
			}
			base.ReturnSuccessResult(context, "添加值成功！", 0, true);
		}

		private void EditName(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			string parameter = base.GetParameter(context, "name", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("错误的参数！");
			}
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(value);
			if (attribute == null)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			parameter = Globals.StripHtmlXmlTags(Globals.StripScriptTags(parameter)).Replace("\\", "").Trim();
			if (string.IsNullOrEmpty(parameter) || parameter.Length > 30)
			{
				throw new HidistroAshxException("属性名称限制在1-15个字符以内,不允许包含html字符和\\\\(反斜杠),系统会自动过滤");
			}
			IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(attribute.TypeId);
			foreach (AttributeInfo item in attributes)
			{
				if (item.AttributeName.Trim() == parameter && item.AttributeId != value)
				{
					throw new HidistroAshxException("同一个类型中不能存相同的规格/属性名称！");
				}
			}
			attribute.AttributeName = parameter;
			if (ProductTypeHelper.UpdateAttributeName(attribute))
			{
				base.ReturnSuccessResult(context, "修改成功！", 0, true);
				return;
			}
			throw new HidistroAshxException(" 操作失败！");
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (ProductTypeHelper.DeleteAttribute(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("有商品在使用此规格，无法删除，不允许包含脚本标签和\\\\(反斜杠)，系统会自动过滤");
		}

		private void DeleteValue(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (ProductTypeHelper.DeleteAttributeValue(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("此属性值有商品在使用，删除失败");
		}

		private void IsMulti(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "typeid", false).Value;
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(value);
			if (attribute.IsMultiView)
			{
				attribute.UsageMode = AttributeUseageMode.View;
			}
			else
			{
				attribute.UsageMode = AttributeUseageMode.MultiView;
			}
			if (ProductTypeHelper.UpdateAttributeName(attribute))
			{
				base.ReturnSuccessResult(context, "操作成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}

		private void SetOrder(HttpContext context)
		{
			string parameter = base.GetParameter(context, "Type", false);
			int num = base.GetIntParam(context, "DisplaySequence", false).Value;
			int value = base.GetIntParam(context, "AttributeId", false).Value;
			int value2 = base.GetIntParam(context, "replaceAttributeId", false).Value;
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(value2);
			int num2 = attribute.DisplaySequence;
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
			ProductTypeHelper.SwapAttributeSequence(value, value2, num, num2);
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}
	}
}
