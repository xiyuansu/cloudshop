using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Point_PointList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_Point_PointList";

		private Repeater repeaterPointDetails;

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterPointDetails.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterPointDetails.DataSource = value;
			}
		}

		public event RepeaterItemEventHandler ItemDataBound;

		public Common_Point_PointList()
		{
			base.ID = "Common_Point_PointList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/Tags/Common_UserCenter/Skin-Common_UserPointList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.repeaterPointDetails = (Repeater)this.FindControl("repeaterPointDetails");
			this.repeaterPointDetails.ItemDataBound += this.repeaterPointDetails_ItemDataBound;
		}

		private void repeaterPointDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			this.ItemDataBound(sender, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterPointDetails.DataSource != null)
			{
				this.repeaterPointDetails.DataBind();
			}
		}
	}
}
