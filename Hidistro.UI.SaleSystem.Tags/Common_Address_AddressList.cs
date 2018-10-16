using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Address_AddressList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, RepeaterCommandEventArgs e);

		public const string TagID = "list_Common_Consignee_ConsigneeList";

		private Repeater repeaterRegionsSelect;

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

		public RepeaterItemCollection Items
		{
			get
			{
				return this.repeaterRegionsSelect.Items;
			}
		}

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterRegionsSelect.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterRegionsSelect.DataSource = value;
			}
		}

		public event CommandEventHandler ItemCommand;

		public Common_Address_AddressList()
		{
			base.ID = "list_Common_Consignee_ConsigneeList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Address_AddressList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.repeaterRegionsSelect = (Repeater)this.FindControl("repeaterRegionsSelect");
			this.repeaterRegionsSelect.ItemCommand += this.repeaterRegionsSelect_ItemCommand;
		}

		private void repeaterRegionsSelect_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.repeaterRegionsSelect.DataSource = this.DataSource;
			this.repeaterRegionsSelect.DataBind();
		}
	}
}
