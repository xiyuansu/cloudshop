using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ClassShowOnDataLitl : Literal
	{
		public string Style
		{
			get
			{
				if (this.ViewState["Style"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["Style"];
			}
			set
			{
				this.ViewState["Style"] = value;
			}
		}

		public string Class
		{
			get
			{
				if (this.ViewState["Class"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["Class"];
			}
			set
			{
				this.ViewState["Class"] = value;
			}
		}

		public string DefaultText
		{
			get
			{
				if (this.ViewState["DefaultText"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["DefaultText"];
			}
			set
			{
				this.ViewState["DefaultText"] = value;
			}
		}

		public bool IsShowLink
		{
			get
			{
				if (this.ViewState["IsShowLink"] == null)
				{
					return false;
				}
				return (bool)this.ViewState["IsShowLink"];
			}
			set
			{
				this.ViewState["IsShowLink"] = value;
			}
		}

		public string Link
		{
			get
			{
				if (this.ViewState["Link"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["Link"] + this.LinkQuery;
			}
			set
			{
				this.ViewState["Link"] = value;
			}
		}

		public string LinkQuery
		{
			get
			{
				if (this.ViewState["LinkQuery"] == null)
				{
					return string.Empty;
				}
				return (string)this.ViewState["LinkQuery"];
			}
			set
			{
				this.ViewState["LinkQuery"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(base.Text))
			{
				base.Text = $"<span>{this.DefaultText}</span>";
			}
			else
			{
				base.Text = $"<span style=\"{this.Style}\" class=\"{this.Class}\">{base.Text}</span>";
				if (this.IsShowLink)
				{
					base.Text = $"<a href=\"{this.Link}\">{base.Text}</a>";
				}
			}
			base.Render(writer);
		}
	}
}
