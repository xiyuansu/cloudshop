using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Referral_SplittinDraw : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Referral_Splitin";

		private Repeater rptSplittinDraws;

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.rptSplittinDraws.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.rptSplittinDraws.DataSource = value;
			}
		}

		public Common_Referral_SplittinDraw()
		{
			base.ID = "Common_Referral_Splitin";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Referral_SplittinDraw.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptSplittinDraws = (Repeater)this.FindControl("rptSplittinDraws");
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rptSplittinDraws.DataSource != null)
			{
				this.rptSplittinDraws.DataBind();
			}
		}
	}
}
