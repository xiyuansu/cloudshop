using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace Hidistro.UI.Web.Admin.sales
{
	public class AddPrintDataItem : AdminPage
	{
		protected TextBox txtName;

		protected TextBox txtContent;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			if (this.IsExistsName(this.txtName.Text.Trim()))
			{
				this.ShowMsg("不能添加重复的自定义项", false);
			}
			else
			{
				XDocument xDocument = XDocument.Load(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
				XElement xElement = new XElement("Item");
				XElement xElement2 = new XElement("Name");
				XElement xElement3 = new XElement("Content");
				xElement2.Value = this.txtName.Text.Trim();
				xElement3.Value = this.txtContent.Text.Trim();
				xElement.Add(xElement2);
				xElement.Add(xElement3);
				xDocument.Root.Add(xElement);
				xDocument.Save(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
				this.CloseWindow();
			}
		}

		private bool IsExistsName(string name)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/DataItems/Item");
			foreach (XmlNode item in xmlNodeList)
			{
				string text = item.ChildNodes[0].InnerText.Trim();
				if (text.Equals(name.Trim()))
				{
					return true;
				}
			}
			return false;
		}
	}
}
