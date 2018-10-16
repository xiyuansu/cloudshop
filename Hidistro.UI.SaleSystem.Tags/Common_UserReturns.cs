using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_UserReturns : AscxTemplatedWebControl
	{
		public const string TagID = "Common_UserReturns";

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

		public Common_UserReturns()
		{
			base.ID = "Common_UserReturns";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserReturns.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptUserReturns = (Repeater)this.FindControl("rptUserReturns");
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
