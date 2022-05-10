//从生产系统取值对应字段信息
//document.getElementById("ddh").value = data.Tables[0].Rows[0].customer_order_id;
//document.getElementById("khlh").value = data.Tables[0].Rows[0].customer_goods_id ;
//document.getElementById("aslh").value = data.Tables[0].Rows[0].company_materials_id;
//document.getElementById("sl").value = data.Tables[0].Rows[0].qty;
//document.getElementById("scph").value = data.Tables[0].Rows[0].number;
//document.getElementById("fph").value = data.Tables[0].Rows[0].id;
//document.getElementById("zq").value = data.Tables[0].Rows[0].dc;
//document.getElementById("zx_cpms").value = data.Tables[0].Rows[0].goods_name + ' ' + data.Tables[0].Rows[0].size_id; //+ ' ' + data.Tables[0].Rows[0].terminal
//data.Tables[0].Rows[0].goods_name + " " + data.Tables[0].Rows[0].size_id +" " + data.Tables[0].Rows[0].parameter(温度系数) + " " + data.Tables[0].Rows[0].terminal + " " + data.Tables[0].Rows[0].sleeve_color(套管颜色) + " " + data.Tables[0].Rows[0].sleeve;
//document.getElementById("zx_scrq").value = new Date(data.Tables[0].Rows[0].datetime_source).format("yyyy-MM-dd").replace('-', '').replace('-', '');
//Date 原型扩展方法 直接点出来 yyyy-MM-dd hh:mm:ss 或者 yyyy-MM-dd
Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}
//给当前日期+6个月
function Addhalfyear() {
    var date = new Date();
    //日期转文本方式一：
    var str = date.format("yyyy-MM-dd");
    var year = date.getFullYear();//年
    var month = Number(date.getMonth()) + 7;//月 +6个月  因为js里month从0开始，所以要加1
    if (month > 12) {
        year++;
        month -= 12;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date2 = new Date(year, month, 0);//新的年月
    var day1 = date.getDate();
    var day2 = date2.getDate();
    if (day1 > day2) {  //防止+6月后没有31天
        day1 = day2;
    }
    //因为下面的方法传的参就是字符类型，所以不用补前导0，但这里是系统通用方法还是需要补的
    if (day1 < 10) {
        day1 = "0" + day1;
    }
    str = year + '-'+ month + '-'+ day1;

    //最后赋值文本框显示
    document.getElementById("yxq").value = str;
}
//根据给定的日期+6个月
function Addhalfyear_s(yxrq) {
    var year = yxrq.substring(0,4);//年
    var month = Number(yxrq.substring(4, 6)) + 6;//月 数字化,后续+0了的
    if (month > 12) {
        year++;
        month -= 12;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date2 = new Date(year, month, 0);//新的年月
    var day1 = yxrq.substring(6, 10);
    var day2 = date2.getDate();
    if (day1 > day2) {  //防止+6月后没有31天
        day1 = day2;
    }
    str = year + '-'+ month + '-'+ day1;

    //最后赋值文本框显示
    document.getElementById("yxq").value = str;
}
//JS cookie 操作方法
var CookieUtil = {
    get: function (name) {
        var cookieName = encodeURIComponent(name) + "=",
            cookieStart = document.cookie.indexOf(cookieName),
            cookieValue = null;
        if (cookieStart > -1) {
            var cookieEnd = document.cookie.indexOf(";", cookieStart);
            if (cookieEnd == -1) {
                cookieEnd = document.cookie.length;
            }
            cookieValue = decodeURIComponent(document.cookie.substring(cookieStart + cookieName.length, cookieEnd));
        }
        return cookieValue;
    },
    set: function (name, value, expires, path, domain, secure) {
        var cookieText = encodeURIComponent(name) + "=" + encodeURIComponent(value);
        if (expires instanceof Date) {
            cookieText += "; expires = " + expires.toGMTString();
        }
        if (path) {
            cookieText += ";path = " + path;
        }
        if (domain) {
            cookieText += ";domain =" + domain;
        }
        if (secure) {
            cookieText += ";secure";
        }
        document.cookie = cookieText;
    },
    unset: function (name, path, domain, secure) {
        this.set(name, "", new Date(0), path, domain, secure);
    }
};
function addhowtouse(tsdiv) {
    //触发事件
    var active = {
        notice: function () {
            //示范一个公告层
            layer.open({
                type: 1
                //,title: false //不显示标题栏
              , title: '标签系统使用方法以及本客户标签特色功能说明'
              , closeBtn: false
              , area: '500px;'
              , shade: 0.8
              , id: 'LAY_layuipro' //设定一个id，防止重复弹出
              , btn: ['前往查看版本更新说明', '已读谢谢']
              , btnAlign: 'c'
              , anim: 2
              , moveType: 1 //拖拽模式，0或者1
              , content: tsdiv
              , success: function (layero) {
                  //var btn = layero.find('.layui-layer-btn');                                          
              }, yes: function (index, layero) {
                  //按钮【按钮一】的回调
                  parent.addTab("版本更新说明", "../Bbgxnote/Index") //调用上一层home Index 里的方法
              }
            });
        }
    };
    $('#layerDemo .layui-btn').on('click', function () {
        var othis = $(this), method = othis.data('method');
        active[method] ? active[method].call(this, othis) : '';
    });
    $('#layerDemo .layui-btn').click();
    CookieUtil.set("gbk", "1", null, "/", null, false);
}
//补充数量前导0
function addPreZero(num,len) {
    var t = (num + '').length,
    s = '';
    for (var i = 0; i < len - t; i++) {
        s += '0';
    }
    return s + num;
}
//补充数量后导空格
function addPreBlank(num, len) {
    var t = (num + '').length,
    s = '';
    for (var i = 0; i < len - t; i++) {
        s += ' ';
    }
    return  num + s;
}
//补充数量后导字符
function addPregang(num, len) {
    var t = (num + '').length,
    s = '';
    for (var i = 0; i < len - t; i++) {
        s += "-";
    }
    return num + s;
}