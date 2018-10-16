using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditMember)]
	public class EditMember : AdminPage
	{
		private int userId;

		protected HiddenField hidTagId;

		protected HiddenField hidTagNames;

		protected Literal lblLoginNameValue;

		protected MemberGradeDropDownList drpMemberRankList;

		protected TextBox txtRealName;

		protected CalendarPanel calBirthday;

		protected GenderRadioButtonList gender;

		protected TextBox txtprivateEmail;

		protected RegionSelector rsddlRegion;

		protected TextBox txtAddress;

		protected TextBox txtQQ;

		protected TextBox txtMSN;

		protected TextBox txtCellPhone;

		protected FormatedTimeLabel lblRegsTimeValue;

		protected Literal lblTotalAmountValue;

		protected HtmlGenericControl divTagContent;

		protected Button btnEditUser;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditUser.Click += this.btnEditUser_Click;
				Dictionary<string, object> calendarParameter = this.calBirthday.CalendarParameter;
				DateTime dateTime = new DateTime(1900, 1, 1);
				calendarParameter.Add("startDate", dateTime.ToString("yyyy-MM-dd"));
				Dictionary<string, object> calendarParameter2 = this.calBirthday.CalendarParameter;
				dateTime = DateTime.Now;
				calendarParameter2.Add("endDate", dateTime.ToString("yyyy-MM-dd"));
				if (!this.Page.IsPostBack)
				{
					this.drpMemberRankList.AllowNull = false;
					this.drpMemberRankList.DataBind();
					this.LoadMemberInfo();
				}
			}
		}

		private void LoadMemberInfo()
		{
			MemberInfo user = Users.GetUser(this.userId);
			if (user == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.drpMemberRankList.SelectedValue = user.GradeId;
				this.lblLoginNameValue.Text = user.UserName;
				this.lblRegsTimeValue.Time = user.CreateDate;
				this.lblTotalAmountValue.Text = Globals.FormatMoney(user.Expenditure);
				this.txtRealName.Text = user.RealName;
				this.calBirthday.SelectedDate = user.BirthDate;
				this.txtAddress.Text = Globals.HtmlDecode(user.Address);
				this.rsddlRegion.SetSelectedRegionId(user.RegionId);
				this.txtQQ.Text = user.QQ;
				this.txtMSN.Text = user.NickName;
				this.txtCellPhone.Text = user.CellPhone;
				this.txtprivateEmail.Text = user.Email;
				this.gender.SelectedValue = user.Gender;
				string tagIds = user.TagIds;
				if (!string.IsNullOrWhiteSpace(tagIds))
				{
					if (tagIds == ",")
					{
						tagIds = "";
					}
					else
					{
						tagIds = tagIds.TrimStart(',');
						tagIds = tagIds.TrimEnd(',');
					}
					this.hidTagId.Value = tagIds;
					if (tagIds != "")
					{
						IList<MemberTagInfo> tagByMember = MemberTagHelper.GetTagByMember(tagIds);
						this.hidTagNames.Value = string.Join(",", (from t in tagByMember
						select t.TagName).ToArray());
					}
				}
			}
		}

		protected void btnEditUser_Click(object sender, EventArgs e)
		{
			MemberInfo user = Users.GetUser(this.userId);
			if (this.txtRealName.Text.Trim().Length > 20 || (this.txtRealName.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtRealName.Text.Trim(), "^[A-Za-z\\u4e00-\\u9fa5]+$")))
			{
				this.ShowMsg("真实姓名限制在20个字符以内,只能由中文或英文组成", false);
			}
			else
			{
				user.GradeId = this.drpMemberRankList.SelectedValue.Value;
				user.RealName = this.txtRealName.Text.Trim();
				if (this.rsddlRegion.GetSelectedRegionId().HasValue)
				{
					user.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
					user.TopRegionId = RegionHelper.GetTopRegionId(user.RegionId, true);
				}
				user.Address = Globals.HtmlEncode(this.txtAddress.Text);
				user.QQ = this.txtQQ.Text.Trim();
				user.NickName = this.txtMSN.Text.Trim();
				if (this.calBirthday.SelectedDate.HasValue)
				{
					user.BirthDate = this.calBirthday.SelectedDate.Value;
				}
				if (!string.IsNullOrEmpty(this.txtprivateEmail.Text.Trim()) && user.Email != this.txtprivateEmail.Text.Trim() && user.UserName != this.txtprivateEmail.Text.Trim())
				{
					MemberInfo memberInfo = MemberProcessor.FindMemberByUsername(this.txtprivateEmail.Text.Trim());
					if (Users.MemberEmailIsExist(this.txtprivateEmail.Text.Trim()) || (memberInfo != null && memberInfo.UserName == this.txtprivateEmail.Text.Trim()))
					{
						this.ShowMsg("邮箱已存在，请重新输入!", false);
						return;
					}
					user.EmailVerification = false;
				}
				user.Email = this.txtprivateEmail.Text.Trim();
				if (!string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()) && user.CellPhone != this.txtCellPhone.Text.Trim() && user.UserName != this.txtCellPhone.Text.Trim())
				{
					MemberInfo memberInfo2 = MemberProcessor.FindMemberByUsername(this.txtCellPhone.Text.Trim());
					if (MemberProcessor.IsUseCellphone(this.txtCellPhone.Text.Trim()) || (memberInfo2 != null && memberInfo2.UserName == this.txtCellPhone.Text.Trim()))
					{
						this.ShowMsg("手机号码已存在!", false);
						return;
					}
					user.CellPhoneVerification = false;
				}
				user.CellPhone = this.txtCellPhone.Text.Trim();
				user.Gender = this.gender.SelectedValue;
				string text = this.hidTagId.Value;
				if (string.IsNullOrWhiteSpace(text))
				{
					text = ",";
				}
				if (text != ",")
				{
					text = "," + text + ",";
				}
				user.TagIds = text;
				if (MemberHelper.Update(user, false))
				{
					this.ShowMsg("成功修改了当前会员的个人资料", true);
				}
				else
				{
					this.ShowMsg("当前会员的个人信息修改失败,请检查您的邮箱是否与其它会员相同了。", false);
				}
			}
		}
	}
}
