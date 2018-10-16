using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPPoint : AppshopMemberTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptPointList;

		private Literal litCurrentPoints;

		private Literal litHistoryPoints;

		private HtmlInputHidden txtTotalPages;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-Point.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptPointList = (AppshopTemplatedRepeater)this.FindControl("rptPointList");
			this.litCurrentPoints = (Literal)this.FindControl("litCurrentPoints");
			this.litHistoryPoints = (Literal)this.FindControl("litHistoryPoints");
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			int pageIndex = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int num = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out num))
			{
				num = 20;
			}
			MemberInfo user = HiContext.Current.User;
			if (user != null)
			{
				Literal literal = this.litCurrentPoints;
				int num2 = user.Points;
				literal.Text = num2.ToString();
				int historyPoints = MemberHelper.GetHistoryPoints(user.UserId);
				this.litHistoryPoints.Text = historyPoints.ToString();
				PointQuery pointQuery = new PointQuery();
				pointQuery.PageIndex = pageIndex;
				pointQuery.PageSize = 10;
				pointQuery.UserId = user.UserId;
				PageModel<PointDetailInfo> userPoints = MemberHelper.GetUserPoints(pointQuery);
				this.rptPointList.DataSource = userPoints.Models;
				this.rptPointList.DataBind();
				HtmlInputHidden control = this.txtTotalPages;
				num2 = userPoints.Total;
				control.SetWhenIsNotNull(num2.ToString());
			}
			PageTitle.AddSiteNameTitle("我的积分");
		}
	}
}
