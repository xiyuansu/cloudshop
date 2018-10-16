using Newtonsoft.Json;
using System;

namespace Hidistro.UI.Common.Controls
{
	public class AdminCallBackPage : AdminPage
	{
		protected string JsCallBack = string.Empty;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.JsCallBack = this.Page.Request["callback"];
		}

		protected void CloseWindowGo(string url)
		{
			base.CloseWindow(url);
		}

		protected override void ShowMsgCloseWindow(string msg, bool success)
		{
			if (!string.IsNullOrWhiteSpace(this.JsCallBack))
			{
				this.CloseWindow(new
				{
					msg,
					success
				});
			}
			else
			{
				base.ShowMsgCloseWindow(msg, success);
			}
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
						this.JsCallBack = this.JsCallBack + "\"" + data.ToString() + "\"";
					}
					else
					{
						this.JsCallBack += JsonConvert.SerializeObject(data);
					}
				}
				this.JsCallBack += ");";
				this.ArtDialogRunScript(this.JsCallBack);
			}
			else
			{
				base.CloseWindow();
			}
		}
	}
}
