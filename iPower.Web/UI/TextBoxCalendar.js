/*日历控件脚本*/
//校验输入的日期数据是否符合格式。
function TextBoxCalendar_Onblur(clientID, date) {
    try {
        var reg = /^(\d{4})(-)(01|02|03|04|05|06|07|08|09|10|11|12)(-)([0-3]?\d)$/;
        var input = document.getElementById(clientID);
        if (input && input.value != "") {
            if (!reg.test(input.value)) {
                alert("日期格式不正确！\r\n (正确格式为：" + date + ")");               
                input.value = date;
                input.focus();
            } else
                return true;
        }
        return false;
    } catch (e) {
        alert(e.description);
    }
}
//控制是否显示日历层。
function TextBoxCalendar_Show(clientID, bShow) {
    try {
        var host = document.getElementById(clientID + "_host");
        var layer = document.getElementById(clientID + "_CalenLayer");
        var input = document.getElementById(clientID);
        if (host && layer) {
            if (!bShow)
                layer.style.display = "none";
            else {
                var todate = new Date();
                if (input && input.value != "") {
                    try {
                        var regDate = /^(\d{4})-(\d{2})-(\d{2})$/
                        if (regDate.test(input.value)) {
                            //alert("input.value:" + input.value);
                            var str = RegExp.$1 + '/' + RegExp.$2 + '/' + RegExp.$3;
                            //alert("$1/$2/$3:" + str);
                            todate = new Date(str);
                            //alert("todate:" + todate.toDateString());
                        }
                    } catch (e) {
                        alert(e.description);
                    }
                }
                //绘制日历。
                TextBoxCalendar_DrawCalen(layer, input, todate);
                //定位。

                var parent = document.getElementById(clientID + "_InputParent");
                if (parent) {
                    var left = parent.offsetLeft;
                    var top = parent.offsetTop;
                    var h = parent.clientHeight;
                    while (parent = parent.offsetParent) {
                        left += parent.offsetLeft;
                        top += parent.offsetTop;
                    }
                    layer.style.left = left + "px";
                    layer.style.top = top + h + "px";
                }
                //显示日历。
                layer.style.display = "block";
                //注册事件,关闭浮动层。
                document.onclick = (function(x) {
                    return function(e) {
                        e = window.event || e;
                        var srcElement = e.srcElement || e.target;
                        if (srcElement) {
                            var bClose = true;
                            if (srcElement.id.indexOf(clientID) > -1)
                                bClose = false;
                            else {
                                while (srcElement = srcElement.parentElement) {
                                    if (srcElement.id.indexOf(clientID) > -1) {
                                        bClose = false;
                                        break;
                                    } else if (srcElement.tagName == "Body") {
                                        break;
                                    }
                                }
                            }
                            if (bClose)
                                x.style.display = "none";
                        }
                    };
                } (layer));
                //滚动条。
                /*
                setTimeout((function(x) {
                    return function() {
                        var posX = 0, posY = 0;
                        if (window.innerHeight) {//兼容判断。
                            posX = window.pageXOffset;
                            posY = window.pageYOffset;
                        } else if (document.documentElement && document.documentElement.scrollTop) {//兼容判断。
                            posX = document.documentElement.scrollLeft;
                            posY = document.documentElement.scrollTop;
                        } else if (document.body) {//兼容判断。
                            posX = document.body.scrollLeft;
                            posY = document.body.scrollTop;
                        }
                        if (posX + posY != 0) {
                            alert("posX : " + posX + "\r\nposY :" + posY);
                            x.style.left += posX + "px";
                            x.style.top += posY + "px";
                        }
                    };
                } (layer)), 100)
                */
                //
            }
        }
    } catch (e) {
        alert(e.description);
    }
}
//清空输入数据。
function TextBoxCalendar_Clean(clientID) {
    try {
        var layer = document.getElementById(clientID + "_CalenLayer");
        var input = document.getElementById(clientID);
        if (input)
            input.value = "";
        if (layer)
            layer.style.display = "none";
    } catch (e) {
        alert(e.dscription);
    }
}
//格式转换。
function TextBoxCalendar_ConvertDateString(date) {
    if (!date)
        date = new Date();
    var m = (date.getMonth() + 1).toString();
    var d = date.getDate().toString();

    return date.getFullYear().toString() + "-" + ((m.length < 2) ? "0" + m : m) + "-" + ((d.length < 2) ? "0" + d : d);
}
//绘制日历。
function TextBoxCalendar_DrawCalen(layer, input, date) {
    try {
        if (layer) {
            if (input)
                input.value = TextBoxCalendar_ConvertDateString(date);
            var table = document.createElement("table");
            table.setAttribute("cellpadding", "0");
            table.setAttribute("cellspacing", "0");
            table.setAttribute("border", "0");
            table.setAttribute("width", "100%");

            //
            //table.onblur = (function(x) { return function() { alert("hello;"); layer.style.display = "none"; }; } (layer));
            //
            var weeks = ["日", "一", "二", "三", "四", "五", "六"];
            //DreawDate
            var dataBody = document.createElement("tbody");
            table.appendChild(dataBody);
            TextBoxCalendar_DrawDate(layer, input, dataBody, weeks, date);
            //清除遗迹。
            while (layer.firstChild) {
                var oldNode = layer.removeChild(layer.firstChild);
                oldNode = null;
            }
            TextBoxCalendar_DrawYearMonth(layer, input, layer, weeks, date);
            layer.appendChild(table);
            //alert(layer.innerHTML);
        }
    } catch (e) {
        alert(e.description);
    }
}
//绘制年月。
function TextBoxCalendar_DrawYearMonth(layer, input, parent, weeks, date) {
    if (!date)
        date = new Date();
    var args = [layer, input, date];
    
    var table = document.createElement("table");
    table.setAttribute("cellpadding", "0");
    table.setAttribute("cellspacing", "0");
    table.setAttribute("border", "0");
    table.setAttribute("width", "100%");

    var tbody = document.createElement("tbody");
    table.appendChild(tbody);

    var htr = document.createElement("tr");
    //上一月
    var lastM = document.createElement("td");
    lastM.setAttribute("align", "center");
    lastM.setAttribute("title", "上一月");
    lastM.setAttribute("width", "12%;");
    var lastMA = document.createElement("a");
    lastMA.setAttribute("href", "#");
    lastMA.appendChild(document.createTextNode("<"));
    lastMA.onclick = (function(x) {
        return function() {
            var l = x[0];
            var p = x[1];
            var tmp = x[2];
            tmp.setMonth(tmp.getMonth() - 1);
            TextBoxCalendar_DrawCalen(l, p, tmp);
        };
    } (args));
    lastM.appendChild(lastMA);
    htr.appendChild(lastM);
    //上一年
    var lastY = document.createElement("td");
    lastY.setAttribute("align", "center");
    lastY.setAttribute("title", "上一年");
    lastY.setAttribute("width", "13%;");
    var lastYA = document.createElement("a");
    lastYA.setAttribute("href", "#");
    lastYA.appendChild(document.createTextNode("<<"));
    lastYA.onclick = (function(x) {
        return function() {
            var l = x[0];
            var p = x[1];
            var tmp = x[2];
            tmp.setFullYear(tmp.getFullYear() - 1);
            TextBoxCalendar_DrawCalen(l, p, tmp);
        };
    } (args));
    lastY.appendChild(lastYA);
    htr.appendChild(lastY)
    //当前年月。
    var strYear = date.getFullYear().toString();
    var strMonth = (date.getMonth() + 1).toString();
    if (strMonth.length < 2)
        strMonth = "0" + strMonth;
    var strCurrentYearMonth = strYear + "年" + strMonth + "月";
    var current = document.createElement("td");
    current.setAttribute("align", "center");
    current.setAttribute("title", strCurrentYearMonth);
    current.setAttribute("width", "50%;");
    current.appendChild(document.createTextNode(strCurrentYearMonth));
    htr.appendChild(current);
    //下一年
    var nextY = document.createElement("td");
    nextY.setAttribute("align", "center");
    nextY.setAttribute("title", "下一年");
    nextY.setAttribute("width", "13%;");
    var nextYA = document.createElement("a");
    nextYA.setAttribute("href", "#");
    nextYA.appendChild(document.createTextNode(">>"));
    nextYA.onclick = (function(x) {
        return function() {
            var l = x[0];
            var p = x[1];
            var tmp = x[2];
            tmp.setFullYear(tmp.getFullYear() + 1);
            TextBoxCalendar_DrawCalen(l, p, tmp);
        };
    } (args));
    nextY.appendChild(nextYA);
    htr.appendChild(nextY);
    //下一月。
    var nextM = document.createElement("td");
    nextM.setAttribute("align", "center");
    nextM.setAttribute("title", "下一月");
    nextM.setAttribute("width", "12%;");
    var nextMA = document.createElement("a");
    nextMA.setAttribute("href", "#");
    nextMA.appendChild(document.createTextNode(">"));
    nextMA.onclick = (function(x) {
        return function() {
            var l = x[0];
            var p = x[1];
            var tmp = x[2];
            tmp.setMonth(tmp.getMonth() + 1);
            TextBoxCalendar_DrawCalen(l, p, tmp);
        };
    } (args));
    nextM.appendChild(nextMA);
    htr.appendChild(nextM);
    //
    tbody.appendChild(htr);
    //
    parent.appendChild(table);
}
//绘制日期。
function TextBoxCalendar_DrawDate(layer, input, parent, weeks, date) {
    //绘制星期。
    var week = document.createElement("tr");
    for (var i = 0; i < weeks.length; i++) {
        var td = document.createElement("td");
        td.setAttribute("align", "center");
        td.setAttribute("width", "14%");
        if (i == 0 || i == weeks.length - 1)
            td.style.color = "red";
        td.appendChild(document.createTextNode(weeks[i]));
        week.appendChild(td);
    }
    parent.appendChild(week);
    //绘制日期。
    if (!date)
        date = new Date();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var dtBegin = new Date(year, month - 1, 1);
    var dtEnd = new Date(year, month, 1);
    dtEnd.setDate(dtEnd.getDate() - 1);    
    //获取日期数组。
    var list = new Array();
    var index = 0;
    for (var t = dtBegin; ((dtEnd - t) / 1000 / 60 / 60 / 24) + 1 > 0; t.setDate(t.getDate() + 1)) {
        if (index++ == 0) {
            var week = t.getDay();
            if (week > 0) {
                for (var i = week; i > 0; i--) {
                    var tmp = new Date(t.getFullYear(), t.getMonth(), t.getDate());
                    tmp.setDate(tmp.getDate() - i);
                    list.push([tmp.getDate(), 0, 0, TextBoxCalendar_ConvertDateString(tmp)]);
                }
            }
        }
        list.push([t.getDate(), 1,
                  ((t.getFullYear() == date.getFullYear()) && (t.getMonth() == date.getMonth()) && (t.getDate() == date.getDate())) ? 1 : 0,
                  TextBoxCalendar_ConvertDateString(t)]);
    }
    //
    var len = list.length % 7;
    if (len > 0) {
        for (var i = 0; i < 7 - len; i++) {
            var tmp = new Date(dtEnd.getFullYear(), dtEnd.getMonth(), dtEnd.getDate());
            tmp.setDate(tmp.getDate() + i + 1);
            list.push([tmp.getDate(), 0, 0, TextBoxCalendar_ConvertDateString(tmp)]);
        }
    }
    //绘制日期。
    if (list && list.length > 0) {
        var tds = new Array();
        for (var i = 0; i < list.length; i++) {
            //插入tr.
            if (i > 0 && i % 7 == 0 && tds.length > 0) {
                var tr = document.createElement("tr");
                for (var j = 0; j < tds.length; j++) {
                    tr.appendChild(tds[j]);
                }
                parent.appendChild(tr);
                tds = null;
                tds = new Array();
            }
            //创建td.
            var data = list[i];
            var td = document.createElement("td");
            if (data[1] == 0) {
                td.style.color = "#cccccc";
            } else {
                td.style.cursor = "pointer";
                if (data[2] == 1)
                    td.style.color = "red";
            }
            td.setAttribute("title", data[3]);
            if (data[1] == 1) {
                var args = [layer, input, data[3]];
                td.onclick = (function(x) {
                    return function() {
                        var l = x[0];
                        var p = x[1];
                        if (p)
                            p.value = x[2];
                        if (l)
                            l.style.display = "none";
                    };
                } (args));
            }
            td.appendChild(document.createTextNode(data[0]));
            tds.push(td);
        }
        //最后一行
        if (tds.length > 0) {
            var tr = document.createElement("tr");
            for (var j = 0; j < tds.length; j++) {
                tr.appendChild(tds[j]);
            }
            parent.appendChild(tr);
        }
    }
}