using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Messages_UserSendedMessageList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, RepeaterCommandEventArgs e);

		public const string TagID = "Common_Messages_UserSendedMessageList";

		private Repeater messagesList;

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
				return this.messagesList.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.messagesList.DataSource = value;
			}
		}

		public event CommandEventHandler ItemCommand;

		public Common_Messages_UserSendedMessageList()
		{
			base.ID = "Common_Messages_UserSendedMessageList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Messages_UserSendedMessageList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.messagesList = (Repeater)this.FindControl("messagesList");
			this.messagesList.ItemCommand += this.messagesList_ItemCommand;
		}

		private void messagesList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.messagesList.DataSource != null)
			{
				this.messagesList.DataBind();
			}
		}
	}
}
