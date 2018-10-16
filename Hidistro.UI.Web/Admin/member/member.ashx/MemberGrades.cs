using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class MemberGrades : AdminBaseHandler
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
			case "delete":
				this.Delete(context);
				break;
			case "isdefault":
				this.IsDefault(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			DataGridViewModel<MemberGradeInfo> listSplittinDraws = this.GetListSplittinDraws();
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<MemberGradeInfo> GetListSplittinDraws()
		{
			DataGridViewModel<MemberGradeInfo> dataGridViewModel = new DataGridViewModel<MemberGradeInfo>();
			IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
			dataGridViewModel.rows = memberGrades.ToList();
			dataGridViewModel.total = memberGrades.Count;
			foreach (MemberGradeInfo row in dataGridViewModel.rows)
			{
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "gradeId", false).Value;
			if (MemberHelper.DeleteMemberGrade(value))
			{
				base.ReturnSuccessResult(context, "已经成功删除选择的会员等级！", 0, true);
				return;
			}
			throw new HidistroAshxException("不能删除默认的会员等级或有会员的等级！");
		}

		private void IsDefault(HttpContext context)
		{
			int value = base.GetIntParam(context, "GradeId", false).Value;
			MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(value);
			if (!memberGrade.IsDefault)
			{
				MemberHelper.SetDefalutMemberGrade(value);
				base.ReturnSuccessResult(context, "", 0, true);
			}
		}
	}
}
