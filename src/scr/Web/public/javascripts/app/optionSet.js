define('optionSet',
    function () {
        var setOption = function (st, dst) {

                for(key in st){
                    
                  if(st[key] != undefined)
                  dst[key]=st[key];
                }

         
         

        };
        return {
            setOption:setOption
        }
    })