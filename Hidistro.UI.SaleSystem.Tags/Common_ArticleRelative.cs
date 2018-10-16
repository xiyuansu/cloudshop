using Hidistro.UI.Common.Controls;
using System.ComponentModel;
using System.Data;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ArticleRelative : ThemedTemplatedRepeater
	{
		public const string TagID = "list_Common_ArticleRelative";

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

		public Common_ArticleRelative()
		{
			base.ID = "list_Common_ArticleRelative";
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
