using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPRequestBalanceDraw_Detail : WAPMemberTemplatedWebControl
	{
		private Literal litRequestDate;

		private Literal litAccount;

		private Literal litAccountDate;

		private Literal litStatus;

		private Literal litMark;

		private Literal litManagerRemark;

		private FormatedMoneyLabel litAmount;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RequestBalanceDraw_Detail.html";
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
			this.litManagerRemark = (Literal)this.FindControl("litManagerRemark");
			PageTitle.AddSiteNameTitle("提现详情");
			int num = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["id"]))
			{
				int.TryParse(this.Page.Request.QueryString["id"], out num);
			}
			BalanceDetailDao balanceDetailDao = new BalanceDetailDao();
			BalanceDrawRequestInfo balanceDrawRequestInfo = balanceDetailDao.Get<BalanceDrawRequestInfo>(num);
			if (balanceDrawRequestInfo == null)
			{
				this.ShowMessage("错误的提现记录ID", false, "", 1);
			}
			this.litAmount.Money = balanceDrawRequestInfo.Amount;
			Literal literal = this.litRequestDate;
			DateTime dateTime = balanceDrawRequestInfo.RequestTime;
			literal.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			this.litMark.Text = balanceDrawRequestInfo.Remark;
			this.litManagerRemark.Text = balanceDrawRequestInfo.ManagerRemark;
			this.litAccount.Text = (balanceDrawRequestInfo.IsWeixin.ToBool() ? "提现至微信账号" : (balanceDrawRequestInfo.IsAlipay.ToBool() ? (balanceDrawRequestInfo.AlipayRealName + "(" + balanceDrawRequestInfo.AlipayCode + ")") : (balanceDrawRequestInfo.BankName + balanceDrawRequestInfo.MerchantCode)));
			if (balanceDrawRequestInfo.IsPass.HasValue)
			{
				Literal literal2 = this.litAccountDate;
				dateTime = balanceDrawRequestInfo.AccountDate.Value;
				literal2.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			this.litStatus.Text = ((!balanceDrawRequestInfo.IsPass.HasValue) ? "未审核" : (balanceDrawRequestInfo.IsPass.Value ? "同意" : "拒绝"));
		}
	}
}
