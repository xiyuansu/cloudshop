using Hidistro.Context;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion.Ascx
{
	public class PromotionView : UserControl
	{
		private PromotionInfo promotion;

		protected HiddenField hidOpenMultStore;

		protected TextBox txtPromoteSalesName;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected MemberGradeCheckBoxList chklMemberGrade;

		protected Ueditor fckDescription;

		protected ImageList ImageList;

		public PromotionInfo Promotion
		{
			get
			{
				PromotionInfo promotionInfo = new PromotionInfo();
				promotionInfo.Name = this.txtPromoteSalesName.Text;
				if (this.calendarStartDate.SelectedDate.HasValue)
				{
					promotionInfo.StartDate = this.calendarStartDate.SelectedDate.Value;
				}
				if (this.calendarEndDate.SelectedDate.HasValue)
				{
					PromotionInfo promotionInfo2 = promotionInfo;
					DateTime dateTime = this.calendarEndDate.SelectedDate.Value;
					dateTime = dateTime.AddHours(23.0);
					dateTime = dateTime.AddMinutes(59.0);
					promotionInfo2.EndDate = dateTime.AddSeconds(59.0);
				}
				promotionInfo.MemberGradeIds = this.chklMemberGrade.SelectedValue;
				promotionInfo.Description = this.fckDescription.Text;
				return promotionInfo;
			}
			set
			{
				this.promotion = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.chklMemberGrade.DataBind();
				if (this.promotion != null)
				{
					this.txtPromoteSalesName.Text = this.promotion.Name;
					this.calendarStartDate.SelectedDate = this.promotion.StartDate;
					this.calendarEndDate.SelectedDate = this.promotion.EndDate;
					this.chklMemberGrade.SelectedValue = this.promotion.MemberGradeIds;
					this.fckDescription.Text = this.promotion.Description;
				}
				if (SettingsManager.GetMasterSettings().OpenMultStore && base.Request.RawUrl.ToString().ToLower().IndexOf("orderpromotion") > -1 && string.IsNullOrEmpty(base.Request.QueryString["isWholesale"]))
				{
					this.hidOpenMultStore.Value = "1";
				}
				else
				{
					this.hidOpenMultStore.Value = "0";
				}
			}
		}
	}
}
