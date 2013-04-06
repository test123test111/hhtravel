// 弹出新窗口
function openwin(url, width, height) {
    var w = width ? width : 740;
    var h = height ? height : 600;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    var win = window.open(url,
                'newwindow',
                'height=' + h + ', width=' + w + ', top=' + top + ', left=' + left + ', toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no');  //  scrollbars=no, location=yes
    return win;
}
// http://www.w3schools.com/js/js_cookies.asp?output=print
function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}
// 数值显示为千分位
var formatToCurrency = function (num) {
    var numStr = num.toString();
    var re = /(\d{1,3})(?=(\d{3})+(?:$|\D))/g;
    numStr = numStr.replace(re, "$1,");
    return numStr;
};
/**Parses string formatted as YYYY-MM-DD to a Date object.
* If the supplied string does not match the format, an
* invalid Date (value NaN) is returned.
* @param {string} dateStringInRange format YYYY-MM-DD, with year in
* range of 0000-9999, inclusive.
* @return {Date} Date object representing the string.
*/
function parseISO8601(dateStringInRange) {
    var isoExp = /^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
       date = new Date(NaN), month,
       parts = isoExp.exec(dateStringInRange);

    if (parts) {
        month = +parts[2];
        date.setFullYear(parts[1], month - 1, parts[3]);
        if (month != date.getMonth() + 1) {
            date.setTime(NaN);
        }
    }
    return date;
}
function share(site, url, content, pics) {
    var a = {
        weibo: 'http://service.weibo.com/share/share.php?url={$url}&title={$content}&pic={$pic}&searchPic=false',
        tqq: 'http://v.t.qq.com/share/share.php?url={$url}&title={$content}&pic={$pic}',
        renren: 'http://share.renren.com/share/buttonshare/post/4001?url={$url}&title={$content}&pic={$pic}',
        qzone: 'http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url={$url}&title={$content}&pics={$pic}'
    };
    pics = pics ? pics : [];
    var b = {
        url: encodeURIComponent(url),
        content: encodeURIComponent(content),
        pic: function () {
            return {
                weibo: pics.join('||'),
                tqq: pics.join("|"),
                renren: pics[0] ? pics[0] : '',
                qzone: pics.join("|")
            };
        } ()
    };
    var url = a[site].replace(/\{\$(\w+)\}/g, function (a, d) { return d === 'pic' ? b.pic[site] : b[d]; });
    openwin(url, 700, 680);
}
$(function () {
    //    http: //www.thefutureoftheweb.com/blog/target-blank-xhtml11
    $('a.new-blank').click(function () {
        window.open(this.href);
        return false;
    });

    $('a.new-window').click(function () {
        openwin(this.href, 760);
        return false;
    });

    $('input.date').attr('autocomplete', 'off').attr('maxlength', 10);
});