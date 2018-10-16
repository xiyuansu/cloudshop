using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Messages_UserReceivedMessageList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, RepeaterCommandEventArgs e);

		public delegate void DataBindEventHandler(object sender, RepeaterItemEventArgs e);

		public const string TagID = "Grid_Common_Messages_UserReceivedMessageList";

		private Repeater repeaterMessageList;

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
				return this.repeaterMessageList.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterMessageList.DataSource = value;
			}
		}

		public event CommandEventHandler ItemCommand;

		public event DataBindEventHandler ItemDataBound;

		public Common_Messages_UserReceivedMessageList()
		{
			base.ID = "Grid_Common_Messages_UserReceivedMessageList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserReceivedMessageList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.repeaterMessageList = (Repeater)this.FindControl("repeaterMessageList");
			this.repeaterMessageList.ItemCommand += this.repeaterMessageList_ItemCommand;
		}

		private void repeaterMessageList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterMessageList.DataSource != null)
			{
				this.repeaterMessageList.DataBind();
			}
		}
	}
}
