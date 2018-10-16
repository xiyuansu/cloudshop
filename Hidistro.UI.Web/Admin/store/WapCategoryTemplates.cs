using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.store
{
	[PrivilegeCheck(Privilege.SetWapCTemplates)]
	public class WapCategoryTemplates : AdminPage
	{
		protected HtmlGenericControl divpwap;

		protected HtmlGenericControl divwaptem1;

		protected Button btnwaptem1;

		protected HtmlGenericControl divwaptem2;

		protected Button btnwaptem2;

		protected HtmlGenericControl divwaptem3;

		protected Button btnwaptem3;

		protected HtmlGenericControl divwaptem0;

		protected Button btnwaptem0;

		protected HtmlGenericControl hrmline;

		protected HtmlGenericControl divpapp;

		protected HtmlGenericControl divapptem1;

		protected Button btnapptem1;

		protected HtmlGenericControl divapptem2;

		protected Button btnapptem2;

		protected HtmlGenericControl divapptem3;

		protected Button btnapptem3;

		protected HtmlGenericControl divapptem0;

		protected Button btnapptem0;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.InitBind();
			}
		}

		private void InitBind()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			Button button = this.btnwaptem0;
			Button button2 = this.btnwaptem1;
			Button button3 = this.btnwaptem2;
			Button button4 = this.btnwaptem3;
			string text2 = button4.Text = "应用";
			string text4 = button3.Text = text2;
			string text7 = button.Text = (button2.Text = text4);
			Button button5 = this.btnwaptem0;
			Button button6 = this.btnwaptem1;
			Button button7 = this.btnwaptem2;
			Button button8 = this.btnwaptem3;
			text2 = (button8.CssClass = "btn btn-primary");
			text4 = (button7.CssClass = text2);
			text7 = (button5.CssClass = (button6.CssClass = text4));
			Button button9 = this.btnwaptem0;
			Button button10 = this.btnwaptem1;
			Button button11 = this.btnwaptem2;
			Button button12 = this.btnwaptem3;
			bool flag2 = button12.Enabled = true;
			bool flag4 = button11.Enabled = flag2;
			bool enabled = button10.Enabled = flag4;
			button9.Enabled = enabled;
			AttributeCollection attributes = this.divwaptem0.Attributes;
			AttributeCollection attributes2 = this.divwaptem1.Attributes;
			AttributeCollection attributes3 = this.divwaptem2.Attributes;
			AttributeCollection attributes4 = this.divwaptem3.Attributes;
			text2 = (attributes4["class"] = "pic");
			text4 = (attributes3["class"] = text2);
			text7 = (attributes["class"] = (attributes2["class"] = text4));
			switch (masterSettings.VCategoryTemplateStatus)
			{
			case 1:
				this.divwaptem1.Attributes["class"] = "pic2";
				this.btnwaptem1.CssClass = "btn btn-default";
				this.btnwaptem1.Enabled = false;
				this.btnwaptem1.Text = "已应用";
				break;
			case 2:
				this.divwaptem2.Attributes["class"] = "pic2";
				this.btnwaptem2.CssClass = "btn btn-default";
				this.btnwaptem2.Text = "已应用";
				this.btnwaptem2.Enabled = false;
				break;
			case 3:
				this.divwaptem3.Attributes["class"] = "pic2";
				this.btnwaptem3.CssClass = "btn btn-default";
				this.btnwaptem3.Text = "已应用";
				this.btnwaptem3.Enabled = false;
				break;
			default:
				this.divwaptem0.Attributes["class"] = "pic2";
				this.btnwaptem0.CssClass = "btn btn-default";
				this.btnwaptem0.Text = "已应用";
				this.btnwaptem0.Enabled = false;
				break;
			}
			Button button13 = this.btnapptem0;
			Button button14 = this.btnapptem1;
			Button button15 = this.btnapptem2;
			Button button16 = this.btnapptem3;
			text2 = (button16.Text = "应用");
			text4 = (button15.Text = text2);
			text7 = (button13.Text = (button14.Text = text4));
			Button button17 = this.btnapptem0;
			Button button18 = this.btnapptem1;
			Button button19 = this.btnapptem2;
			Button button20 = this.btnapptem3;
			text2 = (button20.CssClass = "btn btn-primary");
			text4 = (button19.CssClass = text2);
			text7 = (button17.CssClass = (button18.CssClass = text4));
			Button button21 = this.btnapptem0;
			Button button22 = this.btnapptem1;
			Button button23 = this.btnapptem2;
			Button button24 = this.btnapptem3;
			flag2 = (button24.Enabled = true);
			flag4 = (button23.Enabled = flag2);
			enabled = (button22.Enabled = flag4);
			button21.Enabled = enabled;
			AttributeCollection attributes5 = this.divapptem0.Attributes;
			AttributeCollection attributes6 = this.divapptem1.Attributes;
			AttributeCollection attributes7 = this.divapptem2.Attributes;
			AttributeCollection attributes8 = this.divapptem3.Attributes;
			text2 = (attributes8["class"] = "pic");
			text4 = (attributes7["class"] = text2);
			text7 = (attributes5["class"] = (attributes6["class"] = text4));
			switch (masterSettings.AppCategoryTemplateStatus)
			{
			case 1:
				this.divapptem1.Attributes["class"] = "pic2";
				this.btnapptem1.CssClass = "btn btn-default";
				this.btnapptem1.Text = "已应用";
				this.btnapptem1.Enabled = false;
				break;
			case 2:
				this.divapptem2.Attributes["class"] = "pic2";
				this.btnapptem2.CssClass = "btn btn-default";
				this.btnapptem2.Text = "已应用";
				this.btnapptem2.Enabled = false;
				break;
			case 3:
				this.divapptem3.Attributes["class"] = "pic2";
				this.btnapptem3.CssClass = "btn btn-default";
				this.btnapptem3.Text = "已应用";
				this.btnapptem3.Enabled = false;
				break;
			default:
				this.divapptem0.Attributes["class"] = "pic2";
				this.btnapptem0.CssClass = "btn btn-default";
				this.btnapptem0.Text = "已应用";
				this.btnapptem0.Enabled = false;
				break;
			}
			int num = 0;
			if (masterSettings.OpenWap == 0 && masterSettings.OpenVstore == 0 && masterSettings.OpenAliho == 0)
			{
				num++;
				this.divpwap.Visible = false;
			}
			if (masterSettings.OpenMobbile == 0)
			{
				num++;
				this.divpapp.Visible = false;
			}
			if (num > 0)
			{
				this.hrmline.Visible = false;
			}
			if (num >= 2)
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
			}
		}

		private void SetSiteSave(int type, int ctStatus)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			switch (type)
			{
			case 0:
				masterSettings.VCategoryTemplateStatus = ctStatus;
				break;
			case 1:
				masterSettings.AppCategoryTemplateStatus = ctStatus;
				break;
			}
			SettingsManager.Save(masterSettings);
			this.InitBind();
			this.ShowMsg("成功修改了分类模板！", true);
		}

		protected void btnwaptem0_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(0, 0);
		}

		protected void btnwaptem1_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(0, 1);
		}

		protected void btnwaptem2_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(0, 2);
		}

		protected void btnwaptem3_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(0, 3);
		}

		protected void btnapptem0_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(1, 0);
		}

		protected void btnapptem1_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(1, 1);
		}

		protected void btnapptem2_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(1, 2);
		}

		protected void btnapptem3_Click(object sender, EventArgs e)
		{
			this.SetSiteSave(1, 3);
		}
	}
}
