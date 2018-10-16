using Hidistro.UI.Common.Controls;
using System.ComponentModel;
using System.Data;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.Tags
{
	[ParseChildren(true)]
	public class Common_ProductConsultations : ThemedTemplatedRepeater
	{
		public const string TagID = "list_Common_ProductConsultations";

		private int maxNum = 6;

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

		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}

		[Browsable(false)]
		public override object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				base.DataSource = value;
			}
		}

		public Common_ProductConsultations()
		{
			base.ID = "list_Common_ProductConsultations";
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			DataTable dataTable = (DataTable)this.DataSource;
			if (dataTable != null && dataTable.Rows.Count > this.maxNum)
			{
				int num = dataTable.Rows.Count - 1;
				for (int num2 = num; num2 >= this.maxNum; num2--)
				{
					dataTable.Rows.RemoveAt(num2);
				}
			}
			base.DataSource = dataTable;
			base.DataBind();
		}
	}
}
