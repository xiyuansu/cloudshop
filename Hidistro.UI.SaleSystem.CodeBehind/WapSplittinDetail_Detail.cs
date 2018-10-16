using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapSplittinDetail_Detail : WAPMemberTemplatedWebControl
	{
		private Literal litOrderNo;

		private Literal litSplittinDate;

		private Literal litStatus;

		private Literal litMark;

		private SplittingTypeNameLabel litSplittinType;

		private FormatedMoneyLabel litOrderAmount;

		private FormatedMoneyLabel litSplittinAmount;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDetail_Detail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litOrderAmount = (FormatedMoneyLabel)this.FindControl("litOrderAmount");
			this.litSplittinAmount = (FormatedMoneyLabel)this.FindControl("litSplittinAmount");
			this.litOrderNo = (Literal)this.FindControl("litOrderNo");
			this.litSplittinType = (SplittingTypeNameLabel)this.FindControl("litSplittinType");
			this.litSplittinDate = (Literal)this.FindControl("litSplittinDate");
			this.litStatus = (Literal)this.FindControl("litStatus");
			this.litMark = (Literal)this.FindControl("litMark");
			PageTitle.AddSiteNameTitle("奖励明细");
			long journalNumber = 0L;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
			{
				long.TryParse(this.Page.Request.QueryString["id"], out journalNumber);
			}
			SplittinDetailInfo splittinDetail = MemberProcessor.GetSplittinDetail(journalNumber);
			if (splittinDetail == null)
			{
				this.ShowMessage("错误的明细ID", false, "", 1);
			}
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(splittinDetail.OrderId);
			if (!string.IsNullOrEmpty(splittinDetail.OrderId) && orderInfo != null)
			{
				this.litOrderAmount.Money = orderInfo.GetAmount(false);
			}
			else
			{
				this.litOrderAmount.Money = 0;
			}
			this.litSplittinAmount.Money = splittinDetail.Income;
			this.litOrderNo.Text = splittinDetail.OrderId;
			this.litMark.Text = splittinDetail.Remark;
			this.litSplittinType.SplittingType = ((int)splittinDetail.TradeType).ToString();
			this.litSplittinDate.Text = splittinDetail.TradeDate.ToString("yyyy-MM-dd HH:mm:ss");
			if (splittinDetail.TradeType == SplittingTypes.DrawRequest)
			{
				this.litSplittinAmount.Money = splittinDetail.Expenses;
			}
			this.litStatus.Text = (splittinDetail.IsUse ? "可用" : "不可用");
		}
	}
}
