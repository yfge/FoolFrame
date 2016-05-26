/**
 * 
 * 
 */

define('navbar', ['swapp'], function (swapp) {
    swapp.regmodule('navbar', [], function (navbar) {

        navbar.controller('NavbarController', function ($http, $compile, $rootScope, $scope,navbar) {

            

            $scope.option ={};
            $rootScope.option = {};
           
            
            //前一页
            $scope.prepage = function () {
              
             
                $rootScope.option.currentPage --;
                $rootScope.navCallback($rootScope.option.currentPage);

            };
            //跳转到指定页
            $scope.navToPage = function (pageIndex) {
                $rootScope.option.currentPage=pageIndex;
                $rootScope.navCallback($rootScope.option.currentPage);


            }
            //下一页
            $scope.nextpage = function () {
               
                $rootScope.option.currentPage ++;
                $rootScope.navCallback($rootScope.option.currentPage);

            }

            $rootScope.updateInfo = function () {

                    $('#'+$scope.infoId).text('共' + $rootScope.option.total + '条记录');
                    $('#'+$scope.navId + ' li').remove();
                    var pcount = 0;;
                    if ($rootScope.option.total % $rootScope.option.pageSize > 0)
                        pcount = ($rootScope.option.total - $rootScope.option.total % $rootScope.option.pageSize) / 
                        $rootScope.option.pageSize + 1;
                    else
                        pcount = $rootScope.option.total / $rootScope.option.pageSize;


                    if ($rootScope.option.currentPage > 1) {

                        $('#'+$scope.navId).append(
                            $compile('<li><a href="#"  aria-label="Previous" ng-click="prepage()"> <span aria-hidden="true">&laquo;</span></a></li>')($scope));
                    } else {
                        $('#'+$scope.navId).append(
                     $compile('<li class="disabled" ><a href="#"   aria-label="Previous"  > <span aria-hidden="true">&laquo;</span></a></li>')($scope));
                    }
                    var pagestoshow = new Array();


                    if (pcount < 7)
                        for (i = 1; i <= pcount; i++) {
                            pagestoshow.push(i);
                        }
                    else if ($rootScope.option.currentPage < 4)
                        for (i = 1; i <= 7; i++) {
                            pagestoshow.push(i);
                        }
                    else if (pcount - $rootScope.option.currentPage < 4) {
                        for (i = 1; i <= 7; i++) {
                            pagestoshow.push(pcount - 7 + i);
                        }
                    } else for (i = 1; i <= 7; i++) {
                        pagestoshow.push($rootScope.option.currentPage - 4 + i);
                    }
                    for (i = 0; i < pagestoshow.length; i++) {
                        var html = '<li><a href="#" ng-click="navToPage(\'' + pagestoshow[i] + '\')">' + pagestoshow[i] + '</a></li>';
                        if (pagestoshow[i] == $rootScope.option.currentPage)
                            html = '<li class="active"><a href="#">' + pagestoshow[i] + ' <span class="sr-only">(current)</span></a></li> ';
                        var el = $compile(html)($scope);
                        $('#'+$scope.navId).append(el)
                        ;
                    }

                    if ($rootScope.option.currentPage < pcount) {

                        var el = $compile('<li><a href="#"  aria-label="Next" ng-click="nextpage()"> <span aria-hidden="true">&raquo;</span></a></li>')($scope);
                        $('#'+$scope.navId).append(el)
                        ;

                    } else {
                        var el = $compile('<li class="disabled"><a href="#"  aria-label="Next"  > <span aria-hidden="true">&raquo;</span></a></li>')($scope);
                        $('#'+$scope.navId).append(el)
                        ;
                    }

                }
            
        });

       
        navbar.provider('navbar',function(){
            this.$get = function ($rootScope) {
                var totalPage = 0;
                var currentPage = 0;
                var dataSize = 0;
                var navId = '';

                var service = {};
                var infoId = '';

                
       

                service.getTotalPage = function () {
                    return $rootScope.totalPage;
                }

                service.setOption = function (option) {
                    
                    
                    if ($rootScope.option == undefined) {
                        $rootScope.option = {};
                    }

                    require(['optionSet'],function(optionSet){
                        optionSet.setOption(option,$rootScope.option);
                    })

                }
                service.getOption = function () {
                    return $rootScope.option;

                }
               
                service.navToPage = function (callback) {
                   
                   $rootScope.navCallback = callback;

                }
                service.updateNavbar = function(option){
                    if ($rootScope.option == undefined) {
                        $rootScope.option = {};
                    }
                        require(['optionSet'], function (optionSet) {
                            optionSet.setOption(option, $rootScope.option);
                            if($rootScope.updateInfo!= undefined)
                            $rootScope.updateInfo();
                        });
                     
                    
                }
                return service;
                };
        });
    });
});
    
    