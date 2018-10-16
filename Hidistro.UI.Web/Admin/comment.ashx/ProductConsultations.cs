using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.comment.ashx
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class ProductConsultations : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
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

		private void GetList(HttpContext context)
		{
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
			productConsultationAndReplyQuery.Keywords = context.Request["Keywords"];
			if (!string.IsNullOrEmpty(context.Request["CategoryId"]))
			{
				productConsultationAndReplyQuery.CategoryId = base.GetIntParam(context, "CategoryId", false).Value;
			}
			string value = context.Request["Type"];
			if (!string.IsNullOrEmpty(value))
			{
				productConsultationAndReplyQuery.Type = (ConsultationReplyType)Enum.Parse(typeof(ConsultationReplyType), value);
			}
			productConsultationAndReplyQuery.ProductCode = context.Request["ProductCode"];
			productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
			productConsultationAndReplyQuery.PageIndex = base.CurrentPageIndex;
			productConsultationAndReplyQuery.PageSize = base.CurrentPageSize;
			productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
			productConsultationAndReplyQuery.SortBy = "ReplyDate";
			Globals.EntityCoding(productConsultationAndReplyQuery, true);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(productConsultationAndReplyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ProductConsultationAndReplyQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult consultationProducts = ProductCommentHelper.GetConsultationProducts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(consultationProducts.Data);
				dataGridViewModel.total = consultationProducts.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("Type", (int)query.Type);
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "consultationId", false).Value;
			if (ProductCommentHelper.DeleteProductConsultation(value))
			{
				base.ReturnSuccessResult(context, "成功删除了选择的商品咨询！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除商品咨询失败！");
		}
	}
}
