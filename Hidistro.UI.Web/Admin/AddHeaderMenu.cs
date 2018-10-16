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
	public class AddHeaderMenu : AdminPage
	{
		private string themName;

		protected TextBox txtTitle;

		protected HeaderMenuTypeRadioButtonList radHeaderMenu;

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

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["ThemName"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.themName = base.Request.QueryString["ThemName"];
				this.btnAdd.Click += this.btnAdd_Click;
				if (!base.IsPostBack)
				{
					this.dropSystemPageDropDownList.DataBind();
					this.listProductCategories.DataBind();
					this.listBrandCategories.DataBind();
					this.radProductTags.DataSource = CatalogHelper.GetTags();
					this.radProductTags.DataTextField = "TagName";
					this.radProductTags.DataValueField = "TagID";
					this.radProductTags.DataBind();
					this.radProductTags.Items.Insert(0, new ListItem("--任意--", "0"));
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			string filename = HttpContext.Current.Request.MapPath($"/Templates/master/{this.themName}/config/HeaderMenu.xml");
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("root");
			XmlElement xmlElement = xmlDocument.CreateElement("Menu");
			xmlElement.SetAttribute("Id", this.GetId(xmlNode));
			xmlElement.SetAttribute("Title", this.txtTitle.Text);
			xmlElement.SetAttribute("DisplaySequence", (xmlNode.ChildNodes.Count + 1).ToString());
			xmlElement.SetAttribute("Category", this.txtMenuType.Text);
			xmlElement.SetAttribute("Url", this.GetUrl(this.txtMenuType.Text));
			xmlElement.SetAttribute("Where", this.GetWhere(this.txtMenuType.Text));
			xmlElement.SetAttribute("Visible", "true");
			xmlNode.AppendChild(xmlElement);
			xmlDocument.Save(filename);
			new AspNetCache().Remove("HeadMenuFileCache-Admin");
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/store/SetHeaderMenu.aspx"));
		}

		private string GetId(XmlNode rootNode)
		{
			int num = 1;
			XmlNodeList childNodes = rootNode.ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				if (int.Parse(item.Attributes["Id"].Value) > num)
				{
					num = int.Parse(item.Attributes["Id"].Value);
				}
			}
			return (num + 1).ToString();
		}

		private string GetUrl(string category)
		{
			if (category == "1")
			{
				return this.dropSystemPageDropDownList.SelectedValue;
			}
			if (category == "3")
			{
				return this.txtCustomLink.Text;
			}
			return string.Empty;
		}

		private string GetWhere(string category)
		{
			string text = string.Empty;
			if (category == "2")
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
