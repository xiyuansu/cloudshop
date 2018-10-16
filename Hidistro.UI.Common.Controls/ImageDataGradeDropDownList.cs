using Hidistro.SaleSystem.Store;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ImageDataGradeDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
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

		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			base.Items.Add(new ListItem("默认分类", "0"));
			DataTable photoCategories = GalleryHelper.GetPhotoCategories(0);
			foreach (DataRow row in photoCategories.Rows)
			{
				base.Items.Add(new ListItem(row["CategoryName"].ToString(), row["CategoryId"].ToString()));
			}
		}
	}
}
