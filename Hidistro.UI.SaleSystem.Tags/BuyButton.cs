using Hidistro.Context;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	[ParseChildren(false)]
	[ToolboxData("<{0}:BuyButton runat=\"server\"></{0}:BuyButton>")]
	public class BuyButton : WebControl
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

		public BuyButton()
			: base(HtmlTextWriterTag.Span)
		{
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.CssClass))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "buyButton");
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.UserId != 0)
			{
				writer.Write("<input type=\"hidden\" id=\"hiddenIsLogin\" value=\"logined\" />");
			}
			else
			{
				writer.Write("<input type=\"hidden\" id=\"hiddenIsLogin\" value=\"nologin\" />");
			}
			base.Render(writer);
		}
	}
}
