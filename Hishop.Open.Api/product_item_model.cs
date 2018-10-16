using System;
using System.Collections.Generic;

namespace Hishop.Open.Api
{
	public class product_item_model
	{
		private IList<string> _picurl;

		private IList<product_sku_model> _skus;

		public string desc;

		public string wap_desc;

		public int cid
		{
			get;
			set;
		}

		public string cat_name
		{
			get;
			set;
		}

		public int brand_id
		{
			get;
			set;
		}

		public string brand_name
		{
			get;
			set;
		}

		public int type_id
		{
			get;
			set;
		}

		public string type_name
		{
			get;
			set;
		}

		public int num_iid
		{
			get;
			set;
		}

		public string outer_id
		{
			get;
			set;
		}

		public string title
		{
			get;
			set;
		}

		public IList<string> pic_url
		{
			get
			{
				if (this._picurl == null)
				{
					this._picurl = new List<string>();
				}
				return this._picurl;
			}
			set
			{
				this._picurl = value;
			}
		}

		public DateTime? list_time
		{
			get;
			set;
		}

		public DateTime? modified
		{
			get;
			set;
		}

		public int display_sequence
		{
			get;
			set;
		}

		public string approve_status
		{
			get;
			set;
		}

		public int sold_quantity
		{
			get;
			set;
		}

		public string location
		{
			get;
			set;
		}

		public string props_name
		{
			get;
			set;
		}

		public int sub_stock
		{
			get
			{
				int num = 0;
				foreach (product_sku_model sku in this.skus)
				{
					num += sku.quantity;
				}
				return num;
			}
		}

		public IList<product_sku_model> skus
		{
			get
			{
				if (this._skus == null)
				{
					this._skus = new List<product_sku_model>();
				}
				return this._skus;
			}
			set
			{
				this._skus = value;
			}
		}
	}
}
