var HiShop = HiShop ? HiShop : {};
HiShop.Constant = HiShop.Constant ? HiShop.Constant : {},
    HiShop.popbox = HiShop.popbox ? HiShop.popbox : {},
    HiShop.linkType = {
        1: "选择商品",
        2: "分类导航",
        3: "商品分类",
        4: "品牌专题",
        5: "营销活动",
        6: "店铺主页",
        7: "会员主页",
        8: "购物车",
        9: "全部商品",
        10: "自定义链接",
        11: "调查问卷",
        12: "团购活动",
        13: "限时抢购",
        14: "自定义页面",
        15: "积分商城",
        16: "优惠券列表",
        17: "注册送券",
        18: "选择优惠券",
        19: "火拼团",
        20: "周边门店",
        21: "选择文章",
        22: "摇一摇",
        23: "电话",
        24: "品牌列表",
        25: "专题名称",
        26: "文章列表",
    },
    HiShop.getTimestamp = function () {
        var a = new Date;
        return "" + a.getFullYear() + parseInt(a.getMonth() + 1) + a.getDate() + a.getHours() + a.getMinutes() + a.getSeconds() + a.getMilliseconds()
    },
    HiShop.hint = function (a, b, c) {
        if (a && b) {
            var d = $("#tpl_hint").html(),
                e = _.template(d, {
                    type: a,
                    content: b
                }),
                f = $(e),
                g = 200,
                c = c || 1500;
            $("body").append(f.css({
                opacity: "0",
                zIndex: "999999"
            })),
                f.animate({
                    opacity: 1,
                    top: 200
                },
                    g,
                    function () {
                        setTimeout(function () {
                            f.animate({
                                opacity: 0,
                                top: 600
                            },
                                g,
                                function () {
                                    $(this).remove()
                                })
                        },
                            c)
                    })
        }
    },
    HiShop.FormShowError = function (a, b, c) {
        a && b && (void 0 == c && (c = !0), a.addClass("error").siblings(".fi-help-text").addClass("error").text(b).show(), c && a.focus(), "select" == a[0].nodeName.toLowerCase() && a.siblings(".select-sim").addClass("error"), a.one("change",
            function () {
                HiShop.FormClearError($(this))
            }))
    },
    HiShop.FormClearError = function (a) {
        a && (a.removeClass("error").siblings(".fi-help-text").hide(), "select" == a[0].nodeName.toLowerCase() && a.siblings(".select-sim").removeClass("error"))
    },
    HiShop.showQrcode = function (a) {
        var b = $("#qrcode");
        if (!b.length) {
            var c = _.template($("#tpl_qrcode").html(), {
                src: a
            });
            b = $(c),
                b.click(function () {
                    b.fadeOut(300)
                }),
                $("body").append(b)
        }
        b.find("img").attr("src", a),
            b.fadeIn(300)
    },
    HiShop.changeWizardStep = function (a, b) {
        var c = $(a),
            d = c.find(".wizard-item");
        d.removeClass("process complete");
        for (var e = 0; b - 1 >= e; e++) d.filter(":eq(" + e + ")").addClass("complete");
        d.filter(":eq(" + b + ")").addClass("process")
    },
    HiShop.autoLocation = function (a, b) {
        if (a) {
            var b = b ? b : 2e3;
            timer = setInterval(function () {
                1e3 >= b ? (clearInterval(timer), window.location.href = a) : (b -= 1e3, $("#j-autoLocation-second").text(b / 1e3))
            },
                1e3)
        }
    },
    HiShop.ajaxPopTable = function (a) {
        var b, c, d = {
            title: "",
            url: "",
            data: {
                p: 1
            },
            tpl: "",
            onOpen: null,
            onPageChange: null
        },
            e = $.extend(!0, {},
                d, a),
            f = $("<div></div>"),
            g = function (a) {
                var d = e.tpl,
                    h = e.url,
                    i = function (h) {
                        b = h;
                        var i = _.template(d, h),
                            j = $(i);
                        f.empty().append(j),
                            f.find(".paginate a:not(.disabled,.cur)").click(function () {
                                for (var a = $(this).attr("href"), b = a.split("/"), c = 0; c < b.length; c++)
                                    if ("p" == b[c]) {
                                        e.data.p = b[c + 1],
                                            g();
                                        break
                                    }
                                return !1
                            }),
                            a && a(),
                            e.onPageChange && e.onPageChange(c, b)
                    };
                $.ajax({
                    url: h,
                    type: "post",
                    dataType: "json",
                    data: e.data,
                    success: function (a) {
                        1 == a.status ? i(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            };
        g(function () {
            $.jBox.show({
                title: e.title,
                content: f,
                btnOK: {
                    show: !1
                },
                btnCancel: {
                    show: !1
                },
                onOpen: function (a) {
                    c = a,
                        e.onOpen && e.onOpen(a, b)
                }
            })
        })
    },
    HiShop.popbox.ImgPicker = function (a) {
        var b, c = $("#tpl_popbox_ImgPicker").html(),
            d = $(c),
            e = function (a, c) {
                var f = function (a) {
                    if (b = a.list, b && b.length) {
                        var f = _.template($("#tpl_popbox_ImgPicker_listItem").html(), {
                            dataset: b
                        }),
                            g = $(f);
                        g.filter("li").click(function () {
                            $(this).toggleClass("selected")
                        }),
                            d.find(".imgpicker-list").empty().append(g);
                        var h = a.page,
                            i = $(h);
                        i.filter("a:not(.disabled,.cur)").click(function () {
                            //var a = $(this).attr("href"),
                            //b = a.split("/");
                            //return b = b[b.length - 1],
                            //b = b.replace(/.html/, ""),
                            //e(b),
                            //!1
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            d.find(".paginate").empty().append(i)
                    } else d.find(".imgpicker-list").append("<p class='txtCenter'>对不起，暂无图片</p>");
                    c && c()
                };
                $.ajax({
                    url: "/Design/getImg",
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a)
                    },
                    success: function (a) {
                        1 == a.status ? f(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            f = function (b) {
                var c = [];
                d.find("#imgpicker_upload_input").uploadify({
                    debug: !1,
                    auto: !0,
                    formData: {
                        PHPSESSID: $.cookie("PHPSESSID")
                    },
                    width: 60,
                    height: 60,
                    multi: !0,
                    swf: "/Utility/Ueditor/plugins/uploadify/uploadify.swf",
                    uploader: "/Design/uploadFile",
                    buttonText: "+",
                    fileSizeLimit: "2MB",
                    fileTypeExts: "*.jpg; *.jpeg; *.png; *.gif; *.bmp",
                    onSelectError: function (a, b, c) {
                        switch (b) {
                            case -100:
                                HiShop.hint("danger", "对不起，系统只允许您一次最多上传10个文件");
                                break;
                            case -110:
                                HiShop.hint("danger", "对不起，文件 [" + a.name + "] 大小超出2MB！");
                                break;
                            case -120:
                                HiShop.hint("danger", "文件 [" + a.name + "] 大小异常！");
                                break;
                            case -130:
                                HiShop.hint("danger", "文件 [" + a.name + "] 类型不正确！")
                        }
                    },
                    onFallback: function () {
                        HiShop.hint("danger", "您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。")
                    },
                    onUploadSuccess: function (a, b, e) {
                        var b = $.parseJSON(b),
                            f = $("#tpl_popbox_ImgPicker_uploadPrvItem").html(),
                            g = d.find(".imgpicker-upload-preview"),
                            h = b.file_path;
                        c.push(h);
                        var i = _.template(f, {
                            url: h
                        }),
                            j = $(i);
                        j.find(".j-imgpicker-upload-btndel").click(function () {
                            var a = d.find(".imgpicker-upload-preview li").index($(this).parent("li"));
                            j.fadeOut(300,
                                function () {
                                    c.splice(a, 1),
                                        $(this).remove()
                                })
                        }),
                            g.append(j)
                    },
                    onUploadError: function (a, b, c, d) {
                        HiShop.hint("danger", "对不起：" + a.name + "上传失败：" + d)
                    }
                }),
                    d.find("#j-btn-uploaduse").click(function () {
                        a && a(c),
                            $.jBox.close(b)
                    })
            };
        e(1,
            function () {
                $.jBox.show({
                    title: "选择图片",
                    content: d,
                    btnOK: {
                        show: !1
                    },
                    btnCancel: {
                        show: !1
                    },
                    onOpen: function (c) {
                        var e = d.find("#j-btn-listuse");
                        e.click(function () {
                            var e = [];
                            d.find(".imgpicker-list li.selected").each(function () {
                                e.push(b[$(this).index()])
                            }),
                                a && a(e),
                                $.jBox.close(c)
                        }),
                            d.find(".j-initupload").one("click",
                                function () {
                                    f(c)
                                })
                    }
                })
            })
    },
    HiShop.popbox.IconPicker = function (a) {
        var b, c = $("#icon_imgPicker").html(),
            d = $(c);
        b = $.browser.chrome ? "body" : document.documentElement || document.body,
            $(b).append(d);
        var e = $("#icon_imglist").html(),
            f = {
                style: "style1",
                color: "color0"
            },
            g = function (a) {
                f = a ? a : f;
                var b = _.template(e, {
                    data: HiShop.popbox.iconimgsrc.data[f.style][f.color]
                });
                d.find(".albums-icon-tab").html(b),
                    d.find(".albums-icon-tab").find("li").click(function (a) {
                        var b = $(this);
                        b.hasClass("selected") || b.addClass("selected").siblings("li").removeClass("selected")
                    })
            };
        g(f),
            d.find(".albums-cr-actions").children("a").click(function (a) {
                var b = $(this),
                    c = b.data("style");
                f.style = c,
                    b.addClass("cur").siblings("a").removeClass("cur"),
                    g(f)
            }),
            d.find(".albums-color-tab").find("li").click(function (a) {
                var b = $(this),
                    c = b.data("color");
                f.color = c,
                    b.addClass("cur").siblings("li").removeClass("cur"),
                    g(f),
                    "color1" == c && $(".albums-icon-tab").find("li").css({
                        background: "#333"
                    })
            });
        var h = [];
        d.find("#j-useIcon").click(function (b) {
            var c = d.find(".albums-icon-tab").find("li.selected");
            if (0 == c.length) return HiShop.hint("danger", "对不起，请选择一张小图标"), !1;
            var e = c.children("img").attr("src");
            //e = e.replace("Public", "PublicMob"),
            h.push(e),
            i(),
            a && a(h)
        });
        var i = function () {
            d.remove()
        };
        d.find("#Jclose").click(function (a) {
            i()
        })
    },
    HiShop.popbox.ModulePicker = function (a) {
        var b, c = $("#tpl_popbox_ModulePicker").html(),
            d = $(c),
            e = function (a, c) {
                var f = function (a) {
                    if (b = a.list, b && b.length) {
                        var f = $("#tpl_popbox_ModulePicker_item").html(),
                            g = _.template(f, {
                                dataset: b
                            }),
                            h = $(g);
                        d.find(".modulePicker-list").empty().append(h);
                        var i = a.page,
                            j = $(i);
                        j.filter("a:not(.disabled,.cur)").click(function () {
                            //var a = $(this).attr("href"),
                            //b = a.split("/");
                            //return b = b[b.length - 1],
                            //b = b.replace(/.html/, ""),
                            //e(b),
                            //!1
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            d.find(".paginate").empty().append(j)
                    } else d.find(".modulePicker-list").append("<p class='txtCenter'>对不起，暂无自定义模块</p>");
                    c && c()
                };
                $.ajax({
                    url: "/Design/getModule",
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a)
                    },
                    success: function (a) {
                        1 == a.status ? f(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            };
        e(1,
            function () {
                $.jBox.show({
                    title: "选择自定义模块",
                    content: d,
                    btnOK: {
                        show: !1
                    },
                    btnCancel: {
                        show: !1
                    },
                    onOpen: function (c) {
                        d.on("click", ".j-select",
                            function () {
                                var d = $(".modulePicker-list li").index($(this).parent("li"));
                                a && a(b[d]),
                                    $.jBox.close(c)
                            })
                    }
                })
            })
    },
    HiShop.popbox.GoodsAndGroupPicker = function (a, b) {
        var strUrl = window.location.toLocaleString().toLocaleLowerCase();
        var clientName = "Appshop";

        var c, d, e = $("#tpl_popbox_GoodsAndGroupPicker").html(),
            f = $(e),
            g = [],
            h = [],
            i = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_GoodsAndGroupPicker_goodsitem").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            j = $(e);
                        j.find(".j-select").click(function () {
                            var a, b = $(this),
                                d = b.parent("li"),
                                e = d.index(),
                                f = d.data("item"),
                                i = $(".j-verify").val();
                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                    for (var j = 0; j < g.length; j++)
                                        if (f == g[j].item_id) {
                                            g.splice(j, 1);
                                            break
                                        }
                                if (0 != h.length)
                                    for (var j = 0; j < h.length; j++) {
                                        var k = h[j];
                                        if (f == k) {
                                            h.splice(j, 1);
                                            break
                                        }
                                    }
                                if (0 != a.length) {
                                    for (var j = 0; j < a.length; j++) {
                                        var k = a[j];
                                        if (f == k) {
                                            a.splice(j, 1);
                                            break
                                        }
                                    }
                                    i = a.join(","),
                                        $(".j-verify").val(i)
                                }
                            } else d.addClass("selected"),
                                b.addClass("btn-success").text("已选"),
                                g.push(c[e]),
                                h.push(f),
                                a.push(f),
                                i = a.join(","),
                                $(".j-verify").val(i)
                        }),
                            f.find(".gagp-goodslist").empty().append(j);
                        var k = a.page,
                            l = $(k);
                        l.filter("a:not(.disabled,.cur)").click(function () {
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate:eq(0)").empty().append(l)
                    } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var m = [];
                    "" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
                        var c = $(this).data("item");
                        m.push(c)
                    }),
                        f.find("li").each(function (a, b) {
                            var c = $(this),
                                d = c.data("item");
                            $.each(m,
                                function (a, b) {
                                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                        }),
                        b && b()
                };
                $.ajax({

                    url: HiShop.Config.AjaxUrl.goodsList,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a),
                        client: clientName

                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            j = function (a, b) {
                var c = function (a) {
                    if (d = a.list, d && d.length) {
                        var c = $("#tpl_popbox_GoodsAndGroupPicker_groupitem").html(),
                            e = _.template(c, {
                                dataset: d
                            }),
                            g = $(e);
                        f.find(".gagp-grouplist").empty().append(g);
                        var h = a.page,
                            i = $(h);
                        i.filter("a:not(.disabled,.cur)").click(function () {                          
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate").empty().append(i)
                    } else f.find(".gagp-grouplist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var k = $(".badge-success").data("group");
                    void 0 != k && f.find(".gagp-grouplist li").each(function (a, b) {
                        var c = $(this),
                            d = c.data("group");
                        k == d && c.find(".j-select").addClass("btn-success").text("已选")
                    }),
                        b && b()
                };
                /*s*/
                $.ajax({
                    url: HiShop.Config.AjaxUrl.goodGroup,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a)
                    },
                    success: function (a) {
                        1 == a.status ? c(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            k = function (a, c) {
                f.on("click", ".j-btn-goodsuse",
                    function () {
                        var c = 1;
                        b && b(g, c, h),
                            $.jBox.close(a)
                    })
            },
            l = function (a) {
                var d = 1;
                f.find(".j-btn-goodsuse").remove(),
                    f.on("click", ".gagp-goodslist .j-select",
                        function () {
                            var e = $(".gagp-goodslist li").index($(this).parent("li"));
                            b && b(c[e], d),
                                $.jBox.close(a)
                        })
            },
            m = function (a) {
                var c = 2;
                f.on("click", ".gagp-grouplist .j-select",
                    function () {
                        var e = $(".gagp-grouplist li").index($(this).parent("li"));
                        b && b(d[e], c),
                            $.jBox.close(a)
                    })
            },
            n = function (a) {
                l(a),
                    f.find(".j-tab-group").one("click",
                        function () {
                            j(1,
                                function () {
                                    m(a)
                                })
                        })
            };
        switch (a) {
            case "goods":
            case "goodsMulti":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    i(1,
                        function () {
                            var b = '<span class="fl">选择商品</span><div class="goodsearch"><input type="text" name="title" placeholder="请输入商品名称" style="width:250px;" />';
                            b += '<select class="select small newselect" id="ddlCategories" style="width:200px;">';
                            $($("#dropCategories").find("option")).each(function () {
                                b += '<option value="' + $(this).attr("value") + '">' + $(this).text() + '</option>';
                            });
                            b += '</select><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                            $.jBox.show({
                                title: b,
                                content: f,
                                width: 710,
                                btnOK: {
                                    show: false
                                },
                                btnCancel: {
                                    show: false
                                },
                                onOpen: function (b) {
                                    if ("goodsMulti" == a) {
                                        var d = [];
                                        $(".img-list li").not(".img-list-add").each(function (a, b) {
                                            var c = $(this).data("item");
                                            d.push(c)
                                        }),
                                            f.find("li").each(function (a, b) {
                                                var c = $(this),
                                                    e = c.data("item");
                                                $.each(d,
                                                    function (a, b) {
                                                        e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                                    })
                                            }),
                                            k(b, d)
                                    } else l(b);
                                    $(document).on("click", ".jGetgood",
                                        function (a) {
                                            var b = $(this).siblings("input").val(),
                                                d = $(this).siblings("select").val(),
                                                e = function (a, i) {
                                                    a = a ? a : 1;
                                                    var j = function (a) {
                                                        if (c = a.list, c && c.length) {
                                                            var b = $("#tpl_popbox_GoodsAndGroupPicker_goodsitem").html(),
                                                                d = _.template(b, {
                                                                    dataset: c
                                                                }),
                                                                j = $(d);
                                                            j.find(".j-select").click(function () {
                                                                var a, b = $(this),
                                                                    d = b.parent("li"),
                                                                    e = d.index(),
                                                                    f = d.data("item"),
                                                                    i = $(".j-verify").val();
                                                                if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                                    if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                                                        for (var j = 0; j < g.length; j++)
                                                                            if (f == g[j].item_id) {
                                                                                g.splice(j, 1);
                                                                                break
                                                                            }
                                                                    if (0 != h.length)
                                                                        for (var j = 0; j < h.length; j++) {
                                                                            var k = h[j];
                                                                            if (f == k) {
                                                                                h.splice(j, 1);
                                                                                break
                                                                            }
                                                                        }
                                                                    if (0 != a.length) {
                                                                        for (var j = 0; j < a.length; j++) {
                                                                            var k = a[j];
                                                                            if (f == k) {
                                                                                a.splice(j, 1);
                                                                                break
                                                                            }
                                                                        }
                                                                        i = a.join(","),
                                                                            $(".j-verify").val(i)
                                                                    }
                                                                } else d.addClass("selected"),
                                                                    b.addClass("btn-success").text("已选"),
                                                                    g.push(c[e]),
                                                                    h.push(f),
                                                                    a.push(f),
                                                                    i = a.join(","),
                                                                    $(".j-verify").val(i)
                                                            }),
                                                                f.find(".gagp-goodslist").empty().append(j);
                                                            var k = a.page,
                                                                l = $(k);
                                                            l.filter("a:not(.disabled,.cur)").click(function () {
                                                                //var a = $(this).attr("href"),
                                                                //b = a.split("/");
                                                                //return b = b[b.length - 1],
                                                                //b = b.replace(/.html/, ""),
                                                                //e(b),
                                                                //!1
                                                                var a = $(this).attr("page"),
                                                                    b = a;
                                                                return b = b,
                                                                    b = b,
                                                                    e(b), !1
                                                            }),
                                                                f.find(".paginate:eq(0)").empty().append(l)
                                                        } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                                            f.find(".paginate").empty();
                                                        i && i()
                                                    };
                                                    /*搜索商品*/
                                                    $.ajax({
                                                        url: HiShop.Config.AjaxUrl.goodsList,
                                                        type: "post",
                                                        dataType: "json",
                                                        data: {
                                                            p: parseInt(a),
                                                            title: b,
                                                            client: clientName,
                                                            category: d
                                                        },
                                                        success: function (a) {
                                                            1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                                        }
                                                    })
                                                };
                                            e()
                                        })
                                }

                            })
                        });
                break;
            case "group":
                f.find(".tabs").remove(),
                    f.find(".gagp-grouplist").unwrap().unwrap(),
                    f.find(".tc[data-index='1']").remove(),
                    j(1,
                        function () {
                            $.jBox.show({
                                title: "选择商品分组",
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (a) {
                                    m(a)
                                }
                            })
                        });
                break;
            case "all":
                i(1,
                    function () {
                        $.jBox.show({
                            title: "选择商品或商品分组",
                            content: f,
                            btnOK: {
                                show: !1
                            },
                            btnCancel: {
                                show: !1
                            },
                            onOpen: function (a) {
                                n(a)
                            }
                        })
                    })
        }
    },
    HiShop.popbox.MgzAndMgzCate = function (a, b) {
        var c, d, e = $("#tpl_popbox_MgzAndMgzCate").html(),
            f = $(e),
            g = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_MgzAndMgzCate_item").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            h = $(e);
                        h.find(".j-select").click(function () {
                            var a = $(this),
                                b = a.parent("li");
                            b.hasClass("selected") ? (b.removeClass("selected"), a.removeClass("btn-success").text("选取")) : (b.addClass("selected"), a.addClass("btn-success").text("已选"))
                        }),
                            f.find(".mgz-list-panel1").empty().append(h);
                        var i = a.page,
                            j = $(i);
                        j.filter("a:not(.disabled,.cur)").click(function () {
                            //var a = $(this).attr("href"),
                            //b = a.split("/");
                            //return b = b[b.length - 1],
                            //b = b.replace(/.html/, ""),
                            //g(b),
                            //!1
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate").empty().append(j)
                    } else f.find(".mgz-list-panel1").empty().append("<p class='txtCenter'>对不起，暂无数据</p>");
                    b && b()
                };
                $.ajax({
                    url: "/Design/getMagazine",
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a)
                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            h = function (a, b) {
                var c = function (a) {
                    if (d = a.list, d && d.length) {
                        var c = $("#tpl_popbox_MgzAndMgzCate_item").html(),
                            e = _.template(c, {
                                dataset: d
                            }),
                            g = $(e);
                        g.find(".j-select").click(function () {
                            var a = $(this),
                                b = a.parent("li");
                            b.hasClass("selected") ? (b.removeClass("selected"), a.removeClass("btn-success").text("选取")) : (b.addClass("selected"), a.addClass("btn-success").text("已选"))
                        }),
                            f.find(".mgz-list-panel2").empty().append(g);
                        var i = a.page,
                            j = $(i);
                        j.filter("a:not(.disabled,.cur)").click(function () {
                            //var a = $(this).attr("href"),
                            //b = a.split("/");
                            //return b = b[b.length - 1],
                            //b = b.replace(/.html/, ""),
                            //h(b),
                            //!1
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate").empty().append(j)
                    } else f.find(".mgz-list-panel2").empty().append("<p class='txtCenter'>对不起，暂无数据</p>");
                    b && b()
                };
                $.ajax({
                    url: "/Design/getMagazineCategory",
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a)
                    },
                    success: function (a) {
                        1 == a.status ? c(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            i = function (a) {
                f.on("click", ".mgz-list-panel1 .j-select",
                    function () {
                        var d = $(".mgz-list-panel1 li").index($(this).parent("li"));
                        b && b(c[d], 3),
                            $.jBox.close(a)
                    })
            },
            j = function (a) {
                f.on("click", ".mgz-list-panel2 .j-select",
                    function () {
                        var c = $(".mgz-list-panel2 li").index($(this).parent("li"));
                        b && b(d[c], 4),
                            $.jBox.close(a)
                    })
            },
            k = function (a) {
                f.on("click", ".j-btn-use",
                    function () {
                        var c = [],
                            e = 4;
                        f.find(".mgz-list-panel2 li.selected").each(function () {
                            c.push(d[$(this).index()])
                        }),
                            b && b(c, e),
                            $.jBox.close(a)
                    })
            },
            l = function (a) {
                i(a),
                    f.find(".j-tab-mgzcate").one("click",
                        function () {
                            h(1,
                                function () {
                                    j(a)
                                })
                        })
            };
        switch (a) {
            case "mgzCate":
                f.find(".tabs").remove(),
                    f.find(".mgz-list-panel2").unwrap().unwrap(),
                    f.find(".tc[data-index='1']").remove(),
                    f.find(".j-btn-use").remove(),
                    h(1,
                        function () {
                            $.jBox.show({
                                title: "选择专题分类",
                                width: '750',
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (a) {
                                    j(a)
                                }
                            })
                        });
                break;
            case "mgzCateMulti":
                f.find(".tabs").remove(),
                    f.find(".mgz-list-panel2").unwrap().unwrap(),
                    f.find(".tc[data-index='1']").remove(),
                    h(1,
                        function () {
                            $.jBox.show({
                                title: "选择专题分类",
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (a) {
                                    k(a)
                                }
                            })
                        });
                break;
            case "mgz":
                f.find(".tabs").remove(),
                    f.find(".mgz-list-panel1").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    f.find(".j-btn-use").remove(),
                    g(1,
                        function () {
                            $.jBox.show({
                                title: "选择专题页面",
                                width: '760',
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (a) {
                                    l(a)
                                }
                            })
                        });
                break;
            case "all":
                f.find(".j-btn-use").remove(),
                    g(1,
                        function () {
                            $.jBox.show({
                                title: "选择专题页面或者分类",
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (a) {
                                    l(a)
                                }
                            })
                        })
        }
        switch (a) {
            case "goods":
            case "goodsMulti":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    showListRender_goods(1,
                        function () {
                            $.jBox.show({
                                title: "选择商品",
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (b) {
                                    "goodsMulti" == a ? selectEvent_goods_multi(b) : selectEvent_goods(b)
                                }
                            })
                        });
                break;
            case "group":
                f.find(".tabs").remove(),
                    f.find(".gagp-grouplist").unwrap().unwrap(),
                    f.find(".tc[data-index='1']").remove(),
                    showListRender_group(1,
                        function () {
                            $.jBox.show({
                                title: "选择商品分组",
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (a) {
                                    selectEvent_group(a)
                                }
                            })
                        });
                break;
            case "all":
                showListRender_goods(1,
                    function () {
                        $.jBox.show({
                            title: "选择商品或商品分组",
                            content: f,
                            btnOK: {
                                show: !1
                            },
                            btnCancel: {
                                show: !1
                            },
                            onOpen: function (a) {
                                selectEvent_goodsAndGroup(a)
                            }
                        })
                    })
        }
    },
    HiShop.popbox.GamePicker = function (a, b) {
        var c = $("#tpl_popbox_GamePicker").html(),
            d = $(c),
            e = {
                1: [],
                2: [],
                3: [],
                4: [],
                5: []
            },
            f = function (a, b, c) {
                var g = function (b) {
                    if (e[a] = b.list, e[a] && e[a].length) {
                        var g = $("#tpl_popbox_GamePicker_item").html(),
                            h = _.template(g, {
                                dataset: e[a]
                            }),
                            i = $(h);
                        i.find(".j-select").click(function () {
                            var a = $(this),
                                b = a.parent("li");
                            b.hasClass("selected") ? (b.removeClass("selected"), a.removeClass("btn-success").text("选取")) : (b.addClass("selected"), a.addClass("btn-success").text("已选"))
                        }),
                            d.find(".game-list-panel" + a).empty().append(i);
                        var j = b.page,
                            k = $(j);
                        k.filter("a:not(.disabled,.cur)").click(function () {
                            //var b = $(this).attr("href"),
                            //c = b.split("/");
                            //return c = c[c.length - 1],
                            //c = c.replace(/.html/, ""),
                            //f(a, c),
                            //!1
                            var b = $(this).attr("page"),
                                c = b
                            return c = c,
                                c = c,
                                f(a, c), !1

                        }),
                            d.find(".paginate:eq(" + (a - 1) + ")").empty().append(k)
                    } else d.find(".game-list-panel" + a).empty().append("<p class='txtCenter'>对不起，暂无数据</p>");
                    c && c(a)
                },
                    h = {
                        1: 1,
                        2: 2,
                        3: 3,
                        4: 4,
                        5: 5
                    };
                $.ajax({
                    url: HiShop.Config.AjaxUrl.gamesUrl,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(b),
                        type: parseInt(h[a]),
                        client: "Appshop"
                    },
                    success: function (a) {
                        1 == a.status ? g(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            g = function (a, c) {
                d.on("click", ".game-list-panel" + c + " .j-select",
                    function () {
                        var d = $(".game-list-panel" + c + " li").index($(this).parent("li"));
                        b && b(e[c][d], 5),
                            $.jBox.close(a)
                    })
            };
        f(1, 1,
            function (a) {
                $.jBox.show({
                    title: "选择营销活动",
                    width: '710',
                    content: d,
                    btnOK: {
                        show: !1
                    },
                    btnCancel: {
                        show: !1
                    },
                    onOpen: function (b) {
                        g(b, a),
                            d.find(".j-tab-game").one("click",
                                function () {
                                    var a = $(this).data("index");
                                    f(a, 1,
                                        function (a) {
                                            g(b, a)
                                        })
                                })
                    }
                })
            })
    },
    HiShop.popbox.CouponListPicker = function (a, b) {
        var strUrl = window.location.toLocaleString().toLocaleLowerCase();
        var clientName = "Appshop";     

        var c, d, e = $("#tpl_popbox_CouponListPicker").html(),
            f = $(e),
            g = [],
            h = [],
            i = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_CouponListPicker_graphicsitem").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            j = $(e);
                        j.find(".j-select").click(function () {
                            var a, b = $(this),
                                d = b.parent("li"),
                                e = d.index(),
                                f = d.data("item"),
                                i = $(".j-verify").val();
                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                    for (var j = 0; j < g.length; j++)
                                        if (f == g[j].item_id) {
                                            g.splice(j, 1);
                                            break
                                        }
                                if (0 != h.length)
                                    for (var j = 0; j < h.length; j++) {
                                        var k = h[j];
                                        if (f == k) {
                                            h.splice(j, 1);
                                            break
                                        }
                                    }
                                if (0 != a.length) {
                                    for (var j = 0; j < a.length; j++) {
                                        var k = a[j];
                                        if (f == k) {
                                            a.splice(j, 1);
                                            break
                                        }
                                    }
                                    i = a.join(","),
                                        $(".j-verify").val(i)
                                }
                            } else d.addClass("selected"),
                                b.addClass("btn-success").text("已选"),
                                g.push(c[e]),
                                h.push(f),
                                a.push(f),
                                i = a.join(","),
                                $(".j-verify").val(i)
                        }),
                            f.find(".gagp-goodslist").empty().append(j);
                        var k = a.page,
                            l = $(k);
                        l.filter("a:not(.disabled,.cur)").click(function () {
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate:eq(0)").empty().append(l)
                    } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var m = [];
                    "" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
                        var c = $(this).data("item");
                        m.push(c)
                    }),
                        f.find("li").each(function (a, b) {
                            var c = $(this),
                                d = c.data("item");
                            $.each(m,
                                function (a, b) {
                                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                        }),
                        b && b()
                };
                $.ajax({

                    url: HiShop.Config.AjaxUrl.Coupons,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a),
                        client: clientName
                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            l = function (a) {
                var d = 18;
                f.find(".j-btn-goodsuse").remove(),
                    f.on("click", ".gagp-goodslist .j-select",
                        function () {
                            var e = $(".gagp-goodslist li").index($(this).parent("li"));
                            b && b(c[e], d),
                                $.jBox.close(a)
                        })
            }
        switch (a) {
            case "all":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    i(1,
                        function () {
                            var b = '<span class="fl">选择优惠券</span><div class="goodsearch"><input type="text" name="title" placeholder="请输入优惠券名称" /><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                            $.jBox.show({
                                title: b,
                                width: '700',
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (b) {
                                    if ("goodsMulti" == a) {
                                        var d = [];
                                        $(".img-list li").not(".img-list-add").each(function (a, b) {
                                            var c = $(this).data("item");
                                            d.push(c)
                                        }),
                                            f.find("li").each(function (a, b) {
                                                var c = $(this),
                                                    e = c.data("item");
                                                $.each(d,
                                                    function (a, b) {
                                                        e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                                    })
                                            }),
                                            k(b, d)
                                    } else l(b);
                                    $(document).on("click", ".jGetgood",
                                        function (a) {
                                            var b = $(this).siblings("input").val();

                                            e = function (a, i) {
                                                a = a ? a : 1;
                                                var j = function (a) {
                                                    if (c = a.list, c && c.length) {
                                                        var b = $("#tpl_popbox_CouponListPicker_graphicsitem").html(),
                                                            d = _.template(b, {
                                                                dataset: c
                                                            }),
                                                            j = $(d);
                                                        j.find(".j-select").click(function () {
                                                            var a, b = $(this),
                                                                d = b.parent("li"),
                                                                e = d.index(),
                                                                f = d.data("item"),
                                                                i = $(".j-verify").val();
                                                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                                                    for (var j = 0; j < g.length; j++)
                                                                        if (f == g[j].item_id) {
                                                                            g.splice(j, 1);
                                                                            break
                                                                        }
                                                                if (0 != h.length)
                                                                    for (var j = 0; j < h.length; j++) {
                                                                        var k = h[j];
                                                                        if (f == k) {
                                                                            h.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                if (0 != a.length) {
                                                                    for (var j = 0; j < a.length; j++) {
                                                                        var k = a[j];
                                                                        if (f == k) {
                                                                            a.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                    i = a.join(","),
                                                                        $(".j-verify").val(i)
                                                                }
                                                            } else d.addClass("selected"),
                                                                b.addClass("btn-success").text("已选"),
                                                                g.push(c[e]),
                                                                h.push(f),
                                                                a.push(f),
                                                                i = a.join(","),
                                                                $(".j-verify").val(i)
                                                        }),
                                                            f.find(".gagp-goodslist").empty().append(j);
                                                        var k = a.page,
                                                            l = $(k);
                                                        l.filter("a:not(.disabled,.cur)").click(function () {
                                                            //var a = $(this).attr("href"),
                                                            //b = a.split("/");
                                                            //return b = b[b.length - 1],
                                                            //b = b.replace(/.html/, ""),
                                                            //e(b),
                                                            //!1
                                                            var a = $(this).attr("page"),
                                                                b = a;
                                                            return b = b,
                                                                b = b,
                                                                e(b), !1
                                                        }),
                                                            f.find(".paginate:eq(0)").empty().append(l)
                                                    } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                                        f.find(".paginate").empty();
                                                    i && i()
                                                };
                                                /*搜索优惠券*/
                                                $.ajax({
                                                    url: HiShop.Config.AjaxUrl.Coupons,
                                                    type: "post",
                                                    dataType: "json",
                                                    data: {
                                                        p: parseInt(a),
                                                        Name: b,
                                                        client: clientName
                                                    },
                                                    success: function (a) {
                                                        1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                                    }
                                                })
                                            };
                                            e()
                                        })
                                }
                            })
                        });
                break;

        }
    },
    HiShop.popbox.PointExchangePicker = function (a, b) {
        var c = $("#tpl_popbox_PointExchangePicker").html(),
            d = $(c),
            e = {
                1: [],
                2: [],
                3: [],
                4: []
            },
            f = function (a, b, c) {
                var g = function (b) {
                    if (e[a] = b.list, e[a] && e[a].length) {
                        var g = $("#tpl_popbox_PointExchangePicker_item").html(),
                            h = _.template(g, {
                                dataset: e[a]
                            }),
                            i = $(h);
                        i.find(".j-select").click(function () {
                            var a = $(this),
                                b = a.parent("li");
                            b.hasClass("selected") ? (b.removeClass("selected"), a.removeClass("btn-success").text("选取")) : (b.addClass("selected"), a.addClass("btn-success").text("已选"))
                        }),
                            d.find(".game-list-panel" + a).empty().append(i);
                        var j = b.page,
                            k = $(j);
                        k.filter("a:not(.disabled,.cur)").click(function () {
                            //var b = $(this).attr("href"),
                            //c = b.split("/");
                            //return c = c[c.length - 1],
                            //c = c.replace(/.html/, ""),
                            //f(a, c),
                            //!1
                            var b = $(this).attr("page"),
                                c = b
                            return c = c,
                                c = c,
                                f(a, c), !1
                        }),
                            d.find(".paginate:eq(" + (a - 1) + ")").empty().append(k)
                    } else d.find(".game-list-panel" + a).empty().append("<p class='txtCenter'>对不起，暂无数据</p>");
                    c && c(a)
                };

                $.ajax({
                    url: HiShop.Config.AjaxUrl.PointExChanges,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(b),
                        //type: parseInt(h[a])
                    },
                    success: function (a) {
                        1 == a.status ? g(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            g = function (a, c) {
                d.on("click", ".game-list-panel" + c + " .j-select",
                    function () {
                        var d = $(".game-list-panel" + c + " li").index($(this).parent("li"));
                        b && b(e[c][d], 14),
                            $.jBox.close(a)
                    })
            };
        f(1, 1,
            function (a) {
                $.jBox.show({
                    title: "选择积分兑换活动",
                    content: d,
                    btnOK: {
                        show: !1
                    },
                    btnCancel: {
                        show: !1
                    },
                    onOpen: function (b) {
                        g(b, a),
                            d.find(".j-tab-game").one("click",
                                function () {
                                    var a = $(this).data("index");
                                    f(a, 1,
                                        function (a) {
                                            g(b, a)
                                        })
                                })
                    }
                })
            })
    },
    HiShop.popbox.VotePicker = function (a, b) {
        var c = $("#tpl_popbox_VotePicker").html(),
            d = $(c),
            e = {
                1: [],
                2: [],
                3: [],
                4: []
            },
            f = function (a, b, c) {
                var g = function (b) {
                    if (e[a] = b.list, e[a] && e[a].length) {
                        var g = $("#tpl_popbox_VotePicker_item").html(),
                            h = _.template(g, {
                                dataset: e[a]
                            }),
                            i = $(h);
                        i.find(".j-select").click(function () {
                            var a = $(this),
                                b = a.parent("li");
                            b.hasClass("selected") ? (b.removeClass("selected"), a.removeClass("btn-success").text("选取")) : (b.addClass("selected"), a.addClass("btn-success").text("已选"))
                        }),
                            d.find(".game-list-panel" + a).empty().append(i);
                        var j = b.page,
                            k = $(j);
                        k.filter("a:not(.disabled,.cur)").click(function () {
                            //var b = $(this).attr("href"),
                            //c = b.split("/");
                            //return c = c[c.length - 1],
                            //c = c.replace(/.html/, ""),
                            //f(a, c),
                            //!1
                            var b = $(this).attr("page"),
                                c = b
                            return c = c,
                                c = c,
                                f(a, c), !1
                        }),
                            d.find(".paginate:eq(" + (a - 1) + ")").empty().append(k)
                    } else d.find(".game-list-panel" + a).empty().append("<p class='txtCenter'>对不起，暂无数据</p>");
                    c && c(a)
                };

                $.ajax({
                    url: HiShop.Config.AjaxUrl.Votes,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(b),
                        //type: parseInt(h[a])
                    },
                    success: function (a) {
                        1 == a.status ? g(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            g = function (a, c) {
                d.on("click", ".game-list-panel" + c + " .j-select",
                    function () {
                        var d = $(".game-list-panel" + c + " li").index($(this).parent("li"));
                        b && b(e[c][d], 15),
                            $.jBox.close(a)
                    })
            };
        f(1, 1,
            function (a) {
                $.jBox.show({
                    title: "选择调查问卷",
                    width: '710',
                    content: d,
                    btnOK: {
                        show: !1
                    },
                    btnCancel: {
                        show: !1
                    },
                    onOpen: function (b) {
                        g(b, a),
                            d.find(".j-tab-game").one("click",
                                function () {
                                    var a = $(this).data("index");
                                    f(a, 1,
                                        function (a) {
                                            g(b, a)
                                        })
                                })
                    }
                })
            })
    },
    HiShop.popbox.GraphicPicker = function (a, b) {
        var c, d, e = $("#tpl_popbox_GraphicPicker").html(),
            f = $(e),
            g = [],
            h = [],
            i = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_GraphicPicker_graphicsitem").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            j = $(e);
                        j.find(".j-select").click(function () {
                            var a, b = $(this),
                                d = b.parent("li"),
                                e = d.index(),
                                f = d.data("item"),
                                i = $(".j-verify").val();
                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                    for (var j = 0; j < g.length; j++)
                                        if (f == g[j].item_id) {
                                            g.splice(j, 1);
                                            break
                                        }
                                if (0 != h.length)
                                    for (var j = 0; j < h.length; j++) {
                                        var k = h[j];
                                        if (f == k) {
                                            h.splice(j, 1);
                                            break
                                        }
                                    }
                                if (0 != a.length) {
                                    for (var j = 0; j < a.length; j++) {
                                        var k = a[j];
                                        if (f == k) {
                                            a.splice(j, 1);
                                            break
                                        }
                                    }
                                    i = a.join(","),
                                        $(".j-verify").val(i)
                                }
                            } else d.addClass("selected"),
                                b.addClass("btn-success").text("已选"),
                                g.push(c[e]),
                                h.push(f),
                                a.push(f),
                                i = a.join(","),
                                $(".j-verify").val(i)
                        }),
                            f.find(".gagp-goodslist").empty().append(j);
                        var k = a.page,
                            l = $(k);
                        l.filter("a:not(.disabled,.cur)").click(function () {
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate:eq(0)").empty().append(l)
                    } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var m = [];
                    "" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
                        var c = $(this).data("item");
                        m.push(c)
                    }),
                        f.find("li").each(function (a, b) {
                            var c = $(this),
                                d = c.data("item");
                            $.each(m,
                                function (a, b) {
                                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                        }),
                        b && b()
                };
                $.ajax({

                    url: HiShop.Config.AjaxUrl.Graphics,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a)
                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },


            l = function (a) {
                var d = 13;
                f.find(".j-btn-goodsuse").remove(),
                    f.on("click", ".gagp-goodslist .j-select",
                        function () {
                            var e = $(".gagp-goodslist li").index($(this).parent("li"));
                            b && b(c[e], d),
                                $.jBox.close(a)
                        })
            }


        switch (a) {
            case "all":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    i(1,
                        function () {
                            var b = '<span class="fl">选择素材</span><div class="goodsearch"><input type="text" name="title" placeholder="请输入素材名称" /><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                            $.jBox.show({
                                title: b,
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (b) {
                                    if ("goodsMulti" == a) {
                                        var d = [];
                                        $(".img-list li").not(".img-list-add").each(function (a, b) {
                                            var c = $(this).data("item");
                                            d.push(c)
                                        }),
                                            f.find("li").each(function (a, b) {
                                                var c = $(this),
                                                    e = c.data("item");
                                                $.each(d,
                                                    function (a, b) {
                                                        e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                                    })
                                            }),
                                            k(b, d)
                                    } else l(b);
                                    $(document).on("click", ".jGetgood",
                                        function (a) {
                                            var b = $(this).siblings("input").val();

                                            e = function (a, i) {
                                                a = a ? a : 1;
                                                var j = function (a) {
                                                    if (c = a.list, c && c.length) {
                                                        var b = $("#tpl_popbox_GraphicPicker_graphicsitem").html(),
                                                            d = _.template(b, {
                                                                dataset: c
                                                            }),
                                                            j = $(d);
                                                        j.find(".j-select").click(function () {
                                                            var a, b = $(this),
                                                                d = b.parent("li"),
                                                                e = d.index(),
                                                                f = d.data("item"),
                                                                i = $(".j-verify").val();
                                                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                                                    for (var j = 0; j < g.length; j++)
                                                                        if (f == g[j].item_id) {
                                                                            g.splice(j, 1);
                                                                            break
                                                                        }
                                                                if (0 != h.length)
                                                                    for (var j = 0; j < h.length; j++) {
                                                                        var k = h[j];
                                                                        if (f == k) {
                                                                            h.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                if (0 != a.length) {
                                                                    for (var j = 0; j < a.length; j++) {
                                                                        var k = a[j];
                                                                        if (f == k) {
                                                                            a.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                    i = a.join(","),
                                                                        $(".j-verify").val(i)
                                                                }
                                                            } else d.addClass("selected"),
                                                                b.addClass("btn-success").text("已选"),
                                                                g.push(c[e]),
                                                                h.push(f),
                                                                a.push(f),
                                                                i = a.join(","),
                                                                $(".j-verify").val(i)
                                                        }),
                                                            f.find(".gagp-goodslist").empty().append(j);
                                                        var k = a.page,
                                                            l = $(k);
                                                        l.filter("a:not(.disabled,.cur)").click(function () {
                                                            //var a = $(this).attr("href"),
                                                            //b = a.split("/");
                                                            //return b = b[b.length - 1],
                                                            //b = b.replace(/.html/, ""),
                                                            //e(b),
                                                            //!1
                                                            var a = $(this).attr("page"),
                                                                b = a;
                                                            return b = b,
                                                                b = b,
                                                                e(b), !1
                                                        }),
                                                            f.find(".paginate:eq(0)").empty().append(l)
                                                    } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                                        f.find(".paginate").empty();
                                                    i && i()
                                                };
                                                /*搜索商品*/
                                                $.ajax({
                                                    url: HiShop.Config.AjaxUrl.Graphics,
                                                    type: "post",
                                                    dataType: "json",
                                                    data: {
                                                        p: parseInt(a),
                                                        title: b

                                                    },
                                                    success: function (a) {
                                                        1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                                    }
                                                })
                                            };
                                            e()
                                        })
                                }
                            })
                        });
                break;

        }
    },
    HiShop.popbox.CategoriesPicker = function (a, b) {
        var strUrl = window.location.toLocaleString().toLocaleLowerCase();
        var clientName = "Appshop";
   

        var c, d, e = $("#tpl_popbox_CategoriesPicker").html(),
            f = $(e),
            g = [],
            h = [],
            i = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_CategoriesPicker_graphicsitem").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            j = $(e);
                        j.find(".j-select").click(function () {
                            var a, b = $(this),
                                d = b.parent("li"),
                                e = d.index(),
                                f = d.data("item"),
                                i = $(".j-verify").val();
                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                    for (var j = 0; j < g.length; j++)
                                        if (f == g[j].item_id) {
                                            g.splice(j, 1);
                                            break
                                        }
                                if (0 != h.length)
                                    for (var j = 0; j < h.length; j++) {
                                        var k = h[j];
                                        if (f == k) {
                                            h.splice(j, 1);
                                            break
                                        }
                                    }
                                if (0 != a.length) {
                                    for (var j = 0; j < a.length; j++) {
                                        var k = a[j];
                                        if (f == k) {
                                            a.splice(j, 1);
                                            break
                                        }
                                    }
                                    i = a.join(","),
                                        $(".j-verify").val(i)
                                }
                            } else d.addClass("selected"),
                                b.addClass("btn-success").text("已选"),
                                g.push(c[e]),
                                h.push(f),
                                a.push(f),
                                i = a.join(","),
                                $(".j-verify").val(i)
                        }),
                            f.find(".gagp-goodslist").empty().append(j);
                        var k = a.page,
                            l = $(k);
                        l.filter("a:not(.disabled,.cur)").click(function () {
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate:eq(0)").empty().append(l)
                    } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var m = [];
                    "" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
                        var c = $(this).data("item");
                        m.push(c)
                    }),
                        f.find("li").each(function (a, b) {
                            var c = $(this),
                                d = c.data("item");
                            $.each(m,
                                function (a, b) {
                                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                        }),
                        b && b()
                };
                $.ajax({

                    url: HiShop.Config.AjaxUrl.Categories,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a),
                        client: clientName
                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            l = function (a) {
                var d = 3;
                f.find(".j-btn-goodsuse").remove(),
                    f.on("click", ".gagp-goodslist .j-select",
                        function () {
                            var e = $(".gagp-goodslist li").index($(this).parent("li"));
                            b && b(c[e], d),
                                $.jBox.close(a)
                        })
            }
        switch (a) {
            case "all":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    i(1,
                        function () {
                            var b = '<span class="fl">选择分类</span><div class="goodsearch"><input type="text" name="title" placeholder="请输入分类名称" /><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                            $.jBox.show({
                                title: b,
                                width: '700',
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (b) {
                                    if ("goodsMulti" == a) {
                                        var d = [];
                                        $(".img-list li").not(".img-list-add").each(function (a, b) {
                                            var c = $(this).data("item");
                                            d.push(c)
                                        }),
                                            f.find("li").each(function (a, b) {
                                                var c = $(this),
                                                    e = c.data("item");
                                                $.each(d,
                                                    function (a, b) {
                                                        e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                                    })
                                            }),
                                            k(b, d)
                                    } else l(b);
                                    $(document).on("click", ".jGetgood",
                                        function (a) {
                                            var b = $(this).siblings("input").val();

                                            e = function (a, i) {
                                                a = a ? a : 1;
                                                var j = function (a) {
                                                    if (c = a.list, c && c.length) {
                                                        var b = $("#tpl_popbox_CategoriesPicker_graphicsitem").html(),
                                                            d = _.template(b, {
                                                                dataset: c
                                                            }),
                                                            j = $(d);
                                                        j.find(".j-select").click(function () {
                                                            var a, b = $(this),
                                                                d = b.parent("li"),
                                                                e = d.index(),
                                                                f = d.data("item"),
                                                                i = $(".j-verify").val();
                                                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                                                    for (var j = 0; j < g.length; j++)
                                                                        if (f == g[j].item_id) {
                                                                            g.splice(j, 1);
                                                                            break
                                                                        }
                                                                if (0 != h.length)
                                                                    for (var j = 0; j < h.length; j++) {
                                                                        var k = h[j];
                                                                        if (f == k) {
                                                                            h.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                if (0 != a.length) {
                                                                    for (var j = 0; j < a.length; j++) {
                                                                        var k = a[j];
                                                                        if (f == k) {
                                                                            a.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                    i = a.join(","),
                                                                        $(".j-verify").val(i)
                                                                }
                                                            } else d.addClass("selected"),
                                                                b.addClass("btn-success").text("已选"),
                                                                g.push(c[e]),
                                                                h.push(f),
                                                                a.push(f),
                                                                i = a.join(","),
                                                                $(".j-verify").val(i)
                                                        }),
                                                            f.find(".gagp-goodslist").empty().append(j);
                                                        var k = a.page,
                                                            l = $(k);
                                                        l.filter("a:not(.disabled,.cur)").click(function () {
                                                            //var a = $(this).attr("href"),
                                                            //b = a.split("/");
                                                            //return b = b[b.length - 1],
                                                            //b = b.replace(/.html/, ""),
                                                            //e(b),
                                                            //!1
                                                            var a = $(this).attr("page"),
                                                                b = a;
                                                            return b = b,
                                                                b = b,
                                                                e(b), !1
                                                        }),
                                                            f.find(".paginate:eq(0)").empty().append(l)
                                                    } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                                        f.find(".paginate").empty();
                                                    i && i()
                                                };
                                                /*搜索商品*/
                                                $.ajax({
                                                    url: HiShop.Config.AjaxUrl.Categories,
                                                    type: "post",
                                                    dataType: "json",
                                                    data: {
                                                        p: parseInt(a),
                                                        title: b,
                                                        client: clientName
                                                    },
                                                    success: function (a) {
                                                        1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                                    }
                                                })
                                            };
                                            e()
                                        })
                                }
                            })
                        });
                break;

        }
    },
    HiShop.popbox.TopicsPicker = function (a, b) {
    var strUrl = window.location.toLocaleString().toLocaleLowerCase();
    var clientName = "Appshop";
  

    var c, d, e = $("#tpl_popbox_TopicsPicker").html(),
    f = $(e),
    g = [],
    h = [],
    i = function (a, b) {
        var d = function (a) {
            if (c = a.list, c && c.length) {
                var d = $("#tpl_popbox_TopicsPicker_graphicsitem").html(),
                e = _.template(d)({
                    dataset: c
                }),
                j = $(e);
                j.find(".j-select").click(function () {
                    var a, b = $(this),
                    d = b.parent("li"),
                    e = d.index(),
                    f = d.data("item"),
                    i = $(".j-verify").val();
                    if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                        if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length) for (var j = 0; j < g.length; j++) if (f == g[j].item_id) {
                            g.splice(j, 1);
                            break
                        }
                        if (0 != h.length) for (var j = 0; j < h.length; j++) {
                            var k = h[j];
                            if (f == k) {
                                h.splice(j, 1);
                                break
                            }
                        }
                        if (0 != a.length) {
                            for (var j = 0; j < a.length; j++) {
                                var k = a[j];
                                if (f == k) {
                                    a.splice(j, 1);
                                    break
                                }
                            }
                            i = a.join(","),
                            $(".j-verify").val(i)
                        }
                    } else d.addClass("selected"),
                    b.addClass("btn-success").text("已选"),
                    g.push(c[e]),
                    h.push(f),
                    a.push(f),
                    i = a.join(","),
                    $(".j-verify").val(i)
                }),
                f.find(".gagp-goodslist").empty().append(j);
                var k = a.page,
                l = $(k);
                l.filter("a:not(.disabled,.cur)").click(function () {
                    var a = $(this).attr("page"),
                    b = a;
                    return b = b,
                    b = b,
                    i(b),
                    !1
                }),
                f.find(".paginate:eq(0)").empty().append(l)
            } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
            var m = [];
            //"" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
            //    var c = $(this).data("item");
            //    m.push(c)
            //}),
            f.find("li").each(function (a, b) {
                var c = $(this),
                d = c.data("item");
                $.each(m,
                function (a, b) {
                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                })
            }),
            b && b()
        };
        $.ajax({

            url: HiShop.Config.AjaxUrl.topicsUrl,
            type: "post",
            dataType: "json",
            data: {
                p: parseInt(a),
                client: clientName
            },
            success: function (a) {
                1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
            }
        })
    },
    l = function (a) {
        var d = 14;
        f.find(".j-btn-goodsuse").remove(),
        f.on("click", ".gagp-goodslist .j-select",
        function () {
            var e = $(".gagp-goodslist li").index($(this).parent("li"));
            b && b(c[e], d),
            $.jBox.close(a)
        })
    }
    switch (a) {
        case "all":
            f.find(".tabs").remove(),
            f.find(".gagp-goodslist").unwrap().unwrap(),
            f.find(".tc[data-index='2']").remove(),
            i(1,
            function () {
                var b = '<span class="fl">选择专题</span><div class="goodsearch"><input type="text" name="title" placeholder="请输入专题名称" /><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                $.jBox.show({
                    title: b,
                    content: f,
                    width: 700,
                    height: 500,
                    btnOK: {
                        show: !1
                    },
                    btnCancel: {
                        show: !1
                    },
                    onOpen: function (b) {
                        if ("goodsMulti" == a) {
                            var d = [];
                            $(".img-list li").not(".img-list-add").each(function (a, b) {
                                var c = $(this).data("item");
                                d.push(c)
                            }),
                            f.find("li").each(function (a, b) {
                                var c = $(this),
                                e = c.data("item");
                                $.each(d,
                                function (a, b) {
                                    e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                            }),
                            k(b, d)
                        } else l(b);
                        $(document).on("click", ".jGetgood",
                        function (a) {
                            var b = $(this).siblings("input").val();

                            e = function (a, i) {
                                a = a ? a : 1;
                                var j = function (a) {
                                    if (c = a.list, c && c.length) {
                                        var b = $("#tpl_popbox_TopicsPicker_graphicsitem").html(),
                                        d = _.template(b)({
                                            dataset: c
                                        }),
                                        j = $(d);
                                        j.find(".j-select").click(function () {
                                            var a, b = $(this),
                                            d = b.parent("li"),
                                            e = d.index(),
                                            f = d.data("item"),
                                            i = $(".j-verify").val();
                                            if (i == undefined) return;
                                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length) for (var j = 0; j < g.length; j++) if (f == g[j].item_id) {
                                                    g.splice(j, 1);
                                                    break
                                                }
                                                if (0 != h.length) for (var j = 0; j < h.length; j++) {
                                                    var k = h[j];
                                                    if (f == k) {
                                                        h.splice(j, 1);
                                                        break
                                                    }
                                                }
                                                if (0 != a.length) {
                                                    for (var j = 0; j < a.length; j++) {
                                                        var k = a[j];
                                                        if (f == k) {
                                                            a.splice(j, 1);
                                                            break
                                                        }
                                                    }
                                                    i = a.join(","),
                                                    $(".j-verify").val(i)
                                                }
                                            } else d.addClass("selected"),
                                            b.addClass("btn-success").text("已选"),
                                            g.push(c[e]),
                                            h.push(f),
                                            a.push(f),
                                            i = a.join(","),
                                            $(".j-verify").val(i)
                                        }),
                                        f.find(".gagp-goodslist").empty().append(j);
                                        var k = a.page,
                                        l = $(k);
                                        l.filter("a:not(.disabled,.cur)").click(function () {
                                            //var a = $(this).attr("href"),
                                            //b = a.split("/");
                                            //return b = b[b.length - 1],
                                            //b = b.replace(/.html/, ""),
                                            //e(b),
                                            //!1
                                            var a = $(this).attr("page"),
                                            b = a;
                                            return b = b,
                                            b = b,
                                            e(b),
                                            !1
                                        }),
                                        f.find(".paginate:eq(0)").empty().append(l)
                                    } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                    f.find(".paginate").empty();
                                    i && i()
                                };
                                /*搜索商品*/
                                $.ajax({
                                    url: HiShop.Config.AjaxUrl.topicsUrl,
                                    type: "post",
                                    dataType: "json",
                                    data: {
                                        p: parseInt(a),
                                        title: b,
                                        client: clientName
                                    },
                                    success: function (a) {
                                        1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                    }
                                })
                            };
                            e()
                        })
                    }
                })
            });
            break;
    }
},
    HiShop.popbox.BrandsPicker = function (a, b) {
        var strUrl = window.location.toLocaleString().toLocaleLowerCase();
        var clientName = "Appshop";
      
        var c, d, e = $("#tpl_popbox_BrandsPicker").html(),
            f = $(e),
            g = [],
            h = [],
            i = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_BrandsPicker_graphicsitem").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            j = $(e);
                        j.find(".j-select").click(function () {
                            var a, b = $(this),
                                d = b.parent("li"),
                                e = d.index(),
                                f = d.data("item"),
                                i = $(".j-verify").val();
                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                    for (var j = 0; j < g.length; j++)
                                        if (f == g[j].item_id) {
                                            g.splice(j, 1);
                                            break
                                        }
                                if (0 != h.length)
                                    for (var j = 0; j < h.length; j++) {
                                        var k = h[j];
                                        if (f == k) {
                                            h.splice(j, 1);
                                            break
                                        }
                                    }
                                if (0 != a.length) {
                                    for (var j = 0; j < a.length; j++) {
                                        var k = a[j];
                                        if (f == k) {
                                            a.splice(j, 1);
                                            break
                                        }
                                    }
                                    i = a.join(","),
                                        $(".j-verify").val(i)
                                }
                            } else d.addClass("selected"),
                                b.addClass("btn-success").text("已选"),
                                g.push(c[e]),
                                h.push(f),
                                a.push(f),
                                i = a.join(","),
                                $(".j-verify").val(i)
                        }),
                            f.find(".gagp-goodslist").empty().append(j);
                        var k = a.page,
                            l = $(k);
                        l.filter("a:not(.disabled,.cur)").click(function () {
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate:eq(0)").empty().append(l)
                    } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var m = [];
                    "" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
                        var c = $(this).data("item");
                        m.push(c)
                    }),
                        f.find("li").each(function (a, b) {
                            var c = $(this),
                                d = c.data("item");
                            $.each(m,
                                function (a, b) {
                                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                        }),
                        b && b()
                };
                $.ajax({
                    url: HiShop.Config.AjaxUrl.Brands,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a),
                        client: clientName
                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            l = function (a) {
                var d = 4;
                f.find(".j-btn-goodsuse").remove(),
                    f.on("click", ".gagp-goodslist .j-select",
                        function () {
                            var e = $(".gagp-goodslist li").index($(this).parent("li"));
                            b && b(c[e], d),
                                $.jBox.close(a)
                        })
            }
        switch (a) {
            case "all":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    i(1,
                        function () {
                            var b = '<span class="fl">选择品牌</span><div class="goodsearch"><input type="text" name="title" placeholder="请输入品牌名称" /><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                            $.jBox.show({
                                title: b,
                                width: '710',
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (b) {
                                    if ("goodsMulti" == a) {
                                        var d = [];
                                        $(".img-list li").not(".img-list-add").each(function (a, b) {
                                            var c = $(this).data("item");
                                            d.push(c)
                                        }),
                                            f.find("li").each(function (a, b) {
                                                var c = $(this),
                                                    e = c.data("item");
                                                $.each(d,
                                                    function (a, b) {
                                                        e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                                    })
                                            }),
                                            k(b, d)
                                    } else l(b);
                                    $(document).on("click", ".jGetgood",
                                        function (a) {
                                            var b = $(this).siblings("input").val();

                                            e = function (a, i) {
                                                a = a ? a : 1;
                                                var j = function (a) {
                                                    if (c = a.list, c && c.length) {
                                                        var b = $("#tpl_popbox_BrandsPicker_graphicsitem").html(),
                                                            d = _.template(b, {
                                                                dataset: c
                                                            }),
                                                            j = $(d);
                                                        j.find(".j-select").click(function () {
                                                            var a, b = $(this),
                                                                d = b.parent("li"),
                                                                e = d.index(),
                                                                f = d.data("item"),
                                                                i = $(".j-verify").val();
                                                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                                                    for (var j = 0; j < g.length; j++)
                                                                        if (f == g[j].item_id) {
                                                                            g.splice(j, 1);
                                                                            break
                                                                        }
                                                                if (0 != h.length)
                                                                    for (var j = 0; j < h.length; j++) {
                                                                        var k = h[j];
                                                                        if (f == k) {
                                                                            h.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                if (0 != a.length) {
                                                                    for (var j = 0; j < a.length; j++) {
                                                                        var k = a[j];
                                                                        if (f == k) {
                                                                            a.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                    i = a.join(","),
                                                                        $(".j-verify").val(i)
                                                                }
                                                            } else d.addClass("selected"),
                                                                b.addClass("btn-success").text("已选"),
                                                                g.push(c[e]),
                                                                h.push(f),
                                                                a.push(f),
                                                                i = a.join(","),
                                                                $(".j-verify").val(i)
                                                        }),
                                                            f.find(".gagp-goodslist").empty().append(j);
                                                        var k = a.page,
                                                            l = $(k);
                                                        l.filter("a:not(.disabled,.cur)").click(function () {
                                                            //var a = $(this).attr("href"),
                                                            //b = a.split("/");
                                                            //return b = b[b.length - 1],
                                                            //b = b.replace(/.html/, ""),
                                                            //e(b),
                                                            //!1
                                                            var a = $(this).attr("page"),
                                                                b = a;
                                                            return b = b,
                                                                b = b,
                                                                e(b), !1
                                                        }),
                                                            f.find(".paginate:eq(0)").empty().append(l)
                                                    } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                                        f.find(".paginate").empty();
                                                    i && i()
                                                };
                                                /*搜索商品*/
                                                $.ajax({
                                                    url: HiShop.Config.AjaxUrl.Brands,
                                                    type: "post",
                                                    dataType: "json",
                                                    data: {
                                                        p: parseInt(a),
                                                        title: b,
                                                        client: clientName
                                                    },
                                                    success: function (a) {
                                                        1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                                    }
                                                })
                                            };
                                            e()
                                        })
                                }
                            })
                        });
                break;

        }
    },
    HiShop.popbox.ArticlePicker = function (a, b) {
        var strUrl = window.location.toLocaleString().toLocaleLowerCase();
        var clientName = "Appshop";

        var c, d, e = $("#tpl_popbox_ArticlePicker").html(),
            f = $(e),
            g = [],
            h = [],
            i = function (a, b) {
                var d = function (a) {
                    if (c = a.list, c && c.length) {
                        var d = $("#tpl_popbox_ArticlePicker_graphicsitem").html(),
                            e = _.template(d, {
                                dataset: c
                            }),
                            j = $(e);
                        j.find(".j-select").click(function () {
                            var a, b = $(this),
                                d = b.parent("li"),
                                e = d.index(),
                                f = d.data("item"),
                                i = $(".j-verify").val();
                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                    for (var j = 0; j < g.length; j++)
                                        if (f == g[j].item_id) {
                                            g.splice(j, 1);
                                            break
                                        }
                                if (0 != h.length)
                                    for (var j = 0; j < h.length; j++) {
                                        var k = h[j];
                                        if (f == k) {
                                            h.splice(j, 1);
                                            break
                                        }
                                    }
                                if (0 != a.length) {
                                    for (var j = 0; j < a.length; j++) {
                                        var k = a[j];
                                        if (f == k) {
                                            a.splice(j, 1);
                                            break
                                        }
                                    }
                                    i = a.join(","),
                                        $(".j-verify").val(i)
                                }
                            } else d.addClass("selected"),
                                b.addClass("btn-success").text("已选"),
                                g.push(c[e]),
                                h.push(f),
                                a.push(f),
                                i = a.join(","),
                                $(".j-verify").val(i)
                        }),
                            f.find(".gagp-goodslist").empty().append(j);
                        var k = a.page,
                            l = $(k);
                        l.filter("a:not(.disabled,.cur)").click(function () {
                            var a = $(this).attr("page"),
                                b = a;
                            return b = b,
                                b = b,
                                i(b), !1
                        }),
                            f.find(".paginate:eq(0)").empty().append(l)
                    } else f.find(".gagp-goodslist").append("<p class='txtCenter'>对不起，暂无数据</p>");
                    var m = [];
                    "" != $(".j-verify").val() ? m = $(".j-verify").val().split(",") : $(".img-list li").not(".img-list-add").each(function (a, b) {
                        var c = $(this).data("item");
                        m.push(c)
                    }),
                        f.find("li").each(function (a, b) {
                            var c = $(this),
                                d = c.data("item");
                            $.each(m,
                                function (a, b) {
                                    d == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                })
                        }),
                        b && b()
                };
                $.ajax({

                    url: HiShop.Config.AjaxUrl.ArticleList,
                    type: "post",
                    dataType: "json",
                    data: {
                        p: parseInt(a),
                        client: clientName,                        
                        categoryId:0,
                    },
                    success: function (a) {
                        1 == a.status ? d(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            },
            l = function (a) {
                var d = 21;
                f.find(".j-btn-goodsuse").remove(),
                    f.on("click", ".gagp-goodslist .j-select",
                        function () {
                            var e = $(".gagp-goodslist li").index($(this).parent("li"));
                            b && b(c[e], d),
                                $.jBox.close(a)
                        })
            }
        switch (a) {
            case "all":
                f.find(".tabs").remove(),
                    f.find(".gagp-goodslist").unwrap().unwrap(),
                    f.find(".tc[data-index='2']").remove(),
                    i(1,
                        function () {
                            var b = '<span class="fl">选择文章</span><div class="goodsearch"><input type="text" name="title" placeholder="文章名称" /><a href="javascript:;" class="btn btn-primary jGetgood"><i class="gicon-search white"></i>查询</a></div>';
                            $.jBox.show({
                                title: b,
                                width: '710',
                                content: f,
                                btnOK: {
                                    show: !1
                                },
                                btnCancel: {
                                    show: !1
                                },
                                onOpen: function (b) {
                                    if ("goodsMulti" == a) {
                                        var d = [];
                                        $(".img-list li").not(".img-list-add").each(function (a, b) {
                                            var c = $(this).data("item");
                                            d.push(c)
                                        }),
                                            f.find("li").each(function (a, b) {
                                                var c = $(this),
                                                    e = c.data("item");
                                                $.each(d,
                                                    function (a, b) {
                                                        e == b && (c.addClass("selected"), c.children(".j-select").addClass("btn-success").text("已选"))
                                                    })
                                            }),
                                            k(b, d)
                                    } else l(b);
                                    $(document).on("click", ".jGetgood",
                                        function (a) {
                                            var b = $(this).siblings("input").val();

                                            e = function (a, i) {
                                                a = a ? a : 1;
                                                var j = function (a) {
                                                    if (c = a.list, c && c.length) {
                                                        var b = $("#tpl_popbox_ArticlePicker_graphicsitem").html(),
                                                            d = _.template(b, {
                                                                dataset: c
                                                            }),
                                                            j = $(d);
                                                        j.find(".j-select").click(function () {
                                                            var a, b = $(this),
                                                                d = b.parent("li"),
                                                                e = d.index(),
                                                                f = d.data("item"),
                                                                i = $(".j-verify").val();
                                                            if (a = "" != i ? i.split(",") : [], d.hasClass("selected")) {
                                                                if (d.removeClass("selected"), b.removeClass("btn-success").text("选取"), 0 != g.length)
                                                                    for (var j = 0; j < g.length; j++)
                                                                        if (f == g[j].item_id) {
                                                                            g.splice(j, 1);
                                                                            break
                                                                        }
                                                                if (0 != h.length)
                                                                    for (var j = 0; j < h.length; j++) {
                                                                        var k = h[j];
                                                                        if (f == k) {
                                                                            h.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                if (0 != a.length) {
                                                                    for (var j = 0; j < a.length; j++) {
                                                                        var k = a[j];
                                                                        if (f == k) {
                                                                            a.splice(j, 1);
                                                                            break
                                                                        }
                                                                    }
                                                                    i = a.join(","),
                                                                        $(".j-verify").val(i)
                                                                }
                                                            } else d.addClass("selected"),
                                                                b.addClass("btn-success").text("已选"),
                                                                g.push(c[e]),
                                                                h.push(f),
                                                                a.push(f),
                                                                i = a.join(","),
                                                                $(".j-verify").val(i)
                                                        }),
                                                            f.find(".gagp-goodslist").empty().append(j);
                                                        var k = a.page,
                                                            l = $(k);
                                                        l.filter("a:not(.disabled,.cur)").click(function () {
                                                            //var a = $(this).attr("href"),
                                                            //b = a.split("/");
                                                            //return b = b[b.length - 1],
                                                            //b = b.replace(/.html/, ""),
                                                            //e(b),
                                                            //!1
                                                            var a = $(this).attr("page"),
                                                                b = a;
                                                            return b = b,
                                                                b = b,
                                                                e(b), !1
                                                        }),
                                                            f.find(".paginate:eq(0)").empty().append(l)
                                                    } else f.find(".gagp-goodslist").empty().append("<p class='txtCenter'>对不起，暂无数据</p>"),
                                                        f.find(".paginate").empty();
                                                    i && i()
                                                };
                                                /*搜索商品*/
                                                $.ajax({
                                                    url: HiShop.Config.AjaxUrl.ArticleList,
                                                    type: "post",
                                                    dataType: "json",
                                                    data: {
                                                        p: parseInt(a),
                                                        title: b,
                                                        client: clientName
                                                    },
                                                    success: function (a) {
                                                        1 == a.status ? j(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                                                    }
                                                })
                                            };
                                            e()
                                        })
                                }
                            })
                        });
                break;

        }
    },
    HiShop.popbox.dplPickerColletion = function (a) {
        var strUrl = window.location.toLocaleString().toLocaleLowerCase();
        var clientName = "Appshop";  
        var b = {
            linkType:1,
            callback: null
        },
            c = $.extend(!0, {},
                b, a);
        switch (parseInt(c.linkType)) {
            case 1:
               HiShop.popbox.GoodsAndGroupPicker("goods", c.callback);
                break;
            case 2:
                var d = {
                    title: "分类导航",
                    link: "page:Category"
                };
                c.callback(d, 2)
                break;
            case 3:
                HiShop.popbox.CategoriesPicker("all", c.callback);
                break;
            case 4:
                HiShop.popbox.BrandsPicker("all", c.callback);
                break;
            case 5:
                HiShop.popbox.GamePicker("all", c.callback);
                break;
            case 6:
                var d = {
                    title: "店铺主页",
                    link: "/" + clientName + "/Default.aspx"
                };
                c.callback(d, 6);
                break;
            case 7:
                var d = {
                    title: "会员主页",
                    link: "page:OrderCenter"
                };
                c.callback(d, 7);
                break;
            case 8:
                var d = {
                    title: "购物车",
                    link: "page:ShoppingCart"
                };
                c.callback(d, 8);
                break;
            case 9:
                var d = {
                    title: "全部商品",
                    link: "/" + clientName + "/ProductList.aspx"
                };
                c.callback(d, 9);
                break;
            case 10:
                var d = {
                    title: "",
                    link: ""
                };
                c.callback(d, 10);
                break;
            case 11:
                HiShop.popbox.VotePicker("all", c.callback);
                break;
            case 12:
                var d = {
                    title: "团购活动",
                    link: "/" + clientName + "/GroupBuyList.aspx"
                };
                c.callback(d, 12);
                break;
            case 13:
                var d = {
                    title: "限时抢购",
                    link: "/" + clientName + "/CountDownProducts.aspx"
                };
                c.callback(d, 13);
                break;
            case 14:
                HiShop.popbox.TopicsPicker("all", c.callback);
                break;
            case 15:
                var d = {
                    title: "积分商城",
                    link: "/" + clientName + "/PointMall.aspx"
                };
                c.callback(d, 15);
                break;
            case 16:
                var d = {
                    title: "优惠券列表",
                    link: "/" + clientName + "/couponslist.aspx"
                };
                c.callback(d, 16);
                break;
            case 17:
                var d = {
                    title: "注册送券",
                    link: "/" + clientName + "/RegisteredCoupons.aspx"
                };
                c.callback(d, 17);
                break;
            case 18:
                HiShop.popbox.CouponListPicker("all", c.callback);
                break;
            case 19:
                var d = {
                    title: "火拼团",
                    link: "page:FightGroupList"
                };
                c.callback(d, 19);
                break;
            case 20:
                var d = {
                    title: "周边门店",
                    link: "/" + clientName + "/StoreList"
                };
                c.callback(d, 20);
                break;
            case 21:
                HiShop.popbox.ArticlePicker("all", c.callback);
                break;
            case 22:
                var d = {
                    title: "摇一摇",
                    link: "page:Shake"
                };
                c.callback(d, 22);
                break;
            case 23:
                var d = {
                    title: "",
                    link: ""
                };
                c.callback(d, 23);
                break;
            case 24:
                var d = {
                    title: "品牌列表",
                    link: "/"+clientName+"/BrandList"
                };
                c.callback(d, 24);
                break;
            case 25:
                var d = {
                    title: "",
                    link: ""
                };
                c.callback(d, 25);
                break;
            case 26:
                var d = {
                    title: "文章列表",
                    link: "/" + clientName + "/articles"
                };
                c.callback(d, 6);
                break;
        }
    },
    HiShop.ajaxPopTable = function (a) {
        var b, c, d = {
            title: "",
            url: "",
            width: "auto",
            minHeight: "auto",
            data: {
                p: 1
            },
            tpl: "",
            onOpen: null,
            onPageChange: null
        },
            e = $.extend(!0, {},
                d, a),
            f = $("<div></div>"),
            g = function (a) {
                var d = e.tpl,
                    h = e.url,
                    i = function (h) {
                        b = h;
                        var i = _.template(d, h),
                            j = $(i);
                        f.empty().append(j),
                            f.find(".paginate a:not(.disabled,.cur)").click(function () {
                                for (var a = $(this).attr("href"), b = a.split("/"), c = 0; c < b.length; c++)
                                    if ("p" == b[c]) {
                                        e.data.p = b[c + 1].replace(/.html/, ""),
                                            g();
                                        break
                                    }
                                return !1
                            }),
                            a && a(),
                            e.onPageChange && e.onPageChange(c, b)
                    };
                $.ajax({
                    url: h,
                    type: "post",
                    dataType: "json",
                    data: e.data,
                    success: function (a) {
                        1 == a.status ? i(a) : HiShop.hint("danger", "对不起，获取数据失败：" + a.msg)
                    }
                })
            };
        g(function () {
            $.jBox.show({
                title: e.title,
                width: e.width,
                minHeight: e.minHeight,
                content: f,
                btnOK: {
                    show: !1
                },
                btnCancel: {
                    show: !1
                },
                onOpen: function (a) {
                    c = a,
                        e.onOpen && e.onOpen(a, b)
                }
            })
        })
    },
    HiShop.regRules = {
        email: /^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$/i,
        mobphone: /^(1(([35][0-9])|(47)|[8][01236789]))\d{8}$/,
        telphone: /^0\d{2,3}(\-)?\d{7,8}$/,
        integer: /^\d+$/
    },
    HiShop.popbox.iconimgsrc = {
        data: {
            style1: {
                color0: ["/Admin/shop/Public/images/icon/style1/color0/icon_home.png", "/Admin/shop/Public/images/icon/style1/color0/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color0/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color0/icon_user.png", "/Admin/shop/Public/images/icon/style1/color0/icon_active.png", "/Admin/shop/Public/images/icon/style1/color0/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color0/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color0/style1_gz0.png", "/Admin/shop/Public/images/icon/style1/color0/style1_shopcar0.png"],
                color1: ["/Admin/shop/Public/images/icon/style1/color1/icon_home.png", "/Admin/shop/Public/images/icon/style1/color1/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color1/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color1/icon_user.png", "/Admin/shop/Public/images/icon/style1/color1/icon_active.png", "/Admin/shop/Public/images/icon/style1/color1/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color1/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color1/style1_gz1.png", "/Admin/shop/Public/images/icon/style1/color1/style1_shopcar1.png"],
                color2: ["/Admin/shop/Public/images/icon/style1/color2/icon_home.png", "/Admin/shop/Public/images/icon/style1/color2/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color2/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color2/icon_user.png", "/Admin/shop/Public/images/icon/style1/color2/icon_active.png", "/Admin/shop/Public/images/icon/style1/color2/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color2/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color2/style1_gz2.png", "/Admin/shop/Public/images/icon/style1/color2/style1_shopcar2.png"],
                color3: ["/Admin/shop/Public/images/icon/style1/color3/icon_home.png", "/Admin/shop/Public/images/icon/style1/color3/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color3/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color3/icon_user.png", "/Admin/shop/Public/images/icon/style1/color3/icon_active.png", "/Admin/shop/Public/images/icon/style1/color3/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color3/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color3/style1_gz3.png", "/Admin/shop/Public/images/icon/style1/color3/style1_shopcar3.png"],
                color4: ["/Admin/shop/Public/images/icon/style1/color4/icon_home.png", "/Admin/shop/Public/images/icon/style1/color4/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color4/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color4/icon_user.png", "/Admin/shop/Public/images/icon/style1/color4/icon_active.png", "/Admin/shop/Public/images/icon/style1/color4/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color4/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color4/style1_gz4.png", "/Admin/shop/Public/images/icon/style1/color4/style1_shopcar4.png"],
                color5: ["/Admin/shop/Public/images/icon/style1/color5/icon_home.png", "/Admin/shop/Public/images/icon/style1/color5/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color5/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color5/icon_user.png", "/Admin/shop/Public/images/icon/style1/color5/icon_active.png", "/Admin/shop/Public/images/icon/style1/color5/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color5/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color5/style1_gz5.png", "/Admin/shop/Public/images/icon/style1/color5/style1_shopcar5.png"],
                color6: ["/Admin/shop/Public/images/icon/style1/color6/icon_home.png", "/Admin/shop/Public/images/icon/style1/color6/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color6/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color6/icon_user.png", "/Admin/shop/Public/images/icon/style1/color6/icon_active.png", "/Admin/shop/Public/images/icon/style1/color6/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color6/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color6/style1_gz6.png", "/Admin/shop/Public/images/icon/style1/color6/style1_shopcar6.png"],
                color7: ["/Admin/shop/Public/images/icon/style1/color7/icon_home.png", "/Admin/shop/Public/images/icon/style1/color7/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color7/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color7/icon_user.png", "/Admin/shop/Public/images/icon/style1/color7/icon_active.png", "/Admin/shop/Public/images/icon/style1/color7/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color7/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color7/style1_gz7.png", "/Admin/shop/Public/images/icon/style1/color7/style1_shopcar7.png"],
                color8: ["/Admin/shop/Public/images/icon/style1/color8/icon_home.png", "/Admin/shop/Public/images/icon/style1/color8/icon_allgoods.png", "/Admin/shop/Public/images/icon/style1/color8/icon_newgoods.png", "/Admin/shop/Public/images/icon/style1/color8/icon_user.png", "/Admin/shop/Public/images/icon/style1/color8/icon_active.png", "/Admin/shop/Public/images/icon/style1/color8/icon_hotsale.png", "/Admin/shop/Public/images/icon/style1/color8/icon_subject.png", "/Admin/shop/Public/images/icon/style1/color8/style1_gz8.png", "/Admin/shop/Public/images/icon/style1/color8/style1_shopcar8.png"]
            },
            style2: {
                color0: ["/Admin/shop/Public/images/icon/style2/color0/icon_home.png", "/Admin/shop/Public/images/icon/style2/color0/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color0/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color0/icon_user.png", "/Admin/shop/Public/images/icon/style2/color0/icon_active.png", "/Admin/shop/Public/images/icon/style2/color0/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color0/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color0/style2_gz0.png", "/Admin/shop/Public/images/icon/style2/color0/style2_shopcar0.png"],
                color1: ["/Admin/shop/Public/images/icon/style2/color1/icon_home.png", "/Admin/shop/Public/images/icon/style2/color1/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color1/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color1/icon_user.png", "/Admin/shop/Public/images/icon/style2/color1/icon_active.png", "/Admin/shop/Public/images/icon/style2/color1/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color1/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color1/style2_gz1.png", "/Admin/shop/Public/images/icon/style2/color1/style2_shopcar1.png"],
                color2: ["/Admin/shop/Public/images/icon/style2/color2/icon_home.png", "/Admin/shop/Public/images/icon/style2/color2/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color2/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color2/icon_user.png", "/Admin/shop/Public/images/icon/style2/color2/icon_active.png", "/Admin/shop/Public/images/icon/style2/color2/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color2/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color2/style2_gz2.png", "/Admin/shop/Public/images/icon/style2/color2/style2_shopcar2.png"],
                color3: ["/Admin/shop/Public/images/icon/style2/color3/icon_home.png", "/Admin/shop/Public/images/icon/style2/color3/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color3/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color3/icon_user.png", "/Admin/shop/Public/images/icon/style2/color3/icon_active.png", "/Admin/shop/Public/images/icon/style2/color3/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color3/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color3/style2_gz3.png", "/Admin/shop/Public/images/icon/style2/color3/style2_shopcar3.png"],
                color4: ["/Admin/shop/Public/images/icon/style2/color4/icon_home.png", "/Admin/shop/Public/images/icon/style2/color4/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color4/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color4/icon_user.png", "/Admin/shop/Public/images/icon/style2/color4/icon_active.png", "/Admin/shop/Public/images/icon/style2/color4/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color4/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color4/style2_gz4.png", "/Admin/shop/Public/images/icon/style2/color4/style2_shopcar4.png"],
                color5: ["/Admin/shop/Public/images/icon/style2/color5/icon_home.png", "/Admin/shop/Public/images/icon/style2/color5/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color5/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color5/icon_user.png", "/Admin/shop/Public/images/icon/style2/color5/icon_active.png", "/Admin/shop/Public/images/icon/style2/color5/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color5/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color5/style2_gz5.png", "/Admin/shop/Public/images/icon/style2/color5/style2_shopcar5.png"],
                color6: ["/Admin/shop/Public/images/icon/style2/color6/icon_home.png", "/Admin/shop/Public/images/icon/style2/color6/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color6/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color6/icon_user.png", "/Admin/shop/Public/images/icon/style2/color6/icon_active.png", "/Admin/shop/Public/images/icon/style2/color6/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color6/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color6/style2_gz6.png", "/Admin/shop/Public/images/icon/style2/color6/style2_shopcar6.png"],
                color7: ["/Admin/shop/Public/images/icon/style2/color7/icon_home.png", "/Admin/shop/Public/images/icon/style2/color7/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color7/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color7/icon_user.png", "/Admin/shop/Public/images/icon/style2/color7/icon_active.png", "/Admin/shop/Public/images/icon/style2/color7/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color7/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color7/style2_gz7.png", "/Admin/shop/Public/images/icon/style2/color7/style2_shopcar7.png"],
                color8: ["/Admin/shop/Public/images/icon/style2/color8/icon_home.png", "/Admin/shop/Public/images/icon/style2/color8/icon_allgoods.png", "/Admin/shop/Public/images/icon/style2/color8/icon_newgoods.png", "/Admin/shop/Public/images/icon/style2/color8/icon_user.png", "/Admin/shop/Public/images/icon/style2/color8/icon_active.png", "/Admin/shop/Public/images/icon/style2/color8/icon_hotsale.png", "/Admin/shop/Public/images/icon/style2/color8/icon_subject.png", "/Admin/shop/Public/images/icon/style2/color8/style2_gz8.png", "/Admin/shop/Public/images/icon/style2/color8/style2_shopcar8.png"]
            },
            style3: {
                color0: ["/Admin/shop/Public/images/icon/style3/color0/icon_home.png", "/Admin/shop/Public/images/icon/style3/color0/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color0/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color0/icon_user.png", "/Admin/shop/Public/images/icon/style3/color0/icon_active.png", "/Admin/shop/Public/images/icon/style3/color0/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color0/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color0/style3_gz0.png", "/Admin/shop/Public/images/icon/style3/color0/style3_shopcar0.png"],
                color1: ["/Admin/shop/Public/images/icon/style3/color1/icon_home.png", "/Admin/shop/Public/images/icon/style3/color1/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color1/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color1/icon_user.png", "/Admin/shop/Public/images/icon/style3/color1/icon_active.png", "/Admin/shop/Public/images/icon/style3/color1/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color1/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color1/style3_gz1.png", "/Admin/shop/Public/images/icon/style3/color1/style3_shopcar1.png"],
                color2: ["/Admin/shop/Public/images/icon/style3/color2/icon_home.png", "/Admin/shop/Public/images/icon/style3/color2/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color2/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color2/icon_user.png", "/Admin/shop/Public/images/icon/style3/color2/icon_active.png", "/Admin/shop/Public/images/icon/style3/color2/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color2/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color2/style3_gz2.png", "/Admin/shop/Public/images/icon/style3/color2/style3_shopcar2.png"],
                color3: ["/Admin/shop/Public/images/icon/style3/color3/icon_home.png", "/Admin/shop/Public/images/icon/style3/color3/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color3/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color3/icon_user.png", "/Admin/shop/Public/images/icon/style3/color3/icon_active.png", "/Admin/shop/Public/images/icon/style3/color3/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color3/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color3/style3_gz3.png", "/Admin/shop/Public/images/icon/style3/color3/style3_shopcar3.png"],
                color4: ["/Admin/shop/Public/images/icon/style3/color4/icon_home.png", "/Admin/shop/Public/images/icon/style3/color4/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color4/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color4/icon_user.png", "/Admin/shop/Public/images/icon/style3/color4/icon_active.png", "/Admin/shop/Public/images/icon/style3/color4/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color4/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color4/style3_gz4.png", "/Admin/shop/Public/images/icon/style3/color4/style3_shopcar4.png"],
                color5: ["/Admin/shop/Public/images/icon/style3/color5/icon_home.png", "/Admin/shop/Public/images/icon/style3/color5/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color5/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color5/icon_user.png", "/Admin/shop/Public/images/icon/style3/color5/icon_active.png", "/Admin/shop/Public/images/icon/style3/color5/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color5/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color5/style3_gz5.png", "/Admin/shop/Public/images/icon/style3/color5/style3_shopcar5.png"],
                color6: ["/Admin/shop/Public/images/icon/style3/color6/icon_home.png", "/Admin/shop/Public/images/icon/style3/color6/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color6/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color6/icon_user.png", "/Admin/shop/Public/images/icon/style3/color6/icon_active.png", "/Admin/shop/Public/images/icon/style3/color6/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color6/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color6/style3_gz6.png", "/Admin/shop/Public/images/icon/style3/color6/style3_shopcar6.png"],
                color7: ["/Admin/shop/Public/images/icon/style3/color7/icon_home.png", "/Admin/shop/Public/images/icon/style3/color7/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color7/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color7/icon_user.png", "/Admin/shop/Public/images/icon/style3/color7/icon_active.png", "/Admin/shop/Public/images/icon/style3/color7/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color7/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color7/style3_gz7.png", "/Admin/shop/Public/images/icon/style3/color7/style3_shopcar7.png"],
                color8: ["/Admin/shop/Public/images/icon/style3/color8/icon_home.png", "/Admin/shop/Public/images/icon/style3/color8/icon_allgoods.png", "/Admin/shop/Public/images/icon/style3/color8/icon_newgoods.png", "/Admin/shop/Public/images/icon/style3/color8/icon_user.png", "/Admin/shop/Public/images/icon/style3/color8/icon_active.png", "/Admin/shop/Public/images/icon/style3/color8/icon_hotsale.png", "/Admin/shop/Public/images/icon/style3/color8/icon_subject.png", "/Admin/shop/Public/images/icon/style3/color8/style3_gz8.png", "/Admin/shop/Public/images/icon/style3/color8/style3_shopcar8.png"]
            }
        }
    },
    $(function () {
        $(".header-ctrl-item").hover(function () {
            var a = $(this);
            a.data("type"),
                a.data("cache");
            a.find(".header-ctrl-item-children").length && a.addClass("show")
        },
                function () {
                    $(this).removeClass("show")
                }),
            $(".tips").tooltips();
        try {
            var a = $(".container .inner"),
                b = function () {
                    HiShop.Constant.windowHeight = $(this).height(),
                        HiShop.Constant.windowWidth = $(this).width(),
                        HiShop.Constant.containerOffset = a.offset(),
                        HiShop.Constant.containerWidth = a.outerWidth()
                },
                c = function () {
                    //  $("#j-gotop").css("left", HiShop.Constant.containerWidth + HiShop.Constant.containerOffset.left + 250)
                };
            $(window).resize(function () {
                b(),
                    c()
            }),
                b(),
                c()
        } catch (d) { }
        $(window).scroll(function () {
            $(this).scrollTop() >= 150 ? $("#j-gotop").fadeIn(300) : $("#j-gotop").fadeOut(300)
        })
    }),
    $(function () {
        var a = $(".wxtables").find("input[type='checkbox'].table-ckbs");
        $(".btn_table_selectAll").click(function () {
            a.attr("checked", !0)
        }),
            $(".btn_table_Cancle").click(function () {
                a.attr("checked", !1)
            }),
            $(".paginate").each(function () {
                var a = $(this),
                    b = a.find("input"),
                    c = a.find(".goto"),
                    d = window.location.href.toString();
                b.focus(function () {
                    $(this).addClass("focus").siblings(".goto").addClass("focus")
                }),
                    b.blur(function () {
                        "" == $(this).val() && $(this).removeClass("focus").siblings(".goto").removeClass("focus")
                    }),
                    b.keypress(function (a) {
                        var b = window.event ? a.keyCode : a.which;
                        return 13 == b && (window.location.href = $(this).siblings("a.goto").attr("href")),
                            8 == b || 46 == b || 37 == b || 39 == b ? !0 : 48 > b || b > 57 ? !1 : !0
                    }),
                    b.keyup(function (a) {
                        var b = $(this).val(),
                            e = d.split("/"),
                            f = e.length,
                            g = !1,
                            h = !1;
                        "" == e[f - 1] && (e.pop(), f = e.length, g = !0), (g || "p" != e[f - 2]) && (e.push("p"), f = e.length, h = !0),
                            g || h ? e[f] = b + ".html" : e[f - 1] = b + ".html",
                            c.attr("href", e.join("/"))
                    })
            })
    }),
    $(function () {
        $(document).on("click", ".tabs .tabs_a",
            function () {
                var a = $(this),
                    b = a.data("origin"),
                    c = 0;
                a.parent().hasClass("wizardstep") || a.parent().hasClass("nochange") || (a.addClass("active").siblings(".tabs_a").removeClass("active"), a.data("index") ? (c = a.data("index"), $(".tabs-content[data-origin='" + b + "']").find(".tc[data-index='" + c + "']").removeClass("hide").siblings(".tc").addClass("hide")) : (c = a.index(), $(".tabs-content[data-origin='" + b + "']").find(".tc:eq(" + c + ")").removeClass("hide").siblings(".tc").addClass("hide")))
            })
    }),
    $(function () {
        $(".alert.disable-del").each(function () {
            var a = $('<a href="javascript:;" class="alert-delete" title="隐藏"></a>');
            a.click(function () {
                $(this).parent(".alert").fadeOut()
            }),
                $(this).append(a)
        })
    }),
    function (a, b, c) {
        var d = {
            trigger: "hover"
        };
        a.fn.tooltips = function () {
            return this.each(function () {
                var b = function () {
                    var b = a(this),
                        c = b.data("content"),
                        d = b.offset(),
                        e = {
                            width: b.outerWidth(!0),
                            height: b.outerHeight(!0)
                        },
                        f = b.data("placement");
                    if (this.tip = null, c = void 0 == c || "" == c ? c = b.text() : c, null == this.$tip) {
                        var g = a("#tpl_tooltips").html();
                        if (void 0 == g || "" == g) return void console.log("Please check template!");
                        var h = _.template(g, {
                            content: c,
                            placement: f
                        });
                        this.$tip = a(h),
                            a("body").append(this.$tip);
                        var i = 0,
                            j = 0,
                            k = this.$tip.outerWidth(!0),
                            l = this.$tip.outerHeight(!0);
                        switch (f) {
                            case "top":
                                i = d.top + e.height + 5,
                                    j = d.left - 5;
                                break;
                            case "bottom":
                                i = d.top - l - 5,
                                    j = d.left - 5;
                                break;
                            case "left":
                                i = d.top - e.height / 2,
                                    j = d.left + e.width + 5;
                                break;
                            case "right":
                                i = d.top + e.height / 2 - l / 2,
                                    j = d.left - k - 5
                        }
                        this.$tip.css({
                            top: i,
                            left: j
                        })
                    }
                    this.$tip.stop(!0, !0).fadeIn(300)
                },
                    c = function () {
                        this.$tip && this.$tip.stop(!0, !0).fadeOut(300)
                    },
                    e = a(this).data("trigger");
                switch (e = void 0 != e && "" != e ? e : d.trigger) {
                    case "hover":
                        a(this).hover(b, c);
                        break;
                    case "click":
                        a(this).click(b).mouseleave(c)
                }
            })
        }
    }(jQuery, document, window),
    $(function () {
        $(document).on("mouseover", ".droplist .j-droplist-toggle",
                function () {
                    $(this).siblings(".droplist-menu").show()
                }),
            $(document).on("mouseleave", ".droplist .droplist-menu",
                function () {
                    $(this).hide()
                }),
            $(document).on("mouseleave", ".droplist",
                function () {
                    $(this).find(".droplist-menu").hide()
                }),
            $(document).on("click", ".droplist .droplist-menu a",
                function () {
                    $(this).parents(".droplist-menu").hide()
                })
    }),
    function (a, b, c) {
        var d = {
            callback: null
        },
            e = {};
        Win = {
            width: a(c).width(),
            height: a(c).height()
        },
            Tpl = {
                main: a("#tpl_albums_main").html(),
                overlay: a("#tpl_albums_overlay").html(),
                tree: a("#tpl_albums_tree").html(),
                treeFn: a("#tpl_albums_tree_fn").html(),
                imgs: a("#tpl_albums_imgs").html()
            },
            Cache = {
                folderID: "",
                moveFolderID: 0,
                imgs: {}
            },
            GetImgList = function (b, c, d, e,sort) {
                var f = arguments.callee,
                    g = b.find("#j-panelImgs"),
                    h = b.find("#j-panelPaginate"),
                    i = c >= 0 ? {
                        id: c,
                        p: d,
                        file_name: e,
                        sortby: sort
                    } : {
                        p: d,
                        file_name: e,
                        sortby: sort
                    };
                a.ajax({
                    url: HiShop.Config.AjaxUrl.getImgList,
                    type: "post",
                    dataType: "json",
                    data: i,
                    beforeSend: function () {
                        g.find(".j-loading").show()
                    },
                    success: function (d) {
                        if (1 == d.status) {
                            Cache.imgs = _.isArray(d.data) ? d.data : null;
                            var i = a(_.template(Tpl.imgs, {
                                dataset: Cache.imgs
                            })),
                                j = a(d.page);
                            g.find(".j-loading").hide().end().find("ul,.j-noPic").remove().end().append(i),
                                h.empty().append(j),
                                j.filter("a:not(.disabled,.cur)").click(function () {
                                    var d = a(this).attr("page"),
                                        g = d;
                                    var sort = $("#slsImageOrder").val();
                                    return g = g,
                                        g = g,
                                        f(b, c, g, e, sort), !1
                                })
                        } else HiShop.hint("danger", "对不起，图片获取失败：" + d.msg)
                    }
                })
            },
            UpdateTreeNums = function (b) {
                //alert(HiShop.Config.AjaxUrl.getFolderTree);
                a.ajax({
                    url: HiShop.Config.AjaxUrl.getFolderTree,
                    type: "post",
                    dataType: "json",
                    success: function (a) {
                        if (1 == a.status) {
                            var c = a.data.tree,
                                d = b.find("#j-panelTree");
                            c.push({
                                id: "-1",
                                picNum: a.data.total
                            });
                            var e = function (a) {
                                var b = arguments.callee;
                                _.each(a,
                                    function (a) {
                                        d.find("dt[data-id=" + a.id + "]").find(".j-num").text(a.picNum),
                                            a.subFolder && a.subFolder.length && b(a.subFolder)
                                    })
                            };
                            e(c)
                        } else console.log("更新文件夹树图片数量失败")
                    }
                })
            },
            a.albums = function (c) {
                e = a.extend(!0, {},
                    d, c);
                var f = a("#albums"),
                    g = a("#albums-overlay");
                if (!f.length) {
                    f = a(Tpl.main),
                        g = a(Tpl.overlay),
                        a("body").append(f.hide(), g.hide());
                    var h = f.find("#j-close"),
                        i = f.find("#j-addFolder"),
                        j = f.find("#j-renameFolder"),
                        k = f.find("#j-delFolder"),
                        l = f.find("#j-addImg"),
                        m = f.find("#j-moveImg"),
                        n = f.find("#j-cateImg"),
                        o = f.find("#j-delImg"),
                        p = f.find("#j-panelTree"),
                        q = function () {
                            f.fadeOut("fast"),
                                g.fadeOut("fast"),
                                f.find("#j-panelImgs li").removeClass("selected")
                        };
                    a.ajax({
                        url: HiShop.Config.AjaxUrl.getFolderTree,
                        type: "post",
                        dataType: "json",
                        success: function (b) {
                            if (1 == b.status) {
                                var c = _.template(Tpl.treeFn),
                                    d = c({
                                        dataset: b.data.tree,
                                        templateFn: c
                                    }),
                                    e = a(_.template(Tpl.tree, {
                                        dataset: b.data,
                                        nodes: d
                                    }));
                                p.empty().append(e),
                                    f.find(".j-albumsNodes > dt:first").click()
                            } else HiShop.hint("danger", "对不起，文件夹获取失败：" + b.msg)
                        }
                    }),
                        a(b).on("click", ".j-albumsNodes dt",
                            function (b) {
                                var c = a(this),
                                    d = c.data("id");
                                if (c.parents(".j-albumsNodes").find("dt").removeClass("selected"), c.addClass("selected"), a(b.currentTarget).parents(".j-propagation").length) Cache.moveFolderID = d;
                                else {
                                    if (Cache.folderID == d) return;
                                    Cache.folderID = d;
                                    var e = c.data("add"),
                                        g = c.data("rename"),
                                        h = c.data("del");
                                    1 == e ? i.show() : i.hide(),
                                        1 == g ? j.show() : j.hide(),
                                        1 == h ? k.show() : k.hide(),
                                        GetImgList(f, Cache.folderID, 1)
                                }
                                return !1
                            }),
                        a(b).on("click", ".j-albumsNodes dt i",
                            function () {
                                var b = a(this),
                                    c = b.parent("dt").siblings("dd").find(" > dl"),
                                    d = c.length;
                                if (d) return b.hasClass("open") ? (b.removeClass("open"), c.slideUp(200)) : (b.addClass("open"), c.slideDown(200)), !1
                            }),
                        f.on("click", "#j-panelImgs li",
                            function () {
                                return a(this).toggleClass("selected"),
                                    a(this).find(".img-name-edit").hide(), !1
                            }),
                        f.on("click", "#j-panelImgs li .albums-edit",
                            function () {
                                return a(this).children(".img-name-edit").show(), !1
                            }),
                        f.on("click", "#j-useImg",
                            function () {
                                if (!f.find("#j-panelImgs li.selected").length) return void HiShop.hint("warning", "请选择图片！");
                                var b = [];
                                return f.find("#j-panelImgs li.selected").each(function () {
                                    b.push(Cache.imgs[a(this).index()].file)
                                }),
                                    e.callback && (e.callback(b), q()), !1
                            }),
                        i.click(function () {
                            var b = [{
                                id: 0,
                                name: "未命名文件夹",
                                picNum: 0
                            }];
                            a.ajax({
                                url: HiShop.Config.AjaxUrl.addFolder,
                                type: "post",
                                dataType: "json",
                                data: {
                                    name: b.name,
                                    parent_id: Cache.folderID
                                },
                                success: function (c) {
                                    if (1 == c.status) {
                                        b[0].id = c.data;
                                        var d = _.template(Tpl.treeFn, {
                                            dataset: b
                                        });
                                        $render = a(d),
                                            p.find("dt[data-id='" + Cache.folderID + "']").siblings("dd").append($render),
                                            $render.find("dt:first").click(),
                                            j.click()
                                    } else HiShop.hint("danger", "对不起，添加失败：" + c.msg)
                                }
                            })
                        }),
                        j.click(function () {
                            var b = p.find("dt[data-id='" + Cache.folderID + "']"),
                                c = b.find(".j-treeShowTxt"),
                                d = b.find(".j-ip"),
                                e = b.find(".j-loading");
                            c.hide(),
                                d.show().focus().select(),
                                d.blur(function () {
                                    var b = a(this).val();
                                    a.ajax({
                                        url: HiShop.Config.AjaxUrl.renameFolder,
                                        type: "post",
                                        dataType: "json",
                                        data: {
                                            name: b,
                                            category_img_id: Cache.folderID
                                        },
                                        beforeSend: function () {
                                            e.css("display", "inline-block")
                                        },
                                        success: function (a) {
                                            1 == a.status ? c.find(".j-name").text(b) : HiShop.hint("danger", "对不起，重命名失败：" + a.msg),
                                                c.show(),
                                                d.hide(),
                                                e.hide()
                                        }
                                    })
                                })
                        }),
                        k.click(function () {
                            var b = a("#tpl_albums_delFolder").html(),
                                c = a(b),
                                d = c.find("input[name=isDelImgs]");
                            a.jBox.show({
                                title: "提示",
                                content: c,
                                btnOK: {
                                    onBtnClick: function (b) {
                                        var c = d.filter(":checked").val();
                                        a.ajax({
                                            url: HiShop.Config.AjaxUrl.delFolder,
                                            type: "post",
                                            dataType: "json",
                                            data: {
                                                type: c,
                                                id: Cache.folderID
                                            },
                                            success: function (a) {
                                                if (1 == a.status) {
                                                    UpdateTreeNums(f);
                                                    var b = p.find("dt[data-id=" + Cache.folderID + "]").parent("dl");
                                                    b.parent("dd").siblings("dt").click(),
                                                        b.remove()
                                                } else HiShop.hint("danger", "对不起，删除失败失败：" + a.msg)
                                            }
                                        }),
                                            a.jBox.close(b)
                                    }
                                }
                            })
                        }),
                        o.click(function () {
                            if (!f.find("#j-panelImgs li.selected").length) return void HiShop.hint("warning", "请选择要删除的图片！");
                            var b = [];
                            f.find("#j-panelImgs li.selected").each(function () {
                                b.push(a(this).data("id"))
                            }),
                                a.ajax({
                                    url: HiShop.Config.AjaxUrl.delImg,
                                    type: "post",
                                    dataType: "json",
                                    data: {
                                        file_id: b
                                    },
                                    success: function (b) {
                                        1 == b.status ? (f.find("#j-panelImgs li.selected").fadeOut(300,
                                            function () {
                                                a(this).remove()
                                            }), UpdateTreeNums(f)) : HiShop.hint("danger", "对不起，删除失败：" + b.msg)
                                    }
                                })
                        }),
                        l.uploadify({
                            debug: !1,
                            auto: !0,
                            width: 86,
                            height: 28,
                            multi: !0,
                            swf: "/Utility/Ueditor/plugins/uploadify/uploadify.swf",
                            uploader: HiShop.Config.AjaxUrl.addImg,
                            buttonText: "上传图片",
                            fileSizeLimit: "2MB",
                            fileTypeExts: "*.jpg; *.jpeg; *.png; *.gif; *.bmp",
                            onUploadStart: function () {
                                l.uploadify("settings", "formData", {
                                    cate_id: -1 == Cache.folderID ? 0 : Cache.folderID,
                                    PHPSESSID: a.cookie("PHPSESSID")
                                })
                            },
                            onSelectError: function (a, b, c) {
                                switch (b) {
                                    case -100:
                                        HiShop.hint("danger", "对不起，系统只允许您一次最多上传10个文件");
                                        break;
                                    case -110:
                                        HiShop.hint("danger", "对不起，文件 [" + a.name + "] 大小超出2MB！");
                                        break;
                                    case -120:
                                        HiShop.hint("danger", "文件 [" + a.name + "] 大小异常！");
                                        break;
                                    case -130:
                                        HiShop.hint("danger", "文件 [" + a.name + "] 类型不正确！")
                                }
                            },
                            onFallback: function () {
                                HiShop.hint("danger", "您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。")
                            },
                            onQueueComplete: function (a) {
                                GetImgList(f, Cache.folderID, 1),
                                    UpdateTreeNums(f)
                            },
                            onUploadError: function (a, b, c, d) {
                                HiShop.hint("danger", "对不起：" + a.name + "上传失败：" + d)
                            }
                        }),
                        m.click(function () {
                            if (!f.find("#j-panelImgs li.selected").length) return void HiShop.hint("warning", "请选择要移动的图片！");
                            var b = a("<div class='albums-cl-tree j-albumsNodes j-propagation'></div>");
                            b.append(p.find("dd:first").contents().clone()),
                                a.jBox.show({
                                    title: "请选择移动文件夹",
                                    content: b,
                                    onOpen: function () {
                                        b.find("dt:first").click()
                                    },
                                    btnOK: {
                                        onBtnClick: function (b) {
                                            var c = [];
                                            f.find("#j-panelImgs li.selected").each(function () {
                                                c.push(a(this).data("id"))
                                            }),
                                                a.ajax({
                                                    url: HiShop.Config.AjaxUrl.moveImg,
                                                    type: "post",
                                                    dataType: "json",
                                                    data: {
                                                        file_id: c,
                                                        cate_id: Cache.moveFolderID
                                                    },
                                                    success: function (b) {
                                                        1 == b.status ? (f.find("#j-panelImgs li.selected").fadeOut(300,
                                                            function () {
                                                                a(this).remove()
                                                            }), UpdateTreeNums(f), HiShop.hint("success", "恭喜您，操作成功！")) : HiShop.hint("danger", "对不起，移动失败：" + b.msg)
                                                    }
                                                }),
                                                a.jBox.close(b)
                                        }
                                    }
                                })
                        }),
                        n.click(function () {
                            if (!Cache.folderID) return void HiShop.hint("warning", "请选择要移动的分类！");
                            var b = a("<div class='albums-cl-tree j-albumsNodes j-propagation'></div>");
                            b.append(p.find("dd:first").contents().clone()),
                                a.jBox.show({
                                    title: "请选择移动文件夹",
                                    content: b,
                                    onOpen: function () {
                                        b.find("dt:first").click()
                                    },
                                    btnOK: {
                                        onBtnClick: function (b) {
                                            a.ajax({
                                                url: HiShop.Config.AjaxUrl.moveCateImg,
                                                type: "post",
                                                dataType: "json",
                                                data: {
                                                    cid: Cache.folderID,
                                                    cate_id: Cache.moveFolderID
                                                },
                                                success: function (b) {
                                                    1 == b.status ? (f.find("#j-panelImgs li.selected").fadeOut(300,
                                                        function () {
                                                            a(this).remove()
                                                        }), UpdateTreeNums(f), HiShop.hint("success", "恭喜您，操作成功！")) : HiShop.hint("danger", "对不起，移动失败：" + b.msg)
                                                }
                                            }),
                                                a.jBox.close(b)
                                        }
                                    }
                                })
                        }),
                        h.click(q)
                }
                f.fadeIn("fast"),
                    g.fadeIn("fast"),
                    f.outerHeight() >= Win.height && f.css({
                        position: "absolute",
                        marginTop: "0",
                        top: a(b).scrollTop() + 10
                    }),
                    f.on("click", ".renameImg",
                        function () {
                            var b = a(this),
                                c = b.closest("li").data("id"),
                                d = b.siblings("input.file_name").val();
                            return a.ajax({
                                url: HiShop.Config.AjaxUrl.renameImg,
                                type: "post",
                                dataType: "json",
                                data: {
                                    file_id: c,
                                    file_name: d
                                },
                                success: function (a) {
                                    1 == a.status ? (b.closest(".albums-edit").children(".img-name-edit").hide(), b.closest(".albums-edit").children("p").html(d), b.closest(".albums-edit").children("input.file_name").val(d), HiShop.hint("success", "恭喜您，操作成功！")) : HiShop.hint("danger", "对不起，操作失败")
                                }
                            }), !1
                        }),
                    f.on("click", ".searchImg",
                        function () {
                            var b = a(this).prev().val();
                            GetImgList(f, Cache.folderID, 1, b)
                        }),
                    f.on("change", ".iselect", function () {
                            var sortby = this.value;
                            GetImgList(f,Cache.folderID,1,"",sortby)
                        })
            }
    }(jQuery, document, window),
    HiShop.popbox.ImgPicker = function (a) {
        $.albums({
            callback: a
        })
    };
