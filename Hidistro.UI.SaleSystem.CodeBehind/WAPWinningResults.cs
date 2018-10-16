using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPWinningResults : WAPMemberTemplatedWebControl
	{
		private HtmlInputText txtName;

		private HtmlInputText txtPhone;

		private Literal litPrizeLevel;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vWinningResults.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int activityid = default(int);
			int.TryParse(HttpContext.Current.Request.QueryString.Get("activityid"), out activityid);
			PrizeRecordInfo userPrizeRecord = VshopBrowser.GetUserPrizeRecord(activityid);
			this.litPrizeLevel = (Literal)this.FindControl("litPrizeLevel");
			if (userPrizeRecord != null)
			{
				this.litPrizeLevel.Text = userPrizeRecord.Prizelevel;
			}
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				this.txtName = (HtmlInputText)this.FindControl("txtName");
				this.txtPhone = (HtmlInputText)this.FindControl("txtPhone");
				this.txtName.Value = user.RealName;
				this.txtPhone.Value = user.CellPhone;
			}
			PageTitle.AddSiteNameTitle("中奖记录");
		}
	}
}
