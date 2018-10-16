/*
 * jQuery JavaScript Plugin jquery.zoomImgRollover 0.9.0
 * http://bugsoftware.co.uk/jQuery/
 *
 * Copyright (c) 2010 Ritchie Comley
 * Dual licensed under the MIT and GPL licenses.
 *
 * Date: 2010-01-19 (Thu, 19 January 2010)
 * Revision: 29
 *
 * Dependencies:
 * jQuery 1.4 (jquery.com)
 * 
 */

(function(jQuery){ 

	jQuery.fn.zoomImgRollover = function(options) {

		var defaults = {
			percent:30,
			duration:600
		}; 

		var opts = jQuery.extend(defaults, options);
		
		// static zoom function
		function imageZoomStep(jZoomImage, x, origWidth, origHeight)
		{
			var width = Math.round(origWidth * (.5 + ((x * opts.percent) / 200))) * 2;
			var height = Math.round(origHeight * (.5 + ((x * opts.percent) / 200))) * 2;
				
			var left = (width - origWidth) / 2;
			var top = (height - origHeight) / 2;
		
			jZoomImage.css({width:width, height:height, top:-top, left:-left});
		}

		return this.each(function()
		{
			var jZoomImage = jQuery(this);
			var origWidth = jZoomImage.width();
			var origHeight = jZoomImage.height();
			
			// add css ness. to allow zoom
			jZoomImage.css({position: "relative"});
			jZoomImage.parent().css({overflow: "hidden", display:"block", position: "relative", width: origWidth, height: origHeight});
			
			jZoomImage.mouseover(function()
			{
				jZoomImage.stop().animate({dummy:1},{duration:opts.duration, step:function(x)
				{
					imageZoomStep(jZoomImage, x, origWidth, origHeight)
				}});
			});

			jZoomImage.mouseout(function()
			{
				jZoomImage.stop().animate({dummy:0},{duration:opts.duration, step:function(x)
				{
					imageZoomStep(jZoomImage, x, origWidth, origHeight)
				}});
			});
		});
	};

})(jQuery);


(function(jQuery){ 

	jQuery.fn.zoomImgRollover2 = function(options) {

		var defaults = {
			percent:5,
			duration:600
		}; 

		var opts = jQuery.extend(defaults, options);
		
		// static zoom function
		function imageZoomStep(jZoomImage, x, origWidth, origHeight)
		{
			var width = Math.round(origWidth * (.5 + ((x * opts.percent) / 200))) * 2;
			var height = Math.round(origHeight * (.5 + ((x * opts.percent) / 200))) * 2;
				
			var left = (width - origWidth) / 2;
			var top = (height - origHeight) / 2;
		
			jZoomImage.css({width:width, height:height, top:-top, left:-left});
		}

		return this.each(function()
		{
			var jZoomImage = jQuery(this);
			var origWidth = jZoomImage.width();
			var origHeight = jZoomImage.height();
			
			// add css ness. to allow zoom
			jZoomImage.css({position: "relative"});
			jZoomImage.parent().css({overflow: "hidden", display:"block", position: "relative", width: origWidth, height: origHeight});
			
			jZoomImage.mouseover(function()
			{
				jZoomImage.stop().animate({dummy:1},{duration:opts.duration, step:function(x)
				{
					imageZoomStep(jZoomImage, x, origWidth, origHeight)
				}});
			});

			jZoomImage.mouseout(function()
			{
				jZoomImage.stop().animate({dummy:0},{duration:opts.duration, step:function(x)
				{
					imageZoomStep(jZoomImage, x, origWidth, origHeight)
				}});
			});
		});
	};

})(jQuery);