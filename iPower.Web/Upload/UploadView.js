/*上传控件脚本*/
$(function () {
    //显示或隐藏上传浏览浮动层。
    function UploadView_DisplayUploadBrowserLayer(host, ctrl, bShow) {
        if (ctrl) {
            if (host && bShow) {
                //ctrl.style.left = host.offsetLeft + 10 + "px";
                var os = ctrl.offset();
                var lt = os.left + 10;
                ctrl.offset({ left: lt });
            }
            //ctrl.style.display = bShow ? "block" : "none";
            ctrl.css("display", (bShow ? "block" : "none"));
        }
    };
    //控制显示。
    UploadView_DisplayUploadControl = function (clientID, btn) {
        var host = $("#" + clientID);
        var ctrl = $("#" + clientID + "_UploadControlLayer");
        if (ctrl) {
            //alert(btn); console.info(btn); alert($(btn)); console.info($(btn)); alert($(btn).val());
            var bShow = ($(btn).val() == "上传");//(btn.value == "上传");
            // btn.value = bShow ? "关闭" : "上传";
           $(btn).val(bShow ? "关闭" : "上传");
            UploadView_DisplayUploadBrowserLayer(host, ctrl, bShow);
        }
    };
    //全选。
    UploadView_SelectAll = function (clientID, obj) {
        var host = document.getElementById(clientID);
        if (host && obj) {
            var c = $(obj).attr("checked");//checked;
            var cb = host.getElementsByTagName("input");
            if (cb && cb.length > 0) {
                for (var i = 0; i < cb.length; i++) {
                    //if ((cb[i].type == "checkbox") && (cb[i].id.indexOf(clientID + "_CHK_") > -1)) {
                    var id = $(cb[i]).attr("id");
                    if (($(cb[i]).attr("type") == "checkbox") && (id.indexOf(clientID + "_CHK_") > -1)) {
                        $(cb[i]).attr("checked", c);// checked = c;
                    }
                }
            }
        }
    };
    //上传。
    UploadView_UploadSave = function (clientID) {
        var host = $("#" + clientID);
        var ctrl = $("#" + clientID + "_UploadControlLayer");
        var upload =$("#" + clientID + "_UploadFile");
        if (host && ctrl && upload) {
            if (upload.val() == "") {
                alert("请选择上传文件！");
                upload.focus();
            } else {
                UploadView_DisplayUploadBrowserLayer(host, ctrl, false);
                UploadView_doPostBack("btnSave");
            }
        }
    };
    //删除。
    UploadView_UploadDelete = function (clientID) {
        var result = false;
        var host = document.getElementById(clientID);
        if (host) {
            var cb = host.getElementsByTagName("input");
            if (cb && cb.length > 0) {
                for (var i = 0; i < cb.length; i++) {
                    var id = $(cb[i]).attr("id");
                    if (($(cb[i]).attr("type") == "checkbox") && (id.indexOf(clientID + "_CHK_") > -1)) {
                        if ($(cb[i]).attr("checked")) {
                            result = true;
                            break;
                        }
                    }
                }
            }
        }
        if (result)
            UploadView_doPostBack('btnDelete');
        else
            alert("请选择需要删除的文件！");
    };
    //在线编辑。
    UploadView_DocumentEdit = function (url) {
        var openDocuments = null;
        try {
            //for office 2007
            openDocuments = new ActiveXObject("SharePoint.OpenDocuments.3");
        } catch (e) { }
        try {
            //for office 2003
            if (openDocuments == null || typeof (openDocuments) == "undefined")
                openDocuments = new ActiveXObject("SharePoint.OpenDocuments.2");
        } catch (e) { }

        if (openDocuments == null || typeof (openDocuments) == "undefined") {
            alert("请安装Office 2003或更高版本!");
            return;
        }
        var result = openDocuments.EditDocument(url, "Word.Document");
        if (result == false) alert("无法打开文档。");
    };
});