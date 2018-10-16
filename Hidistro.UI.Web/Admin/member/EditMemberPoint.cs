using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	public class EditMemberPoint : AdminCallBackPage
	{
		private int userId;

		protected HtmlInputRadioButton radAdd;

		protected TextBox txtAddPoints;

		protected HtmlInputRadioButton RadMinus;

		protected TextBox txtMinusPoints;

		protected TextBox txtRemark;

		protected Button btnSubmitPoint;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitPoint.Click += this.btnSubmitPoint_Click;
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
		}

		protected void btnSubmitPoint_Click(object sender, EventArgs e)
		{
			ManagerHelper.CheckPrivilege(Privilege.UpdateMemberPoint);
			MemberInfo user = Users.GetUser(this.userId);
			if (user == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				int num = 0;
				int num2 = 0;
				if (this.radAdd.Checked)
				{
					if (string.IsNullOrEmpty(this.txtAddPoints.Text) || !int.TryParse(this.txtAddPoints.Text.Trim(), out num))
					{
						this.ShowMsg("要增加的积分数不能为空且为正数", false);
						return;
					}
					if (num <= 0)
					{
						this.ShowMsg("请输入大于0的积分数", false);
						return;
					}
				}
				else if (this.RadMinus.Checked)
				{
					if (string.IsNullOrEmpty(this.txtMinusPoints.Text) || !int.TryParse(this.txtMinusPoints.Text.Trim(), out num2))
					{
						this.ShowMsg("要减少的积分数不能为空且为正数", false);
						return;
					}
					if (num2 <= 0)
					{
						this.ShowMsg("请输入大于0的积分数", false);
						return;
					}
					if (num2 > user.Points)
					{
						this.ShowMsg("会员【" + user.UserName + "】的积分不足，请调整要减去的积分", false);
						this.Page.ClientScript.RegisterStartupScript(base.GetType(), "msg", "<script>onRadioClick(2);</script>");
						return;
					}
				}
				PointDetailInfo pointDetailInfo = new PointDetailInfo();
				pointDetailInfo.OrderId = "";
				pointDetailInfo.UserId = this.userId;
				pointDetailInfo.TradeDate = DateTime.Now;
				pointDetailInfo.TradeType = PointTradeType.AdministratorUpdate;
				if (this.radAdd.Checked)
				{
					pointDetailInfo.Increased = num;
					pointDetailInfo.Points = num + user.Points;
				}
				else if (this.RadMinus.Checked)
				{
					pointDetailInfo.Reduced = num2;
					pointDetailInfo.Points = user.Points - num2;
				}
				if (pointDetailInfo.Points > 2147483647)
				{
					pointDetailInfo.Points = 2147483647;
				}
				if (pointDetailInfo.Points < 0)
				{
					pointDetailInfo.Points = 0;
				}
				pointDetailInfo.Remark = "操作员:" + HiContext.Current.Manager.UserName + " &nbsp;&nbsp;&nbsp;&nbsp;" + this.txtRemark.Text.Trim();
				PointDetailDao pointDetailDao = new PointDetailDao();
				if (pointDetailDao.Add(pointDetailInfo, null) > 0)
				{
					user.Points = pointDetailInfo.Points;
					MemberDao memberDao = new MemberDao();
					int historyPoint = pointDetailDao.GetHistoryPoint(this.userId, null);
					memberDao.ChangeMemberGrade(this.userId, user.Points, historyPoint, null);
					Users.ClearUserCache(this.userId, user.SessionId);
					base.CloseWindow(null);
				}
				else
				{
					this.ShowMsg("操作失败", false);
				}
			}
		}
	}
}
