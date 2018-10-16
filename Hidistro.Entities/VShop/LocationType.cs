namespace Hidistro.Entities.VShop
{
	public enum LocationType
	{
		[EnumShowText("自定义页面")]
		Topic,
		[EnumShowText("活动")]
		Activity = 2,
		[EnumShowText("首页")]
		Home,
		[EnumShowText("分类页")]
		Category,
		[EnumShowText("购物车")]
		ShoppingCart,
		[EnumShowText("会员中心")]
		OrderCenter,
		[EnumShowText("链接")]
		Link = 8,
		[EnumShowText("电话")]
		Phone,
		[EnumShowText("团购")]
		GroupBuy = 11,
		[EnumShowText("品牌")]
		Brand,
		[EnumShowText("文章页")]
		Article,
		[EnumShowText("限时抢购")]
		CountDownBuy,
		[EnumShowText("积分商城")]
		PointMall = 0x10,
		[EnumShowText("优惠券列表")]
		CouponList,
		[EnumShowText("注册送券")]
		RegisterCoupon,
		[EnumShowText("选择优惠券")]
		ChoiceCoupone,
		[EnumShowText("周边门店")]
		AroundStores,
		[EnumShowText("火拼团列表")]
		FightGroupList,
		[EnumShowText("订单列表")]
		OrderList,
		[EnumShowText("我的火拼团")]
		MyFightGroups,
		[EnumShowText("商品详情页")]
		ProductDetail
	}
}
