define('swchartline', ['swapp', 'echarts', 'showerror', 'serverUtil', 'timer'], function (swapp) {
    swapp.regmodule('swchartline', ['showerror', 'serverUtil', 'timer'],
        function (swchartline, $) {

        swchartline.provider('swchartline', function () {


            this.$get = function () {

                var getchartoption = function (datas) {
                    var option = {
                        xAxis: {
                            type: 'category',
                            boundaryGap: false,
                    
                            nameLocation: 'end',
                            data: []
                        },
                        grid: {
                            left: 'left',
                            bottom: '25',
                            right: '20%'
                        },
                        legend: {
                            left: 'right',
                            top: 'middle',
                            data: []
                        },
                        title: {
                            show: true,
                            text:' '
                        },
                        tooltip: {
                            trigger: 'axis'
                        },
                        yAxis: {
                            boundaryGap: [0, '50%'],
                            type: 'value'
                        },
                        series: [

                        ]
                    };
                    for (var dataIndex = 0; dataIndex < datas.length; dataIndex++) {
                        var items = datas[dataIndex].Items;
                        if (dataIndex == 0) {
                            for (var i = 0; i < items.length; i++) {
                                if (items[i].EditType == 11) {
                                    option.xAxis.name = items[i].PrpShowName;
                                    option.xAxis.data = [];
                                } else if (items[i].EditType == 12) {
                                    //折线图
                                    option.legend.data.push(items[i].PrpShowName);
                                    option.series.push({
                                        'name': items[i].PrpShowName,
                                        "type": "line",
                                        'smooth': true,
                                        'symbol': 'none',
                                        'stack': items[i].PrpShowName,
                                        'areaStyle': {
                                            normal: {}
                                        },
                                        label: {
                                            normal: { show: true }
                                        },
                                        data: []
                                    });
                                } else if (
                                    items[i].EditType == 13) {
                                    //柱状图
                                    option.legend.data.push(items[i].PrpShowName);
                                    option.series.push({
                                        'name': items[i].PrpShowName,
                                        "type": "bar",
                                        'stack': items[i].PrpShowName,
                                        barMaxWidth:15,
                                        label: {
                                            normal: { show: true }
                                        },
                                        data:[]
                                    });
                                } else if (
                                    items[i].EditType == 14) {
                                    option.legend.data.push(items[i].PrpShowName);
                                    option.series.push({
                                        'name': items[i].PrpShowName,
                                        "type": "scatter",
                                        'symbol': 'circle',

                                        'stack': items[i].PrpShowName,
                                        label: {
                                            normal: { show: true }
                                        },
                                        data: []
                                    });

                                }
                            }
                        }
                        var j = 0;
                        for (var i = 0; i < items.length; i++) {
                            if (items[i].EditType == 11) {
                                option.xAxis.data.push(items[i].FmtValue);

                            } else if (items[i].EditType == 12

                                || items[i].EditType == 13
                                || items[i].EditType == 14) {
                                option.series[j].data.push(items[i].FmtValue);

                                j++;

                            }

                        }

                    }
                    return option;

                }

                var initChart = function (chartId, callback) {
                    require(['domReady'], function (domReady) {
                        domReady(function () {
                            var ec = require('echarts');
                            var myChart = ec.init(document.getElementById(chartId));
                            if (callback != undefined) {
                                callback(myChart);
                            }
                        });
                    });
                }
                var service = {};
                service.getOption = getchartoption;
                service.initChart = initChart;
                
                return service;
            }

        });
        swchartline.controller('LineChartController',
            function ($scope, $http, serverUtil, timer, swchartline) {

              
                 
                $scope.chart;
                $scope.interval = 0;


                $scope.chartData = {};
                $scope.chartData.xAxis = [];
                $scope.chartData.series = [];
                $scope.objid = '';
                $scope.exp = '';

                function addData(shift,item) {
                  
                    $scope.chartData.xAxis.push(item.xAxis);
                    for(var i = 0;i<item.series.length;i++){
                        if($scope.chartData.series.length>i){
                            $scope.chartData.series[i].push(item.series[i]);
                        }
                    }
                    if (shift && $scope.chartData.xAxis.length > 100) {
                        $scope.chartData.xAxis.shift();
                        for (var i = 0; i < $scope.chartData.series.length;i++)
                            $scope.chartData.series[i].shift();
                    }
                }
                var updateChart = function () {
                    var opt = {
                        xAxis: {
                            data: $scope.chartData.xAxis
                        },
                        series: []
                    };
                    for (var i = 0; i < $scope.chartData.series.length; i++) {
                        opt.series.push({
                            data: $scope.chartData.series[i]
                        });
                    }
                    $scope.chart.setOption(opt);
                }
                $scope.updateData = function(){
                    serverUtil.post('/itemview',
                            {
                                id: $scope.viewid,
                                objid:$scope. objid,
                                idxep:$scope. exp
                            }, function (serverdata) {
                                if ($scope.IsInitChart == false) {
                                    $scope.IsInitChart = true;

                                    option = {
                                        xAxis: {
                                            type: 'category',
                                            boundaryGap: false,
                                            data: $scope.xData,
                                            nameLocation:'end'
                                        },
                                        grid: {
                                            left: 'left',
                                            bottom: '25',
                                            right: '20%'
                                        },
                                        legend: {
                                            left: 'right',
                                            top: 'middle',
                                            data: []
                                        },
                                        title: {
                                            show: true,
                                            text: $scope.chartname
                                        },
                                        tooltip: {
                                            trigger: 'axis'
                                        },
                                        yAxis: {
                                            boundaryGap: [0, '50%'],
                                            type: 'value'
                                        },
                                        series: [
                                           
                                        ]
                                    };
                                

                                    var ob = option["series"];
                                    var ob2 = option.series;

                               

                                    for (var i = 0; i < serverdata.data.Data.SimpleData.length; i++) {
                                        if (serverdata.data.Data.SimpleData[i].EditType == 11) {
                                            option.xAxis.name = serverdata.data.Data.SimpleData[i].PrpShowName;
                                            option.xAxis.data = getEmptyArray('', 100);
                                           
                                            $scope.chartData.xAxis = getEmptyArray('', 100);
                                        } else if (serverdata.data.Data.SimpleData[i].EditType == 12) {
                                            //折线图
                                            option.legend.data.push(serverdata.data.Data.SimpleData[i].PrpShowName);
                                            option.series.push({
                                                'name': serverdata.data.Data.SimpleData[i].PrpShowName,
                                                "type": "line",
                                                'smooth': true,
                                                'symbol': 'none',
                                                'stack': 'a',
                                                'areaStyle': {
                                                    normal: {}
                                                },
                                                label: {
                                                    normal: { show: true }
                                                },
                                                'data': getEmptyArray(0, 100)
                                            });
                                            $scope.chartData.series.push(getEmptyArray(0,100));
                                        }else if (
                                            serverdata.data.Data.SimpleData[i].EditType == 13) {
                                            //柱状图
                                            option.legend.data.push(serverdata.data.Data.SimpleData[i].PrpShowName);
                                            option.series.push({
                                                'name': serverdata.data.Data.SimpleData[i].PrpShowName,
                                                "type": "bar",
                                                'stack': 'a',                                               
                                                label: {
                                                    normal: { show: true }
                                                },
                                                'data': getEmptyArray(0, 100)
                                            });
                                            $scope.chartData.series.push(getEmptyArray(0, 100));
                                        } else if (
                                            serverdata.data.Data.SimpleData[i].EditType == 14) {
                                            option.legend.data.push(serverdata.data.Data.SimpleData[i].PrpShowName);
                                            option.series.push({
                                                'name': serverdata.data.Data.SimpleData[i].PrpShowName,
                                                "type": "scatter",
                                                'symbol': 'circle',
                                                label: {
                                                    normal: { show: true }
                                                },
                                                'data': getEmptyArray(0, 100)
                                            });
                                            $scope.chartData.series.push(getEmptyArray(0, 100));
                                        }
                                    }
                                    $scope.chart.setOption(option);
                                }
                                var itemToAdd = {};
                                itemToAdd.series = [];
                                for(var i = 0;i<serverdata.data.Data.SimpleData.length;i++){
                                    if (serverdata.data.Data.SimpleData[i].EditType == 11) {
                                        itemToAdd.xAxis = serverdata.data.Data.SimpleData[i].FmtValue;

                                    } else if (serverdata.data.Data.SimpleData[i].EditType == 12

                                        || serverdata.data.Data.SimpleData[i].EditType == 13
                                        || serverdata.data.Data.SimpleData[i].EditType == 14) {
                                        itemToAdd.series.push(serverdata.data.Data.SimpleData[i].FmtValue);

                                    }  

                                }
                                addData(true, itemToAdd);
                                updateChart();

                                $scope.interval = serverdata.data.AutoFreshTime;

                            });
                }
             
                 
                var getEmptyArray =function(val,length){
                    var items = [];
                    for (var i = 0; i < length; i++)
                        items.push(val);
                    return items;
                }
                require(['domReady'], function (domReady) {
                    domReady(function () { 
                        var ec = require('echarts');

                        var myChart = ec.init(document.getElementById($scope.chartid));
                        $scope.chart = myChart;
                      

 


                        $scope.IsInitChart = false;
                        $scope.updateData();
                        
                        $scope.$watch('interval', function (oldval, newval) {

                            timer.unreg($scope.updateData);
                            if ($scope.interval > 0) {
                                timer.reg($scope.updateData, $scope.interval);
                            }

                        });
                     
                        window.onresize = function () {
                            myChart.resize();
                        }
                    }
                    );
                }
                );
            });


      
    });

});