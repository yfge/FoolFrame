define('groupview', ['swapp', 'showerror', 'ngCookies'],
function (swapp) {
    swapp.regmodule('groupview', ['ngCookies', 'showerror'], function (groupview, $) {
        groupview.controller('GroupViewController',
                    function ($scope, $location, $compile, $http, $window, $cookies, showerror) {
                        require(['domReady'], function (domReady) {
                            domReady(function () {
                                $http.post('/view', { id: $scope.viewid }).success(function (data, status, headers, config) {
                                    if (data.error != undefined) {
                                        showerror.showerrormsg(data.error);
                                    }

                                    i = 0;
                                    while(i<data.view.data.Items.length) {
                                        var subview = data.view.data.Items[i].ListViewId;
                                        var viewType = data.view.data.  Items[i].ListViewType;
                                        var tabid = $scope.tabid + '-tab-' + data.view.data.Items[i].Name + '-' + data.view.data.Items[i].ListViewId;
                                        var panelid = $scope.tabid + '-tabpanel-' + data.view.data.Items[i].Name + '-' + data.view.data.Items[i].ListViewId


                                        var liStr = "<li role='presentation' ><a href='#" + tabid + "' taget='_self' data-toggle='tab' >" + data.view.data.Items[i].Name + "</a></li>";
                                        if (i == 0) {
                                            liStr = "<li role='presentation'  class='active'><a href='#" + tabid + "' taget='_self' data-toggle='tab'>" + data.view.data.Items[i].Name + "</a></li>";
                                        }
                                        $('#' + $scope.navid).append($(liStr));
                                      
                                        

                                        if (viewType == 0) {

                                            var tabStr = "<div  id ='" + tabid + "'  role='tab-pane' class = 'tab-pane ";
                                            if (i == 0)
                                                tabStr += " active";
                                            tabStr += "'>";
                                            tabStr += "<div class='panel' id='" + panelid + "' ";


                                          
                                            var control = $scope.tabid + '-listtable-' + data.view.data.Items[i].Name + '-' + data.view.data.Items[i].ListViewId;

                                            tabStr += 'ng-controller="QuerylistdataController" ';
                                            tabStr += 'ng-init="';
                                            tabStr += "viewid='" + subview + "';tableid='" + control + "';interval=5; page='1' ;size='5';count='0';pages='0';orderitem='0';ordertype='0';filter=''";
                                            tabStr += '"';
                                            tabStr += "><table class = 'table table-hover' id='" + control + "' ></table> \
                                            <p class='pull-right'><a class='pull-left'>详细</a>\
                                            <span  >更新时间<span ng-bind=\"querytime\"></span>\
                                            <a    taget='_self' ng-click=\"query()\">刷新</a></span></p></div>";
                                            tabStr += '</div>';
                                            $('#' + $scope.tabid).append($compile(tabStr)($scope));
                                        } else if (viewType == 1) {
                                            var tabStr = "<div  id ='" + tabid + "'  role='tab-pane' class = 'tab-pane ";
                                            if (i == 0)
                                                tabStr += " active";
                                            tabStr += "'>";
                                            tabStr += "<div class='panel' id='" + panelid + "'></div>";


                                            tabStr += '</div>';
                                            $('#' + $scope.tabid).append(tabStr);
                                            $('#' + panelid).append('这是简单项');

                                        }



                                        i++;

                                    }


                                });

                            });
                        });

                    });
    });
});