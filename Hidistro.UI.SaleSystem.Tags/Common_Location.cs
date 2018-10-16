using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Location : WebControl
	{
		public const string TagID = "common_Location";

		private string separatorString = ">>";

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		public string SeparatorString
		{
			get
			{
				return this.separatorString;
			}
			set
			{
				this.separatorString = value;
			}
		}

		public string CateGoryPath
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public Common_Location()
		{
			base.ID = "common_Location";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(this.CateGoryPath))
			{
				string[] array = this.CateGoryPath.Split('|');
				string[] array2 = array;
				foreach (string s in array2)
				{
					CategoryInfo category = CatalogHelper.GetCategory(int.Parse(s));
					if (category != null)
					{
						string arg = (!string.IsNullOrWhiteSpace(category.RewriteName)) ? base.GetRouteUrl("subCategory_Rewrite", new
						{
							rewrite = category.RewriteName,
							categoryId = category.CategoryId
						}) : base.GetRouteUrl("subCategory", new
						{
							categoryId = category.CategoryId
						});
						stringBuilder.AppendFormat("<a href ='{0}'>{1}</a>{2}", arg, category.Name, this.SeparatorString);
					}
				}
				string text = stringBuilder.ToString();
				if (!string.IsNullOrEmpty(this.ProductName))
				{
					text += this.ProductName;
				}
				else if (text.Length > this.SeparatorString.Length)
				{
					text = text.Remove(text.Length - this.SeparatorString.Length);
				}
				writer.Write(text);
			}
		}
	}
}
