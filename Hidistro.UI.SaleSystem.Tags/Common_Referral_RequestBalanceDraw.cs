using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Referral_RequestBalanceDraw : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Referral_RequestBalance";

		private Repeater rptRequestBalanceDraws;

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.rptRequestBalanceDraws.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.rptRequestBalanceDraws.DataSource = value;
			}
		}

		public Common_Referral_RequestBalanceDraw()
		{
			base.ID = "Common_Referral_RequestBalance";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_Referral_RequestBalanceDraw.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptRequestBalanceDraws = (Repeater)this.FindControl("rptRequestBalanceDraws");
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rptRequestBalanceDraws.DataSource != null)
			{
				this.rptRequestBalanceDraws.DataBind();
			}
		}
	}
}
