define('operation', ['swapp', 'showerror'], function (swapp) {
    swapp.regmodule('operation', ['showerror', 'ngRoute', 'ngCookies'], function (operation, $) {
        operation.provider('operation', function () {
            this.$get = function ($http, showerror) {

                var service = {


                    runoperation: function (viewid, objid, operationid) {

                        $http.post(
                            '/data/exoperation',
                            {
                                objid: objid,
                                viewid: viewid,
                                opid: operationid
                            }).success(function (data, status, headers, config) {

                                if (data.Error==undefined)
                                    showerror.showdetailinfo(data.IsSuccess, '执行结果', data.ReturnMsg);
                                else
                                    showerror.showdetailinfo(data.IsSuccess, '执行结果', data.Error.Message);
                            });
                    }

                };

                return service;
            };

        });
    });
});