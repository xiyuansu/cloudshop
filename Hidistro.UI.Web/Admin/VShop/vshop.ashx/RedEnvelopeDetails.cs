using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class RedEnvelopeDetails : AdminBaseHandler
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
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		public void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			RedEnvelopeGetRecordQuery redEnvelopeGetRecordQuery = new RedEnvelopeGetRecordQuery();
			redEnvelopeGetRecordQuery.RedEnvelopeId = base.GetIntParam(context, "RedEnvelopeId", false).Value;
			redEnvelopeGetRecordQuery.PageIndex = num;
			redEnvelopeGetRecordQuery.PageSize = num2;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(redEnvelopeGetRecordQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(RedEnvelopeGetRecordQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<RedEnvelopeGetRecordInfo> redEnvelopeGetRecord = WeiXinRedEnvelopeProcessor.GetRedEnvelopeGetRecord(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = redEnvelopeGetRecord.Total;
				foreach (RedEnvelopeGetRecordInfo model in redEnvelopeGetRecord.Models)
				{
					Dictionary<string, object> dictionary = model.ToDictionary();
					string text = "";
					WeiXinRedEnvelopeInfo weiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(model.RedEnvelopeId);
					text = ((weiXinRedEnvelope == null) ? "未知" : weiXinRedEnvelope.Name);
					dictionary.Add("RedEnvelopeName", text);
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}
	}
}
