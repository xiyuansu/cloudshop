using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPSplittinDraw_Detail : WAPMemberTemplatedWebControl
	{
		private Literal litRequestDate;

		private Literal litAccount;

		private Literal litAccountDate;

		private Literal litStatus;

		private Literal litMark;

		private FormatedMoneyLabel litAmount;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplitinDraw_Detail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litAmount = (FormatedMoneyLabel)this.FindControl("litAmount");
			this.litRequestDate = (Literal)this.FindControl("litRequestDate");
			this.litAccount = (Literal)this.FindControl("litAccount");
			this.litAccountDate = (Literal)this.FindControl("litAccountDate");
			this.litStatus = (Literal)this.FindControl("litStatus");
			this.litMark = (Literal)this.FindControl("litMark");
			PageTitle.AddSiteNameTitle("提现详情");
			long journalNumber = 0L;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
			{
				long.TryParse(this.Page.Request.QueryString["id"], out journalNumber);
			}
			SplittinDrawInfo splittinDraw = MemberProcessor.GetSplittinDraw(journalNumber);
			if (splittinDraw == null)
			{
				this.ShowMessage("错误的提现记录ID", false, "", 1);
			}
			this.litAmount.Money = splittinDraw.Amount;
			Literal literal = this.litRequestDate;
			DateTime dateTime = splittinDraw.RequestDate;
			literal.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			this.litMark.Text = splittinDraw.ManagerRemark;
			this.litAccount.Text = (splittinDraw.IsWithdrawToAccount.ToBool() ? "至预付款账户" : (splittinDraw.IsWeixin.ToBool() ? "提现至微信账号" : (splittinDraw.IsAlipay.ToBool() ? (splittinDraw.AlipayRealName + "(" + splittinDraw.AlipayCode + ")") : (splittinDraw.BankName + splittinDraw.MerchantCode))));
			if (splittinDraw.AuditStatus != 1 && splittinDraw.AccountDate.HasValue)
			{
				Literal literal2 = this.litAccountDate;
				dateTime = splittinDraw.AccountDate.Value;
				literal2.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			this.litStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)(SplitDrawStatus)splittinDraw.AuditStatus, 0);
		}
	}
}
