using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class FormatedMoneyLabel : Label
	{
		private string nullToDisplay = "-";

		public object Money
		{
			get
			{
				if (this.ViewState["Money"] == null)
				{
					return null;
				}
				return this.ViewState["Money"];
			}
			set
			{
				if (value == null || value == DBNull.Value)
				{
					this.ViewState["Money"] = null;
				}
				else
				{
					this.ViewState["Money"] = value;
				}
			}
		}

		public object ShowZero
		{
			get
			{
				if (this.ViewState["ShowZero"] == null)
				{
					return null;
				}
				return this.ViewState["ShowZero"];
			}
			set
			{
				if (value == null || value == DBNull.Value)
				{
					this.ViewState["ShowZero"] = null;
				}
				else
				{
					this.ViewState["ShowZero"] = value;
				}
			}
		}

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			bool flag = this.ShowZero == null || this.ShowZero.ToBool();
			try
			{
				if (this.Money != null && this.Money != DBNull.Value)
				{
					string text = Globals.FormatMoney((decimal)this.Money);
					if (!flag)
					{
						if (text == "0.00")
						{
							base.Text = "";
						}
						else
						{
							base.Text = text;
						}
					}
					else
					{
						base.Text = text;
					}
				}
			}
			catch
			{
				base.Text = "";
			}
			if (string.IsNullOrEmpty(base.Text) & flag)
			{
				base.Text = this.NullToDisplay;
			}
			base.Render(writer);
		}
	}
}
