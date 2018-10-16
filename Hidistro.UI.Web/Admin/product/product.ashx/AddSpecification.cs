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
	public class AddSpecification : AdminBaseHandler
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
			case "isupload":
				this.IsUplod(context);
				break;
			case "editsku":
				this.EditSku(context);
				break;
			case "addsku":
				this.AddSku(context);
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
			IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.Choose);
			dataGridViewModel.rows = attributes.ToList();
			dataGridViewModel.total = attributes.Count;
			foreach (AttributeInfo row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "ids", false).Value;
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(value);
			if (ProductTypeHelper.DeleteAttribute(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("有商品在使用此规格，无法删除，不允许包含脚本标签和\\\\(反斜杠)，系统会自动过滤");
		}

		private void IsUplod(HttpContext context)
		{
			int value = base.GetIntParam(context, "AttributeId", false).Value;
			bool value2 = base.GetBoolParam(context, "UseAttributeImage", false).Value;
			int value3 = base.GetIntParam(context, "typeId", false).Value;
			AttributeInfo attributeInfo = new AttributeInfo();
			attributeInfo.AttributeId = value;
			bool flag = false;
			if (value2)
			{
				flag = false;
			}
			else
			{
				flag = true;
				if (ProductTypeHelper.HasSetUseImg(value3) > 0)
				{
					throw new HidistroAshxException("已有其他规格可上传图片，最多只有一个规格允许上传商品规格图！");
				}
			}
			attributeInfo.UseAttributeImage = flag;
			if (ProductTypeHelper.UpdateIsUseAttribute(attributeInfo))
			{
				base.ReturnSuccessResult(context, "", 0, true);
			}
		}

		private void EditSku(HttpContext context)
		{
			int value = base.GetIntParam(context, "attributeId", false).Value;
			bool value2 = base.GetBoolParam(context, "UseAttributeImage", false).Value;
			int value3 = base.GetIntParam(context, "typeId", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("请选择要修改的规格，或刷新页面重试！");
			}
			int num = ProductTypeHelper.HasSetUseImg(value3);
			if (num > 0 && (num != value & value2))
			{
				throw new HidistroAshxException("已有其他规格可上传图片，最多只有一个规格允许上传商品规格图！");
			}
			string parameter = base.GetParameter(context, "SkuName", false);
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(value);
			if (attribute == null)
			{
				throw new HidistroAshxException("规格不存在，请刷新页面重试！");
			}
			string text = Globals.StripHtmlXmlTags(Globals.StripScriptTags(parameter)).Replace("\\", "").Trim();
			if (string.IsNullOrEmpty(text) || text.Length > 30)
			{
				throw new HidistroAshxException("规格名称限制在1-30个字符以内，不允许包含脚本标签、HTML标签和\\\\(反斜杠)，系统会自动过滤！");
			}
			IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(attribute.TypeId);
			foreach (AttributeInfo item in attributes)
			{
				if (item.AttributeName.Trim() == text && item.AttributeId != value)
				{
					throw new HidistroAshxException("同一个类型中不能存相同的规格/属性名称！");
				}
			}
			attribute.AttributeName = text;
			attribute.UsageMode = AttributeUseageMode.Choose;
			attribute.UseAttributeImage = value2;
			if (ProductTypeHelper.UpdateAttributeName(attribute))
			{
				base.ReturnSuccessResult(context, "修改成功！", 0, true);
				return;
			}
			throw new HidistroAshxException(" 修改规格错误！");
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
			if (ProductTypeHelper.HasSetUseImg(value) > 0 && value2)
			{
				throw new HidistroAshxException("已有其他规格可上传图片，最多只有一个规格允许上传商品规格图！");
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
			base.ReturnSuccessResult(context, "", 0, true);
		}
	}
}
