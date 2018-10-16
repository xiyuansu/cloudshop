using Hidistro.Core;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class PriceLabel : WebControl
	{
		private string displayControlId;

		private string valueControlId;

		private string nullToDisplay = "-";

		public decimal? Value
		{
			get
			{
				if (this.ViewState["Value"] == null)
				{
					return null;
				}
				return (decimal)this.ViewState["Value"];
			}
			set
			{
				if (value.HasValue)
				{
					this.ViewState["Value"] = value.Value;
				}
				else
				{
					this.ViewState["Value"] = null;
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

		public override ControlCollection Controls
		{
			get
			{
				base.EnsureChildControls();
				return base.Controls;
			}
		}

		public PriceLabel(string _displayControlId, string _valueControlId)
			: base(HtmlTextWriterTag.Span)
		{
			this.displayControlId = _displayControlId;
			this.valueControlId = _valueControlId;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.Value.HasValue)
			{
				this.Controls.Add(new LiteralControl(this.Value.Value.F2ToString("f2")));
			}
			else
			{
				this.Controls.Add(new LiteralControl(this.NullToDisplay));
			}
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.displayControlId);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			writer.AddAttribute("id", this.valueControlId);
			writer.AddAttribute("name", this.valueControlId);
			writer.AddAttribute("value", this.Value.HasValue ? this.Value.Value.ToString() : "0");
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
		}
	}
}
