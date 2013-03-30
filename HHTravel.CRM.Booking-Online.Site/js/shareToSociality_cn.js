// append http://widget.renren.com/js/rrshare.js START
var js_element=document.createElement("script");
    js_element.setAttribute("type","text/javascript");
    js_element.setAttribute("src","http://widget.renren.com/js/rrshare.js");
document.getElementsByTagName("head")[0].appendChild(js_element);


// append http://widget.renren.com/js/rrshare.js EDN
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
function weibo(prodNo,prodNm,prodPic){
    var url = "http://" + location.hostname + "/Product/prod_no=" + prodNo;
    var photo = prodPic;
    var description = prodNm + " | 鸿鹄逸游 HHtravel"; 
  var param = {
	url:url,
	type:'3',
	// count:'1', 		  // 顯示分享數，1顯示(可選)
	// appkey:'', 		  // 申請的appkey
	// ralateUid:'',  	  // 關聯用戶的UID 
	title: description,   // 分享的文字內容
	pic:photo, 			  // 分享的圖片路徑
	language: 'zh_cn', 	  // 設置的語言 ，zh_cn|zh_tw (目前無效果)
	rnd:new Date().valueOf() 
  };
  var temp = [];
  for( var p in param ){
	temp.push(p + '=' + encodeURIComponent( param[p] || '' ) );
  } 
  window.open("http://service.weibo.com/share/share.php?"+temp.join('&'),"share","width=800,height=600,top=50,left=100,resizable=yes");	
} // end function weibo(prodNm,prodNo){

// 人人網
function renren(prodNo, prodNm, prodPic) {
    var url = "http://" + location.hostname + "/Product/prod_no=" + prodNo;
  var photo = prodPic;
  var description = prodNm +" | 鸿鹄逸游 HHtravel";
  var rrShareParam = {
		resourceUrl : url,			//分享的資源Url
		pic : photo,				//分享的圖片Url
		title : description,		//分享的標題
		description : url			//分享的描述
	  };
  rrShareOnclick(rrShareParam);
} // end function renren(prodNm,prodNo) {




// 噗浪
function plurk(prodNm,prodNo){
  var url = "http://"+location.hostname+"/ezTopTravel/cn/index.jsp?prod_no="+prodNo;
  var photo = "http://www.eztravel.com.tw/img/FRN/"+prodNo+".gif";
  var description = " (" + prodNm +" | 鸿鹄逸游 HHtravel)";
  window.open("http://www.plurk.com/?qualifier=shares&status="+url+"+"+description+"+"+photo,"share","width=800,height=600,top=50,left=100,resizable=yes");
} // end function plurk(prodNm,prodNo){




// 推特
// 目前無法加入推圖片
function twitter(prodNm,prodNo){
  var url = "http://"+location.hostname+"/ezTopTravel/cn/index.jsp?prod_no="+prodNo;
  var description = prodNm +" │ 鸿鹄逸游 HHtravel：";
  window.open("http://twitter.com/home/?status="+description+url,"share","width=800,height=600,top=50,left=100,resizable=yes");
}




// 臉書
// 目前無法加入推圖片
function facebook(prodNo){
  var url = "http://"+location.hostname+"/ezTopTravel/cn/index.jsp?prod_no="+prodNo;
  window.open("http://www.facebook.com/sharer/sharer.php?u="+url,"share","width=800,height=600,top=50,left=100,resizable=yes");
} // end function facebook(prodNo){




// 谷歌
// 目前無法加入推圖片
function google(prodNo){
  var url = "http://"+location.hostname+"/ezTopTravel/cn/index.jsp?prod_no="+prodNo;
  window.open("https://plusone.google.com/_/+1/confirm?hl=en&url="+url,"share","width=800,height=600,top=50,left=100,resizable=yes");
} // end function google(prodNo){


