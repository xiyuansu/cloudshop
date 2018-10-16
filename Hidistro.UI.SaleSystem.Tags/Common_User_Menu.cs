using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_User_Menu : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "ascx/tags/Common_UserCenter/Skin-Common_User_Menu.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			Literal literal = (Literal)this.FindControl("messageNum");
			HtmlGenericControl htmlGenericControl = (HtmlGenericControl)this.FindControl("liReferralRegisterAgreement");
			HtmlGenericControl htmlGenericControl2 = (HtmlGenericControl)this.FindControl("liReferralLink");
			HtmlGenericControl htmlGenericControl3 = (HtmlGenericControl)this.FindControl("liReferralSplittin");
			HtmlGenericControl htmlGenericControl4 = (HtmlGenericControl)this.FindControl("liSubReferral");
			HtmlGenericControl htmlGenericControl5 = (HtmlGenericControl)this.FindControl("liListReferral");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)this.FindControl("hidAction");
			IDictionary<string, int> statisticsNum = MemberProcessor.GetStatisticsNum();
			literal.Text = statisticsNum["noReadMessageNum"].ToString();
			try
			{
				string obj = this.Page.Request.Url.Segments[this.Page.Request.Url.Segments.Count() - 1];
				char[] separator = new char[1]
				{
					'.'
				};
				string text2 = htmlInputHidden.Value = obj.Split(separator)[0];
			}
			catch
			{
				htmlInputHidden.Value = string.Empty;
			}
			MemberInfo user = HiContext.Current.User;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenReferral == 1)
			{
				if (user.UserId != 0 && user.IsReferral())
				{
					htmlGenericControl.Visible = false;
					htmlGenericControl2.Visible = true;
					htmlGenericControl3.Visible = true;
					htmlGenericControl4.Visible = true;
				}
				else
				{
					htmlGenericControl.Visible = true;
					htmlGenericControl2.Visible = false;
					htmlGenericControl3.Visible = false;
					htmlGenericControl4.Visible = false;
				}
			}
			else
			{
				htmlGenericControl5.Visible = false;
			}
		}
	}
}
