define('serverUtil', ['swapp', 'showerror', 'timer'], function (swapp) {
    swapp.regmodule('serverUtil', ['showerror', 'timer'], function (serverUtil, $) {

        serverUtil.provider('serverUtil', function () {
            this.$get = function (showerror, timer, $http) {
                var postCommunitcation = function (posturl, postdata, callback) {
                    $http.post(posturl, postdata).success(
                        function (data, status, headers, config) {
                            if (data.error != undefined) {
                                showerror.showerrormsg(data.error);
                            } else {
                                callback(data);
                            }
                        });
                }
                return {
                    post: postCommunitcation
                };
            }
        });

    });
});