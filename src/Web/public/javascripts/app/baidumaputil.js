var Maputil = {};

Maputil.init2=function(){
   
    Maputil.loadmap = true;
    if (Maputil.callback != undefined) {
        Maputil.callback();
    }
}
Maputil.loadmap = false;
define('baidumaputil', ['swapp'], function (swapp) {

    var initmap = function (callback) {

        if (Maputil.loadmap != true) {
            Maputil.callback = callback;
            require(['domReady'], function (domReady) {
                domReady(function () {
                    var script = document.createElement("script");
                    script.type = "text/javascript";
                    script.src = "http://api.map.baidu.com/api?v=2.0&ak=d93uy5PGxv8udM93irHVF9GY&callback=Maputil.init2";
                    document.body.appendChild(script);
                });
            });
        } else {
            callback();
        }

    }
    swapp.regmodule('baidumaputil', [], function (baidumaputil, $) {

        baidumaputil.provider('baidumaputil', function () {
            this.$get = function() {
                
                return {
                    initmap: initmap
                
                }
            }
        });

    });
    return {
        initmap: initmap
    }
   
});