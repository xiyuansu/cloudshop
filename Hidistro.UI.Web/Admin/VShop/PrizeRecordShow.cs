using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class PrizeRecordShow : AdminPage
	{
		protected int activeid;

		protected string userName;

		protected int AwardGrade;

		protected int OrderStatus = 0;

		private int type = 0;

		protected HtmlAnchor alist;

		protected Literal LitListTitle;

		protected Literal Literal1;

		protected HtmlGenericControl lbTotal;

		protected HtmlGenericControl lbCount;

		protected HtmlGenericControl lbWinCount;

		protected HtmlGenericControl lbGetCount;

		protected HtmlGenericControl li1;

		protected HtmlSelect selectAwardItem;

		protected HtmlGenericControl liStoreFilter;

		protected Literal LitTitle;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (int.TryParse(base.Request.QueryString["id"], out this.activeid))
			{
				if (!this.Page.IsPostBack)
				{
					this.BindSelect(this.activeid);
					this.BindPrizeList();
				}
			}
			else
			{
				this.ShowMsg("参数错误", false);
			}
		}

		protected void BindPrizeList()
		{
			this.type = base.Request.QueryString["typeid"].ToInt(0);
			this.alist.HRef = "NewLotteryActivity?type=" + this.type;
			switch (this.type)
			{
			case 1:
			{
				Literal litListTitle4 = this.LitListTitle;
				Literal litTitle4 = this.LitTitle;
				string text3 = litListTitle4.Text = (litTitle4.Text = "大转盘");
				break;
			}
			case 2:
			{
				Literal litListTitle3 = this.LitListTitle;
				Literal litTitle3 = this.LitTitle;
				string text3 = litListTitle3.Text = (litTitle3.Text = "刮刮卡");
				break;
			}
			case 3:
			{
				Literal litListTitle2 = this.LitListTitle;
				Literal litTitle2 = this.LitTitle;
				string text3 = litListTitle2.Text = (litTitle2.Text = "砸金蛋");
				break;
			}
			case 4:
			{
				Literal litListTitle = this.LitListTitle;
				Literal litTitle = this.LitTitle;
				string text3 = litListTitle.Text = (litTitle.Text = "微抽奖");
				this.alist.HRef = "ManageLotteryTicket.aspx";
				break;
			}
			}
			if (!int.TryParse(this.Page.Request.QueryString["Status"], out this.OrderStatus))
			{
				this.OrderStatus = 0;
			}
			this.userName = this.Page.Request.QueryString["userName"];
			this.AwardGrade = this.Page.Request.QueryString["awardGrade"].ToInt(0);
			this.selectAwardItem.Items.FindByValue(this.AwardGrade.ToNullString()).Selected = true;
			DataTable dataTable = ActivityHelper.ActivityStatistics(this.activeid).Tables[0];
			if (dataTable.Rows.Count > 0)
			{
				HtmlGenericControl htmlGenericControl = this.lbTotal;
				int num = dataTable.Rows[0]["JoinUsers"].ToInt(0);
				htmlGenericControl.InnerText = num.ToString();
				HtmlGenericControl htmlGenericControl2 = this.lbCount;
				num = dataTable.Rows[0]["AllJoinNum"].ToInt(0);
				htmlGenericControl2.InnerText = num.ToString();
				HtmlGenericControl htmlGenericControl3 = this.lbWinCount;
				num = dataTable.Rows[0]["AllWinningNum"].ToInt(0);
				htmlGenericControl3.InnerText = num.ToString();
				HtmlGenericControl htmlGenericControl4 = this.lbGetCount;
				num = dataTable.Rows[0]["AlreadyReceive"].ToInt(0);
				htmlGenericControl4.InnerText = num.ToString();
			}
			ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(this.activeid);
			string str = (activityInfo.ActivityName.Length > 25) ? (activityInfo.ActivityName.Substring(0, 25) + "...") : activityInfo.ActivityName;
			this.Literal1.Text = "\"" + str + "\"中奖信息";
		}

		private void BindSelect(int activeid)
		{
			List<ActivityAwardItemInfo> activityItemList = ActivityHelper.GetActivityItemList(activeid);
			ListItem item = new ListItem("请选择奖项", "0");
			this.selectAwardItem.Items.Add(item);
			foreach (ActivityAwardItemInfo item3 in activityItemList)
			{
				ListItem item2 = new ListItem(this.ReturnWeekCN(item3.AwardGrade), string.Concat(item3.AwardGrade));
				this.selectAwardItem.Items.Add(item2);
			}
		}

		public string ReturnWeekCN(int n)
		{
			switch (n)
			{
			case 1:
				return "一等奖";
			case 2:
				return "二等奖";
			case 3:
				return "三等奖";
			case 4:
				return "四等奖";
			case 5:
				return "五等奖";
			case 6:
				return "六等奖";
			default:
				return "";
			}
		}
	}
}
