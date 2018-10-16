/* nav.js zhaokun 20150709 主要应用于首页右侧导航栏 */
$(document).ready(function(){
	$('.tbar-cart-item').hover(function (){ $(this).find('.p-del').show(); },function(){ $(this).find('.p-del').hide(); });
	$('.jth-item').hover(function (){ $(this).find('.add-cart-button').show(); },function(){ $(this).find('.add-cart-button').hide(); });
	$('.toolbar-tab').hover(function () { $(this).find('.tab-text').addClass("tbar-tab-hover"); $(this).find('.footer-tab-text').addClass("tbar-tab-footer-hover"); $(this).addClass("tbar-tab-selected"); }, function () { $(this).find('.tab-text').removeClass("tbar-tab-hover"); $(this).find('.footer-tab-text').removeClass("tbar-tab-footer-hover"); $(this).removeClass("tbar-tab-selected"); });
	$('.tbar-tab-cart').click(function (){ 
		if($('.toolbar-wrap').hasClass('toolbar-open')){
			if($(this).find('.tab-text').length > 0){
				if(! $('.tbar-tab-follow').find('.tab-text').length > 0){
					var info = "<em class='tab-text '>我的关注</em>";
					$('.tbar-tab-follow').append(info);
					$('.tbar-tab-follow').removeClass('tbar-tab-click-selected'); 
					$('.tbar-panel-follow').css({'visibility':"hidden","z-index":"-1"});
				}
				if(! $('.tbar-tab-history').find('.tab-text').length > 0){
					var info = "<em class='tab-text '>优惠券</em>";
					$('.tbar-tab-history').append(info);
					$('.tbar-tab-history').removeClass('tbar-tab-click-selected'); 
					$('.tbar-panel-history').css({'visibility':"hidden","z-index":"-1"});
				}
				$(this).addClass('tbar-tab-click-selected'); 
				$(this).find('.tab-text').remove();
				$('.tbar-panel-cart').css({'visibility':"visible","z-index":"1"});
				
			}else{
				var info = "<em class='tab-text '>我的关注</em>";
				$('.toolbar-wrap').removeClass('toolbar-open');
				$('.toolbar-wrap').animate({position:'fixed',right:'0px'}).removeClass('toolbar-open');
				$(this).append(info);
				$(this).removeClass('tbar-tab-click-selected');
				$('.tbar-panel-cart').css({'visibility':"hidden","z-index":"-1"});
			}
			 
			
		}else{ 
			$(this).addClass('tbar-tab-click-selected'); 
			$(this).find('.tab-text').remove();
			$('.tbar-panel-cart').css({'visibility':"visible","z-index":"1"});
			$('.tbar-panel-follow').css('visibility','hidden');
			$('.tbar-panel-history').css('visibility','hidden');
			$('.toolbar-wrap').addClass('toolbar-open'); 
			$('.toolbar-wrap').animate({position:'fixed',right:'200px'}).addClass('toolbar-open');
		}
	});
	$('.tbar-tab-follow').click(function (){ 
		if($('.toolbar-wrap').hasClass('toolbar-open')){
			if($(this).find('.tab-text').length > 0){
				if(! $('.tbar-tab-cart').find('.tab-text').length > 0){
					var info = "<em class='tab-text '>购物车</em>";
					$('.tbar-tab-cart').append(info);
					$('.tbar-tab-cart').removeClass('tbar-tab-click-selected'); 
					$('.tbar-panel-cart').css({'visibility':"hidden","z-index":"-1"});
				}
				if(! $('.tbar-tab-history').find('.tab-text').length > 0){
					var info = "<em class='tab-text '>优惠券</em>";
					$('.tbar-tab-history').append(info);
					$('.tbar-tab-history').removeClass('tbar-tab-click-selected'); 
					$('.tbar-panel-history').css({'visibility':"hidden","z-index":"-1"});
				}
				$(this).addClass('tbar-tab-click-selected'); 
				$(this).find('.tab-text').remove();
				$('.tbar-panel-follow').css({'visibility':"visible","z-index":"1"});
				
			}else{
				var info = "<em class='tab-text '>我的关注</em>";
				$('.toolbar-wrap').removeClass('toolbar-open');
				$('.toolbar-wrap').animate({position:'fixed',right:'0px'}).removeClass('toolbar-open');
				$(this).append(info);
				$(this).removeClass('tbar-tab-click-selected');
				$('.tbar-panel-follow').css({'visibility':"hidden","z-index":"-1"});
			}
			 
			
		}else{ 
			$(this).addClass('tbar-tab-click-selected'); 
			$(this).find('.tab-text').remove();
			$('.tbar-panel-cart').css('visibility','hidden');
			$('.tbar-panel-follow').css({'visibility':"visible","z-index":"1"});
			$('.tbar-panel-history').css('visibility','hidden');
		$('.toolbar-wrap').addClass('toolbar-open'); 
			$('.toolbar-wrap').animate({position:'fixed',right:'200px'}).addClass('toolbar-open');
		}
	});
	
	$('.tbar-tab-history').click(function (){ 
		if($('.toolbar-wrap').hasClass('toolbar-open')){
			if($(this).find('.tab-text').length > 0){
				if(! $('.tbar-tab-follow').find('.tab-text').length > 0){
					var info = "<em class='tab-text '>我的关注</em>";
					$('.tbar-tab-follow').append(info);
					$('.tbar-tab-follow').removeClass('tbar-tab-click-selected'); 
					$('.tbar-panel-follow').css({'visibility':"hidden","z-index":"-1"});
				}
				if(! $('.tbar-tab-cart').find('.tab-text').length > 0){
					var info = "<em class='tab-text '>购物车</em>";
					$('.tbar-tab-cart').append(info);
					$('.tbar-tab-cart').removeClass('tbar-tab-click-selected'); 
					$('.tbar-panel-cart').css({'visibility':"hidden","z-index":"-1"});
				}
				$(this).addClass('tbar-tab-click-selected'); 
				$(this).find('.tab-text').remove();
				$('.tbar-panel-history').css({'visibility':"visible","z-index":"1"});
				
			}else{
				var info = "<em class='tab-text '>优惠券</em>";
			$('.toolbar-wrap').removeClass('toolbar-open');
				$('.toolbar-wrap').animate({position:'fixed',right:'0px'}).removeClass('toolbar-open');
				$(this).append(info);
				$(this).removeClass('tbar-tab-click-selected');
				$('.tbar-panel-history').css({'visibility':"hidden","z-index":"-1"});
			}
			
		}else{ 
			$(this).addClass('tbar-tab-click-selected'); 
			$(this).find('.tab-text').remove();
			$('.tbar-panel-cart').css('visibility','hidden');
			$('.tbar-panel-follow').css('visibility','hidden');
			$('.tbar-panel-history').css({'visibility':"visible","z-index":"1"});
			$('.toolbar-wrap').addClass('toolbar-open'); 
			$('.toolbar-wrap').animate({position:'fixed',right:'200px'}).addClass('toolbar-open');
		}
	});
	


$(document).click(function(){
	
    $('.toolbar-wrap').animate({position:'fixed',right:'0px'}).removeClass('toolbar-open');
	$('.toolbar-tabs').find('.tbar-tab-click-selected').removeClass('tbar-tab-click-selected');
	$('.coupon2').css('display','none')
	})
	$(".toolbar-wrap").click(
	function stopEvent(){ //阻止冒泡事件
 		var e=arguments.callee.caller.arguments[0]||event; 
 		if (e && e.stopPropagation) { 
 			 e.stopPropagation(); }
 		else if (window.event) { 
  				window.event.cancelBubble = true; 
 } 
 
}
)



	

 

	


	

	
});
