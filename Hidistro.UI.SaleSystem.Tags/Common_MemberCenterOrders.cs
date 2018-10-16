using Hidistro.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_MemberCenterOrders : WAPTemplatedWebControl
	{
		public delegate void DataBindEventHandler(object sender, RepeaterItemEventArgs e);

		public delegate void CommandEventHandler(object sender, RepeaterCommandEventArgs e);

		public const string TagID = "Common_MemberCenterOrder";

		private Repeater listOrders;

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
				return this.listOrders.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listOrders.DataSource = value;
			}
		}

		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}

		public event DataBindEventHandler ItemDataBound;

		public event CommandEventHandler ItemCommand;

		public Common_MemberCenterOrders()
		{
			base.ID = "Common_MemberCenterOrder";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/tags/skin-Common_MemberCenterOrders.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.listOrders = (Repeater)this.FindControl("listOrders");
			this.listOrders.ItemDataBound += this.listOrders_ItemDataBound;
			this.listOrders.ItemCommand += this.listOrders_RowCommand;
		}

		private void listOrders_RowCommand(object sender, RepeaterCommandEventArgs e)
		{
			this.ItemCommand(sender, e);
		}

		private void listOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			this.ItemDataBound(sender, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listOrders.DataSource = this.DataSource;
			this.listOrders.DataBind();
		}
	}
}
