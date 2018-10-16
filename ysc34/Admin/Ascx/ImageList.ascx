
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Ascx.ImageList" %>
   <script type="text/j-template" id="tpl_popbox_ImgPicker">
        <div id="ImgPicker">
            <div class="tabs clearfix">
                <a href="javascript:;" class="active tabs_a fl" data-origin="imgpicker" data-index="1">选择图片</a>
                <a href="javascript:;" class="tabs_a fl j-initupload" data-origin="imgpicker" data-index="2">上传新图片</a>
            </div>
            <!-- end tabs-->
            <div class="tabs-content" data-origin="imgpicker">
                <div class="tc" data-index="1">
                    <ul class="img-list imgpicker-list clearfix"></ul>
                    <!-- end img-list -->
                    <div class="imgpicker-actionPanel clearfix">
                        <div class="fl">
                            <a href="javascript:;" class="btn btn-primary" id="j-btn-listuse">使用选中图片</a>
                        </div>
                        <div class="fr">
                            <div class="paginate"></div>
                        </div>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>

                <div class="tc hide" data-index="2">
                    <div class="uploadifyPanel clearfix">
                        <ul class="img-list imgpicker-upload-preview"></ul>
                        <input type="file" name="imgpicker_upload_input" id="imgpicker_upload_input">
                    </div>

                    <div class="imgpicker-actionPanel">
                        <a href="javascript:;" class="btn btn-primary" id="j-btn-uploaduse">使用上传的图片</a>
                    </div>
                    <!-- end imgpicker-actionPanel -->
                </div>
            </div>
            <!-- end tabs-content -->
        </div>
    </script>

    <script type="text/j-template" id="tpl_popbox_ImgPicker_listItem">
        <# _.each(dataset,function(url){ #>
        <li>
            <span class="img-list-overlay"><i class="img-list-overlay-check"></i></span>
            <img src="<#= url #>">
        </li>
        <# }) #>
    </script>

    <script type="text/j-template" id="tpl_popbox_ImgPicker_uploadPrvItem">
        <li>
            <span class="img-list-btndel j-imgpicker-upload-btndel"><i class="gicon-trash white"></i></span>
            <span class="img-list-overlay"></span>
            <img src="<#= url #>">
        </li>
    </script>
    <!-- end ImgPicker -->

    <!-- end BrandsPicker -->
    <script type="text/j-template" id="tpl_albums_main">
        <div id="albums">
            <div class="albums-title clearfix">
                <span class="fl">我的图库</span>
                <a href="javascript:;" class="fr" id="j-close" title="关闭"><i class="iconfont">&#xe601;</i></a>
            </div>
            <div class="albums-container clearfix">
                <div class="albums-cl fl">
                    <div class="albums-cl-actions">
                        <a href="javascript:;" id="j-addFolder"><i class="gicon-plus"></i><span>添加</span></a>
                        <a href="javascript:;" id="j-renameFolder"><i class="gicon-pencil"></i><span>重命名</span></a>
                        <a href="javascript:;" id="j-delFolder"><i class="gicon-trash"></i><span>删除</span></a>
                    </div>
                    <div class="albums-cl-tree" id="j-panelTree">
                        <p class="txtCenter pdt10 loading j-loading"><i class="icon-loading"></i></p>
                    </div>
                </div>
                <div class="albums-cr fl">
                    <div class="albums-cr-actions">
                        <input type="file" name="imgpicker_upload_input" id="j-addImg">
                        <a href="javascript:;" id="j-moveImg" class="btn btn-primary mgl10">移动图片</a>
                        <a href="javascript:;" id="j-cateImg" class="btn btn-primary mgl5" style="display:none">移动分类</a>
                        <a href="javascript:;" id="j-delImg" class="btn btn-danger mgl5">删除</a>
                        <input type="text" placeholder="请输入图片名称" style="width: 100px;vertical-align: 0;height:32px;margin-left:10px;border-radius: 3px;border: 1px solid #ccc;"><a href="javascript:;" id="j-delImg" class="btn btn-primary mgl10 searchImg">搜索</a>
                      <select name="j-slsImageOrder" onchange="" id="slsImageOrder" class="iselect_one" style="float:right">
	<option selected="selected" value="0">按上传时间从晚到早</option>
	<option value="1">按上传时间从早到晚</option>
	<option value="2">按图片名升序</option>
	<option value="3">按图片名降序</option>
	<option value="6">按图片从大到小</option>
	<option value="7">按图片从小到大</option>
</select>
        </div>
                    <div class="albums-cr-imgs" id="j-panelImgs">
                        <p class="txtCenter pdt10 loading j-loading"><i class="icon-loading"></i></p>
                    </div>
                    <div class="albums-cr-ctrls clearfix">
                        <a href="javascript:;" id="j-useImg" class="btn btn-primary fl">确定</a>
                        <div class="paginate fr" id="j-panelPaginate"></div>
                    </div>
                </div>
            </div>
        </div>
    </script>
    <!-- end tpl_albums_main -->



    <script type="text/j-template" id="tpl_albums_overlay">
        <div id="albums-overlay"></div>
    </script>
    <!-- end tpl_albums_overlay -->

    <script type="text/j-template" id="tpl_albums_tree">
        <dl class="j-albumsNodes">
            <dt data-id="-1" data-add="1" data-rename="0" data-del="0" id="defaultFolder">
                <i class="icon-folder open"></i>
                <span class="j-treeShowTxt"><em class="j-name">默认分类</em>(<em class="j-num"><#=dataset.total#></em>)</span>
            </dt>
            <dd><#=nodes#></dd>
        </dl>
    </script>
    <!-- end tpl_albums_tree -->

    <script type="text/j-template" id="tpl_albums_tree_fn">
        <# _.each(dataset,function(item){#>
        <dl>
            <#if(item.id==0){#>
            <dt data-id="<#=item.id#>" data-add="0" data-rename="0" data-del="0">
                <#}else{#>
            <dt data-id="<#=item.id#>" data-add="1" data-rename="1" data-del="1" id="subFolder">
                <#}#>
                <#if(item.subFolder && item.subFolder.length){#>
                <i class="icon-folder open"></i>
                <#}else{#>
                <i class="icon-folder"></i>
                <#}#>
                <span class="j-treeShowTxt"><em class="j-name"><#=item.name#></em>(<em class="j-num"><#=item.picNum#></em>)</span>
                <#if(item.id!=0){#>
                <input type="text" class="ipt j-ip" maxlength="10" value="<#=item.name#>"><i class="icon-loading j-loading"></i>
                <#}#>
            </dt>
            <dd>
                <#if(item.subFolder && item.subFolder.length){#>
                <#= templateFn({dataset:item.subFolder, templateFn:templateFn}) #>
                <#}#>
            </dd>
        </dl>
        <#})#>
    </script>

    <script type="text/j-template" id="tpl_albums_delFolder">
        <div>
            <p class="ftsize14 bold">删除该文件夹同时会删除其子文件夹，是否继续？</p>
            <div class="radio-group mgt5">
                <label><input type="radio" name="isDelImgs" value="1" checked>不删除图片</label>
                <label><input type="radio" name="isDelImgs" value="2">同时删除图片</label>
            </div>
        </div>
    </script>
    <!-- end tpl_albums_delFolder -->

    <script type="text/j-template" id="tpl_albums_imgs">
        <#if(dataset){#>
        <ul class="clearfix">

            <# _.each(dataset,function(item,index){ #>
            <li class="fl" data-id="<#=item.id#>">
                <img src="<#=item.file#>">
                <div class="albums-cr-imgs-selected"><i></i></div>
                <div class="albums-edit">
                    <span><i class="gicon-pencil edit-img-name"></span></i>
                    <p><#=item.name#></p>
                    <div class="img-name-edit">
                        <input type="text" value="<#=item.name#>" style="width:60%;" name="rename" class="file_name" />
                        <a href="javascript:;" class="renameImg">确定</a>
                    </div>
                </div>
            </li>
            <# }) #>
        </ul>
        <#}else{#>
        <p class="albums-cr-imgs-noPic j-noPic">暂无图片</p>
        <#}#>
    </script>
    <!-- end tpl_albums_imgs -->