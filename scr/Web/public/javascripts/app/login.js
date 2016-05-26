define('login', ['swapp', 'showerror'], function (swapp) {
    swapp.regmodule('login', ['showerror', 'ngRoute', 'ngCookies'], function (login, $) {
       // var login = angular.module('login', ['showerror', ]);
        login.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

            //..省略代码
            $locationProvider.html5Mode({
                enabled: true,
                requireBase: false
            });
        }]);
        login.controller('LoginController', function ($scope, $location, $http, $window, showerror,
            $cookies) {
            $scope.pwdinptid = "#password";
            $scope.hello = function () {
                var name = $scope.username;
                var pwd = $($scope.pwdinptid).val();
                var chkcode = $scope.chkcode;
                var chkkey = $scope.chkkey;
                var dbid = $scope.dbid;


                $http.post(
                    'user/login',
                    {
                        name: name,
                        pwd: pwd,
                        chkid: chkkey,
                        chk: chkcode,
                        dbid: dbid,

                    }).success(function (data, status, headers, config) {
                        if (data.IsLogin) {
                            $cookies.put('token', data.Token, { domain: $location.host() });
                            $location.path('/main').replace();
                            $window.$location = $location;
                            $window.location.reload();

                        } else {

                            showerror.showmsg(data.Error.Code, data.Error.Message, $scope.refresh);

                        }
                    }).error(function (data, status, headers, config) {

                    });

                $(this).ready(function () {
                    $('#flash').coinslider({ hoverPause: false, width: 565, height: 565 });
                });

            }
            $scope.refresh = function () {
                $http.post('/user/getchk', {})
                    .success(function (data, status, headers, config) {
                        $scope.chkkey = data.chkkey;

                        $scope.chkimg = chkimg;
                        $('#chkimg img').remove();
                        $('#chkimg').append('<image src="data:image/gif;base64,' + data.chkimg + '" />');

                    });
            }
            $scope.showerror = function () {
                $('#modal-dialog').on('hidden.bs.modal', function (e) {
                    $scope.refresh();
                })
                showerror.showmsg();
                //  $('#modal-dialog').modal().on();

            }

        });

    });
}
);