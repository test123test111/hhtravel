var Calendar = {
    _url: "",
    _onclick: null,
    _year: 2012,
    _month: 10,
    _columns: 7,

    getCalendarModel: function (year, month) {
        var r;
        $.ajax({
            url: this._url + "&year=" + year + "&month=" + month,
            async: false,
            cache: false,
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
        var calendarModel = this.getCalendarModel(year, month);
        if (!calendarModel) return;
        var monthModel = calendarModel.MonthModel;
        if (!monthModel) return;

        var today = new Date();
        if (year < calendarModel.BeginYear
            || (year == calendarModel.BeginYear && month <= calendarModel.BeginMonth)) {
            $('a.calendar_l').hide();
        } else {
            $('a.calendar_l').show();
        }

        if (monthModel.Year > calendarModel.EndYear
            || (monthModel.Year == calendarModel.EndYear && monthModel.Month >= calendarModel.EndMonth)) {
            $('a.calendar_r').hide();
        } else {
            $('a.calendar_r').show();
        }

        // UI初始化
        $tbl = $('.tbl_calendar').empty();
        $('.calendar_month>h5').text(monthModel.Year + "年" + monthModel.Month + "月");
        this._year = monthModel.Year;
        this._month = monthModel.Month;

        // draw
        $tbl.append('<tr class="tbl_header"><td>日</td><td>一</td><td>二</td><td>三</td><td>四</td><td>五</td><td>六</td></tr>');

        var rows = monthModel.Days.length / this._columns;
        for (var i = 0; i < rows; i++) {
            $tr = $('<tr/>');
            for (var j = 0; j < this._columns; j++) {
                var index = this._columns * i + j;
                var dayModel = monthModel.Days[index];

                $td = $('<td>' + dayModel.Day + '</td>');

                if (!dayModel.IsSetOff && !dayModel.Price) {
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

                    if (dayModel.IsSetOff) {
                        $td.append('<p class="calendar_price">额满</p>');
                    }
                    else {
                        if (dayModel.MinGroupNum > 0) {
                            $td.append('<p class="calendar_people">最小成团：' + dayModel.MinGroupNum + '人</p>');
                        }
                        $td.append('<p class="calendar_price">RMB ' + formatToCurrency(dayModel.Price) + '起</p>');
                        // hold住
                        $td.attr('dateString', dayModel.DateString);
                        $td.click(function () {
                            Calendar._onclick($(this).attr('dateString'));
                        });
                    }
                }
                $tr.append($td);
            }
            $tbl.append($tr);
        }
    }
};