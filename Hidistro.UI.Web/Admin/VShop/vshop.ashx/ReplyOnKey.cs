using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class ReplyOnKey : AdminBaseHandler
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
			case "release":
				this.Release(context);
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
			List<ReplyInfo> list = ReplyHelper.GetAllReply().ToList().FindAll((ReplyInfo a) => a.ReplyType < ReplyType.Wheel);
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			dataGridViewModel.total = list.Count;
			foreach (ReplyInfo item in list)
			{
				Dictionary<string, object> dictionary = item.ToDictionary();
				dictionary.Add("ReplyTypeName", this.GetReplyTypeName(item.ReplyType));
				dataGridViewModel.rows.Add(dictionary);
			}
			return dataGridViewModel;
		}

		private string GetReplyTypeName(ReplyType type)
		{
			string text = string.Empty;
			bool flag = false;
			if (ReplyType.Subscribe == (type & ReplyType.Subscribe))
			{
				text += "[关注时回复]";
				flag = true;
			}
			if (ReplyType.NoMatch == (type & ReplyType.NoMatch))
			{
				text += "[无匹配回复]";
				flag = true;
			}
			if (ReplyType.Keys == (type & ReplyType.Keys))
			{
				text += "[关键字回复]";
				flag = true;
			}
			if (!flag)
			{
				text = ((Enum)(object)type).ToShowText();
			}
			return text;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (ReplyHelper.DeleteReply(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		private void Release(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (ReplyHelper.UpdateReplyRelease(intParam.Value))
			{
				base.ReturnSuccessResult(context, "操作成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
