using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.AddReferralGrade)]
	public class AddReferralGrade : AdminPage
	{
		protected TextBox txtGradeName;

		protected TextBox txtCommissionThreshold;

		protected Button btnSubmitReferralGrades;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSubmitReferralGrades.Click += this.btnSubmitReferralGrades_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		private void btnSubmitReferralGrades_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			ReferralGradeInfo referralGradeInfo = new ReferralGradeInfo();
			referralGradeInfo.Name = Globals.StripAllTags(this.txtGradeName.Text.Trim());
			referralGradeInfo.CommissionThreshold = this.txtCommissionThreshold.Text.ToDecimal(0);
			if (referralGradeInfo.Name.Length <= 0 || referralGradeInfo.Name.Length > 8)
			{
				this.ShowMsg("请输入等级名称，长度在1-8个字符之间", false);
			}
			else if (referralGradeInfo.CommissionThreshold < decimal.Zero || referralGradeInfo.CommissionThreshold > 99999m)
			{
				this.ShowMsg("佣金门槛金额必须在0-99999之间", false);
			}
			else if (MemberProcessor.HasSameCommissionThresholdGrade(referralGradeInfo.CommissionThreshold, 0))
			{
				this.ShowMsg("已存在相同的佣金门槛金额!", false);
			}
			else
			{
				IList<ReferralGradeInfo> referralGrades = MemberProcessor.GetReferralGrades();
				if (referralGrades != null && referralGrades.Count() >= 10)
				{
					this.ShowMsg("分销员等级数量不能超过10个!", false);
				}
				else if (MemberProcessor.AddReferralGrade(referralGradeInfo))
				{
					this.ShowMsg("成功添加了一个分销员等级", true);
				}
				else
				{
					this.ShowMsg("添加会员等级失败", false);
				}
			}
		}
	}
}
