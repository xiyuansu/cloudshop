using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class StockLabel : WebControl
	{
		public string ShowText
		{
			get;
			set;
		}

		public int Stock
		{
			get
			{
				if (this.ViewState["Stock"] == null)
				{
					return 0;
				}
				return (int)this.ViewState["Stock"];
			}
			set
			{
				this.ViewState["Stock"] = value;
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

		public StockLabel()
			: base(HtmlTextWriterTag.Span)
		{
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!string.IsNullOrEmpty(this.ShowText))
			{
				this.Controls.Add(new LiteralControl(this.ShowText.ToString()));
			}
			else
			{
				this.Controls.Add(new LiteralControl(this.Stock.ToString()));
			}
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute("stock", this.Stock.ToString());
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "productDetails_Stock");
		}
	}
}
