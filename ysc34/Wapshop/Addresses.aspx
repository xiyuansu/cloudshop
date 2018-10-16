<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Addresses.aspx.cs" Inherits="Hidistro.UI.Web.Wapshop.Addresses" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head> 
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <meta name="screen-orientation" content="portrait">
    <meta name="x5-orientation" content="portrait">
    <script charset="utf-8" src="https://map.qq.com/api/js?v=2.exp&libraries=convertor"></script>
    <title>定位</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            border: 0;
        }

        body {
            position: absolute;
            width: 100%;
            height: 100%;
            text-align: center;
        }

        #pos-area {
            background-color: #009DDC;
            margin-bottom: 10px;
            width: 100%;
            overflow: scroll;
            text-align: left;
            color: white;
        }

        #demo {
            padding: 8px;
            font-size: small;
        }

        #btn-area {
            height: 100px;
        }

        button {
            margin-bottom: 10px;
            padding: 12px 8px;
            width: 42%;
            border-radius: 8px;
            background-color: #009DDC;
            color: white;
        }
    </style>

</head>
<body>
    <iframe id="mapPage" width="100%" height="100%" frameborder=0
            src="https://apis.map.qq.com/tools/locpicker?search=1&type=1&key=OB4BZ-D4W3U-B7VVO-4PJWW-6TKDJ-WPB77&referer=myapp"></iframe>
    <iframe id="geoPage" width=0 height=0 frameborder=0  style="display:none;" scrolling="no"
    src="https://apis.map.qq.com/tools/geolocation?key=OB4BZ-D4W3U-B7VVO-4PJWW-6TKDJ-WPB77&referer=myapp">
</iframe>
    <script>
    window.addEventListener('message', function(event) {
        // 接收位置信息，用户选择确认位置点后选点组件会触发该事件，回传用户的位置信息
        var loc = event.data;
        if (loc && loc.module == 'locationPicker') {//防止其他应用也会向该页面post信息，需判断module是否为'locationPicker'
            alert(loc.poiaddress);
        }
        if (loc && loc.module == 'geolocation') {//防止其他应用也会向该页面post信息，需判断module是否为'geolocation'
            alert(JSON.stringify(loc, null, 4));
        }
    }, false);
    </script>
</body>
</html>
