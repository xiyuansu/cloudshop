using Hidistro.Context;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PrimaryClass : AscxTemplatedWebControl
	{
		private Repeater rp_MainCategorys;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-Common_PrimaryClass.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rp_MainCategorys = (Repeater)this.FindControl("rp_MainCategorys");
			this.rp_MainCategorys.ItemDataBound += this.rp_MainCategorys_ItemDataBound;
			this.rp_MainCategorys.ItemCreated += this.rp_MainCategorys_ItemCreated;
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{HiContext.Current.SiteSettings.Theme}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			int count = int.Parse(xmlNode.Attributes["CategoryNum"].Value);
			IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			if (mainCategories != null)
			{
				this.rp_MainCategorys.DataSource = mainCategories.Take(count);
				this.rp_MainCategorys.DataBind();
			}
		}

		private void rp_MainCategorys_ItemCreated(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("rp_towCategorys");
			if (repeater != null)
			{
				repeater.ItemDataBound += this.rp_towCategorys_ItemDataBound;
			}
		}

		private void rp_MainCategorys_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("rp_towCategorys");
			if (repeater != null)
			{
				int categoryId = ((CategoryInfo)e.Item.DataItem).CategoryId;
				repeater.DataSource = CatalogHelper.GetSubCategories(categoryId);
				repeater.DataBind();
			}
		}

		private void rp_towCategorys_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Control control = e.Item.Controls[0];
			Repeater repeater = (Repeater)control.FindControl("rp_threeCategroys");
			if (repeater != null)
			{
				int categoryId = ((CategoryInfo)e.Item.DataItem).CategoryId;
				repeater.DataSource = CatalogHelper.GetSubCategories(categoryId);
				repeater.DataBind();
			}
		}
	}
}
