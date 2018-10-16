//回到顶部
$(function () {
    //快速跳转
    $(".btn_more").click(function (a) {
        $(".wx_aside_item").fadeIn(300);

        $(document).click(function () {
            $(".wx_aside_item").hide();
        })
        a.stopPropagation();
    })

    $(".wx_aside_item").click(function (e) {
        e.stopPropagation();
    })

    $('.att-popup').on('click', function (event) {
        if ($(event.target).is('.att-popup-submit-pay')) {//如果是提交订单支付点击则跳转订单详情页
            var parentOrderId = $("#hidParentOrderId").val();
            if (parentOrderId == "-1") {
                document.location.href = "MemberOrders?ParentOrderId=" + $("#hidOrderId").val();
            }
            else {
                document.location.href = "MemberOrderDetails?orderId=" + $("#hidOrderId").val();
            }
        }
        if ($(event.target).is('.att-popup-close,#addToCart') || $(event.target).is('.att-popup')) {
            event.preventDefault();
            $(this).removeClass('is-visible');
        }
    });


    $(window).scroll(function () {
        $(this).scrollTop() >= 400 ? $(".go_top").fadeIn(300) : $(".go_top").fadeOut(300);
    });

    $(".go_top").click(function () {
        $("html,body").animate({ scrollTop: 0 });
    });
})

    //商品规格选择弹出层
    $(function () {
        $('#specification,.buy,.delivery').on('click', function () {
            $('.att-popup').addClass('is-visible');
        });

        if ($('#choose').length == 0) {
            $('#choose-result').css('display', 'none');
        }
    })

    //领取优惠券及优惠活动
    $(function () {
        $('#coupons').on('click', function () {
            $('.coupons_popup').addClass('is-visible');
            $("#cul").show();
            $("#aul").hide();
            $("#consult").hide();
            $('.coupons_title span').text("领取优惠券");
        });

        $('.pop_close').on('click', function () {
            $('.coupons_popup').removeClass('is-visible');
        });

        $('.coupons_activity').on('click', function () {
            $('.coupons_popup').addClass('is-visible');
            $("#cul").hide();
            $("#aul").show();
            $("#consult").hide();
            $('.coupons_title span').text("优惠活动");
        });

        $('.FAB_consult').on('click', function () {
            $('.coupons_popup').addClass('is-visible');
            $("#cul").hide();
            $("#aul").hide();
            $("#consult").show();
            $('.coupons_title span').text("商品咨询");
        });

        $(".close_pop").click(function () {
            $('.att-popup').removeClass('is-visible');
        })

        $('#p_rule').on('click', function () {
            $('.coupons_popup').addClass('is-visible');
            $("#cul").hide();
            $("#aul").hide();
            $("#consult").hide();
            $("#pul").show();
            $('.coupons_title span').text("预售规则");

        });

    })

    //隐藏右上角菜单
    document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
        WeixinJSBridge.call('showOptionMenu');
    });

    $(document).ready(function () {
        $("footer .glyphicon-refresh").click(function () {
            location.reload();
        })

        $("footer .glyphicon-arrow-left").click(function () {
            history.go(-1);
        })

    });

    function goUrl(url) {
        window.location.href = url;
    }

    function myConfirm(title, content, ensureText, ensuredCallback) {
        var myConfirmCode = '<div class="modal fade" id="myConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">' + title + '</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-default" data-dismiss="modal">再逛逛 </button>\
                        <button type="button" class="btn btn-danger">' + ensureText + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
        if ($("#myConfirm").length == 0) {
            $("body").append(myConfirmCode);
        }
        $('#myConfirm').modal();
        $('#myConfirm button.btn-danger').unbind("click", "");
        $('#myConfirm button.btn-default').unbind("click", "");
        $('#myConfirm button.btn-default').click(function (event) {
            goHomePage();
        });
        $('#myConfirm button.btn-danger').click(function (event) {
            if (ensuredCallback)
                ensuredCallback();
            $('#myConfirm').modal('hide')
        });
    }

    function myConfirm1(title, content) {
        var myConfirmCode = '<div class="modal fade" id="myConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">' + title + '</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-default" data-dismiss="modal">取消 </button>\
                        <button type="button" class="btn btn-danger">确定</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
        if ($("#myConfirm").length == 0) {
            $("body").append(myConfirmCode);
        }
        $('#myConfirm').modal();
        $('#myConfirm button.btn-danger').unbind("click", "");
        $('#myConfirm button.btn-danger').click(function (event) {
            __doPostBack('WAPPointInfo$btnClearCart', '');
            $('#myConfirm').modal('hide');
        });
        $('#myConfirm button.btn-default').click(function (event) {
            $('#myConfirm').modal('hide');
        });

    }

    function myConfirmBox(title, content, ensureText, cancelText, ensuredCallback, cancelCallbak) {
        var myConfirmCode = '<div class="modal fade" id="myConfirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">' + title + '</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button" class="btn btn-default" data-dismiss="modal">'+ cancelText + ' </button>\
                        <button type="button" class="btn btn-danger">' + ensureText + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
        if ($("#myConfirm").length == 0) {
            $("body").append(myConfirmCode);
        }
        $('#myConfirm').modal();
        $('#myConfirm button.btn-danger').unbind("click", "");
        $('#myConfirm button.btn-default').unbind("click", "");
        $('#myConfirm button.btn-default').click(function (event) {
            if (cancelCallbak)
                cancelCallbak();
            $('#myConfirm').modal('hide')
        });
        $('#myConfirm button.btn-danger').click(function (event) {
            if (ensuredCallback)
                ensuredCallback();
            $('#myConfirm').modal('hide')
        });
    }

    function alert_h(content, ensuredCallback) {
        var myConfirmCode = '<div class="modal fade" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content">\
                      <div class="modal-header">\
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>\
                        <h4 class="modal-title" id="myModalLabel">操作提示</h4>\
                      </div>\
                      <div class="modal-body">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer">\
                        <button type="button"  style="width:100%;text-algin:center;border-radius: 0 0 5px 5px;" data-dismiss="modal">确认</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';

        if ($("#alert_h").length != 0) {
            $('#alert_h').remove();
        }
        $("body").append(myConfirmCode);
        $('#alert_h').modal();

        $('#alert_h').off('hide.bs.modal').on('hide.bs.modal', function (e) {
            if (ensuredCallback)
                ensuredCallback();
        });
    }

    function alert_m(content, ensuredCallback) {
        var myConfirmCode = '<div class="modal fade" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content" style="border-radius: 8px;">\
                      <div class="modal-header a_1" style="padding:0;margin-top:0;height:0.75rem;">\
                        <button type="button"  data-dismiss="modal" aria-hidden="true" style="position: absolute;right:-0.5rem;top:-0.5rem;background: url(/templates/common/images/icon_close_bigwheel.png);background-size: 100% 100%;width:1rem;height:1rem;border:0;"></button>\
                        <h4 class="modal-title" id="myModalLabel"></h4>\
                      </div>\
                      <div class="modal-body" style="padding:1.5rem 0;">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer" style="text-align:center;border:0;">\
                        <button type="button"  style="width:100%;border:0;text-algin:center;border-radius: 0 0 5px 5px;float:none;position: initial;display: inline-block;width: 7rem;height: 1.875rem;background: url(/templates/common/images/btn_box.png);background-size: 100% 100%;text-align: center;line-height: 1.875rem;color: #f4511e;font-size: 0.75rem;" data-dismiss="modal">知道了</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
        if ($("#alert_h").length != 0) {
            $('#alert_h').remove();
        }
        $("body").append(myConfirmCode);
        $('#alert_h').modal();

        $('#alert_h').off('hide.bs.modal').on('hide.bs.modal', function (e) {
            if (ensuredCallback)
                ensuredCallback();
        });
    }

    function confirm_m(content, ensuredCallback, cancelBtn, successBtn) {
        var myConfirmCode = '<div class="modal fade" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content" style="border-radius: 8px;">\
                      <div class="modal-header a_1" style="padding:0;margin-top:0;height:0.75rem;">\
                        <button type="button"  data-dismiss="modal" aria-hidden="true" style="position: absolute;right:-0.5rem;top:-0.5rem;background: url(/templates/common/images/icon_close_bigwheel.png);background-size: 100% 100%;width:1rem;height:1rem;border:0;"></button>\
                        <h4 class="modal-title" id="myModalLabel"></h4>\
                      </div>\
                      <div class="modal-body" style="padding:1.5rem 0;">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer" style="text-align:center;border:0;">\
                        <button id="cancelBtn" type="button"  style="width:100%;border:0;text-algin:center;border-radius: 0 0 5px 5px;float:none;position: initial;display: inline-block;width: 5rem;height: 1.875rem;background: url(/templates/common/images/btn_box.png);background-size: 100% 100%;text-align: center;line-height: 1.875rem;color: #f4511e;font-size: 0.75rem;margin-right:20px;">' + cancelBtn + '</button>\
                        <button id="successBtn" type="button"  style="width:100%;border:0;text-algin:center;border-radius: 0 0 5px 5px;float:none;position: initial;display: inline-block;width: 5rem;height: 1.875rem;background: url(/templates/common/images/btn_box.png);background-size: 100% 100%;text-align: center;line-height: 1.875rem;color: #f4511e;font-size: 0.75rem;">' + successBtn + '</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
        if ($("#alert_h").length != 0) {
            $('#alert_h').remove();
        }
        $("body").append(myConfirmCode);
        $('#alert_h').modal();

        $('#successBtn').on('click', function (e) {
            ensuredCallback();
            $('#alert_h').modal('hide');
        });
        $('#cancelBtn').on('click', function (e) {
            $('#alert_h').modal('hide');
        });
    }

    function confirm_s(content) {
        var myConfirmCode = '<div class="modal fade" id="alert_h" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">\
                  <div class="modal-dialog">\
                    <div class="modal-content" style="border-radius: 8px;">\
                      <div class="modal-header a_1" style="padding:0;margin-top:0;height:0.75rem;">\
                        <button type="button"  data-dismiss="modal" aria-hidden="true" style="position: absolute;right:-0.5rem;top:-0.5rem;background: url(/templates/common/images/icon_close_bigwheel.png);background-size: 100% 100%;width:1rem;height:1rem;border:0;"></button>\
                        <h4 class="modal-title" id="myModalLabel"></h4>\
                      </div>\
                      <div class="modal-body" style="padding:1.5rem 0;">\
                        ' + content + '\
                      </div>\
                      <div class="modal-footer" style="text-align:center;border:0;">\
                        <a  id="cancelBtn" type="button"  style="width:100%;border:0;text-algin:center;border-radius: 0 0 5px 5px;float:none;position: initial;display: inline-block;width: 5rem;height: 1.875rem;background: url(/templates/common/images/btn_box.png);background-size: 100% 100%;text-align: center;line-height: 1.875rem;color: #f4511e;font-size: 0.75rem;margin-right:20px;">查看奖品</a>\
                        <button id="successBtn" type="button"  style="width:100%;border:0;text-algin:center;border-radius: 0 0 5px 5px;float:none;position: initial;display: inline-block;width: 5rem;height: 1.875rem;background: url(/templates/common/images/btn_box.png);background-size: 100% 100%;text-align: center;line-height: 1.875rem;color: #f4511e;font-size: 0.75rem;">继续抽奖</button>\
                      </div>\
                    </div>\
                  </div>\
                </div>';
        if ($("#alert_h").length != 0) {
            $('#alert_h').remove();
        }
        $("body").append(myConfirmCode);
        $('#alert_h').modal();

        $('#successBtn').on('click', function (e) {
            $('#alert_h').modal('hide');
        });
        var aurl = window.location.pathname;
        var arr = aurl.split('/');
        var burl = arr[1];
        $("#cancelBtn").attr('href', '/' + burl + '/' + 'myprize');
    }

    windowPop = function (divWidth, divHeight, msgInfo, switchResult) {
        this.divWidth = divWidth;
        this.divHeight = divHeight;
        this.msgInfo = msgInfo;
        this.switchResult = switchResult;

        this.showHTml = function () {
            var isCase = this.switchResult;
            var icoNum = 1;
            var html;
            if (isCase == 0 || isCase == "error" || isCase == false) { isCase = 0; icoNum = 2; }
            if (isCase == 1 || isCase == true) { isCase = 1; icoNum = 1; }
            switch (isCase) {
                case 0:
                case 1:
                    html = '<div class="windowsdialog" id="windowsdialog">' +
                       '<div class="opactiy">' + '<div class="ico_' + icoNum + '  toast"><i class="warn_small"></i>' + this.msgInfo + '</div>' + '</div>' +
                        '</div>';
                    break;
                case 2:
                case 3:
            }
            return html;
        }

        //关闭窗口
        this.closewindowPop = function () {
            $(".dialog-close").click(function () {
                $("#popDivLock").remove();
                $("#windowsdialog").fadeOut(300);
            });
        }

        this.autoClosePop = function () {
            setTimeout(function () {
                $("#popDivLock").remove();
                $("#windowsdialog").fadeOut(200).remove();
            }, 3000)
        }
        this.windoPopOpen = function () {
            var html = this.showHTml();
            var clientH = $(window).height();	//浏览器高度
            var clientW = $(window).width();	//浏览器宽度
            var div_X = (clientW - this.divWidth) / 2;
            var div_Y = (clientH - this.divHeight) / 2;
            div_X += window.document.documentElement.scrollLeft;	//DIV显示的实际横坐标
            div_Y += window.document.documentElement.scrollTop;	//DIV显示的实际纵坐标
            $("body").append(html);//增加DIV
            this.closewindowPop()//添加关闭窗口事件
            //$("#"+objDiv).show();	
            //divWindow的样式
            $("#windowsdialog").fadeIn(100);

        }

        //锁定背景屏幕
        this.lockScreen = function () {
            if ($("#popDivLock").length == 0) {	//判断DIV是否存在
                var clientH = $(window).height();	//浏览器高度
                var clientW = $(window).width();	//浏览器宽度		
                var docH = $("body").height();	//网页高度
                var docW = $("body").width();	//网页宽度
                var bgW = clientW > docW ? clientW : docW;	//取有效宽
                var bgH = clientH > docH ? clientH : docH;	//取有效高		
                $("body").append("<div id='popDivLock'></div>")	//增加DIV
                $("#popDivLock").height(bgH + 100);
                $("#popDivLock").width(bgW);
                $("#popDivLock").css("display", "block");
                $("#popDivLock").css("background-color", "#333333");
                $("#popDivLock").css("position", "absolute");
                $("#popDivLock").css("z-index", "500");
                $("#popDivLock").css("top", "0px");
                $("#popDivLock").css("left", "0px");
                $("#popDivLock").css("opacity", "0.7");
            }
            else {
                clientH = $(window).height();	//浏览器高度
                clientW = $(window).width();	//浏览器宽度
                $("#popDivLock").height(clientH);
                $("#popDivLock").width(clientW);
            }
        }
    }


    function ShowMsg(msg, success, ensuredCallback) {
        if (success) {    //参数(宽value,高value,提示信息string,信息类别提示0,1,error,true,false,3,4)
            var popWin = new windowPop(350, 200, msg, 1);
            popWin.windoPopOpen();
            popWin.autoClosePop()
        }
        else {
            var popWin = new windowPop(350, 200, msg, 0);
            //popWin.lockScreen();	//锁定背景
            popWin.windoPopOpen();
            popWin.autoClosePop();
        }
        if (ensuredCallback) {
            setTimeout(function () {
                ensuredCallback();
            }, 3000)
        }
    }

    //模态窗
    +function ($) {
        'use strict';
        var Modal = function (element, options) {
            this.options = options
            this.$body = $(document.body)
            this.$element = $(element)
            this.$dialog = this.$element.find('.modal-dialog')
            this.$backdrop = null
            this.isShown = null
            this.originalBodyPad = null
            this.scrollbarWidth = 0
            this.ignoreBackdropClick = false

            if (this.options.remote) {
                this.$element
                  .find('.modal-content')
                  .load(this.options.remote, $.proxy(function () {
                      this.$element.trigger('loaded.bs.modal')
                  }, this))
            }
        }

        Modal.VERSION = '3.3.6'

        Modal.TRANSITION_DURATION = 300
        Modal.BACKDROP_TRANSITION_DURATION = 150

        Modal.DEFAULTS = {
            backdrop: true,
            keyboard: true,
            show: true
        }

        Modal.prototype.toggle = function (_relatedTarget) {
            return this.isShown ? this.hide() : this.show(_relatedTarget)
        }

        Modal.prototype.show = function (_relatedTarget) {
            var that = this
            var e = $.Event('show.bs.modal', { relatedTarget: _relatedTarget })
            this.$element.trigger(e)
            if (this.isShown || e.isDefaultPrevented()) return
            this.isShown = true
            this.checkScrollbar()
            this.setScrollbar()
            this.$body.addClass('modal-open')

            this.escape()
            this.resize()

            this.$element.on('click.dismiss.bs.modal', '[data-dismiss="modal"]', $.proxy(this.hide, this))

            this.$dialog.on('mousedown.dismiss.bs.modal', function () {
                that.$element.one('mouseup.dismiss.bs.modal', function (e) {
                    if ($(e.target).is(that.$element)) that.ignoreBackdropClick = true
                })
            })

            this.backdrop(function () {
                var transition = $.support.transition && that.$element.hasClass('fade')
                if (!that.$element.parent().length) {
                    that.$element.appendTo(that.$body)
                }
                that.$element
                  .show()
                  .scrollTop(0)
                that.adjustDialog()
                if (transition) {
                    that.$element[0].offsetWidth
                }

                that.$element.addClass('in')
                that.enforceFocus()
                var e = $.Event('shown.bs.modal', { relatedTarget: _relatedTarget })

                transition ?
                  that.$dialog
                    .one('bsTransitionEnd', function () {
                        that.$element.trigger('focus').trigger(e)
                    })
                    .emulateTransitionEnd(Modal.TRANSITION_DURATION) :
                  that.$element.trigger('focus').trigger(e)
            })
        }

        Modal.prototype.hide = function (e) {
            if (e) e.preventDefault()
            e = $.Event('hide.bs.modal')
            this.$element.trigger(e)
            if (!this.isShown || e.isDefaultPrevented()) return
            this.isShown = false
            this.escape()
            this.resize()

            $(document).off('focusin.bs.modal')

            this.$element
              .removeClass('in')
              .off('click.dismiss.bs.modal')
              .off('mouseup.dismiss.bs.modal')

            this.$dialog.off('mousedown.dismiss.bs.modal')

            $.support.transition && this.$element.hasClass('fade') ?
              this.$element
                .one('bsTransitionEnd', $.proxy(this.hideModal, this))
                .emulateTransitionEnd(Modal.TRANSITION_DURATION) :
              this.hideModal()
        }

        Modal.prototype.enforceFocus = function () {
            $(document)
              .off('focusin.bs.modal')
              .on('focusin.bs.modal', $.proxy(function (e) {
                  if (document !== e.target &&
                      this.$element[0] !== e.target &&
                      !this.$element.has(e.target).length) {
                      this.$element.trigger('focus')
                  }
              }, this))
        }

        Modal.prototype.escape = function () {
            if (this.isShown && this.options.keyboard) {
                this.$element.on('keydown.dismiss.bs.modal', $.proxy(function (e) {
                    e.which == 27 && this.hide()
                }, this))
            } else if (!this.isShown) {
                this.$element.off('keydown.dismiss.bs.modal')
            }
        }

        Modal.prototype.resize = function () {
            if (this.isShown) {
                $(window).on('resize.bs.modal', $.proxy(this.handleUpdate, this))
            } else {
                $(window).off('resize.bs.modal')
            }
        }

        Modal.prototype.hideModal = function () {
            var that = this
            this.$element.hide()
            this.backdrop(function () {
                that.$body.removeClass('modal-open')
                that.resetAdjustments()
                that.resetScrollbar()
                that.$element.trigger('hidden.bs.modal')
            })
        }

        Modal.prototype.removeBackdrop = function () {
            this.$backdrop && this.$backdrop.remove()
            this.$backdrop = null
        }

        Modal.prototype.backdrop = function (callback) {
            var that = this
            var animate = this.$element.hasClass('fade') ? 'fade' : ''

            if (this.isShown && this.options.backdrop) {
                var doAnimate = $.support.transition && animate

                this.$backdrop = $(document.createElement('div'))
                  .addClass('modal-backdrop ' + animate)
                  .appendTo(this.$body)

                this.$element.on('click.dismiss.bs.modal', $.proxy(function (e) {
                    if (this.ignoreBackdropClick) {
                        this.ignoreBackdropClick = false
                        return
                    }
                    if (e.target !== e.currentTarget) return
                    this.options.backdrop == 'static'
                      ? this.$element[0].focus()
                      : this.hide()
                }, this))

                if (doAnimate) this.$backdrop[0].offsetWidth

                this.$backdrop.addClass('in')

                if (!callback) return

                doAnimate ?
                  this.$backdrop
                    .one('bsTransitionEnd', callback)
                    .emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) :
                  callback()

            } else if (!this.isShown && this.$backdrop) {
                this.$backdrop.removeClass('in')

                var callbackRemove = function () {
                    that.removeBackdrop()
                    callback && callback()
                }
                $.support.transition && this.$element.hasClass('fade') ?
                  this.$backdrop
                    .one('bsTransitionEnd', callbackRemove)
                    .emulateTransitionEnd(Modal.BACKDROP_TRANSITION_DURATION) :
                  callbackRemove()

            } else if (callback) {
                callback()
            }
        }

        Modal.prototype.handleUpdate = function () {
            this.adjustDialog()
        }

        Modal.prototype.adjustDialog = function () {
            var modalIsOverflowing = this.$element[0].scrollHeight > document.documentElement.clientHeight

            this.$element.css({
                paddingLeft: !this.bodyIsOverflowing && modalIsOverflowing ? this.scrollbarWidth : '',
                paddingRight: this.bodyIsOverflowing && !modalIsOverflowing ? this.scrollbarWidth : ''
            })
        }

        Modal.prototype.resetAdjustments = function () {
            this.$element.css({
                paddingLeft: '',
                paddingRight: ''
            })
        }

        Modal.prototype.checkScrollbar = function () {
            var fullWindowWidth = window.innerWidth
            if (!fullWindowWidth) {
                var documentElementRect = document.documentElement.getBoundingClientRect()
                fullWindowWidth = documentElementRect.right - Math.abs(documentElementRect.left)
            }
            this.bodyIsOverflowing = document.body.clientWidth < fullWindowWidth
            this.scrollbarWidth = this.measureScrollbar()
        }

        Modal.prototype.setScrollbar = function () {
            var bodyPad = parseInt((this.$body.css('padding-right') || 0), 10)
            this.originalBodyPad = document.body.style.paddingRight || ''
            if (this.bodyIsOverflowing) this.$body.css('padding-right', bodyPad + this.scrollbarWidth)
        }

        Modal.prototype.resetScrollbar = function () {
            this.$body.css('padding-right', this.originalBodyPad)
        }

        Modal.prototype.measureScrollbar = function () {
            var scrollDiv = document.createElement('div')
            scrollDiv.className = 'modal-scrollbar-measure'
            this.$body.append(scrollDiv)
            var scrollbarWidth = scrollDiv.offsetWidth - scrollDiv.clientWidth
            this.$body[0].removeChild(scrollDiv)
            return scrollbarWidth
        }



        function Plugin(option, _relatedTarget) {
            return this.each(function () {
                var $this = $(this)
                var data = $this.data('bs.modal')
                var options = $.extend({}, Modal.DEFAULTS, $this.data(), typeof option == 'object' && option)

                if (!data) $this.data('bs.modal', (data = new Modal(this, options)))
                if (typeof option == 'string') data[option](_relatedTarget)
                else if (options.show) data.show(_relatedTarget)
            })
        }

        var old = $.fn.modal

        $.fn.modal = Plugin
        $.fn.modal.Constructor = Modal



        $.fn.modal.noConflict = function () {
            $.fn.modal = old
            return this
        }


        $(document).on('click.bs.modal.data-api', '[data-toggle="modal"]', function (e) {
            var $this = $(this)
            var href = $this.attr('href')
            var $target = $($this.attr('data-target') || (href && href.replace(/.*(?=#[^\s]+$)/, '')))
            var option = $target.data('bs.modal') ? 'toggle' : $.extend({ remote: !/#/.test(href) && href }, $target.data(), $this.data())

            if ($this.is('a')) e.preventDefault()

            $target.one('show.bs.modal', function (showEvent) {
                if (showEvent.isDefaultPrevented()) return
                $target.one('hidden.bs.modal', function () {
                    $this.is(':visible') && $this.trigger('focus')
                })
            })
            Plugin.call($target, option, this)
        })

    }(jQuery);

    //弹出选择框应用在省市区
    +function ($) {
        'use strict';
        var backdrop = '.dropdown-backdrop'
        var toggle = '[data-toggle="dropdown"]'
        var Dropdown = function (element) {
            $(element).on('click.bs.dropdown', this.toggle)
        }

        Dropdown.VERSION = '3.3.6'

        function getParent($this) {
            var selector = $this.attr('data-target')

            if (!selector) {
                selector = $this.attr('href')
                selector = selector && /#[A-Za-z]/.test(selector) && selector.replace(/.*(?=#[^\s]*$)/, '')
            }

            var $parent = selector && $(selector)

            return $parent && $parent.length ? $parent : $this.parent()
        }

        function clearMenus(e) {
            if (e && e.which === 3) return
            $(backdrop).remove()
            $(toggle).each(function () {
                var $this = $(this)
                var $parent = getParent($this)
                var relatedTarget = { relatedTarget: this }

                if (!$parent.hasClass('open')) return

                if (e && e.type == 'click' && /input|textarea/i.test(e.target.tagName) && $.contains($parent[0], e.target)) return

                $parent.trigger(e = $.Event('hide.bs.dropdown', relatedTarget))

                if (e.isDefaultPrevented()) return

                $this.attr('aria-expanded', 'false')
                $parent.removeClass('open').trigger($.Event('hidden.bs.dropdown', relatedTarget))
            })
        }

        Dropdown.prototype.toggle = function (e) {
            var $this = $(this)

            if ($this.is('.disabled, :disabled')) return

            var $parent = getParent($this)
            var isActive = $parent.hasClass('open')

            clearMenus()

            if (!isActive) {
                if ('ontouchstart' in document.documentElement && !$parent.closest('.navbar-nav').length) {
                    $(document.createElement('div'))
                      .addClass('dropdown-backdrop')
                      .insertAfter($(this))
                      .on('click', clearMenus)
                }

                var relatedTarget = { relatedTarget: this }
                $parent.trigger(e = $.Event('show.bs.dropdown', relatedTarget))

                if (e.isDefaultPrevented()) return

                $this
                  .trigger('focus')
                  .attr('aria-expanded', 'true')

                $parent
                  .toggleClass('open')
                  .trigger($.Event('shown.bs.dropdown', relatedTarget))
            }

            return false
        }

        Dropdown.prototype.keydown = function (e) {
            if (!/(38|40|27|32)/.test(e.which) || /input|textarea/i.test(e.target.tagName)) return

            var $this = $(this)

            e.preventDefault()
            e.stopPropagation()

            if ($this.is('.disabled, :disabled')) return

            var $parent = getParent($this)
            var isActive = $parent.hasClass('open')

            if (!isActive && e.which != 27 || isActive && e.which == 27) {
                if (e.which == 27) $parent.find(toggle).trigger('focus')
                return $this.trigger('click')
            }

            var desc = ' li:not(.disabled):visible a'
            var $items = $parent.find('.dropdown-menu' + desc)

            if (!$items.length) return

            var index = $items.index(e.target)

            if (e.which == 38 && index > 0) index--
            if (e.which == 40 && index < $items.length - 1) index++
            if (!~index) index = 0

            $items.eq(index).trigger('focus')
        }


        function Plugin(option) {
            return this.each(function () {
                var $this = $(this)
                var data = $this.data('bs.dropdown')

                if (!data) $this.data('bs.dropdown', (data = new Dropdown(this)))
                if (typeof option == 'string') data[option].call($this)
            })
        }

        var old = $.fn.dropdown

        $.fn.dropdown = Plugin
        $.fn.dropdown.Constructor = Dropdown


        $.fn.dropdown.noConflict = function () {
            $.fn.dropdown = old
            return this
        }


        $(document)
          .on('click.bs.dropdown.data-api', clearMenus)
          .on('click.bs.dropdown.data-api', '.dropdown form', function (e) { e.stopPropagation() })
          .on('click.bs.dropdown.data-api', toggle, Dropdown.prototype.toggle)
          .on('keydown.bs.dropdown.data-api', toggle, Dropdown.prototype.keydown)
          .on('keydown.bs.dropdown.data-api', '.dropdown-menu', Dropdown.prototype.keydown)

    }(jQuery);


var pageLoadTime;

function GetRTime() {
    var d;
    var h;
    var m;
    var s;

    var startVal = document.getElementById("startTime").value;
    var endVal = document.getElementById("endTime").value;
    var startTime = new Date(startVal);
    var endTime = new Date(endVal); //截止时间 前端路上 http://www.51xuediannao.com/qd63/
    var nowTime = new Date();
    var now_startTime = nowTime.getTime() - startTime.getTime();    //当前时间 减去开始时间
    var s_nTime = startTime.getTime() - nowTime.getTime();          //开始时间减去当前时间
    var start_endTime = endTime.getTime() - startTime.getTime();    //结束时间减去开始时间
    var now_endTime = endTime.getTime() - nowTime.getTime();     //结束时间减去当前时间
    var now_pTime = nowTime.getTime() - pageLoadTime;               //当前时间减去页面刷新时间
    var p_sTime = startTime.getTime() - pageLoadTime;               //开始时间减去页面刷新时间
    var wid = now_startTime / start_endTime * 100;                    //开始后离结束的时间比
    var wid1 = now_pTime / p_sTime * 100;                             //未开始离开始的时间比
    var tuan_button = document.getElementById("buyButton");
    var progress = document.getElementById("progress");
    var tuan_time = document.getElementById("tuan_time");
    function docu() {
        document.getElementById("t_d").innerHTML = d + "天";
        document.getElementById("t_h").innerHTML = h + "时";
        document.getElementById("t_m").innerHTML = m + "分";
        document.getElementById("t_s").innerHTML = s + "秒";
    }
    if (pageLoadTime == null) {
        pageLoadTime = new Date();
    }
    if (100 >= wid1 >= 0 && wid < 0) {
        d = Math.floor(Math.abs(now_startTime) / 1000 / 60 / 60 / 24);
        h = Math.floor(Math.abs(now_startTime) / 1000 / 60 / 60 % 24);
        m = Math.floor(Math.abs(now_startTime) / 1000 / 60 % 60);
        s = Math.floor(Math.abs(now_startTime) / 1000 % 60);
        docu();
        tuan_time.innerHTML = "团购开始时间：";
        progress.style.width = wid1 + "%";
        tuan_button.disabled = true;
    }
    if (wid1 > 100 || wid1 < 0) {
        if (wid >= 0 && wid < 70) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = "团购结束时间：";
            progress.style.width = (100 - wid) + "%";
            tuan_button.disabled = false;
        } else if (wid >= 70 && wid < 90) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = "团购结束时间：";
            progress.className = "progress-bar progress-bar-warning";
            progress.style.width = (100 - wid) + "%";
            tuan_button.disabled = false;
        } else if (wid >= 90 && wid <= 100) {
            d = Math.floor(now_endTime / 1000 / 60 / 60 / 24);
            h = Math.floor(now_endTime / 1000 / 60 / 60 % 24);
            m = Math.floor(now_endTime / 1000 / 60 % 60);
            s = Math.floor(now_endTime / 1000 % 60);
            docu();
            tuan_time.innerHTML = "团购结束时间：";
            progress.style.width = (100 - wid) + "%";
            progress.className = "progress-bar progress-bar-danger";
            tuan_button.disabled = false;
        }

        if (wid > 100) {
            tuan_time.innerHTML = "团购已结束!";
            progress.style.width = 0;
            tuan_button.disabled = true;
        }
    }



    /* 

    if(now_startTime>0&&t>0){
         document.getElementById("progress").style.width=wid+"%";
         document.getElementById("t_d").innerHTML = d+"天";
         document.getElementById("t_h").innerHTML = h+"时";
         document.getElementById("t_m").innerHTML = m+"分";
         document.getElementById("t_s").innerHTML = s+"秒";
     }
     
    */

    /*var d=Math.floor(t/1000/60/60/24);
    t-=d*(1000*60*60*24);
    var h=Math.floor(t/1000/60/60);
    t-=h*60*60*1000;
    var m=Math.floor(t/1000/60);
    t-=m*60*1000;
    var s=Math.floor(t/1000);*/
}

function loadIframeURL(url) {
    var iFrame;
    iFrame = document.createElement("iframe");
    iFrame.setAttribute("src", url);
    iFrame.setAttribute("style", "display:none;");
    iFrame.setAttribute("height", "0px");
    iFrame.setAttribute("width", "0px");
    iFrame.setAttribute("frameborder", "0");
    document.body.appendChild(iFrame);
    // 发起请求后这个iFrame就没用了，所以把它从dom上移除掉
    //iFrame.parentNode.removeChild(iFrame);
    iFrame = null;
}

function getCookie(Name) {
    var search = Name + "="
    if (document.cookie.length > 0) {
        offset = document.cookie.indexOf(search)
        if (offset != -1) {
            offset += search.length
            end = document.cookie.indexOf(";", offset)
            if (end == -1) end = document.cookie.length
            return unescape(document.cookie.substring(offset, end))
        }
        else return ""
    }
}

var browser = {
    versions: function () {
        var u = navigator.userAgent, app = navigator.appVersion;
        return { //移动终端浏览器版本信息 
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端 
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或uc浏览器 
            iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器 
            iPad: u.indexOf('iPad') > -1, //是否iPad 
        };
    }(),
}
//1代表ios，2代表安卓，3代表其他
function GetAgentType() {
    if (browser.versions.iPhone || browser.versions.iPad || browser.versions.ios) {
        return 1;
    } else if (browser.versions.android) {
        return 2;
    } else {
        return 3;
    }
}

//去拼团成功原生页
function goFightGroupSuccess(status, personal, shareJson) {
    
    OpenUrl("SubmitFightSuccess", '{"NeedNum":' + personal + ',"ShareInfo":' + shareJson + '}');
}

//重新toFixed方法
Number.prototype.toFixed = function (d) {
    var s = this + "";
    if (!d) d = 0;
    if (s.indexOf(".") == -1) s += ".";
    s += new Array(d + 1).join("0");
    if (new RegExp("^(-|\\+)?(\\d+(\\.\\d{0," + (d + 1) + "})?)\\d*$").test(s)) {
        var s = "0" + RegExp.$2, pm = RegExp.$1, a = RegExp.$3.length, b = true;
        if (a == d + 2) {
            a = s.match(/\d/g);
            //不四舍五入了 bug  37380
            //if (parseInt(a[a.length - 1]) > 4) {
            //    for (var i = a.length - 2; i >= 0; i--) {
            //        a[i] = parseInt(a[i]) + 1;
            //        if (a[i] == 10) {
            //            a[i] = 0;
            //            b = i != 1;
            //        } else break;
            //    }
            //}
            s = a.join("").replace(new RegExp("(\\d+)(\\d{" + d + "})\\d$"), "$1.$2");
        }
        if (b) s = s.substr(1);
        return (pm + s).replace(/\.$/, "");
    }
    return this + "";
};
