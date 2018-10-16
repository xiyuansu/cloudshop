using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserPoints : MemberTemplatedWebControl
	{
		private Literal litUserPoint;

		private Literal litMyCoupons;

		private Common_Point_PointList pointList;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserPoints.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.pointList = (Common_Point_PointList)this.FindControl("Common_Point_PointList");
			this.pager = (Pager)this.FindControl("pager");
			this.litUserPoint = (Literal)this.FindControl("litUserPoint");
			this.litMyCoupons = (Literal)this.FindControl("litMyCoupons");
			PageTitle.AddSiteNameTitle("我的积分");
			this.pointList.ItemDataBound += this.pointList_ItemDataBound;
			if (!this.Page.IsPostBack)
			{
				this.BindPoint();
				CouponItemInfo couponItemInfo = new CouponItemInfo();
				couponItemInfo.CouponStatus = 0;
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					Literal literal = this.litUserPoint;
					int num = user.Points;
					literal.Text = num.ToString();
					Literal literal2 = this.litMyCoupons;
					num = CouponHelper.GetUserObtainCouponNum(HiContext.Current.UserId);
					literal2.Text = num.ToString();
				}
			}
		}

		protected void pointList_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Control control = e.Item.Controls[0];
				Label label = (Label)control.FindControl("lblPointType");
				if (label != null)
				{
					if (label.Text == "0")
					{
						label.Text = "兑换优惠券";
					}
					else if (label.Text == "1")
					{
						label.Text = "兑换礼品";
					}
					else if (label.Text == "2")
					{
						label.Text = "购物奖励";
					}
					else if (label.Text == "3")
					{
						label.Text = "退款或关闭还原积分";
					}
					else if (label.Text == "4")
					{
						label.Text = "抽奖获得积分";
					}
					else if (label.Text == "5")
					{
						label.Text = "摇一摇抽奖";
					}
					else if (label.Text == "6")
					{
						label.Text = "每日签到";
					}
					else if (label.Text == "7")
					{
						label.Text = "管理员修改";
					}
					else if (label.Text == "8")
					{
						label.Text = "会员注册";
					}
					else if (label.Text == "9")
					{
						label.Text = "连续签到";
					}
					else if (label.Text == "10")
					{
						label.Text = "评论商品";
					}
					else if (label.Text == "11")
					{
						label.Text = "购物抵扣";
					}
					else if (label.Text == "12")
					{
						label.Text = "大转盘抽奖";
					}
					else if (label.Text == "13")
					{
						label.Text = "刮刮卡抽奖";
					}
					else if (label.Text == "14")
					{
						label.Text = "砸金蛋抽奖";
					}
					else if (label.Text == "15")
					{
						label.Text = "参与微抽奖";
					}
					else if (label.Text == "16")
					{
						label.Text = "商品评论";
					}
					else
					{
						label.Text = "";
					}
				}
			}
		}

		private void BindPoint()
		{
			DbQueryResult userPoints = TradeHelper.GetUserPoints(this.pager.PageIndex);
			this.pointList.DataSource = userPoints.Data;
			this.pointList.DataBind();
			this.pager.TotalRecords = userPoints.TotalRecords;
		}
	}
}
