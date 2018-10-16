using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.comment.ashx
{
	[PrivilegeCheck(Privilege.Affiches)]
	public class AfficheList : AdminBaseHandler
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
			DataGridViewModel<AfficheInfo> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<AfficheInfo> GetDataList()
		{
			DataGridViewModel<AfficheInfo> dataGridViewModel = new DataGridViewModel<AfficheInfo>();
			IList<AfficheInfo> afficheList = NoticeHelper.GetAfficheList();
			dataGridViewModel.rows = afficheList.ToList();
			dataGridViewModel.total = afficheList.Count;
			foreach (AfficheInfo row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			List<int> list = new List<int>();
			bool flag = false;
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			string[] array = text.Split(',');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					list.Add(text2.ToInt(0));
				}
			}
			int num = NoticeHelper.DeleteAffiches(list);
			if (num > 0)
			{
				base.ReturnSuccessResult(context, $"成功删除了选择的{num}条公告！", 0, true);
				return;
			}
			throw new HidistroAshxException("请选择要删除的公告！");
		}
	}
}
