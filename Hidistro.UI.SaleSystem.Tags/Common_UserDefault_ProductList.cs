using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_UserDefault_ProductList : AscxTemplatedWebControl
	{
		private Repeater rp_guest;

		private Repeater rp_hot;

		private Repeater rp_new;

		private int maxNum = 12;

		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_UserDefault_ProductList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rp_guest = (Repeater)this.FindControl("rp_guest");
			this.rp_hot = (Repeater)this.FindControl("rp_hot");
			this.rp_new = (Repeater)this.FindControl("rp_new");
			this.BindList();
		}

		private void BindList()
		{
			if (this.rp_guest != null)
			{
				IList<int> browedProductList = BrowsedProductQueue.GetBrowedProductList(this.MaxNum);
				this.rp_guest.DataSource = ProductBrowser.GetVistiedProducts(browedProductList);
				this.rp_guest.DataBind();
			}
			if (this.rp_hot != null)
			{
				this.rp_hot.DataSource = ProductBrowser.GetSaleProductRanking(0, this.maxNum);
				this.rp_hot.DataBind();
			}
			if (this.rp_new != null)
			{
				SubjectListQuery subjectListQuery = new SubjectListQuery();
				subjectListQuery.MaxNum = this.maxNum;
				subjectListQuery.SortBy = "DisplaySequence";
				this.rp_new.DataSource = ProductBrowser.GetSubjectList(subjectListQuery);
				this.rp_new.DataBind();
			}
		}
	}
}
