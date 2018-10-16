using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPSplittinRuleDetail : WAPTemplatedWebControl
	{
		private Literal lblSubMemberDeduct;

		private Literal lblSecondLevelDeduct;

		private Literal lblThreeLevelDeduct;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinRuleDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.lblSubMemberDeduct = (Literal)this.FindControl("lblSubMemberDeduct");
			this.lblSecondLevelDeduct = (Literal)this.FindControl("lblSecondLevelDeduct");
			this.lblThreeLevelDeduct = (Literal)this.FindControl("lblThreeLevelDeduct");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.lblSubMemberDeduct.Text = masterSettings.SubMemberDeduct.F2ToString("f2");
			this.lblSecondLevelDeduct.Text = masterSettings.SecondLevelDeduct.F2ToString("f2");
			this.lblThreeLevelDeduct.Text = masterSettings.ThreeLevelDeduct.F2ToString("f2");
			if (!masterSettings.OpenMultReferral)
			{
				this.FindControl("secondDeduct").Visible = false;
				this.FindControl("threeDeduct").Visible = false;
			}
		}
	}
}
