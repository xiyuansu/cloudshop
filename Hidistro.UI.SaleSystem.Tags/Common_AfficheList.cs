using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AfficheList : ThemedTemplatedRepeater
	{
		private int maxNum;

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

		protected override void OnLoad(EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				base.DataSource = this.GetDataSource();
				base.DataBind();
			}
		}

		private IList<AfficheInfo> GetDataSource()
		{
			IList<AfficheInfo> afficheList = CommentBrowser.GetAfficheList(true);
			if (this.MaxNum > 0 && this.MaxNum < afficheList.Count)
			{
				for (int num = afficheList.Count - 1; num >= this.MaxNum; num--)
				{
					afficheList.RemoveAt(num);
				}
			}
			return afficheList;
		}
	}
}
