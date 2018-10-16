using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class MemberPriceDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

		private bool _ShowGradeList = true;

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

		public bool ShowGradeList
		{
			get
			{
				return this._ShowGradeList;
			}
			set
			{
				this._ShowGradeList = value;
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
			base.Items.Add(new ListItem("成本价", "-2"));
			base.Items.Add(new ListItem("一口价", "-3"));
			if (this.ShowGradeList)
			{
				IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
				foreach (MemberGradeInfo item in memberGrades)
				{
					this.Items.Add(new ListItem(Globals.HtmlDecode(item.Name + "价"), item.GradeId.ToString()));
				}
			}
		}
	}
}
