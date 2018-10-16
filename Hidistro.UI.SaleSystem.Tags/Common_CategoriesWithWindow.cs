using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CategoriesWithWindow : AscxTemplatedWebControl
	{
		private Repeater recordsone;

		private int maxCNum = 13;

		private int maxBNum = 1000;

		public int MaxCNum
		{
			get
			{
				return this.maxCNum;
			}
			set
			{
				this.maxCNum = value;
			}
		}

		public int MaxBNum
		{
			get
			{
				return this.maxBNum;
			}
			set
			{
				this.maxBNum = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/HomeTags/Skin-CategoriesWithWindow.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.recordsone = (Repeater)this.FindControl("recordsone");
			this.recordsone.ItemDataBound += this.recordsone_ItemDataBound;
			this.recordsone.ItemCreated += this.recordsone_ItemCreated;
			IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			if (mainCategories != null)
			{
				this.recordsone.DataSource = mainCategories.Take(this.MaxCNum);
				this.recordsone.DataBind();
			}
		}

		private void recordsone_ItemCreated(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("recordstwo");
			repeater.ItemDataBound += this.recordstwo_ItemDataBound;
		}

		private void recordsone_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("recordstwo");
			Repeater repeater2 = (Repeater)control.FindControl("repMainTow");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)control.FindControl("hidMainCategoryId");
			Repeater repeater3 = (Repeater)control.FindControl("recordsbrands");
			Repeater repeater4 = (Repeater)control.FindControl("rphotkey");
			repeater3.DataSource = CatalogHelper.GetBrandCategories(int.Parse(htmlInputHidden.Value), 12);
			repeater3.DataBind();
			Repeater repeater5 = repeater2;
			Repeater repeater6 = repeater;
			object obj2 = repeater5.DataSource = (repeater6.DataSource = CatalogHelper.GetSubCategories(int.Parse(htmlInputHidden.Value)));
			repeater.DataBind();
			repeater2.DataBind();
			if (repeater4 != null)
			{
				repeater4.DataSource = CommentBrowser.GetHotKeywords(int.Parse(htmlInputHidden.Value), 12);
				repeater4.DataBind();
			}
		}

		private void recordstwo_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("recordsthree");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)control.FindControl("hidTwoCategoryId");
			repeater.DataSource = CatalogHelper.GetSubCategories(int.Parse(htmlInputHidden.Value));
			repeater.DataBind();
		}
	}
}
