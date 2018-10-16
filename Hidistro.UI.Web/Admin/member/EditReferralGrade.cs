using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.EditMemberGrade)]
	public class EditReferralGrade : AdminPage
	{
		private int gradeId;

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
			this.gradeId = this.Page.Request.QueryString["GradeId"].ToInt(0);
			if (this.gradeId <= 0)
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				ReferralGradeInfo referralGradeInfo = MemberProcessor.GetReferralGradeInfo(this.gradeId);
				if (referralGradeInfo == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					Globals.EntityCoding(referralGradeInfo, false);
					this.txtGradeName.Text = referralGradeInfo.Name;
					this.txtCommissionThreshold.Text = referralGradeInfo.CommissionThreshold.ToString("f0");
				}
			}
		}

		private void btnSubmitReferralGrades_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			ReferralGradeInfo referralGradeInfo = MemberProcessor.GetReferralGradeInfo(this.gradeId);
			if (referralGradeInfo == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
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
				else if (MemberProcessor.HasSameCommissionThresholdGrade(referralGradeInfo.CommissionThreshold, this.gradeId))
				{
					this.ShowMsg("已存在相同的佣金门槛金额!", false);
				}
				else if (MemberProcessor.UpdateReferralGrade(referralGradeInfo))
				{
					this.ShowMsg("修改分销员等级成功", true);
				}
				else
				{
					this.ShowMsg("修改分销员等级失败", false);
				}
			}
		}
	}
}
