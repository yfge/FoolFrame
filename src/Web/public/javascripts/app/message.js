define('message', ['swapp', 'timer', 'showerror'], function (swapp) {
    swapp.regmodule('message', ['showerror', 'timer'], function (message, $) {
        Date.prototype.format = function (format) //author: meizz
        {
            var o = {
                "M+": this.getMonth() + 1, //month
                "d+": this.getDate(),    //day
                "h+": this.getHours(),   //hour
                "m+": this.getMinutes(), //minute
                "s+": this.getSeconds(), //second
                "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
                "S": this.getMilliseconds() //millisecond
            }
            if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
            (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o) if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1,
              RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
            return format;
        }
        message.controller('MessageController', function (showerror, timer, $http) {
            timer.reg(function (
                ) {
                $http.post('getmsg', {}).success(function (data, status, headers, config) {
                    console.log('get msg');
                  
                    if (data.length > 0) {
                        var re = /-?\d+/;
                        var m = re.exec(data[0].GernerationTime);
                        var d = new Date(parseInt(m[0]));
                        var fmtdate= d.format("yyyy-MM-dd hh:mm:ss");
                        showerror.showevtmsg(fmtdate, data[0].MessageContent, data[0].ResultView, data[0].ResultKey);
                    }

                });

            }, 15);
        })
    });
});