using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddMemberGrade)]
	public class AddMemberGrade : AdminPage
	{
		protected TextBox txtRankName;

		protected TextBox txtPoint;

		protected TextBox txtValue;

		protected YesNoRadioButtonList chkIsDefault;

		protected TextBox txtRankDesc;

		protected Button btnSubmitMemberRanks;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSubmitMemberRanks.Click += this.btnSubmitMemberRanks_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		private void btnSubmitMemberRanks_Click(object sender, EventArgs e)
		{
			string text = string.Empty;
			if (this.txtValue.Text.Trim().Contains("."))
			{
				this.ShowMsg("折扣必须为正整数", false);
			}
			else
			{
				MemberGradeInfo memberGradeInfo = new MemberGradeInfo();
				memberGradeInfo.Name = this.txtRankName.Text.Trim();
				memberGradeInfo.Description = this.txtRankDesc.Text.Trim();
				int points = default(int);
				if (int.TryParse(this.txtPoint.Text.Trim(), out points))
				{
					memberGradeInfo.Points = points;
				}
				else
				{
					text += Formatter.FormatErrorMessage("积分设置无效或不能为空，必须大于等于0的整数");
				}
				memberGradeInfo.IsDefault = this.chkIsDefault.SelectedValue;
				int discount = default(int);
				if (int.TryParse(this.txtValue.Text, out discount))
				{
					memberGradeInfo.Discount = discount;
				}
				else
				{
					text += Formatter.FormatErrorMessage("等级折扣设置无效或不能为空，等级折扣必须在1-10之间");
				}
				if (!string.IsNullOrEmpty(text))
				{
					this.ShowMsg(text, false);
				}
				else if (this.ValidationMemberGrade(memberGradeInfo))
				{
					if (MemberHelper.HasSamePointMemberGrade(memberGradeInfo))
					{
						this.ShowMsg("已经存在相同积分的等级，每个会员等级的积分不能相同", false);
					}
					else if (MemberHelper.CreateMemberGrade(memberGradeInfo))
					{
						this.ShowMsg("成功添加了一个会员等级", true);
					}
					else
					{
						this.ShowMsg("添加会员等级失败", false);
					}
				}
			}
		}

		private bool ValidationMemberGrade(MemberGradeInfo memberGrade)
		{
			ValidationResults validationResults = Validation.Validate(memberGrade, "ValMemberGrade");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
