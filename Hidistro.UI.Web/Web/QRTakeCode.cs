using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web
{
	public class QRTakeCode : Page
	{
		public string takeCode;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.takeCode = this.Page.Request["takeCode"].ToNullString();
		}
	}
}
