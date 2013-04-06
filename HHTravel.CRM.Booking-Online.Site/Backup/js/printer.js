function printer(urlPDF) {
    var doc;
    $.ajax({ url: urlPDF,
        async: false,
        success: function (r) {
            doc = r;
        }
    });
    var win = openwin('', 760, 600);
    with (win.document) {
        open();
        write(doc);
        close();
    }
    $(win.document).find('head style').remove();
    $(win.document).find('.header_warp').attr('style', 'width:710px; margin-left:10px;');
    $(win.document).find('.pdf_header img').attr('style', 'width:710px;');
    $(win.document).find('.send_order_footer').attr('style', 'width:710px; clear:left; float:left;');
    $(win.document).find('.send_order_footer img').attr('style', 'width:710px;');
    $title = $(win.document).find('h3');
    $title.before("<div class='r_ope'><a id='printer' href='javascript:window.focus();window.print();window.close();'  class='print'>打印本页</a></div>");
}