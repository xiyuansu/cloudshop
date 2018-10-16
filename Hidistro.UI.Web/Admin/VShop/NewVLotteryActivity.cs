using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class NewVLotteryActivity : AdminPage
	{
		protected HtmlAnchor alist;

		protected Literal LitListTitle;

		protected Literal LitTitle;

		protected TextBox txtActiveName;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected TextBox txtShareDetail;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected OnOff ooOpen;

		protected TextBox txtMaxNum;

		protected TextBox txtUsePoints;

		protected TextBox txtdesc;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = base.Request["act"];
			string a = text;
			if (a == "GetYHQ")
			{
				this.GetYHQ();
			}
			if (!base.IsPostBack)
			{
				this.SetDateControl();
				this.ooOpen.Parameter.Add("onSwitchChange", "fuenableDeduct");
			}
		}

		public void GetYHQ()
		{
			List<CouponInfo> allCoupons = CouponHelper.GetAllCoupons();
			StringBuilder stringBuilder = new StringBuilder();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.Serialize(allCoupons, stringBuilder);
			base.Response.Write(stringBuilder);
			base.Response.End();
		}

		private void SetDateControl()
		{
			Dictionary<string, object> calendarParameter = this.calendarStartDate.CalendarParameter;
			DateTime now = DateTime.Now;
			calendarParameter.Add("startDate ", now.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter2 = this.calendarEndDate.CalendarParameter;
			now = DateTime.Now;
			calendarParameter2.Add("startDate ", now.ToString("yyyy-MM-dd"));
			this.calendarStartDate.FunctionNameForChangeDate = "fuChangeStartDate";
			this.calendarEndDate.FunctionNameForChangeDate = "fuChangeEndDate";
			this.calendarStartDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarStartDate.CalendarParameter["minView"] = "0";
			this.calendarEndDate.CalendarParameter["format"] = "yyyy-mm-dd hh:ii:00";
			this.calendarEndDate.CalendarParameter["minView"] = "0";
		}
	}
}
