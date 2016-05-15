define('timer', ['swapp'], function (swapp) {
    swapp.regmodule('timer', [], function (timer, $) {
        timer.provider('timer', function () {
            this.$get = function ($interval) {


                var lisener = [];
                $interval(function () {

               
                    lisener.forEach(function (item) {
                        if (item.time == item.interval && item.interval != 0) {
                            item.callback();
                            item.time = 0;
                        }
                        item.time++;
                    })
                }, 1000);
         
                var service = {

                    reg: function (callback, interval) {

                        lisener.push({
                            callback: callback,
                            interval: interval,
                            time: 0
                        });
                    },
                    unreg: function (callback) {
                   
                        for (i = 0; i < lisener.length; i++) {
                            if (lisener[i].callback == callback) {
                                lisener.splice(i, 1);
                            }
                        }
                    },
                    get: function () {
                        return lisener;
                    }
                };

                return service;
            };

        });
    });
}
);