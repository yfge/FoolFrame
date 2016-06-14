
/*
 * GET home page.
 */
 
var express = require('express');
var app = express();
var soway = require('../cloud-social/index.js');
//var  cookieparser = require('cookie-parser');
var dict = require('dict');
var serverDic = dict();
var colors = require('colors2');

exports.checkauth = function (req, res, next){
    var ip = req.headers['x-forwarded-for'] || 
     req.connection.remoteAddress || 
     req.socket.remoteAddress ||
     req.connection.socket.remoteAddressk;
    console.log('check auth req.method '.green + req.method .green +ip );
    if (res.cookie[req.hostname + '_token'] != undefined) {
        console.log('check success ,token:' + res.cookie[req.hostname + '_token']+'host:'+ req.hostname.green + ' remote address:'+ req.connection.remoteAddress.green);        
        next();
    } else {
        if (req.method == 'POST') {
            res.send({ error: { Code: 10009, Message: "服务器要求重新登录" } });
          
        }
        else if (req.method == 'GET') {
            res.clearCookie(req.hostname + '_token');
            res.redirect('/');
        }
    }

}
 
exports.index = function (req, res,next) {
    if (res.cookie[req.hostname + '_token'] != undefined) {
        console.log('token:' + res.cookie[req.hostname + '_token']);
        soway.getmain(res.cookie[req.hostname + '_token'],
            function (maindata) {
            
            if (maindata.Error == undefined) {
                if (maindata.App.DefaultViewId != 0 && maindata.App.DefaultViewId != undefined) {
                    soway.getlistview(
                        res.cookie[req.hostname + '_token'],
                         maindata.App.DefaultViewId, function (data) {
                            console.log('get main success');
                            if (data.data.TempFile != '')
                                res.render(data.data.TempFile, { data: maindata, view: data , app: data.data.TempFile });
                            else
                                res.render('view', { data: maindata, view: data, app: 'querylistdata' });

                        }
                    );
                } else
                    res.render('main', {
                        data: maindata,app:'Sudoku'
                    });
            } else {
                
                next();
            }
        }
        );
    } else {
        
        soway.initapp(req.hostname, function (data) {
            console.log(data.AppName);
            res.render('index', { data: data });
        });
 
    }
};


exports.about = function (req, res) {
    res.render('about', { title: 'About', year: new Date().getFullYear(), message: 'Your application description page' });
};

exports.contact = function (req, res) {
    res.render('contact', { title: 'Contact', year: new Date().getFullYear(), message: 'Your contact page' });
};
 

exports.login = function (req, res) {
     
    soway.login(req.body.name,
        req.body.pwd, 
        req.body.chk, 
        req.body.chkid, 
        req.body.dbid,
        req.hostname,
        function (data) {
        
        if (data.LoginSucess) {
            console.log(req.body.name + 'login sucess');
            res.cookie[req.hostname+'_token'] = data.Token;
            console.log('token:' + res.cookie[req.hostname + '_token']);
            res.send({ IsLogin: true, Token: data.Token });
               
        } else {
            res.clearCookie(req.hostname + '_token');
            res.send({ IsLogin: false, Error: data.Error });
        }
    }
    );

}

exports.main = function (req, res,next) {
   
        soway.getmain(res.cookie[req.hostname + '_token'],
            function (maindata) {
        
        if (maindata.Error == undefined) {
            if (maindata.App.DefaultViewId != 0 && maindata.App.DefaultViewId !=undefined) {
                soway.getlistview(
                    res.cookie[req.hostname + '_token'], maindata.App.DefaultViewId, function (data) {
                        console.log('get main success');
                        if (data.data.TempFile != '')
                            res.render(data.data.TempFile, { data: maindata, view: data ,app: data.data.TempFile });
                        else
                            res.render('view', { data: maindata, view: data,app:'querylistdata'});
                    }
                );
            }else
                res.render('Sudoku', { data: maindata,app:'Sudoku' });
        } else {
          
            next();
            }
        }
        );
    
       
}

exports.getmenu = function (req, res){
         soway.getsub(
            res.cookie[req.hostname + '_token'], req.body.authcode, function (data) {
                if (data.Error ==undefined) {
                    res.send(data.Items);
            } else {

                res.send({ error: data.Error });
                }

            }
        );

    
}


exports.getview = function (req, res,next) {
    
    var id = req.params.id;
    var token = res.cookie[req.hostname + '_token'];
    soway.getmain(token, function (maindata) {
        if (maindata.Error == undefined) {
            soway.getlistview(
                token, id, function (data) {
                    console.log('get view success view :'+ data.data.TempFile);
                    if (data.data.TempFile != '')
                        res.render(data.data.TempFile, { data: maindata, view: data ,app: data.data.TempFile});
                        else
                    res.render('view', { data: maindata, view: data, app: 'querylistdata' });
                }
            );

        } else {
            next();
            }
        })
      
}

exports.getqueryview = function (req, res,next) {
    
        var id = req.body.id;
    var token = res.cookie[req.hostname + '_token'];
    console.log('query view id :' + id);
    
        soway.getlistview(
        token, id, function (data) {
            res.send({ view: data });
                    }
                );
}
exports.querylistdata = function (req, res,next){
           var sessionid = res.cookie[req.hostname + '_token'];
        var viewid = req.body.viewid;
        var filter = req.body.filter;
        var page = req.body.page;
        var size = req.body.pagesize;
        var orderitem = req.body.orderitem;
        var ordertype = req.body.ordertype;
        soway.querylistdata(
            sessionid, viewid, size, page, filter, orderitem, ordertype,
            function (data) {
            if (data.Error == undefined) {
                res.send(data);
            } else {
                next();
            }
            });
    
}
exports.refrechchkcode = function (req, res,next){
    soway.refreshcheckcode(function (data) {
        console.log(data);
        console.log('chk:' + data.Error);
        console.log(data.Key);
        console.log(data.ChkCodeImg);
        if (data.Error == undefined) {
            console.log('chk send');
            res.send({
                chkkey: data.Key,
                chkimg: data.ChkCodeImg
            });
        }
    });
}


exports.getreaditemview = function (req, res,next) {
         var id = req.params.id;
        var token = res.cookie[req.hostname + '_token'];
        soway.getmain(token, function (maindata) {
            if (maindata.Error == undefined) {
                soway.getreaditemview(
                    token, id, function (data) {
                        console.log('get view success');
                        res.render('item', { data: maindata, view: data });
                    }
                );

        } else {
            next();
            }
        })
    
}

exports.getItem = function (req, res,next) {
          var token = res.cookie[req.hostname + '_token'];

        soway.getmain(token, function (maindata) {
            if (maindata.Error == undefined) {
            var id = req.params.id;
            var objid = req.params.objid;
            var ownerviewid = req.params.ownerviewid;
            var prpid = req.params.prpid;
            if (id == undefined)
                id = '';
            if (objid == undefined)
                objid = '';
            if (ownerviewid == undefined)
                ownerviewid = '';
            if (prpid == undefined)
                prpid = '';     
                soway.getitem(
                token, id, objid, '', function (data) {
                    res.render('detailView', {
                        data: maindata, view: data, parentid: objid, ownerviewid: ownerviewid,
                        viewid: id, prpid: prpid, app: 'detailview'
                    });
                    }
                );

        } else {
            
            next();
            }
        })
      

   
}

exports.getItemPost = function (req, res, next) {
    var token = res.cookie[req.hostname + '_token'];
      
            var id = req.body.id;
        var objid = req.body.objid;
        var idexp = req.body.idexp;
        if (objid == undefined)
            objid = '';
        if (idexp == undefined)
            idexp = '';
    
    console.log('getItemPost id:' + id + 'obj:' + objid + 'exp:' + idexp);
            soway.getitem(
            token, id, objid, idexp, function (data) {
                if (data.Error == undefined) {                    
                    res.send({ data: data, viewid: id });
                } else {
                    next();
                }
            }
            );
}


exports.getenum = function(req, res,next) {
         soway.getenum(
            res.cookie[req.hostname + '_token'], req.body.modelid, function (data) {
                if (data.Error == undefined) {
                    res.send(data.EnumValues);
            } else {
                next();
                }

            }
        );

   
}


exports.inputquery = function (req, res,next) {
    soway.inputqurery(
        res.cookie[req.hostname + '_token'], 
        req.body.viewid,
        req.body.itemid,
        req.body.text,
        req.body.objid,
        req.body.ownerid,
        req.body.newadd,
             function (data) {
                if (data.Error == undefined) {
                    res.send(data.Items);
                } else {
                next();
                }

            }
        );
 
}
exports.saveobj = function (req, res,next){
    soway.saveobj(
        res.cookie[req.hostname + '_token'],
        req.body.obj
        , function (data){
            if (data.Error == undefined) {
                res.send('ok');
            } else {
                next();
            }
        }
    )
}
exports.initnew = function (req, res, next){
    var token = res.cookie[req.hostname + '_token'];
    soway.getmain(token, function (maindata) {
        if (maindata.Error == undefined) {
            var id = req.params.id;
            var objid = req.params.objid;
            var ownerviewid = req.params.ownerviewid;
            var prpid = req.params.prpid;
            if (id == undefined)
                id = '';
            if (objid == undefined)
                objid = '';
            if (ownerviewid == undefined)
                ownerviewid = '';
            if (prpid == undefined)
                prpid = '';
            soway.initnew(
                token, id, objid, function (data) {
                    res.render('detailView', { data: maindata, view: data,parentid:objid, ownerviewid: ownerviewid,opt:'new', viewid: id,prpid:prpid, app: 'detailview'});
                }
            );

        } else {
            
            next();
        }
    })
}
exports.savenew = function (req, res, next) {
    soway.savenew(
        res.cookie[req.hostname + '_token'],
        req.body.obj,
        req.body.ownerviewid,
        req.body.ownerid,
        req.body.prpid,
         function (data) {
            if (data.Error == undefined) {
                res.send('ok');
            } else {
                next();
            }
        }
    )
}
exports.redirectToLogin = function (req, res){
    
    

    console.log('check session on server failed'.green+' clear cookie:'+ req.hostname.green + '_token'.green);
    res.clearCookie(req.hostname + '_token');
    res.cookie[req.hostname + '_token'] = undefined;
    res.redirect('/');
}
exports.logout = function (req, res){
    soway.logout(
        res.cookie[req.hostname + '_token'],
        function (data){
            res.clearCookie(req.hostname + '_token');
            res.cookie[req.hostname + '_token'] = undefined;
            res.redirect('/');
        }
    )
}

exports.getquerymodel = function (req, res, next) {
    soway.getquerymodel(
        res.cookie[req.hostname + '_token'], 
            req.body.viewid, 
        
             function (data) {
            if (data.Error == undefined) {
                res.send(data.Cols);
            } else {
                next();
            }

        }
    );
 
}
exports.runoperation = function (req, res, next){
    soway.runoperation(res.cookie[req.hostname + '_token'],
    req.body.objid,
    req.body.viewid,
    req.body.opid,
    function (data) {
  console.log(data);
        if (data.Error == undefined

            || data.Error.RequireLogin == false
            ) {
          
            res.send(data);
        } 
        else {
            next();
        }
    });
}

exports.mkrpt = function (req, res, next){
    soway.mkrpt(res.cookie[req.hostname + '_token'],
        req.body.viewid,
        req.body.cols,
        req.body.pagesize,
        req.body.pageindex,
        req.body.exp,
        function (data) {
        if (data.Error == undefined) {
            res.send(data);
        } else {
            next();
        }
    });
}

exports.saverpt= function (req, res, next) {
    soway.saverpt(res.cookie[req.hostname + '_token'],
        req.body.viewid,
        req.body.cols,
        req.body.exp,
        req.body.reportname,
        function (data) {
        if (data.Error == undefined) {
            res.send(data);
        } else {
            next();
        }
    });
}
exports.getmsg = function (req, res, next){
    soway.getmsg(res.cookie[req.hostname + '_token'],
    function (data) {
       
        if (data.Error == undefined) {
            if (data.Messages.length > 0)
                res.send(data.Messages);
            else
                res.send(true);
        } else {
            next();
        }
        }
    );
}