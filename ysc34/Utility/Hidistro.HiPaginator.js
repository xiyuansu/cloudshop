/*
 *
 *
 *
 * 
 */

(function ($) {
    'use strict';

    var HIPAGINATOR_DATA_NAME = "HiPaginator";

    $.HiPaginator = function (el, options) {
        if(!(this instanceof $.HiPaginator)){
            return new $.HiPaginator(el, options);
        }

        var self = this;

        self.container = $(el);

        self.container.data(HIPAGINATOR_DATA_NAME, self);

        self.init = function (opt) {

            self.options = $.extend({}, $.HiPaginator.defaultOptions, opt);
            self.verify();

            self.extendJquery();

            self.render();

            self.isredraw = false;
            self.fireEvent(this.options.currentPage, 'init');
        };

        self.verify = function () {
            var opts = self.options;

            if (!self.isNumber(opts.totalPages)) {
                throw new Error('[HiPaginator] type error: totalPages');
            }

            if (!self.isNumber(opts.totalCounts)) {
                throw new Error('[HiPaginator] type error: totalCounts');
            }

            if (!self.isNumber(opts.pageSize)) {
                throw new Error('[HiPaginator] type error: pageSize');
            }

            if (!self.isNumber(opts.currentPage)) {
                throw new Error('[HiPaginator] type error: currentPage');
            }

            if (!self.isNumber(opts.visiblePages)) {
                throw new Error('[HiPaginator] type error: visiblePages');
            }

            if (!opts.totalPages && !opts.totalCounts) {
                throw new Error('[HiPaginator] totalCounts or totalPages is required');
            }

            if (!opts.totalPages && !opts.totalCounts) {
                throw new Error('[HiPaginator] totalCounts or totalPages is required');
            }

            if (!opts.totalPages && opts.totalCounts && !opts.pageSize) {
                throw new Error('[HiPaginator] pageSize is required');
            }

            if (opts.totalCounts && opts.pageSize) {
                opts.totalPages = Math.ceil(opts.totalCounts / opts.pageSize);
            }

            if (opts.currentPage < 1) {
                throw new Error('[HiPaginator] currentPage is incorrect');
            }

            /*
            if (opts.currentPage > opts.totalPages) {
                throw new Error('[HiPaginator] currentPage is incorrect');
            }*/

            if (opts.totalPages < 1) {
                throw new Error('[HiPaginator] totalPages cannot be less currentPage');
            }
        };

        self.extendJquery = function () {
            $.fn.HiPaginatorHTML = function (s) {
                return s ? this.before(s).remove() : $('<p>').append(this.eq(0).clone()).html();
            };
        };

        self.render = function () {
            self.renderHtml();
            self.setStatus();
            self.bindEvents();
        };

        self.renderHtml = function () {
            var html = [];
            var pages = self.getPages();
            for (var i = 0, j = pages.length; i < j; i++) {
                html.push(self.buildItem('page', pages[i]));
            }

            self.isEnable('prev') && html.unshift(self.buildItem('prev', self.options.currentPage - 1));
            self.isEnable('first') && html.unshift(self.buildItem('first', 1));
            self.isEnable('statistics') && html.unshift(self.buildItem('statistics'));
            self.isEnable('next') && html.push(self.buildItem('next', self.options.currentPage + 1));
            self.isEnable('last') && html.push(self.buildItem('last', self.options.totalPages));
            
            html.push('<span class="page-skip">');
            html.push('第' + self.options.currentPage + '/' + self.options.totalPages + '页 共' + self.options.totalCounts+ '记录');
            html.push('<input type="text" class="text" value="' + self.options.currentPage + '" size="3" id="txtGoto">页');
            html.push('<input type="button" class="button" value="确定" jp-role="page" >');
            html.push('</span>');

            if (self.options.wrapper) {
                self.container.html($(self.options.wrapper).html(html.join('')).HiPaginatorHTML());
            } else {
                self.container.html(html.join(''));
            }
        };

        self.buildItem = function (type, pageData) {
            var html = self.options[type]
                .replace(/{{page}}/g, pageData)
                .replace(/{{totalPages}}/g, self.options.totalPages)
                .replace(/{{totalCounts}}/g, self.options.totalCounts);

            return $(html).attr({
                'jp-role': type,
                'jp-data': pageData
            }).HiPaginatorHTML();
        };

        self.setStatus = function () {
            var options = self.options;

            if (!self.isEnable('first') || options.currentPage === 1) {
                $('[jp-role=first]', self.container).addClass(options.disableClass);
            }
            if (!self.isEnable('prev') || options.currentPage === 1) {
                $('[jp-role=prev]', self.container).addClass(options.disableClass);
            }
            if (!self.isEnable('next') || options.currentPage >= options.totalPages) {
                $('[jp-role=next]', self.container).addClass(options.disableClass);
            }
            if (!self.isEnable('last') || options.currentPage >= options.totalPages) {
                $('[jp-role=last]', self.container).addClass(options.disableClass);
            }

            $('[jp-role=page]', self.container).removeClass(options.activeClass);
            $('[jp-role=page][jp-data=' + options.currentPage + ']', self.container).addClass(options.activeClass);
        };

        self.getPages = function () {
            var pages = [],
                visiblePages = self.options.visiblePages,
                currentPage = self.options.currentPage,
                totalPages = self.options.totalPages;

            if (visiblePages > totalPages) {
                visiblePages = totalPages;
            }

            var half = Math.floor(visiblePages / 2);
            var start = currentPage - half + 1 - visiblePages % 2;
            var end = currentPage + half;

            if (start < 1) {
                start = 1;
                end = visiblePages;
            }
            if (end > totalPages) {
                end = totalPages;
                start = 1 + totalPages - visiblePages;
            }

            var itPage = start;
            while (itPage <= end) {
                pages.push(itPage);
                itPage++;
            }

            return pages;
        };

        self.isNumber = function (value) {
            var type = typeof value;
            return type === 'number' || type === 'undefined';
        };

        self.isEnable = function (type) {
            return self.options[type] && typeof self.options[type] === 'string';
        };

        self.switchPage = function (pageIndex) {
            self.options.currentPage = pageIndex;
            self.render();
        };

        self.fireEvent = function (pageIndex, type) {
            return (typeof self.options.onPageChange !== 'function') || (self.options.onPageChange(pageIndex, type) !== false);
        };

        self.callMethod = function (method, options) {
            switch (method) {
                case 'option':  //设置选项值
                    self.options = $.extend({}, self.options, options);
                    self.verify();
                    self.render();
                    break;
                case 'destroy':  //释放插件
                    self.container.empty();
                    self.container.removeData(HIPAGINATOR_DATA_NAME);
                    break;
                case 'getOption':
                    if (options) {
                        return self.options[options[0]];
                    } else {
                        return self.options;
                    }
                case 'getPagesize':
                    return self.options.pageSize;
                case 'getCurrentPage':
                    return self.options.currentPage;
                case 'redraw':
                    if (options) {
                        self.options = $.extend({}, self.options, options);
                    }
                    self.init(self.options);
                    break;
                default :
                    throw new Error('[HiPaginator] method "' + method + '" does not exist');
            }

            return self.container;
        };

        self.bindEvents = function () {
            var opts = self.options;

            self.container.off();
            self.container.on('click', '[jp-role]', function () {
                var $el = $(this);
                if ($el.hasClass(opts.disableClass) || $el.hasClass(opts.activeClass)) {
                    return;
                }
                //处理跳转按钮
                if ($el.attr("type") == "button")
                {
                    var txtval = parseInt($el.siblings(".text").val());
                    if (txtval <= 0 || isNaN(txtval)) {
                        return;
                    }
                    $el.attr("jp-data", txtval);
                }

                var pageIndex = +$el.attr('jp-data');
                //if (pageIndex > opts.totalPages) {
                //    throw new Error("The pageIndex cannot be greater than "+opts.totalPages+";");
                //}
                if (self.fireEvent(pageIndex, 'change')) {
                    self.switchPage(pageIndex);
                }
            });
        };

        self.init(options);

        return self.container;
    };

    $.HiPaginator.defaultOptions = {
        wrapper: '',
        first: '<li class="page-first"><a href="javascript:;">First</a></li>',
        prev: '<li class="page-prev"><a href="javascript:;">Previous</a></li>',
        next: '<li class="page-next"><a href="javascript:;">Next</a></li>',
        last: '<li class="page-last"><a href="javascript:;">Last</a></li>',
        page: '<li><a href="javascript:;">{{page}}</a></li>',
        totalPages: 0,
        totalCounts: 0,
        pageSize: 0,
        currentPage: 1,
        visiblePages: 7,
        disableClass: 'disabled',
        activeClass: 'page-cur',
        onPageChange: null
    };

    $.fn.HiPaginator = function () {
        var self = this,
            args = Array.prototype.slice.call(arguments);

        if (typeof args[0] === 'string') {
            var $instance = $(self).data(HIPAGINATOR_DATA_NAME);
            if (args[0] == "isExist")
            {
                return !!$instance;
            }
            if (!$instance) {
                throw new Error('[HiPaginator] the element is not instantiated');
            } else {
                return $instance.callMethod(args[0], args[1]);
            }
        } else {
            return new $.HiPaginator(this, args[0]);
        }
    };

})(jQuery);