using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind.BaseClasses
{
	public class AppletTemplatedWebControl : Page
	{
		public void CheckLogin()
		{
			HtmlInputHidden htmlInputHidden = new HtmlInputHidden();
			htmlInputHidden.ID = "hidIsLogin";
			HtmlInputHidden htmlInputHidden2 = htmlInputHidden;
			bool flag = HiContext.Current.UserId > 0 && HiContext.Current.User != null;
			htmlInputHidden2.Value = flag.ToString().ToLower();
			if (this.Page.Controls.Contains(htmlInputHidden))
			{
				htmlInputHidden = (HtmlInputHidden)this.FindControl("hidIsLogin");
				HtmlInputHidden htmlInputHidden3 = htmlInputHidden;
				flag = (HiContext.Current.UserId > 0 && HiContext.Current.User != null);
				htmlInputHidden3.Value = flag.ToString().ToLower();
			}
			else
			{
				this.Page.Controls.Add(htmlInputHidden);
			}
		}

		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);
		}

		protected override void OnInit(EventArgs e)
		{
			string text = this.Page.Request.QueryString["openId"].ToNullString();
			int num = this.Page.Request.QueryString["AppletType"].ToInt(0);
			if (!string.IsNullOrEmpty(text))
			{
				string openType = "hishop.plugins.openid.wxapplet";
				if (num == 2)
				{
					openType = "hishop.plugins.openid.o2owxapplet";
				}
				MemberInfo memberByOpenId = MemberProcessor.GetMemberByOpenId(openType, text);
				if (memberByOpenId != null)
				{
					HiContext.Current.User = memberByOpenId;
					Users.SetCurrentUser(memberByOpenId.UserId, 30, true, false);
				}
			}
			base.OnInit(e);
		}

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
		}

		protected override void OnPreLoad(EventArgs e)
		{
			base.OnPreLoad(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
		}

		protected override void OnLoadComplete(EventArgs e)
		{
			base.OnLoadComplete(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
		}

		protected override void OnSaveStateComplete(EventArgs e)
		{
			base.OnSaveStateComplete(e);
		}

		protected override void OnUnload(EventArgs e)
		{
			base.OnUnload(e);
		}
	}
}
