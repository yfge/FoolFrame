define('swapp',
    ["angular", "jquery"],
    function (angular, $) {
        Array.prototype.indexOf = function (val) {
            for (var i = 0; i < this.length; i++) {
                if (this[i] == val) return i;
            }
            return -1;
        }
        Array.prototype.removeAt = function (index) {
            this.splice(index, 1);
        }
        Array.prototype.remove = function (obj) {
            var index = this.indexOf(obj);
            if (index >= 0) {
                this.removeAt(index);
            }
        }
        //Store modules of require.
        var jsModules = new Array();
        //Store modules of angular .
        var angularModules = new Array();
        function Confirm(val) {
            if (jsModules.indexOf(val) > -1)
                return;
            else
                jsModules.push(val);

        }
         

        function ConfirmAngular(val) {
            Confirm(val);
            if (angularModules.indexOf(val) > -1)
                return;
            else
                angularModules.push(val);
        }
        Confirm("angular");
        Confirm("swapp");
        return {
            regmodule: function (modulename, deps, callback) {

                ConfirmAngular(modulename);
                var module = angular.module(modulename, deps);
                callback(module, $);

            },
            angularmodules: angularModules,
            reqmodules: jsModules,
            runapp: function (modulename) {
                Confirm(modulename);
                requirejs(jsModules, function (angular) {
                    angular.bootstrap(document, angularModules);
                }
                    );
            }
        }
    }
);
