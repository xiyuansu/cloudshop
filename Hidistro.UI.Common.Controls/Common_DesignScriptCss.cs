using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class Common_DesignScriptCss : Literal
	{
		private string css = "blue.css";

		private string srcFormat = "<script src=\"/utility/jquery-1.8.3.min.js\" type=\"text/javascript\"></script> <script src=\"/utility/jquery.artDialog.js\" type=\"text/javascript\"></script><script src=\"/dialogtemplates/js/Hidistro_Dialog.js\" type=\"text/javascript\"></script><script src=\"/utility/jquery_hashtable.js\" type=\"text/javascript\"></script><script src=\"/utility/jquery.scrollLoading.min.js\" type=\"text/javascript\"></script><script src=\"/dialogtemplates/js/Hidistro_Design.js\" type=\"text/javascript\"></script><link rel=\"stylesheet\" href=\"/dialogtemplates/css/design.css\" type=\"text/css\"/><link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\"/>";

		public virtual string Css
		{
			get
			{
				if (!this.css.StartsWith("/"))
				{
					this.css = "/utility/skins/" + this.css;
				}
				return this.css;
			}
			set
			{
				this.css = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(this.Css))
			{
				writer.Write(this.srcFormat, this.Css);
			}
		}
	}
}
