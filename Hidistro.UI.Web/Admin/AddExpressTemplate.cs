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
	[PrivilegeCheck(Privilege.AddExpressTemplate)]
	public class AddExpressTemplate : AdminPage
	{
		protected string ems = "";

		protected HtmlSelect printItems;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindDefindDataItem();
				DataTable expressTable = ExpressHelper.GetExpressTable();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (DataRow row in expressTable.Rows)
				{
					stringBuilder.AppendFormat("<option value='{0}'>{0}</option>", row["Name"]);
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
