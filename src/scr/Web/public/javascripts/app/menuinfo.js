define('menu', ['swapp', 'showerror'], function (swapp) {
    swapp.regmodule('menu', ['showerror','ngRoute', 'ngCookies'], function (menuinfo, $) {
        menuinfo.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider, showerror) {

            //..省略代码
            $locationProvider.html5Mode({
                enabled: true,
                requireBase: false
            });
        }]);
        menuinfo.controller('MenuController',
            function ($scope, $location, $http, $window, $cookies, showerror) {
            
                $scope.getSub = function (authCode, panelid,collapseid,viewid) {

                 
                    if (viewid != '0') {
                        window.location.href = '../view' + viewid;
                    } else {
                        $http.post(
                            '/user/getmenu',
                            {
                                authcode: authCode
                            }).success(function (data, status, headers, config) {

                                if (data.error != null) {

                                    showerror.showmsg(data.error.Code, data.error.Message, null, '');

                                } else {
                                    var count = data.length;
                                   $('#' + panelid + ' .list-group-item').remove();

                                    for (i = 0; i < data.length; i++) {
                                        var linkText = data[i].Text;
                                        if (data[i].ViewId != 0)
                                            linkText = "<a href='/view" + data[i].ViewId + "' target='_self'>" + linkText + "</a>";
                                        if (data[i].ImageUrl != null &&
                                            data[i].ImageUrl != undefined &&
                                            data[i].ImageUrl != '') {
                                            $('#' + panelid).append("<li class='list-group-item'> <img class ='sw-menuimg' src='" + data[i].ImageUrl + "'/>" + linkText + "</li>");
                                        } else {
                                            $('#' + panelid).append(
                                     "<li class='list-group-item'>" + linkText + "</li>");
                                        }

                                    }
                                }
                            });
                    }
                };
                $scope.logout = function () {
             
                    $http.post('/user/logout', {}).success(function (data, status, headers, config) {
                        $location.path('').replace();
                        $window.$location = $location;
                        $window.location.reload();
                    });
                }
            });
    });
});
