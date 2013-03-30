function printer(tab, prodNo, prodName, baseUrl) {
    var $content = $("#pro_list").clone();
    switch (tab) {
        case 2:
            //			    $content.find("div:[class='day_list_warp']").show();       //顯示詳細行程所有結果
            break;
        case 0:
            $content.find("#ddate1").hide();
            $content.find("#ddate2").show();    //顯示出發日期所有結果 
            break;
    }
    $content.find("p:[class='page']").remove();
    $content.find("div:[class='mask_layer']").remove();
    $content.find("div:[class='img_layer']").remove();
    $content.find("p:[class='d_visited_type']").remove();
    //        $content.find("a:[id='printer']").attr("href", "javascript:window.focus();window.print();window.close();");
    var body = $content.html();
    var printPage = window.open("", "Print", "top=50, left=100, height=600, width=800, scrollbars=yes, toolbar =no, menubar=no,location=yes, status=yes");
    printPage.document.open();
    printPage.document.writeln("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
    printPage.document.writeln("<html xmlns='http://www.w3.org/1999/xhtml'>");
    printPage.document.writeln("<head>");
    printPage.document.writeln("<title>打印本页</title>");
    printPage.document.writeln("<meta http-equiv=Content-Type' content='text/html; charset=utf-8' />");
    printPage.document.writeln("<link href='" + baseUrl + "css/reset.css' rel='stylesheet' type='text/css' />");
    printPage.document.writeln("<link href='" + baseUrl + "css/layout.css' rel='stylesheet' type='text/css' />");
    printPage.document.writeln("<style type='text/css'>*{color:#000;}</style>");
    printPage.document.writeln("</head>");
    printPage.document.writeln("<body style='width:710px; margin-left:30px;'>");
    printPage.document.writeln("<img src='" + baseUrl + "images/email_h_bg.jpg' style='width:710px;'>");
    printPage.document.writeln("<div style='height:10px'></div>");
    printPage.document.writeln("<div class='r_ope' style='width:710px;'><a id='printer' href='javascript:window.focus();window.print();window.close();'  class='print'>打印本页</a></div>");
    printPage.document.writeln("<h3 class='r_title' style='width:710px;'>" + prodName + "<span class='pro_num'>" + prodNo + "</span></h3>");
    printPage.document.write(body);
    printPage.document.write("<div style='clear:both;height:26px;'></div>");
    printPage.document.writeln("<img src='" + baseUrl + "images/email_f_bg.jpg' style='width:710px;'>");
    printPage.document.writeln("</body>");
    printPage.document.writeln("</html>");
    var isSafari = navigator.userAgent.search("Safari") > -1; //Google瀏覽器是用這核心 
    if (!isSafari) {
        printPage.location.reload();                          //不是 Google 瀏覽器才去reload page
    }
}