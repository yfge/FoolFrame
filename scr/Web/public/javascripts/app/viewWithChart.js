define('viewWithChart', ['swapp', 'querylistdata', 'domReady', 'echarts', 'navbar',
    'showerror', 'mkreport', 'timer', 'ngCookies', 'mkreport', 'serverUtil', 'swchartline'], function (swapp) {
        swapp.regmodule('viewWithChart',
            ['showerror', 'querylistdata', 'navbar', 'timer',
                'ngCookies', 'mkreport', 'serverUtil', 'swchartline'], function (
        viewWithChart,$
    ) {


        viewWithChart.controller('ViewWithChartController', function ($scope,
            querylistdata, swchartline) {

            $scope.isInitChart = false;

            querylistdata.getData(function (items) {
                console.log('get data haha !');
                if ($scope.isInitChart == false) {
                    $scope.isInitChart = true;
                    $($('#' + $scope.chartpan)).height($($('#' + $scope.tabpan)).height());

                    swchartline.initChart($scope.chartpan, function (chart) {
                        $scope.chart = chart;
                        $scope.chart.setOption(swchartline.getOption(items));
                      
 
                    });

                }else
                    $scope.chart.setOption(swchartline.getOption(items));
                if($scope.chart != undefined)
                $scope.chart.resize();
            });
            $scope.query = function () {
                querylistdata.query();
            }
        });
        
    });
});