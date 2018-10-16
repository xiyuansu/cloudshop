using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	public class BatchEditMemberPoint : AdminCallBackPage
	{
		private string userIds;

		protected Repeater grdSelectedUsers;

		protected HtmlInputRadioButton radAdd;

		protected TextBox txtAddPoints;

		protected HtmlInputRadioButton RadMinus;

		protected TextBox txtMinusPoints;

		protected TextBox txtRemark;

		protected Button btnSubmitBatchPoint;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitBatchPoint.Click += this.btnSubmitBatchPoint_Click;
			this.userIds = this.Page.Request.QueryString["userIds"];
			if (!this.Page.IsPostBack)
			{
				this.BindUsers();
			}
		}

		private void BindUsers()
		{
			if (!string.IsNullOrEmpty(this.userIds))
			{
				this.grdSelectedUsers.DataSource = MemberHelper.GetUsersBaseInfo(this.userIds);
				this.grdSelectedUsers.DataBind();
			}
		}

		protected void btnSubmitBatchPoint_Click(object sender, EventArgs e)
		{
			ManagerHelper.CheckPrivilege(Privilege.UpdateMemberPoint);
			if (string.IsNullOrEmpty(this.userIds))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				Hashtable hashtable = new Hashtable();
				if (this.grdSelectedUsers.Items.Count > 0)
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
						string[] array = this.userIds.Split(',');
						for (int i = 0; i < array.Length; i++)
						{
							MemberInfo user = Users.GetUser(int.Parse(array[i]));
							if (num2 > user.Points)
							{
								this.ShowMsg("会员【" + user.UserName + "】的积分不足，请调整要减去的积分", false);
								this.Page.ClientScript.RegisterStartupScript(base.GetType(), "msg", "<script>onRadioClick(2);</script>");
								return;
							}
						}
					}
					PointDetailInfo pointDetailInfo = new PointDetailInfo();
					pointDetailInfo.OrderId = "";
					pointDetailInfo.TradeType = PointTradeType.AdministratorUpdate;
					if (this.radAdd.Checked)
					{
						pointDetailInfo.Increased = num;
						pointDetailInfo.Reduced = 0;
					}
					else if (this.RadMinus.Checked)
					{
						pointDetailInfo.Reduced = num2;
						pointDetailInfo.Increased = 0;
					}
					foreach (RepeaterItem item in this.grdSelectedUsers.Items)
					{
						TextBox textBox = item.FindControl("txtListRemark") as TextBox;
						HiddenField hiddenField = item.FindControl("hidUserId") as HiddenField;
						string text = textBox.Text;
						if (string.IsNullOrEmpty(textBox.Text))
						{
							text = this.txtRemark.Text;
						}
						text = "操作员:" + HiContext.Current.Manager.UserName + " &nbsp;&nbsp;&nbsp;&nbsp;" + text;
						hashtable.Add(hiddenField.Value, text);
					}
					PointDetailDao pointDetailDao = new PointDetailDao();
					if (pointDetailDao.BatchEditPoints(pointDetailInfo, this.userIds, hashtable))
					{
						string[] array2 = this.userIds.Split(',');
						string[] array3 = array2;
						foreach (string s in array3)
						{
							MemberDao memberDao = new MemberDao();
							int userId = 0;
							if (int.TryParse(s, out userId))
							{
								MemberInfo user2 = Users.GetUser(userId);
								if (this.radAdd.Checked)
								{
									user2.Points += num;
								}
								else if (this.RadMinus.Checked)
								{
									int num3 = user2.Points - num2;
									user2.Points = ((num3 >= 0) ? num3 : 0);
								}
								int historyPoint = pointDetailDao.GetHistoryPoint(int.Parse(s), null);
								memberDao.ChangeMemberGrade(userId, user2.Points, historyPoint, null);
								if (user2 != null)
								{
									Users.ClearUserCache(userId, user2.SessionId);
								}
							}
						}
						base.CloseWindow(null);
					}
					else
					{
						this.ShowMsg("批量操作失败", false);
					}
				}
			}
		}
	}
}
