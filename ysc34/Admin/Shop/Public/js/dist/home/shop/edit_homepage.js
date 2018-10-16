$(function () {   
    var a;
    var b = ($(this), $("#pageclient").val());
    var topicid = "0";
    var themeName = "default";
    if (b == "topic" || b == "Apptopic" || b == "pctopic") {
        topicid = $("#topicid").val();
    }
    else {
        themeName = $("#themeName").val();
    }
    $.ajax({
        url: HiShop.Config.AjaxUrl.getPage,
        type: 'GET',
        dataType: 'text',
        data: {
            client: b,
            topicid: topicid,
            themeName: themeName
        },
        success: function (data) {
            a = data.length ? $.parseJSON(data) : Defaults[b];
            $(".j-pagetitle").text(a.page.title);
            $(".j-pagetitle-ipt").val(a.page.title);
            if (b == "topic") {
                $(".j-pagetitle-describe").val(a.page.describe); //获取页面描述数据
            }
            if (b == "vshop") {
                $(".j-pagetitle-describe").val(a.page.describe); //获取页面描述数据
                var imgSrc = a.page.sharepic ? a.page.sharepic.replace("/temp/", "/topic/") : "";
                $('#imageContainer span[name="defaultImage"]').hishopUpload(
                          {
                              title: '分享图标',
                              url: "/admin/UploadHandler.ashx?action=newupload",
                              imageDescript: '',
                              displayImgSrc: imgSrc,
                              imgFieldName: "defaultImage",
                              defaultImg: '',
                              pictureSize: '200*200',
                              imagesCount: 1,
                              dataWidth: 9
                          });
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
    }),

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
                is_preview: 0,
                themeName: themeName
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
     $("#j-savePage-topic").click(function () {
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
                    client: b,
                    themeName:themeName
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
                topicid: topicid,
                themeName:themeName
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {
                1 == a.status ? (HiShop.hint("success", "恭喜您，保存成功！"), setTimeout(function () {
                    window.open(a.link + "?fromSource=1")
                },
                1e3)) : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    })
});