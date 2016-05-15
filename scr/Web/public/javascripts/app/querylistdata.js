define('querylistdata', ['swapp', 'navbar','showerror', 'mkreport','timer', 'ngCookies','mkreport'], function (swapp) {
    swapp.regmodule('querylistdata', ['showerror', 'navbar','timer', 'ngCookies','mkreport'], function (
        query, $) {
        query.provider('querylistdata', function () {
            this.$get = function ($rootScope) {
                var viewid = '';
                var clickcallback;
                var editMsg = '';
                var tableid = '';
                var freshInterval = 0;
                var showop = false;
              
                var service = {

                    setview: function (id) {
                        viewid = id;

                    },
                    getview: function () {
                        return viewid;
                    },
                    getInterval: function () {
                        return freshInterval;
                    },
                    setIterval: function (interval) {
                        freshInterval = interval;
                    },
                    setclickcallback: function (callback) {
                        clickcallback = callback;
                    },
                    getclickcallback: function () {
                        return clickcallback;
                    },
                    setEditMsg: function (txt) {
                        editMsg = txt;
                    },
                    getEditMsg: function () {
                        return editMsg;
                    },
                    getTable: function () {
                        return tableid;
                    },
                    setTable: function (id) {
                        tableid = id;
                    },
                    getShowOp: function () {
                        return showop;
                    },
                    setShop: function (isshow) {
                        showop = isshow;
                    },
                    query: function () {
                        $rootScope.query();
                    },
                    getData: function (callback) {
                     
                        $rootScope.getData = callback;
                    }
                };

                return service;
            };

        });

        query.controller('QuerylistdataController',
            function ($scope, $location, $http, $window, $cookies, $compile, $interval, 
            showerror, querylistdata, timer, mkreport,navbar,$rootScope) {


                $scope.tableid = "";
                $scope.editMsg = "";
                $scope.operations = [];
                querylistdata.setclickcallback(function (viewid, dataid, dataitem) {
                    window.location.href = '../view' + viewid + '/' + dataid;
                });

              
                querylistdata.setEditMsg($scope.editMsg);
                $scope.querydata = function () {
                    $scope.querytime = '更新中..';
                    var viewid = $scope.viewid;
                    if (viewid ==
                        0 || viewid == "0" || viewid == undefined) {
                        viewid = querylistdata.getview();
                    }
                    if ($scope.editMsg != '' && $scope.editMsg != undefined) {
                        querylistdata.setEditMsg($scope.editMsg);
                    }

                    if (viewid != 0 && viewid != undefined)
                        $http.post('/data/querylist',
                          {
                              viewid: viewid,
                              filter: $scope.filter,
                              page: $scope.page,
                              pagesize: $scope.size,
                              orderitem: $scope.orderitem,
                              ordertype: $scope.ordertype,
                              Token: $scope.token

                          }).success(function (data, status, headers, config) {

                              
                              if (data.error != undefined) {
                                  showerror.showerrormsg(data.error);
                              }

                          
                              if ($('#' + $scope.tableid + ' tr th').length == 0) {
                                  var thstr = "<tr>";
                                  for (i = 0; i < data.Cols.length; i++) {
                                      thstr += "<th>" + data.Cols[i] + "</th>"
                                  }
                                  thstr += "</tr>";
                                  $('#' + $scope.tableid).append(thstr);
                              }

                              $('#' + $scope.tableid + ' tr:gt(0)').remove();


                              var colspan = 0;

                              for (i = 0; i < data.Data.length; i++) {

                                  var rowdata = 'id="tr-' + i + '" data-itemid="' + data.Data[i].Id + '">';
                                  var classstr = 'class = "';
                                  for (j = 0; j < data.Data[i].Items.length; j++) {
                                      if (data.Data[i].Items[j].EditType == 10)
                                      {
                                          classstr = classstr + data.Data[i].Items[j].FmtValue + ' ';
                                      }else
                                        rowdata = rowdata + '<td  data-propertyvalue="' + data.Data[i].Items[j].ObjId + '"   data-property="' + data.Data[i].Items[j].PrpId + '">' + data.Data[i].Items[j].FmtValue + "</td>"
                                  }
                                  classstr = classstr + '"';
                                  rowdata = '<tr ' + classstr +rowdata + '<td ><div class="btn-group btn-group-sm">';
                                  if (querylistdata.getEditMsg() != '') {

                                      rowdata = rowdata + '<span><a class="btn btn-default" ng-click="setselect(\'' + $scope.detailId + '\',\'' + data.Data[i].Id + '\', ' + i + ')">' + querylistdata.getEditMsg() + '</a></span>';
                                  }


                                  if (querylistdata.getShowOp) {
                                      if ($scope.operations != undefined) {
                                          for (j = 0; j < $scope.operations.length; j++) {
                                              if ($scope.operations[j].RequireSelect) {
                                                  if ($scope.operations[j].ViewId > 0)
                                                      rowdata = rowdata + '<span class="btn"><a  ng-click="setselect(\'' + $scope.operations[j].ViewId + '\',\'' + data.Data[i].Id + '\', ' + i + ')">' + $scope.operations[j].Name + '</a></span>';
                                                  else
                                                      rowdata = rowdata + '<span class="btn"><a>' + $scope.operations[j].Name + '</a></span>';
                                              }
                                          }
                                      }
                                  }
                                  rowdata = rowdata + '</div>&nbsp;</td>';
                                  rowdata = rowdata + '</tr>';
                                  $('#' + $scope.tableid).append($compile(rowdata)($scope));
                                  colspan = data.Data[i].Items.length + 1;
                              }
                              var dataStr = data.FreshTime;
                              $scope.querytime = eval('new ' + dataStr.substr(1, dataStr.length - 2)).toLocaleString();// data.FreshTime;
                              while (i < $scope.size) {
                                  var rowdata = '<tr><td colspan="' + colspan + '">&nbsp;</td></tr>';
                                  $('#' + $scope.tableid).append(rowdata);
                                  i++;

                              }
                              if ($rootScope.getData != undefined) {
                                  $rootScope.getData(data.Data);
                              }
                              navbar.updateNavbar({
                                  total:data.TotalItem,
                                  pageSize:$scope.size,
                                  currentPage:$scope.page
                              });
                              $scope.interval = data.AutoFreshTime;
                          });
                };
                $rootScope.query = $scope.querydata;
                $scope.query = function () {

                    if ($("#" + $scope.tableid).is(":visible") == true) {
                        $scope.page = 1;
                        $scope.querydata();
                    }

                };
                $scope.setselect = function (viewid, dataid, rowid) {
                    var dataItem = {};
                    dataItem.ItemId = dataid;
                    dataItem.Propertyies = [];
                    $('#' + $scope.tableid + ' tbody #tr-' + rowid + ' td[data-property]').each(function () {
                        dataItem.Propertyies.push({
                            FmtValue: $(this).text(),
                            Value: $(this).attr("data-propertyvalue"),
                            Key: $(this).attr("data-property")
                        });
                    })
                    querylistdata.getclickcallback()(viewid, dataid, dataItem);
                };
                $scope.addop = function (ID, RequireSelect, ViewId, Name) {
                    $scope.operations.push({
                        ID: ID,
                        RequireSelect: RequireSelect,
                        ViewId: ViewId,
                        Name: Name
                    });
                }
                $scope.$watch('interval', function (oldval, newval) {

                    timer.unreg($scope.query);
                    if ($scope.interval > 0) {
                        timer.reg($scope.query, $scope.interval);
                    }

                });
                $scope.$watch('viewid', function (oldval, newval) {
                    navbar.setOption({
                        pageSize:$scope.size
                    });
                    navbar.navToPage(
                        function (pageIndex) {
                            $scope.page = pageIndex;
                            $scope.querydata();
                            
                        }
                    );
                    $scope.query();

                });

                $scope.mkquery = function () {
                    var viewid = $scope.viewid;
                    
                    if (viewid ==
                        0 || viewid == "0" || viewid == undefined) {
                        viewid = querylistdata.getview();
                    }

                    mkreport.initquery(viewid);

                }

          


            });
    });
});