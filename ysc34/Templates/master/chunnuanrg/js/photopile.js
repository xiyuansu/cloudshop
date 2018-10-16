//
// File: photopile.js
// Auth: Brian W. Howell
// Date: 8 May 2014
//
// Photopile image gallery
//
var photopile = (function() {

    //---------------------------------------------------------------------------------------------
    //  PHOTOPILE SETTINGS
    //---------------------------------------------------------------------------------------------

    // Thumbnails
    var numLayers         = 5;          // number of layers in the pile (max zindex)
    var thumbOverlap      = 50;         // overlap amount (px)
    var thumbRotation     = 45;         // maximum rotation (deg)
    var thumbBorderWidth  = 2;          // border width (px)
    var thumbBorderColor  = 'white';    // border color
    var thumbBorderHover  = '#6DB8FF';  // border hover color
    var draggable         = true;       // enable draggable thumbnails

    // Photo container
    var fadeDuration      = 200;        // speed at which photo fades (ms)
    var pickupDuration    = 500;        // speed at which photo is picked up & put down (ms)
    var photoZIndex       = 100;        // z-index (show above all)
    var photoBorder       = 10;         // border width around fullsize image
    var photoBorderColor  = 'white';    // border color
    var showInfo          = true;       // include photo description (alt tag) in photo container

    // Images
    var loading    = 'images/loading.gif';  // path to img displayed while gallery/thumbnails loads

    //---- END SETTINGS ----

    // Initializes Photopile
    function init() {

        // display gallery loading image in container div while loading
        $('.js div.photopile-wrapper').css({
            'background-repeat'   : 'no-repeat',
            'background-position' : '50%, 50%',
            'background-image'    : 'url(' + loading + ')'
        });

        // initialize thumbnails and photo container
        $('ul.photopile').children().each( function() { 
            thumb.init($(this));
        });
        photo.init();

        // once gallery has loaded completely
        $(window).load(function() {
            $('.js div.photopile-wrapper').css({  // style container
                'padding' : thumbOverlap + 'px',
                'background-image' : 'none'
            }).children().css({  // display thumbnails
                'opacity' : '0',
                'display' : 'inline-block'
            }).fadeTo(fadeDuration, 1);
            navigator.init();  // init navigator
        });
    
    } // init

    //-----------------------------------------------------
    // THUMBNAIL
    // List-item containing a link to a fullsize image
    //-----------------------------------------------------

    var thumb = {

        active : 'photopile-active-thumbnail',  // active (or clicked) thumbnail class name

        // Initializes thumbnail.
        init : function( thumb ) {
            thumb.children().css( 'padding', thumbBorderWidth + 'px' );
            this.bindUIActions(thumb);
            this.setRotation(thumb);
            this.setOverlap(thumb);
            this.setRandomZ(thumb);
            if (draggable) {
                thumb.draggable({
                    start: function(event, ui) { thumb.addClass('preventClick'); }
                });  
            }
            thumb.css('background', thumbBorderColor );
        },

        // Binds UI actions to thumbnail.
        bindUIActions : function( thumb ) {
            var self = this;
       
            // Mouseover | Move to top of pile and change border color.
            thumb.mouseover( function() { 
                $(this).css({
                    'z-index'    : numLayers + 1,
                    'background' : thumbBorderHover 
                });
            });

            // Mouseout | Move down one layer and return to default border color.
            thumb.mouseout( function() { 
                $(this).css({
                    'z-index'    : numLayers,
                    'background' : thumbBorderColor
                });
            });

            // Pickup the thumbnail on click (if not being dragged).
            thumb.click( function(e) {
                e.preventDefault();
                if ($(this).hasClass('preventClick')) {
                    $(this).removeClass('preventClick');
                } else {
                    if ($(this).hasClass(self.active)) return;
                    photo.pickup( $(this) );
                }
            });

            // Prevent user from having to double click thumbnail after dragging.
            thumb.mousedown( function(e) { $(this).removeClass('preventClick'); });

        }, // bindUIActions

        // Setters for various thumbnail properties.
        setOverlap  : function( thumb ) { thumb.css( 'margin', ((thumbOverlap * -1) / 2) + 'px' ); },
        setZ        : function( thumb, layer ) { thumb.css( 'z-index', layer ); },
        setRandomZ  : function( thumb ) { thumb.css({ 'z-index' : Math.floor((Math.random() * numLayers) + 1) }); },
        setRotation : function( thumb ) {
            var min = -1 * thumbRotation;
            var max = thumbRotation;
            var randomRotation = Math.floor( Math.random() * (max - min + 1)) + min;
            thumb.css({ 'transform' : 'rotate(' + randomRotation + 'deg)' });
        },

        // ----- Active thumbnail -----

        // Sets the active thumbnail.
        setActive : function( thumb ) { thumb.addClass(this.active); },

        // Getters for active thumbnail properties
        getActiveOffset   : function() { return $('li.' + this.active).offset(); },
        getActiveHeight   : function() { return $('li.' + this.active).height(); },
        getActiveWidth    : function() { return $('li.' + this.active).width(); },
        getActiveImgSrc   : function() { return $('li.' + this.active).children().first().attr('href'); },
        getActiveRotation : function() {
                var transform = $('li.' + this.active).css("transform");
                var values = transform.split('(')[1].split(')')[0].split(',');
                var angle = Math.round( Math.asin( values[1]) * (180/Math.PI) );
                return angle;
        },

        // Gets the active thumbnail if set, or returns false.
        getActive : function() { 
            return ($('li.' + this.active)[0]) ? $('li.' + this.active).first() : false;
        },

        // Returns a shift amount used to better position the photo container 
        // on top of the active thumb. Needed because offset is skewed by thumbnail's rotation.
        getActiveShift : function() {
            return ( this.getActiveRotation() < 0 )
                ? -( this.getActiveRotation(thumb) * 0.40 )
                :  ( this.getActiveRotation(thumb) * 0.40 );
        },

        // Removes the active class from all thumbnails.
        clearActiveClass : function() { $('li.' + this.active).fadeTo(fadeDuration, 1).removeClass(this.active); }

    } // thumbnail
 
    //--------------------------------------------------------------------
    // PHOTO CONTAINER
    // Dynamic container div wrapping an img element that displays the 
    // fullsize image associated with the active thumbnail
    //--------------------------------------------------------------------

    var photo = {

        // Photo container elements
        container : $( '<div id="photopile-active-image-container"/>' ), 
        image     : $( '<img id="photopile-active-image" />'),
        info      : $( '<div id="photopile-active-image-info"/>'),

        isPickedUp     : false,  // track if photo container is currently viewable
        fullSizeWidth  : null,   // will hold width of active thumbnail's fullsize image
        fullSizeHeight : null,   // will hold height of active thumbnail's fullsize image
        windowPadding  : 40,     // minimum space between container and edge of window (px)
        
        // Adds photo container elements to DOM.
        init : function() {

            // append and style photo container
            $('body').append( this.container );
            this.container.css({
                'display'    : 'none',
                'position'   : 'absolute',
                'padding'    : thumbBorderWidth,
                'z-index'    : photoZIndex,
                'background' : photoBorderColor,
                'background-image'    : 'url(' + loading + ')',
                'background-repeat'   : 'no-repeat',
                'background-position' : '50%, 50%'
            });

            // append and style image
            this.container.append( this.image );
            this.image.css('display', 'block');

            // append and style info div
            if ( showInfo ) {
                this.container.append( this.info );
                this.info.append('<p></p>');
                this.info.css('opacity', '0');
            };
   
        }, // init

        // Simulates picking up a photo from the photopile.
        pickup : function( thumbnail ) {
            var self = this;
            if ( self.isPickedUp ) {
                // photo already picked up. put it down and then pickup the clicked thumbnail
                self.putDown( function() { self.pickup( thumbnail ); });
            } else {
                self.isPickedUp = true;
                thumb.clearActiveClass();
                thumb.setActive( thumbnail );
                self.loadImage( thumb.getActiveImgSrc(), function() {
                    self.image.fadeTo(fadeDuration, '1');
                    self.enlarge();
                    $('body').bind('click', function() { self.putDown(); }); // bind putdown event to body
                });
            }
        }, // pickup

        // Simulates putting a photo down, or returning to the photo pile.
        putDown : function( callback ) {
            self = this;
            $('body').unbind();
            self.hideInfo();
            navigator.hideControls();
            thumb.setZ( thumb.getActive(), numLayers );
            self.container.stop().animate({
                'top'     : thumb.getActiveOffset().top + thumb.getActiveShift(),
                'left'    : thumb.getActiveOffset().left + thumb.getActiveShift(),
                'width'   : thumb.getActiveWidth() + 'px',
                'height'  : thumb.getActiveHeight() + 'px',
                'padding' : thumbBorderWidth + 'px'
            }, pickupDuration, function() {
                self.isPickedUp = false;
                thumb.clearActiveClass();
                self.container.fadeOut( fadeDuration, function() {
                    if (callback) callback();
                });
            });
        },

        // Handles the loading of an image when a thumbnail is clicked.
        loadImage : function ( src, callback ) {
            var self = this;
            self.image.css('opacity', '0');         // Image is not visible until
            self.startPosition();                   // the container is positioned,
            var img = new Image;                    // the source is updated,
            img.src = src;                          // and the image is loaded.
            img.onload = function() {               // Restore visibility in callback
                self.fullSizeWidth = this.width;
                self.fullSizeHeight = this.height;
                self.setImageSource( src );
                if (callback) callback();
            };
        },

        // Positions the div container over the active thumb and brings it into view.
        startPosition : function() {
            this.container.css({
                'top'       : thumb.getActiveOffset().top + thumb.getActiveShift(),
                'left'      : thumb.getActiveOffset().left + thumb.getActiveShift(),
                'transform' : 'rotate(' + thumb.getActiveRotation() + 'deg)',
                'width'     : thumb.getActiveWidth() + 'px',
                'height'    : thumb.getActiveHeight() + 'px',
                'padding'   : thumbBorderWidth
            }).fadeTo(fadeDuration, '1');
            thumb.getActive().fadeTo(fadeDuration, '0');
        },

        // Enlarges the photo container based on window and image size (loadImage callback).
        enlarge : function() {
            var windowHeight = window.innerHeight ? window.innerHeight : $(window).height(); // mobile safari hack
            var availableWidth = $(window).width() - (2 * this.windowPadding);
            var availableHeight = windowHeight - (2 * this.windowPadding);
            if ((availableWidth < this.fullSizeWidth) && ( availableHeight < this.fullSizeHeight )) {
                // determine which dimension will allow image to fit completely within the window
                if ((availableWidth * (this.fullSizeHeight / this.fullSizeWidth)) > availableHeight) {
                    this.enlargeToWindowHeight( availableHeight );
                } else {
                    this.enlargeToWindowWidth( availableWidth );
                }
            } else if ( availableWidth < this.fullSizeWidth ) {
                this.enlargeToWindowWidth( availableWidth );
            } else if ( availableHeight < this.fullSizeHeight ) {
                this.enlargeToWindowHeight( availableHeight );
            } else {
                this.enlargeToFullSize();
            }
        }, // enlarge

        // Updates the info div text and makes visible within the photo container.
        showInfo : function() {
            if ( showInfo ) {
                this.info.children().text( thumb.getActive().children('a').children('img').attr('alt') );
                this.info.css({
                    'margin-top' : -(this.info.height()) + 'px'
                }).fadeTo(fadeDuration, 1);
            }
        },

        // Hides the info div.
        hideInfo : function() {
            if ( showInfo ) {
                this.info.fadeTo(fadeDuration, 0);
            };
        },

        // Fullsize image will fit in window. Display it and show nav controls.
        enlargeToFullSize : function() {
            self = this;
            self.container.css('transform', 'rotate(0deg)').animate({
                'top'     : ($(window).scrollTop()) + ($(window).height() / 2) - (self.fullSizeHeight / 2),
                'left'    : ($(window).scrollLeft()) + ($(window).width() / 2) - (self.fullSizeWidth / 2),
                'width'   : (self.fullSizeWidth - (2 * photoBorder)) + 'px',
                'height'  : (self.fullSizeHeight - (2 * photoBorder)) + 'px',
                'padding' : photoBorder + 'px',
            }, function() {
                self.showInfo();
                navigator.showControls();
            });
        },

        // Fullsize image width exceeds window width. Display it and show nav controls.
        enlargeToWindowWidth : function( availableWidth ) {
            self = this;
            var adjustedHeight = availableWidth * (self.fullSizeHeight / self.fullSizeWidth);
            self.container.css('transform', 'rotate(0deg)').animate({
                'top'     : $(window).scrollTop()  + ($(window).height() / 2) - (adjustedHeight / 2),
                'left'    : $(window).scrollLeft() + ($(window).width() / 2)  - (availableWidth / 2),
                'width'   : availableWidth + 'px',
                'height'  : adjustedHeight + 'px',
                'padding' : photoBorder + 'px'
            }, function() {
                self.showInfo();
                navigator.showControls();
            });
        },

        // Fullsize image height exceeds window height. Display it and show nav controls.
        enlargeToWindowHeight : function( availableHeight ) {
            self = this;
            var adjustedWidth = availableHeight * (self.fullSizeWidth / self.fullSizeHeight);
            self.container.css('transform', 'rotate(0deg)').animate({
                'top'     : $(window).scrollTop()  + ($(window).height() / 2) - (availableHeight / 2),
                'left'    : $(window).scrollLeft() + ($(window).width() / 2)  - (adjustedWidth / 2),
                'width'   : adjustedWidth + 'px',
                'height'  : availableHeight + 'px',
                'padding' : photoBorder + 'px'
            }, function() {
                self.showInfo();
                navigator.showControls();
            });
        },

        // Sets the photo container's image source.
        setImageSource : function( src ) { 
            this.image.attr('src', src).css({
                'width'      : '100%',
                'height'     : '100%',
                'margin-top' : '0' 
            });
        }

    } // photo

    //----------------------------------------------------------------------
    // NAVIGATOR
    // Collection of div elements used to navigate the photos in gallery
    //----------------------------------------------------------------------

    var navigator = {

        // Navigator controls.
        next : $( '<div id="photopile-nav-next" />' ),
        prev : $( '<div id="photopile-nav-prev" />' ),

        init : function() {
            photo.container.append( this.next );           // add next control button
            photo.container.append( this.prev );           // add prev control button
            $('ul.photopile li:first').addClass('first');  // add 'first' class to first thumb
            $('ul.photopile li:last').addClass('last');    // add 'last' class to last thumb
            this.bindUIActions();
        },

        bindUIActions : function() {

            // Bind next/prev event to the left and right arrow controls
            this.next.click( function(e) {
                e.preventDefault();
                navigator.pickupNext();
            });
            this.prev.click( function(e) {
                e.preventDefault();
                navigator.pickupPrev();
            });

            // bind prev & next to LR arrow respectively
            $(document.documentElement).keyup(function (e) {
                if (e.keyCode == 39) { navigator.pickupNext(); } // right arrow clicks
                if (e.keyCode == 37) { navigator.pickupPrev(); } // left arrow clicks
            });

        }, // bindUIActions

        pickupNext : function() {
            var activeThumb = thumb.getActive();
            if ( !activeThumb ) return;
            if ( activeThumb.hasClass('last')) {
                photo.pickup( $('ul.photopile').children().first() );  // pickup first 
            } else {
                photo.pickup( activeThumb.next('li') ); // pickup next
            }
        },

        pickupPrev : function() {
            var activeThumb = thumb.getActive();
            if ( !activeThumb ) return;
            if ( activeThumb.hasClass('first')) {
                photo.pickup( $('ul.photopile').children().last() );  // pickup last 
            } else {
                photo.pickup( activeThumb.prev('li') ); // pickup prev
            }
        },

        hideControls : function() {
            this.next.css('opacity', '0');
            this.prev.css('opacity', '0');
        },

        showControls : function() {
            this.next.css('opacity', '100');
            this.prev.css('opacity', '100');
        }

    }; // navigator

    return { scatter : init }  // Photopile's 1 method API

})(); // photopile

photopile.scatter();  // ### initialize the photopile ###
