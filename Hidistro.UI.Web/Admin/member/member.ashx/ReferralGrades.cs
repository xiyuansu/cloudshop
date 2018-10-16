using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class ReferralGrades : AdminBaseHandler
	{
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

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
			case "savedata":
				this.SaveData(context);
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
			PageModel<ReferralGradeInfo> pageModel = new PageModel<ReferralGradeInfo>();
			pageModel.Models = MemberProcessor.GetReferralGrades();
			pageModel.Total = pageModel.Models.Count();
			string s = base.SerializeObjectToJson(pageModel);
			context.Response.Write(s);
			context.Response.End();
		}

		private void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "GradeId", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			try
			{
				if (ManagerHelper.CheckAdminPrivilege(Privilege.DeleteReferralGrade))
				{
					if (MemberProcessor.DeleteReferralGrade(intParam.Value))
					{
						base.ReturnSuccessResult(context, "删除成功!", 0, true);
						goto end_IL_002a;
					}
					throw new HidistroAshxException("删除失败");
				}
				throw new HidistroAshxException("对不起,您没有删除分销员等级的权限!");
				end_IL_002a:;
			}
			catch
			{
				throw new HidistroAshxException("删除失败!");
			}
		}

		private void SaveData(HttpContext context)
		{
			int num = context.Request["GradeId"].ToInt(0);
			string name = Globals.StripAllTags(context.Request["GradeName"].ToNullString());
			decimal num2 = context.Request["CommissionThreshold"].ToDecimal(0);
			bool flag = false;
			if (num2 < decimal.Zero)
			{
				throw new HidistroAshxException("佣金门槛金额必须大于等于0!");
			}
			ReferralGradeInfo referralGradeInfo = new ReferralGradeInfo();
			if (num == 0)
			{
				flag = true;
				if (!ManagerHelper.CheckAdminPrivilege(Privilege.AddReferralGrade))
				{
					throw new HidistroAshxException("对不起,您没有添加分销员等级的权限!");
				}
			}
			else
			{
				if (!ManagerHelper.CheckAdminPrivilege(Privilege.EditReferralGrade))
				{
					throw new HidistroAshxException("对不起,您没有编辑分销员等级的权限!");
				}
				referralGradeInfo = MemberProcessor.GetReferralGradeInfo(num);
				if (referralGradeInfo == null)
				{
					throw new HidistroAshxException("错误的分销员等级ID!");
				}
			}
			if (MemberProcessor.HasSameCommissionThresholdGrade(num2, num))
			{
				throw new HidistroAshxException("佣金门槛不能和其他分销等级相同!");
			}
			referralGradeInfo.CommissionThreshold = num2;
			referralGradeInfo.Name = name;
			if (flag)
			{
				if (MemberProcessor.AddReferralGrade(referralGradeInfo))
				{
					base.ReturnSuccessResult(context, "添加成功!", 0, true);
					return;
				}
				throw new HidistroAshxException("添加分销员等级失败!");
			}
			if (MemberProcessor.UpdateReferralGrade(referralGradeInfo))
			{
				base.ReturnSuccessResult(context, "保存成功!", 0, true);
				return;
			}
			throw new HidistroAshxException("编辑分销员等级失败!");
		}
	}
}
