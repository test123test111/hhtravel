window.share = function () {
    var a = {
        sina: "http://service.weibo.com/share/share.php?url={$url}&title={$content}&pic={$pic}&appkey=968446907",
        qq: "http://v.t.qq.com/share/share.php?url={$url}&title={$content}&pic={$pic}&appkey=e5d288d65a1143e59c49231879081bb0&site=www.ctrip.com",
        renren: "http://share.renren.com/share/buttonshare?link={$url}&title={$content}",
        qzone: "http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url={$url}&title={$content}&pics={$pic}&site={$from}",
        kaixin: "http://campaign.ctrip.com/Destinations/fenxiang/fenxiang.asp?type=1&url={$url}&ti={$content}",
        douban: "http://campaign.ctrip.com/Destinations/fenxiang/fenxiang.asp?type=5&url={$url}&ti={$content}",
        souhu: "http://t.sohu.com/third/post.jsp?url={$url}&title={$content}&pic={$pic}&appkey=&content=utf-8",
        163: "http://t.163.com/article/user/checkLogin.do?info={$content}{$url}&images={$pic}&togImg=true&link=http://www.ctrip.com/&source={$from}"
    },
    b = {
        content: encodeURIComponent(document.title),
        url: encodeURIComponent(document.location),
        pic: function () {
            var a = [], b = document.getElementById("player-pic-list");
            if (b) {
                b = b.getElementsByTagName("img");
                if (b && b.length)
                    for (var c = 0, d = b.length; c < d; c++)
                        a[c] = b[c].getAttribute("bsrc")
            }
            return {
                sina: a.join("||"),
                qq: a.join("|"),
                qzone: a.join("|"),
                souhu: a.length ? a[0] : "",
                163: a.join(",")
            }
        } (),
        from: encodeURIComponent($s2t("携程旅行网"))
    };
    return function (c) {
        window.open(a[c].replace(/\{\$(\w+)\}/g,
            function (a, d) {
                return d === "pic" ? b.pic[c] : b[d]
            }), "", "width=700, height=680, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, location=yes, resizable=no, status=no")
    }
} ();