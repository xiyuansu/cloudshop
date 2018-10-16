using System.Collections.Generic;
using System.Web;

namespace Hidistro.Entities.VShop
{
	[TableName("vshop_Menu")]
	public class MenuInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int MenuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ParentMenuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Type
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ReplyId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}

		public IList<MenuInfo> Chilren
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public ClientType Client
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Bind
		{
			get;
			set;
		}

		public BindType BindType
		{
			get
			{
				switch (this.Bind)
				{
				case 0:
					return BindType.None;
				case 1:
					return BindType.Key;
				case 2:
					return BindType.Topic;
				case 3:
					return BindType.HomePage;
				case 4:
					return BindType.ProductCategory;
				case 5:
					return BindType.ShoppingCar;
				case 6:
					return BindType.OrderCenter;
				case 7:
					return BindType.MemberCard;
				case 8:
					return BindType.Url;
				default:
					return BindType.None;
				}
			}
		}

		[FieldType(FieldType.CommonField)]
		public string Content
		{
			get;
			set;
		}

		public virtual string BindTypeName
		{
			get
			{
				switch (this.BindType)
				{
				case BindType.Key:
					return "关键字";
				case BindType.Topic:
					return "自定义页面";
				case BindType.HomePage:
					return "首页";
				case BindType.ProductCategory:
					return "分类页";
				case BindType.ShoppingCar:
					return "购物车";
				case BindType.OrderCenter:
					return "会员中心";
				case BindType.MemberCard:
					return "会员卡";
				case BindType.Url:
					return "自定义链接";
				default:
					return string.Empty;
				}
			}
		}

		public string ulrs
		{
			get;
			set;
		}

		public virtual string Url
		{
			get
			{
				string host = HttpContext.Current.Request.Url.Host;
				string text = (this.Client == ClientType.VShop) ? "Vshop" : "AliOH";
				switch (this.BindType)
				{
				case BindType.Key:
					return this.ReplyId.ToString();
				case BindType.Url:
					return this.Content;
				case BindType.Topic:
					return string.Format("http://{0}/{2}/Topics?TopicId={1}", host, this.Content, text);
				case BindType.HomePage:
					return $"http://{host}/{text}/Default";
				case BindType.ProductCategory:
					return $"http://{host}/{text}/ProductSearch";
				case BindType.ShoppingCar:
					return $"http://{host}/{text}/ShoppingCart";
				case BindType.OrderCenter:
					return $"http://{host}/{text}/MemberCenter";
				case BindType.MemberCard:
					return $"http://{host}/{text}/MemberCard";
				default:
					return string.Empty;
				}
			}
		}
	}
}
