using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.Members)]
	public class MemberSearch : AdminPage
	{
		private string lastConsumeTime;

		private string customConsumeStartTime;

		private string customConsumeEndTime;

		private string consumeTimes;

		private int? customStartTimes;

		private int? customEndTimes;

		private string consumePrice;

		private decimal? customStartPrice;

		private decimal? customEndPrice;

		private string orderAvgPrice;

		private decimal? customStartAvgPrice;

		private decimal? customEndAvgPrice;

		private string productCategory;

		private string memberTags;

		private string userGroupType;

		protected HiddenField hidTime;

		protected HiddenField hidNum;

		protected HiddenField hidPrice;

		protected HiddenField hidAvgPrice;

		protected HiddenField hidCategoryId;

		protected HiddenField hidTagId;

		protected HiddenField hidUserGroupType;

		protected HiddenField hidIsOpenApp;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected HtmlInputText txtCustomStartTimes;

		protected HtmlInputText txtCustomEndTimes;

		protected HtmlInputText txtStartPrice;

		protected HtmlInputText txtEndPrice;

		protected HtmlInputText txtStartAvgPrice;

		protected HtmlInputText txtEndAvgPrice;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected Literal litsmscount;

		protected HtmlTextArea txtmsgcontent;

		protected HtmlTextArea txtemailcontent;

		protected HtmlTextArea txtsitecontent;

		protected HtmlInputHidden hdenablemsg;

		protected HtmlInputHidden hdenableemail;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				SiteSettings siteSetting = this.GetSiteSetting();
				int num;
				if (siteSetting.SMSEnabled)
				{
					Literal literal = this.litsmscount;
					num = this.GetAmount(siteSetting);
					literal.Text = num.ToString();
					this.hdenablemsg.Value = "1";
				}
				if (siteSetting.EmailEnabled)
				{
					this.hdenableemail.Value = "1";
				}
				HiddenField hiddenField = this.hidIsOpenApp;
				num = siteSetting.OpenMobbile;
				hiddenField.Value = num.ToString();
			}
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastConsumeTime"]))
				{
					this.lastConsumeTime = base.Server.UrlDecode(this.Page.Request.QueryString["lastConsumeTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["consumeTimes"]))
				{
					this.consumeTimes = base.Server.UrlDecode(this.Page.Request.QueryString["consumeTimes"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["consumePrice"]))
				{
					this.consumePrice = base.Server.UrlDecode(this.Page.Request.QueryString["consumePrice"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderAvgPrice"]))
				{
					this.orderAvgPrice = base.Server.UrlDecode(this.Page.Request.QueryString["orderAvgPrice"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCategory"]))
				{
					this.productCategory = base.Server.UrlDecode(this.Page.Request.QueryString["productCategory"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["memberTags"]))
				{
					this.memberTags = base.Server.UrlDecode(this.Page.Request.QueryString["memberTags"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customConsumeStartTime"]))
				{
					this.customConsumeStartTime = base.Server.UrlDecode(this.Page.Request.QueryString["customConsumeStartTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customConsumeEndTime"]))
				{
					this.customConsumeEndTime = base.Server.UrlDecode(this.Page.Request.QueryString["customConsumeEndTime"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userGroupType"]))
				{
					this.userGroupType = base.Server.UrlDecode(this.Page.Request.QueryString["userGroupType"]);
				}
				int num = 0;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customStartTimes"]) && int.TryParse(this.Page.Request.QueryString["customStartTimes"], out num) && num > 0)
				{
					this.customStartTimes = num;
				}
				int num2 = 0;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customEndTimes"]) && int.TryParse(this.Page.Request.QueryString["customEndTimes"], out num2) && num2 > 0)
				{
					this.customEndTimes = num2;
				}
				decimal value = default(decimal);
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customStartPrice"]) && decimal.TryParse(this.Page.Request.QueryString["customStartPrice"], out value))
				{
					this.customStartPrice = value;
				}
				decimal num3 = default(decimal);
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customEndPrice"]) && decimal.TryParse(this.Page.Request.QueryString["customEndPrice"], out num3) && num3 > decimal.Zero)
				{
					this.customEndPrice = num3;
				}
				decimal value2 = default(decimal);
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customStartAvgPrice"]) && decimal.TryParse(this.Page.Request.QueryString["customStartAvgPrice"], out value2))
				{
					this.customStartAvgPrice = value2;
				}
				decimal num4 = default(decimal);
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["customEndAvgPrice"]) && decimal.TryParse(this.Page.Request.QueryString["customEndAvgPrice"], out num4) && num4 > decimal.Zero)
				{
					this.customEndAvgPrice = num4;
				}
				this.hidTime.Value = this.lastConsumeTime;
				this.hidNum.Value = this.consumeTimes;
				this.hidPrice.Value = this.consumePrice;
				this.hidAvgPrice.Value = this.orderAvgPrice;
				this.hidCategoryId.Value = this.productCategory;
				this.hidTagId.Value = this.memberTags;
				this.hidUserGroupType.Value = this.userGroupType;
				if (!string.IsNullOrEmpty(this.customConsumeStartTime))
				{
					this.calendarStartDate.SelectedDate = this.customConsumeStartTime.ToDateTime();
				}
				if (!string.IsNullOrEmpty(this.customConsumeEndTime))
				{
					this.calendarEndDate.SelectedDate = this.customConsumeEndTime.ToDateTime();
				}
				if (this.customStartTimes.HasValue)
				{
					this.txtCustomStartTimes.Value = this.customStartTimes.ToString();
				}
				if (this.customEndTimes.HasValue)
				{
					this.txtCustomEndTimes.Value = this.customEndTimes.ToString();
				}
				if (this.customStartPrice.HasValue)
				{
					this.txtStartPrice.Value = this.customStartPrice.ToString();
				}
				if (this.customEndPrice.HasValue)
				{
					this.txtEndPrice.Value = this.customEndPrice.ToString();
				}
				if (this.customStartAvgPrice.HasValue)
				{
					this.txtStartAvgPrice.Value = this.customStartAvgPrice.ToString();
				}
				if (this.customEndAvgPrice.HasValue)
				{
					this.txtEndAvgPrice.Value = this.customEndAvgPrice.ToString();
				}
			}
			else
			{
				this.lastConsumeTime = this.hidTime.Value;
				this.consumeTimes = this.hidNum.Value;
				this.consumePrice = this.hidPrice.Value;
				this.orderAvgPrice = this.hidAvgPrice.Value;
				this.productCategory = this.hidCategoryId.Value;
				this.memberTags = this.hidTagId.Value;
				this.userGroupType = this.hidUserGroupType.Value;
				if (this.calendarStartDate.SelectedDate.HasValue && this.calendarEndDate.SelectedDate.HasValue)
				{
					DateTime value3 = this.calendarStartDate.SelectedDate.Value;
					this.customConsumeStartTime = value3.ToString();
					value3 = this.calendarEndDate.SelectedDate.Value;
					this.customConsumeEndTime = value3.ToString();
				}
				if (!string.IsNullOrEmpty(this.txtCustomStartTimes.Value))
				{
					this.customStartTimes = int.Parse(this.txtCustomStartTimes.Value);
				}
				if (!string.IsNullOrEmpty(this.txtCustomEndTimes.Value))
				{
					this.customEndTimes = int.Parse(this.txtCustomEndTimes.Value);
				}
				if (!string.IsNullOrEmpty(this.txtStartPrice.Value) && !string.IsNullOrEmpty(this.txtEndPrice.Value))
				{
					this.customStartPrice = decimal.Parse(this.txtStartPrice.Value);
					this.customEndPrice = decimal.Parse(this.txtEndPrice.Value);
				}
				if (!string.IsNullOrEmpty(this.txtStartAvgPrice.Value) && !string.IsNullOrEmpty(this.txtEndAvgPrice.Value))
				{
					this.customStartAvgPrice = decimal.Parse(this.txtStartAvgPrice.Value);
					this.customEndAvgPrice = decimal.Parse(this.txtEndAvgPrice.Value);
				}
			}
		}

		private SiteSettings GetSiteSetting()
		{
			return SettingsManager.GetMasterSettings();
		}

		protected int GetAmount(SiteSettings settings)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(settings.SMSSettings))
			{
				string xml = HiCryptographer.TryDecypt(settings.SMSSettings);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
				string postData = "method=getAmount&Appkey=" + innerText;
				string text = base.PostData("http://sms.huz.cn/getAmount.aspx", postData);
				int num = default(int);
				if (int.TryParse(text, out num))
				{
					result = Convert.ToInt32(text);
				}
			}
			return result;
		}
	}
}
