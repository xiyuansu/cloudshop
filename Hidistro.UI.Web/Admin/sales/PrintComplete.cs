using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.sales
{
	public class PrintComplete : AdminPage
	{
		protected string script;

		protected void Page_Load(object sender, EventArgs e)
		{
			string startNumber = base.Request["mailNo"];
			string[] array = base.Request["orderIds"].Split(',');
			List<string> list = new List<string>();
			string[] array2 = array;
			foreach (string str in array2)
			{
				list.Add("'" + str + "'");
			}
			OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), base.Request["templateName"], base.Request["templateName"]);
			OrderHelper.SetOrderShipNumber(array, startNumber, base.Request["templateName"]);
			OrderHelper.SetOrderPrinted(array, true);
		}
	}
}
