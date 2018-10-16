using Newtonsoft.Json;
using System;

namespace Hidistro.UI.Common.Controls
{
	public class StoreAdminCallBackPage : StoreAdminPage
	{
		protected string JsCallBack = string.Empty;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.JsCallBack = this.Page.Request["callback"];
		}

		protected void CloseWindow(object data = null)
		{
			if (!string.IsNullOrWhiteSpace(this.JsCallBack))
			{
				this.JsCallBack = "artwin." + this.JsCallBack + "(";
				if (data != null)
				{
					if (data is string)
					{
						this.JsCallBack += data.ToString();
					}
					else
					{
						this.JsCallBack += JsonConvert.SerializeObject(data);
					}
				}
				this.JsCallBack += ")";
				this.ArtDialogRunScript(this.JsCallBack);
			}
			else
			{
				base.CloseWindow();
			}
		}
	}
}
