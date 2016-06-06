﻿ define('mapview', ['swapp','baidumaputil', 'showerror', 'serverUtil'], function (swapp) {

     swapp.regmodule('mapview', ['showerror', 'baidumaputil','serverUtil','timer'], function (mapview, $) {
                   // var maputil = require('baidumaputil');
                    mapview.controller('MapViewController',
             function ($scope, $location, $compile, $http, $window, $cookies, serverUtil, baidumaputil) { 
                 baidumaputil.initmap(function () {
                            var map = new window.BMap.Map($scope.mapid);
                            var point = new BMap.Point(116.32, 39.94917);
                            map.centerAndZoom(point, 18);
                            map.enableScrollWheelZoom();
                            serverUtil.post('/data/querylist',
                          {
                              viewid: parseInt(  $scope.viewid),
                              filter: $scope.filter,
                              page: $scope.page,
                              pagesize: $scope.size,
                              orderitem: $scope.orderitem,
                              ordertype: $scope.ordertype,
                              Token: $scope.token

                          }, function (data) {

                              console.log('getdata');

                         
                            
                              var points = [];
                              for (var i = 0; i < data.Data.length; i++) {
                                  var longitude, latitute;
                                  var info = [];
                                  var title;
                                  for (var j = 0; j < data.Data[i].Items.length; j++) {
                                    
                                      if (data.Data[i].Items[j].EditType == 16) {
                                          longitude = data.Data[i].Items[j].ObjId;
                                      }else

                                      if (data.Data[i].Items[j].EditType == 17) {
                                          latitute = data.Data[i].Items[j].ObjId;
                                      }else if (data.Data[i].Items[j].EditType == 18) {
                                          title = {
                                              label: data.Data[i].Items[j].PrpShowName,
                                              text: data.Data[i].Items[j].FmtValue
                                          };
                                      } else {
                                         
                                          info.push(
                                              {
                                                  label: data.Data[i].Items[j].PrpShowName,
                                                  text: data.Data[i].Items[j].FmtValue
                                              }
                                              );

                                      }


                                  }
                                  if (longitude != undefined && latitute != undefined) {
                                      var point = new BMap.Point(longitude, latitute);
                                      var marker = new BMap.Marker(point);

                                      if (title != undefined || info.length > 0) {
                                         

                                          addInfoWindow(title, info, marker);
                                          
                                      }
                                      points.push(point);
                                      map.addOverlay(marker);
                                      
                                     
                                  }
                              }
                             
                              map.setViewport(points);
                          });

                            var addInfoWindow = function (title, items, marker) {

                                var opts = {
                                    width: 240,     // 信息窗口宽度
                                    height: 100,     // 信息窗口高度
                                   
                                };
                                if (title != undefined) {
                                    opts.title = title.label + " " + title.text;

                                } else {
                                    opts.title = items[0].label + " " + items[0].text;
                                }
                                var msg = '';
                                for (var i = 0; i < items.length; i++) {
                
                                    if (i % 2==0)
                                        msg += '<p>';

                                    msg += items[i].label + ':' + items[i].text + ' ';
                                    if (i % 2 == 1)

                                        msg += '</p>';
                                }
                                var infoWindow = new BMap.InfoWindow(msg, opts);
                                marker.addEventListener("click", function (e) {

                                    var p = e.target;
                                    var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
                                    map.openInfoWindow(infoWindow, point); //开启信息窗口
                                });
                            }
                          


                        });
                    });

                });

            });

 
 