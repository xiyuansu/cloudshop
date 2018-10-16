using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Themes)]
	public class EditHeaderMenu : AdminPage
	{
		private string themName;

		private int id;

		private string title;

		private string category;

		private string url;

		private string where;

		protected TextBox txtTitle;

		protected HtmlGenericControl txtTitleTip;

		protected TrimTextBox txtMenuType;

		protected SystemPageDropDownList dropSystemPageDropDownList;

		protected TextBox txtMinPrice;

		protected TextBox txtMaxPrice;

		protected TextBox txtKeyword;

		protected ProductCategoriesListBox listProductCategories;

		protected BrandCategoriesList listBrandCategories;

		protected ListBox radProductTags;

		protected TextBox txtCustomLink;

		protected HtmlGenericControl txtCustomLinkTip;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["ThemName"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.themName = base.Request.QueryString["ThemName"];
				int.TryParse(base.Request.QueryString["Id"], out this.id);
				string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
				foreach (XmlNode item in childNodes)
				{
					if (item.Attributes["Id"].Value == this.id.ToString())
					{
						this.title = item.Attributes["Title"].Value;
						this.category = item.Attributes["Category"].Value;
						this.url = item.Attributes["Url"].Value;
						this.where = item.Attributes["Where"].Value;
						break;
					}
				}
				this.btnSave.Click += this.btnSave_Click;
				if (!base.IsPostBack)
				{
					this.txtMenuType.Text = this.category;
					this.dropSystemPageDropDownList.DataBind();
					this.listProductCategories.DataBind();
					this.listBrandCategories.DataBind();
					this.radProductTags.DataSource = CatalogHelper.GetTags();
					this.radProductTags.DataTextField = "TagName";
					this.radProductTags.DataValueField = "TagID";
					this.radProductTags.DataBind();
					this.radProductTags.Items.Insert(0, new ListItem("--任意--", "0"));
					this.txtTitle.Text = this.title;
					if (this.category == "1")
					{
						this.dropSystemPageDropDownList.SelectedValue = this.url;
					}
					else if (this.category == "3")
					{
						this.txtCustomLink.Text = this.url;
					}
					else
					{
						string[] array = this.where.Split(',');
						int selectedCategoryId = 0;
						if (int.TryParse(array[0], out selectedCategoryId))
						{
							this.listProductCategories.SelectedCategoryId = selectedCategoryId;
						}
						this.listBrandCategories.SelectedValue = array[1];
						this.radProductTags.SelectedValue = array[2];
						this.txtMinPrice.Text = array[3];
						this.txtMaxPrice.Text = array[4];
						this.txtKeyword.Text = array[5];
					}
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNodeList childNodes = xmlDocument.SelectSingleNode("root").ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				if (item.Attributes["Id"].Value == this.id.ToString())
				{
					item.Attributes["Title"].Value = this.txtTitle.Text;
					item.Attributes["Url"].Value = this.GetUrl();
					item.Attributes["Where"].Value = this.GetWhere();
					break;
				}
			}
			xmlDocument.Save(filename);
			new AspNetCache().Remove("HeadMenuFileCache-Admin");
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/store/SetHeaderMenu.aspx"));
		}

		private string GetUrl()
		{
			if (this.category == "1")
			{
				return this.dropSystemPageDropDownList.SelectedValue;
			}
			if (this.category == "3")
			{
				return this.txtCustomLink.Text;
			}
			return string.Empty;
		}

		private string GetWhere()
		{
			string text = string.Empty;
			if (this.category == "2")
			{
				decimal num = default(decimal);
				decimal num2 = default(decimal);
				text = text + this.listProductCategories.SelectedCategoryId.ToString() + "," + this.listBrandCategories.SelectedValue + "," + this.radProductTags.SelectedValue + ",";
				if (decimal.TryParse(this.txtMinPrice.Text, out num))
				{
					text += this.txtMinPrice.Text;
				}
				text += ",";
				if (decimal.TryParse(this.txtMaxPrice.Text, out num2))
				{
					text += this.txtMaxPrice.Text;
				}
				text += ",";
				text += this.txtKeyword.Text;
			}
			return text;
		}
	}
}
