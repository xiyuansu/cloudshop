$(function () {
    var a;
    var b = ($(this), $("#pageclient").val());
    var topicid = "0";
    if (b == "topic" || b == "Apptopic" || b == "pctopic")
    {
        topicid = $("#topicid").val();        
    }
    if (topicid != "0") {
        $.ajax({
            url: HiShop.Config.AjaxUrl.getPage,
            type: 'GET',
            dataType: 'text',
            data: {
                client: b,
                topicid: topicid
            },
            success: function (data) {
                a = data.length ? $.parseJSON(data) : Defaults[b];
                $(".backgroundColor").val(a.page.backgroundColor);
                $("#hidUploadImages").val(a.page.backgroundImg);
                initImageUpload();
                $('#diy-contain').css('background-color', a.page.backgroundColor);
                $('#diy-contain').css('background-image', "url(" + a.page.backgroundImg + ")");
                $('#diy-contain').css('background-repeat', a.page.fillingMethod);
                var position = a.page.bgAlign + " top";
                $('#diy-contain').css('background-position', position);
                var r = a.page.fillingMethod;
                $(".repeat").each(function () {
                    var a = $(this).val();
                    if (a == r) {
                        $(this).attr("checked", 'checked');
                        $(this).parent().siblings().children().removeAttr("checked");
                    }
                })

                var p = a.page.bgAlign;
                $(".position").each(function () {
                    var a = $(this).val();
                    if (a == r) {
                        $(this).attr("checked", 'checked');
                        $(this).parent().siblings().children().removeAttr("checked");
                    }
                })

                $(".j-pagetitle-ipt").val(a.page.title);
                if (b == "topic") {
                    $(".j-pagetitle-describe").val(a.page.describe); //获取页面描述数据
                }
               
                _.each(a.PModules,
                 function (a, b) {
                     var c = 0 == b ? !0 : !1;
                     HiShop.DIY.add(a, c);
                 });
                _.each(a.LModules,
                function (a) {
                    HiShop.DIY.add(a);
                });
            }
        });
    }
    $("#j-savePage").click(function () { 
        return HiShop.DIY.Unit.getData() ? ($.ajax({
            url: HiShop.Config.AjaxUrl.savePage,
            type: "post",
            dataType: "json",
            data: {
                content: JSON.stringify(HiShop.DIY.Unit.getData()),
                client: b,
                getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                is_preview: 0                
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {               
                1 == a.status ? HiShop.hint("success", "恭喜您，保存成功！") : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    }),
     $("#j-savePage-pc").click(function () {
         var srcImg = $('#imageContainer span[name="bgImage"]').hishopUpload("getImgSrc");
         $("#hidUploadImages").val(srcImg);
         $('#diy-contain').css('background-image', "url(" + srcImg + ")");
         var a = $(".j-pagetitle-ipt").val();
         if (a == "") {
             HiShop.hint("danger", "请填写专题名称")
             return false;
         }
         return HiShop.DIY.Unit.getData() ? ($.ajax({
             url: HiShop.Config.AjaxUrl.savePage,
             type: "post",
             dataType: "json",
             data: {
                 content: JSON.stringify(HiShop.DIY.Unit.getData()),
                 client: b,
                 getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                 getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                 is_preview: 0,
                 topicid: topicid
             },
             beforeSend: function () {
                 $.jBox.showloading()
             },
             success: function (a) {
               
                 1 == a.status ? HiShop.hint("success", "恭喜您，保存成功！") : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                 $.jBox.hideloading();
            
                 if (topicid == "0") {
                     if (b == "topic") {
                         window.location.href = "TopicTempEdit?topicId=" + a.topicid;                        
                     }
                     else if (b == "pctopic") {
                         window.location.href = "PcTopicTempEdit?topicId=" + a.topicid;
                     }
                     else {
                         window.location.href = "AppTopicTempEdit?topicId=" + a.topicid;                        
                     }
                 }
                 window.opener.location.reload();
             }
         }), !1) : void 0
     }),
      $("#j-savePage-app").click(function () {
          return HiShop.DIY.Unit.getData.App() ? ($.ajax({
              url: HiShop.Config.AjaxUrl.savePage,
              type: "post",
              dataType: "json",
              data: {
                  content: JSON.stringify(HiShop.DIY.Unit.getData.App()),
                  client: b,
                  getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                  getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                  is_preview: 0                 
              },
              beforeSend: function () {
                  $.jBox.showloading()
              },
              success: function (a) {
                  1 == a.status ? HiShop.hint("success", "恭喜您，保存成功！") : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                  $.jBox.hideloading()
              }
          }), !1) : void 0
      }),

    $("#j-resetToInit").click(function () {
        if (confirm("还原后您编辑的模板将不能保存，确认还原吗?")) {
            return HiShop.DIY.Unit.getData() ? ($.ajax({
                url: HiShop.Config.AjaxUrl.pageRecover,
                type: "post",
                dataType: "json",
                data: {
                    client: b
                },
                beforeSend: function () {
                    $.jBox.showloading()
                },
                success: function (a) {
                    if (1 == a.status) {
                        HiShop.hint("success", "成功还原到初始状态,请重新打开页面并保存");
                        window.location.reload();
                    }
                    else {
                        HiShop.hint("danger", "对不起，还原失败：" + a.msg)
                    }

                    $.jBox.hideloading()
                }
            }), !1) : void 0
        }
    }),

    $("#j-saveAndPrvPage").click(function () {
        return HiShop.DIY.Unit.getData() ? ($.ajax({
            url: HiShop.Config.AjaxUrl.savePage,
            type: "post",
            dataType: "json",
            data: {
                content: JSON.stringify(HiShop.DIY.Unit.getData()),
                client: b,
                is_preview: 1,
                getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                topicid: topicid
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {
                1 == a.status ? (HiShop.hint("success", "恭喜您，保存成功！"), setTimeout(function () {
                    window.open(a.link)
                },
                1e3)) : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    })
});