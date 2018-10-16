using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Favorite_ProductList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);

		public const string TagID = "list_Common_Favorite_ProList";

		private DataList dtlstFavorite;

		private RepeatDirection repeatDirection;

		private int repeatColumns = 1;

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
				return this.dtlstFavorite.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dtlstFavorite.DataSource = value;
			}
		}

		public DataListItemCollection Items
		{
			get
			{
				return this.dtlstFavorite.Items;
			}
		}

		public DataKeyCollection DataKeys
		{
			get
			{
				return this.dtlstFavorite.DataKeys;
			}
		}

		public int EditItemIndex
		{
			get
			{
				return this.dtlstFavorite.EditItemIndex;
			}
			set
			{
				this.dtlstFavorite.EditItemIndex = value;
			}
		}

		public RepeatDirection RepeatDirection
		{
			get
			{
				return this.repeatDirection;
			}
			set
			{
				this.repeatDirection = value;
			}
		}

		public int RepeatColumns
		{
			get
			{
				return this.repeatColumns;
			}
			set
			{
				this.repeatColumns = value;
			}
		}

		public event CommandEventHandler ItemCommand;

		public Common_Favorite_ProductList()
		{
			base.ID = "list_Common_Favorite_ProList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Favorite_ProductList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dtlstFavorite = (DataList)this.FindControl("dtlstFavorite");
			this.dtlstFavorite.RepeatDirection = this.RepeatDirection;
			this.dtlstFavorite.RepeatColumns = this.RepeatColumns;
			this.dtlstFavorite.ItemCommand += this.dtlstFavorite_ItemCommand;
		}

		private void dtlstFavorite_ItemCommand(object source, DataListCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.dtlstFavorite.DataSource = this.DataSource;
			this.dtlstFavorite.DataBind();
		}
	}
}
