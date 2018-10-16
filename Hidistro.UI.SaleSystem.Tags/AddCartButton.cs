using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	[ParseChildren(false)]
	[ToolboxData("<{0}:AddCartButton runat=\"server\"></{0}:AddCartButton>")]
	public class AddCartButton : WebControl
	{
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

		public AddCartButton()
			: base(HtmlTextWriterTag.Span)
		{
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "addcartButton");
		}
	}
}
