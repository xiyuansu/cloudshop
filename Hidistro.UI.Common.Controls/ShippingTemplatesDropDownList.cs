using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ShippingTemplatesDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

		private bool _ShowNoSetTemplates = false;

		private IList<int> _FilterValuationMethods = new List<int>();

		private string _NoSetTempaltesDisplay = "未设置运费模板";

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

		public bool ShowNoSetTempaltes
		{
			get
			{
				return this._ShowNoSetTemplates;
			}
			set
			{
				this._ShowNoSetTemplates = value;
			}
		}

		public IList<int> FilterValuationMethods
		{
			get
			{
				return this._FilterValuationMethods;
			}
			set
			{
				this._FilterValuationMethods = value;
			}
		}

		public string NoSetTemplatesDisplay
		{
			get
			{
				return this._NoSetTempaltesDisplay;
			}
			set
			{
				this._NoSetTempaltesDisplay = value;
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
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			if (this.ShowNoSetTempaltes)
			{
				base.Items.Add(new ListItem(this.NoSetTemplatesDisplay, "0"));
			}
			IList<ShippingTemplateInfo> shippingAllTemplates = SalesHelper.GetShippingAllTemplates();
			string text = "";
			foreach (ShippingTemplateInfo item in shippingAllTemplates)
			{
				if (this.FilterValuationMethods.Count <= 0 || !this.FilterValuationMethods.Contains((int)item.ValuationMethod))
				{
					ListItemCollection items = this.Items;
					string text2 = Globals.HtmlDecode(item.TemplateName);
					int num = item.TemplateId;
					items.Add(new ListItem(text2, num.ToString()));
					string str = text;
					string str2 = (text == "") ? "" : ",";
					object str3;
					if (!item.IsFreeShipping)
					{
						num = (int)item.ValuationMethod;
						str3 = num.ToString();
					}
					else
					{
						str3 = "1";
					}
					text = str + str2 + (string)str3;
				}
			}
			base.Attributes.Add("ValuationMethodList", text);
		}
	}
}
