using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RequestBalanceDrawIsPassLable : Label
	{
		public object IsPass
		{
			get;
			set;
		}

		public object RequestState
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			if (this.IsPass == null || string.IsNullOrEmpty(this.IsPass.ToString()))
			{
				base.Text = "审核中";
				if (this.RequestState.ToInt(0) > 1)
				{
					base.Text = "处理中";
				}
			}
			try
			{
				if (Convert.ToBoolean(this.IsPass))
				{
					base.Text = "同意";
				}
				else
				{
					base.Text = "拒绝";
				}
			}
			catch
			{
			}
			base.Render(writer);
		}
	}
}
