define('Sudoku', ['swapp', 'showerror'], function (swapp) {
    swapp.regmodule('Sudoku', [], function (sumItem, $) {
 
        
       

        require(['domReady'],
                         function (domReady) {
                             domReady(function () {

                                 var maxheight = 0;
                                 var ctrls = $('.sw-flowcontrol');
                                 for (var i = 0; i < ctrls.length; i++) {
                                     maxheight = Math.max(maxheight, $(ctrls[i]).height());
                                 }

                                 for (var i = 0; i < ctrls.length; i++) {
                                     $(ctrls[i]).height(maxheight);
                                 }
                            
                             });
                         });
    });
});