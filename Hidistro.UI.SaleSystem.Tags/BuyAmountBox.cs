using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	[ParseChildren(false)]
	[ToolboxData("<{0}:BuyAmountBox runat=\"server\"></{0}:BuyAmountBox>")]
	public class BuyAmountBox : WebControl
	{
		private string boxStyle = "width:30px;";

		public int Quantity
		{
			get
			{
				if (this.ViewState["Quantity"] == null)
				{
					return 1;
				}
				return (int)this.ViewState["Quantity"];
			}
			set
			{
				this.ViewState["Quantity"] = value;
			}
		}

		public int MinQuantity
		{
			get
			{
				if (this.ViewState["MinQuantity"] == null)
				{
					return 1;
				}
				return (int)this.ViewState["MinQuantity"];
			}
			set
			{
				this.ViewState["MinQuantity"] = value;
			}
		}

		public int? MaxQuantity
		{
			get
			{
				if (this.ViewState["MaxQuantity"] == null)
				{
					return null;
				}
				return (int)this.ViewState["MaxQuantity"];
			}
			set
			{
				if (value.HasValue)
				{
					this.ViewState["MaxQuantity"] = value.Value;
				}
				else
				{
					this.ViewState["MaxQuantity"] = null;
				}
			}
		}

		public string BoxStyle
		{
			get
			{
				return this.boxStyle;
			}
			set
			{
				this.boxStyle = value;
			}
		}

		public override ControlCollection Controls
		{
			get
			{
				base.EnsureChildControls();
				return base.Controls;
			}
		}

		public BuyAmountBox()
			: base(HtmlTextWriterTag.Span)
		{
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			WebControl webControl = new WebControl(HtmlTextWriterTag.Input);
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				webControl.Attributes.Add("class", base.CssClass);
			}
			if (!string.IsNullOrEmpty(this.BoxStyle))
			{
				webControl.Attributes.Add("style", this.BoxStyle);
			}
			webControl.Attributes.Add("id", "buyAmount");
			webControl.Attributes.Add("type", "text");
			AttributeCollection attributes = webControl.Attributes;
			int quantity = this.Quantity;
			attributes.Add("value", quantity.ToString(CultureInfo.InvariantCulture));
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Input);
			webControl2.Attributes.Add("id", "oldBuyNumHidden");
			webControl2.Attributes.Add("type", "hidden");
			AttributeCollection attributes2 = webControl2.Attributes;
			quantity = this.Quantity;
			attributes2.Add("value", quantity.ToString(CultureInfo.InvariantCulture));
			this.Controls.Add(webControl);
			this.Controls.Add(webControl2);
		}
	}
}
