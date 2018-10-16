using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot
{
	public class MemberDetails : StoreAdminPage
	{
		private int userId;

		protected Literal litUserName;

		protected Literal litGrade;

		protected Literal litRealName;

		protected Literal litBirthDate;

		protected Literal litGender;

		protected Literal litEmail;

		protected Literal litAddress;

		protected Literal litQQ;

		protected Literal litMSN;

		protected Literal litCellPhone;

		protected Literal litCreateDate;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.LoadMemberInfo();
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
				Uri url = HttpContext.Current.Request.Url;
				string text = "";
				this.litUserName.Text = user.UserName;
				this.litGrade.Text = MemberHelper.GetMemberGrade(user.GradeId).Name;
				this.litCreateDate.Text = user.CreateDate.ToString();
				this.litRealName.Text = user.RealName;
				this.litBirthDate.Text = user.BirthDate.ToString();
				this.litAddress.Text = RegionHelper.GetFullRegion(user.RegionId, "", true, 0) + user.Address;
				this.litQQ.Text = user.QQ;
				this.litMSN.Text = user.NickName;
				this.litCellPhone.Text = user.CellPhone;
				this.litEmail.Text = user.Email;
				if (user.Gender == Gender.Female)
				{
					this.litGender.Text = "女";
				}
				else if (user.Gender == Gender.Male)
				{
					this.litGender.Text = "男";
				}
				else
				{
					this.litGender.Text = "保密";
				}
			}
		}
	}
}
