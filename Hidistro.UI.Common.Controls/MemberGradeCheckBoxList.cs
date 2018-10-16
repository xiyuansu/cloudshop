using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class MemberGradeCheckBoxList : CheckBoxList
	{
		public new IList<int> SelectedValue
		{
			get
			{
				IList<int> list = new List<int>();
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						list.Add(int.Parse(this.Items[i].Value));
					}
				}
				return list;
			}
			set
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					this.Items[i].Selected = false;
				}
				foreach (int item in value)
				{
					for (int j = 0; j < this.Items.Count; j++)
					{
						if (this.Items[j].Value == item.ToString())
						{
							this.Items[j].Selected = true;
						}
					}
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].Attributes.Add("class", "icheck");
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
			int num = 0;
			foreach (MemberGradeInfo item in memberGrades)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(item.Name), item.GradeId.ToString()));
				this.Items[num++].Selected = true;
			}
		}
	}
}
