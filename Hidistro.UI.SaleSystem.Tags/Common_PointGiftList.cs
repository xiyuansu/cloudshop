using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PointGiftList : AscxTemplatedWebControl
	{
		private Repeater repPointGifts;

		private HtmlGenericControl divGiftList;

		public const string TagID = "Common_PointGiftList";

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

		public object DataSource
		{
			get
			{
				return this.repPointGifts.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repPointGifts.DataSource = value;
			}
		}

		public Common_PointGiftList()
		{
			base.ID = "Common_PointGiftList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common-PointGiftList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.repPointGifts = (Repeater)this.FindControl("repPointGifts");
			this.divGiftList = (HtmlGenericControl)this.FindControl("divGiftList");
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repPointGifts.DataSource != null)
			{
				this.divGiftList.Visible = true;
				this.repPointGifts.DataBind();
			}
		}

		public void ShowPointGifts(bool flag)
		{
			this.divGiftList.Visible = flag;
		}
	}
}
