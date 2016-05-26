define('subitem', ['swapp', 'showerror','serverUtil'], function (swapp) {
    swapp.regmodule('subitem', ['showerror','serverUtil'], function (sumItem, $) {
        sumItem.controller('GetItemController', function ($scope, $location, $compile, $http, $window, showerror, serverUtil) {
            $scope.refresh = function () {

            }
            require(['domReady'],
                function (domReady) {


                var objid = $scope.objid || '';
                var exp = $scope.idexp || '';
                serverUtil.post('/itemview',
                    {
                        id: $scope.viewid,
                        objid: objid,
                        idxep: exp
                    }, function (data) {


                        var compilestr = '<table class="table">';
                        var i = 0;
                        for (i = 0; i < data.data.Data.SimpleData.length; i++) {

                            if (i % 2 == 0)
                                compilestr += "<tr>"
                            compilestr += "<td><strong>";
                            compilestr += data.data.Data.SimpleData[i].PrpShowName;
                            compilestr += "<strong></td><td>"
                            compilestr += data.data.Data.SimpleData[i].FmtValue;
                            compilestr += "</td>"
                            if (i % 2 == 1)
                                compilestr += "</tr>"
                        }
                        if (i % 2 == 1)
                            compilestr += "<td>&nbsp;</td><td>&nbsp;</td></tr>"

                        var r = i % 2 + i / 2;
                        while (r < 6) {
                            compilestr += "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                            r++;
                        }
                        compilestr += "</table><p class='pull-right'>更新时间<span ng-bind=\"querytime\"></span><a  taget='_self'>刷新</a><p></div>"


                        $('#' + $scope.tabid).append(compilestr);
                    });
            });
        });
    });

});