using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class MemberTags : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "savetag":
				this.SaveTag(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			Pagination dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private Pagination GetDataQuery(HttpContext context)
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = base.CurrentPageIndex;
			pagination.PageSize = base.CurrentPageSize;
			pagination.SortBy = "TagId";
			pagination.SortOrder = SortAction.Desc;
			return pagination;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(Pagination query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult tags = MemberTagHelper.GetTags(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(tags.Data);
				dataGridViewModel.total = tags.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			try
			{
				MemberTagHelper.DeleteTag(intParam.Value);
				base.ReturnSuccessResult(context, "删除成功!", 0, true);
			}
			catch
			{
				throw new HidistroAshxException("删除失败!");
			}
		}

		private void SaveTag(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", true);
			bool flag = !intParam.HasValue;
			MemberTagInfo memberTagInfo = new MemberTagInfo();
			memberTagInfo.TagName = base.GetParameter(context, "TagName", true);
			if (string.IsNullOrWhiteSpace(memberTagInfo.TagName))
			{
				throw new HidistroAshxException("请输入标签名称！");
			}
			if (memberTagInfo.TagName.Length > 20)
			{
				throw new HidistroAshxException("标签名称限制最多输入20个字符！");
			}
			memberTagInfo.OrderCount = base.GetIntParam(context, "OrderCount", false).Value;
			if (memberTagInfo.OrderCount < 0 || memberTagInfo.OrderCount > 10000)
			{
				throw new HidistroAshxException("请输入正确的交易笔数，限制为0-10000的正整数！");
			}
			memberTagInfo.OrderTotalAmount = base.GetParameter(context, "OrderTotalAmount", decimal.Zero);
			if (memberTagInfo.OrderTotalAmount < decimal.Zero || memberTagInfo.OrderTotalAmount > 100000000m)
			{
				throw new HidistroAshxException("请输入正确的交易笔数，为0-100000000之间的数字，限制两位小数！");
			}
			if (!flag)
			{
				memberTagInfo.TagId = intParam.Value;
				if (MemberTagHelper.Update(memberTagInfo))
				{
					base.ReturnSuccessResult(context, "编辑成功", 0, true);
					return;
				}
				throw new HidistroAshxException("编辑失败！");
			}
			if (MemberTagHelper.AddTag(memberTagInfo) > 0)
			{
				base.ReturnSuccessResult(context, "添加成功", 0, true);
				return;
			}
			throw new HidistroAshxException("添加失败！");
		}
	}
}
