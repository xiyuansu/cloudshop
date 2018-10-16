
function showLoading(target) {
    target = $(target);
    container = target;
    if (container.is("tbody")) {
        container = container.parent().parent();
    }
    var loadingcontent = "<div class='DataLoading'><img src='../images/loading.gif'></div>";
    target = $(".DataLoading", container);
    if (target.length < 1) {
        target = $(loadingcontent);
        target.show();
        container.append(target);
    } else {
        target.show();
    }
    return {
        close: function () {
            return function (t) {
                t.hide();
            }(target);
        }
    };
}

function showCommonLoading(title) {
    title = title || "操作等待...";
    var loadingdlg=art.dialog({
        esc: false,
        resize: false,
        drag: false,
        cancel: false,
        title: title,
        lock:true
    });
    return {
        close: function (dlg) {
            return function () {
                dlg.close();
            }
        }(loadingdlg)
    };
}

//无数据显示
var DataNullHelper = {
    NO_DATA_MSG: "<div class='dataNull'><img src='../images/data_null.png'><p>没有找到符合条件的数据!</p></div>",
    target: null,
    container: null,
    init: function () {
        if (container.is("tbody")) {
            container = container.parent().parent();
        }
        target = $(".dataNull", container);
        if (target.length < 1) {
            target = $(this.NO_DATA_MSG);
            target.hide();
            container.append(target);
        }
    },
    show: function (c) {
        container = $(c);
        DataNullHelper.init();
        target.show();
    },
    hide: function (c) {
        container = $(c);
        DataNullHelper.init();
        target.hide();
    }
}