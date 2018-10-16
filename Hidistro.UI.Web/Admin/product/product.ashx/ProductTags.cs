using Hidistro.Entities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	public class ProductTags : AdminBaseHandler
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
			case "edit":
				this.Edit(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<TagInfo> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<TagInfo> GetDataList()
		{
			DataGridViewModel<TagInfo> dataGridViewModel = new DataGridViewModel<TagInfo>();
			IList<TagInfo> tags = CatalogHelper.GetTags();
			dataGridViewModel.rows = tags.ToList();
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			foreach (TagInfo row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (CatalogHelper.DeleteTags(value))
			{
				base.ReturnSuccessResult(context, "删除商品标签成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除商品标签失败！");
		}

		private void Edit(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			string parameter = base.GetParameter(context, "tagname", false);
			bool flag = value == 0;
			if (string.IsNullOrEmpty(parameter))
			{
				throw new HidistroAshxException("标签名称不允许为空！");
			}
			if (parameter.Length > 20)
			{
				throw new HidistroAshxException("标签名称过长，不要超过20字符！");
			}
			if (flag)
			{
				if (CatalogHelper.AddTags(parameter) > 0)
				{
					base.ReturnSuccessResult(context, "添加标签成功！", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, "添加操作失败,请确认输入的商品标签名称是否存在同名！", 0, true);
				}
			}
			else if (CatalogHelper.UpdateTags(value, parameter))
			{
				base.ReturnSuccessResult(context, "修改标签成功！", 0, true);
			}
			else
			{
				base.ReturnSuccessResult(context, "修改操作失败,请确认输入的商品标签名称是否存在同名！", 0, true);
			}
		}
	}
}
