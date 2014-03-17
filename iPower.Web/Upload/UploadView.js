/*上传控件脚本*/
//显示或隐藏上传浏览浮动层。
function UploadView_DisplayUploadBrowserLayer(host, ctrl, bShow) {
    try {
        if (ctrl) {
            if (host && bShow) {
                ctrl.style.left = host.offsetLeft + 10 + "px";
                /*ctrl.style.top = host.offsetTop + host.clientHeight + "px";*/
            }
            ctrl.style.display = bShow ? "block" : "none";
        }
    } catch (e) {
        alert(e.description);
    }
}
//控制显示。
function UploadView_DisplayUploadControl(clientID, btn) {
    try {
        var host = document.getElementById(clientID);
        var ctrl = document.getElementById(clientID + "_UploadControlLayer");
        if (ctrl) {
            var bShow = (btn.value == "上传");
            btn.innerText = bShow ? "关闭" : "上传";
            UploadView_DisplayUploadBrowserLayer(host, ctrl, bShow);
        }
    } catch (e) {
        alert(e.description);
    }
}

//全选。
function UploadView_SelectAll(clientID, obj) {
    try {
        var host = document.getElementById(clientID);
        if (host && obj) {
            var c = obj.checked;
            var cb = host.getElementsByTagName("input");
            if (cb && cb.length > 0) {
                for (var i = 0; i < cb.length; i++) {
                    if ((cb[i].type == "checkbox") && (cb[i].id.indexOf(clientID + "_CHK_") > -1)) {
                        cb[i].checked = c;
                    }
                }
            }
        }
    } catch (e) {
        alert(e.description);
    }
}
//上传。
function UploadView_UploadSave(clientID) {
    try {
        var host = document.getElementById(clientID);
        var ctrl = document.getElementById(clientID + "_UploadControlLayer");
        var upload = document.getElementById(clientID + "_UploadFile");
        if (host && ctrl && upload) {
            if (upload.value == "") {
                alert("请选择上传文件！");
                upload.focus();
            } else {
                UploadView_DisplayUploadBrowserLayer(host, ctrl, false);
                UploadView_doPostBack("btnSave");
            }
        }
    } catch (e) {
        alert(e.description);
    }
}
//删除。
function UploadView_UploadDelete(clientID) {
    try {
        var result = false;
        var host = document.getElementById(clientID);
        if (host) {
            var cb = host.getElementsByTagName("input");
            if (cb && cb.length > 0) {
                for (var i = 0; i < cb.length; i++) {
                    if ((cb[i].type == "checkbox") && (cb[i].id.indexOf(clientID + "_CHK_") > -1)) {
                        if (cb[i].checked) {
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

    } catch (e) {
        alert(e.description);
    }
}
//在线编辑。
function UploadView_DocumentEdit(url) {
    try {
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
        if (result == false)
            alert("无法打开文档。");
    } catch (e) {
        alert(e.description)
    }
}