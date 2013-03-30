
var Calendar = {
    _url: "",
    _onclick: null,
    _year: 2012,
    _month: 8,
    _columns: 7,

    getMonthModel: function (year, month) {
        var r;
        $.ajax({
            url: this._url + "&year=" + year + "&month=" + month,
            async: false,
            cache: true,
            dataType: "json",
            success: function (data) {
                r = data;
            }
        });
        return r;
    },

    prevMonth: function () {
        var year = (this._month == 1) ? this._year - 1 : this._year;
        var month = (this._month == 1) ? 12 : this._month - 1;
        this.draw(year, month);
    },
    nextMonth: function () {
        var year = (this._month == 12) ? this._year + 1 : this._year;
        var month = (this._month == 12) ? 1 : this._month + 1;
        this.draw(year, month);
    },

    draw: function (year, month) {
        var monthModel = this.getMonthModel(year, month);
        if (!monthModel) {
            return;
        }
        // UI初始化
        $tbl = $('.tbl_calendar').empty();
        $('.calendar_month>h5').text(year + "年" + month + "月");
        this._year = year;
        this._month = month;

        // draw
        $tbl.append('<tr class="tbl_header"><td>日</td><td>一</td><td>二</td><td>三</td><td>四</td><td>五</td><td>六</td></tr>');

        var rows = monthModel.Days.length / this._columns;
        for (var i = 0; i < rows; i++) {
            $tr = $('<tr/>');
            for (var j = 0; j < this._columns; j++) {
                var index = this._columns * i + j;
                var dayModel = monthModel.Days[index];

                $td = $('<td>' + dayModel.Day + '</td>');

                if (!dayModel.Price) {
                    if (dayModel.BelongsOtherMonth) {
                        $td.addClass('other_month');
                    }
                }
                else {
                    if (dayModel.BelongsOtherMonth) {
                        $td.addClass('other_month_depar');
                    }
                    else {
                        $td.addClass('depar_date');
                    }
                    if (dayModel.MinPersonNum > 0) {
                        $td.append('<p class="calendar_people">最低成行：' + dayModel.MinPersonNum + '人</p>');
                    }
                    $td.append('<p class="calendar_price">RMB ' + dayModel.Price + '起</p>');
                    // hold住
                    $td.attr('dateString', dayModel.DateString);
                    $td.click(function () {
                        Calendar._onclick($(this).attr('dateString'));
                    });
                }
                $tr.append($td);
            }
            $tbl.append($tr);
        }
    }
};
