using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ExpressTemplates)]
	public class EditExpressTemplate : AdminPage
	{
		protected string ems = "";

		protected string width = "";

		protected string height = "";

		protected HtmlSelect printItems;

		protected void Page_Load(object sender, EventArgs e)
		{
			int num = 0;
			string text = this.Page.Request.QueryString["ExpressName"];
			string text2 = this.Page.Request.QueryString["XmlFile"];
			if (!int.TryParse(this.Page.Request.QueryString["ExpressId"], out num))
			{
				base.GotoResourceNotFound();
			}
			else if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || !text2.EndsWith(".xml"))
			{
				base.GotoResourceNotFound();
			}
			else if (!base.IsPostBack)
			{
				this.BindDefindDataItem();
				DataTable expressTable = ExpressHelper.GetExpressTable();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(HttpContext.Current.Request.MapPath($"/Storage/master/flex/{text2}"));
				XmlNode xmlNode = xmlDocument.SelectSingleNode("/printer/size");
				string innerText = xmlNode.InnerText;
				this.width = innerText.Split(':')[0];
				this.height = innerText.Split(':')[1];
				StringBuilder stringBuilder = new StringBuilder();
				foreach (DataRow row in expressTable.Rows)
				{
					stringBuilder.AppendFormat("<option value='{0}' {1}>{0}</option>", row["Name"], row["Name"].Equals(text) ? "selected" : "");
				}
				this.ems = stringBuilder.ToString();
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
