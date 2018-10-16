using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_UserRefunds : AscxTemplatedWebControl
	{
		public const string TagID = "Common_UserRefunds";

		private Repeater rptUserReturns;

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.rptUserReturns.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.rptUserReturns.DataSource = value;
			}
		}

		public Common_UserRefunds()
		{
			base.ID = "Common_UserRefunds";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserRefunds.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptUserReturns = (Repeater)this.FindControl("rptUserRefunds");
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rptUserReturns.DataSource != null)
			{
				this.rptUserReturns.DataBind();
			}
		}
	}
}
