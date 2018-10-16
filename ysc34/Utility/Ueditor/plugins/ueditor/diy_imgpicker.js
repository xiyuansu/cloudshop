UE.registerUI('diyimg', function (editor, uiName) {
    var btn = new UE.ui.Button({
        name: uiName, //按钮的名字
        title: "添加图片",//提示
        cssRules: 'background-position: -380px 0;',
        onclick: function () {
            //调用会员道选择图片组件
            HiShop.popbox.ImgPicker(function (data) {
                for (var i = 0; i < data.length; i++) {
                    editor.execCommand('inserthtml', '<p><img src="' + data[i] + '">​</p>');//向光标插入内容
                }
            });
        }
    });

    //当点到编辑内容上时，按钮要做的状态反射
    editor.addListener('selectionchange', function () {
        var state = editor.queryCommandState(uiName);
        if (state == -1) {
            btn.setDisabled(true);
            btn.setChecked(false);
        } else {
            btn.setDisabled(false);
            btn.setChecked(state);
        }
    });

    return btn;
}, 2);