using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class ManageRedEnvelope : AdminBaseHandler
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
			case "setstatus":
				this.SetStatus(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
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
			redEnvelopeGetRecordQuery.SortBy = "Id";
			redEnvelopeGetRecordQuery.SortOrder = SortAction.Desc;
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
				PageModel<WeiXinRedEnvelopeInfo> weiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = weiXinRedEnvelope.Total;
				foreach (WeiXinRedEnvelopeInfo model in weiXinRedEnvelope.Models)
				{
					model.ActualNumber = WeiXinRedEnvelopeProcessor.GetActualNumber(model.Id);
					Dictionary<string, object> dictionary = model.ToDictionary();
					string value = "";
					switch (model.State)
					{
					case 1:
						value = "已开启";
						break;
					case 0:
						value = "已关闭";
						break;
					case 2:
						value = "已过期";
						break;
					}
					dictionary.Add("StatusText", value);
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (WeiXinRedEnvelopeProcessor.DeleteRedEnvelope(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		private void SetStatus(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			string text = context.Request["Command"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的命令");
			}
			text = text.ToLower();
			string a = text;
			if (!(a == "close"))
			{
				if (a == "open")
				{
					WeiXinRedEnvelopeProcessor.SetRedEnvelopeState(intParam.Value, RedEnvelopeState.Enabled);
					goto IL_00a3;
				}
				throw new HidistroAshxException("错误的命令");
			}
			WeiXinRedEnvelopeProcessor.SetRedEnvelopeState(intParam.Value, RedEnvelopeState.Close);
			goto IL_00a3;
			IL_00a3:
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}
	}
}
