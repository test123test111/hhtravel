// append http://widget.renren.com/js/rrshare.js START
var js_element = document.createElement("script");
js_element.setAttribute("type", "text/javascript");
js_element.setAttribute("src", "http://widget.renren.com/js/rrshare.js");
document.getElementsByTagName("head")[0].appendChild(js_element);
// append http://widget.renren.com/js/rrshare.js END

$(document).ready(function () {
    $("#img_share_close").click(function () {
        $("#div_share").slideToggle(500);
    });
    $("#share").click(function () {
        $("#div_share").slideToggle(500);
    });
    $(".share_path td").click(function () {
        $("#div_share").slideToggle(500);
    });
}); // end $(document).ready(function(){
// 微博
function weibo(prodUrl, prodNm, prodPic) {
    var description = prodNm + " | 鸿鹄逸游 HHtravel";
    var param = {
        url: prodUrl,
        type: '3',
        // count:'1', 		  // 顯示分享數，1顯示(可選)
        // appkey:'', 		  // 申請的appkey
        // ralateUid:'',  	  // 關聯用戶的UID
        title: description,   // 分享的文字內容
        pic: prodPic, 			  // 分享的圖片路徑
        language: 'zh_cn', 	  // 設置的語言 ，zh_cn|zh_tw (目前無效果)
        rnd: new Date().valueOf()
    };
    var temp = [];
    for (var p in param) {
        temp.push(p + '=' + encodeURIComponent(param[p] || ''));
    }
    window.open("http://service.weibo.com/share/share.php?" + temp.join('&'), "share", "width=800,height=600,top=50,left=100,resizable=yes");
} // end function weibo(prodNm,prodNo){
// 人人網
function renren(prodUrl, prodNm, prodPic) {
    var description = prodNm + " | 鸿鹄逸游 HHtravel";
    var rrShareParam = {
        resourceUrl: prodUrl, 		//分享的資源Url
        pic: prodPic, 			//分享的圖片Url
        title: description, 	//分享的標題
        description: prodUrl			//分享的描述
    };
    rrShareOnclick(rrShareParam);
} // end function renren(prodNm,prodNo) {
function qzone(prodUrl, prodNm, prodPic) {
    var title = prodNm + " | 鸿鹄逸游 HHtravel";
    var url = 'http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url='
                + encodeURIComponent(prodUrl)
                + '&title=' + encodeURIComponent("" + title)
                + '&pics=' + prodPic;
    window.open(url);
}