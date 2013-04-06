// 下載《繁體版》的PDF
/*
Web 分享功能："PDF下載"：
(1)《繁體版》： http://www.hhtravel.com/ezTopTravel/convertPDF/prodInfoPDF.jsp?prod_no=FRN0000008813&local=
(2)《簡體版》： http://www.hhtravel.com/ezTopTravel/convertPDF/prodInfoPDF.jsp?prod_no=FRN0000009162&local=cn
*/
function downloadPDF(basePath, local) {
    $(document).ready(function () {
        var pdfURL = basePath + "?local=" + $.trim(local); // (ex.空值:繁體版、cn:簡體版)

        $.ajax({
            url: pdfURL,
            type: "GET",
            dataType: "html",
            beforeSend: function () {
                $(".pdf_loading").slideToggle(500);
            },
            success: function () {
                $("#PDFDownload_ifm").attr("src", pdfURL);
            },
            error: function () {
            }, complete: function () {
                $(".pdf_loading").slideToggle(500);
            }
        }); // end $.ajax({
    });  // end $(document).ready(function(){
} // end function downloadPDF(){