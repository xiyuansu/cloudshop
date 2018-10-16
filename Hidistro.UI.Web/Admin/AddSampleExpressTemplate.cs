using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ExpressTemplates)]
	public class AddSampleExpressTemplate : AdminPage
	{
		protected HtmlSelect printItems;

		protected void Page_Load(object sender, EventArgs e)
		{
			string value = this.Page.Request.QueryString["ExpressName"];
			string text = this.Page.Request.QueryString["XmlFile"];
			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(text) || !text.EndsWith(".xml"))
			{
				base.GotoResourceNotFound();
			}
			else if (!base.IsPostBack)
			{
				this.BindDefindDataItem();
			}
		}

		private void BindDefindDataItem()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/DataItems/Item");
			foreach (XmlNode item in xmlNodeList)
			{
				this.printItems.Items.Add(new ListItem("自定义-" + item.ChildNodes[0].InnerText, "自定义-" + item.ChildNodes[0].InnerText));
			}
		}
	}
}
