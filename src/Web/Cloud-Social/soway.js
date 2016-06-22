var http = require('http');
var config = require('../Cloud-Config')
var request = require('request');
var getinitoption = function (invoketype, method) {
    
    return {
        hostname: config.serverconfig.hostname,
        port: config.serverconfig.hostport,
        path: config.serverconfig.rootpath+method,
        method: invoketype,
        headers:config.serverconfig.globalheaders
    };
};

var postandget = function (method, data, callback){
    

    var req1 = http.request(getinitoption('POST', method),
         function (res) {
        console.log('request :' + method);
        res.setEncoding('utf8');
        var alldata = '';
        res.on('data', function (chunk) {
            alldata = alldata + chunk;
        });
        res.on('end', function () {
            try {
                 
                
                var senddata = JSON.parse(alldata);
                callback(senddata);
            } catch (a) {
                console.log(method + ' ' + a);
                console.log(alldata);
            }
     
        });
    });
    req1.on('error', function (e) {
        console.log('problem with request: ' + e.message);
    });
    
 
    req1.write(JSON.stringify(data));
    req1.end();
}
var initapp = function (hostname, callback) {
    
    if (config.appconfig.hasOwnProperty(hostname)) {
        
        
        postandget('initapp', config.appconfig[hostname], callback);
    }
    else {
        console.log(config.appconfig['*'].AppId);
        postandget('initapp', config.appconfig['*'], callback);
    }
   
};
exports.initapp = initapp;
exports.login = function (name, pwd, chk, chkid, dbid,hostname, callback){
    initapp(hostname, function (data) {
        var app = null;
        if (config.appconfig.hasOwnProperty(hostname)) {
            app = config.appconfig[hostname];
        } else {
            app = config.appconfig['*'];
        }
        var postdata = {
            UserId: name,
            PassWord: pwd,
            DbId: dbid,
            CheckCode: chk,
            AppId: app.AppId,
            AppKey: app.AppKey,
            CheckCodeKey: chkid
            
        };
  
        postandget('loginv2',
            postdata
            , callback
        );
    });


}

exports.getmain = function (sessionid, callback) {
    
    
    
    postandget('getmain',
        sessionid, callback
    );


}


exports.getsub = function (sessionid, authcode, callback){
     
    postandget('getsubmenu',
        {
        Token: sessionid,
        ParentAuthCode: authcode,
    }, callback
    );
}
exports.getlistview = function (token, viewid, callback){
    
    postandget('getlistview',
        {
        Token: token,
        ViewId: viewid,
    }, callback
    );
     
}
exports.querylistdata = function (sessionid,
     viewid, pagesize, page, filter, orderitem, ordertype, callback){

    postandget('querydata',
    {
        Token: sessionid,
        ViewId: viewid,
        PageSize: pagesize,
        PageIndex: page,
        QueryFilter: filter,
        OrderByItem: orderitem,
        OrderByType: ordertype

    }, callback);
}

exports.refreshcheckcode = function (callback) {
    postandget('getcheckcode', {

    },callback);

}

exports.getreaditemview = function (token, viewid, callback) {
    postandget('getreaditemview',
             {
        Token: token,
        ViewId: viewid
    },
    callback);
}
exports.getitem = function (token, viewid, objId,idexp, callback) {
    
    
    console.log(' get item view id '+viewid+',obj id :'+ objId)

    postandget('querydatadetail',
             {
        Token: token,
        viewId: viewid,
        objId: objId,
        IdExp: idexp
    },
    callback);
}
exports.getenum = function (token, modelid, callback){
    postandget('getenums',
        {
        Token: token,
        ModelId: modelid
    }, callback
    );
}

exports.inputquery = function (token, viewid, itemid, text,objid,ownerid, newadd,callback){
    console.log('inputquery token:' + token);
    postandget('inputquery',
    {
        Token: token,
        Text: text,
        ViewName: viewid,
        ViewItemId: itemid,
        ObjID: objid,
        OwnerId: ownerid,
        IsAdded:newadd

    }, callback);
}
exports.saveobj = function (token, obj,callback){
    postandget('saveobj', {
        Token: token,
        SaveObj: obj
    }, callback
    )
}


exports.initnew = function (token, viewid, parentid, callback) {
    console.log('init new viewid '+viewid)
    postandget('initnew', {
        Token: token,
        ViewId: viewid,
        ParentObjId:parentid
    }, callback
    )
}

exports.savenew = function (token, obj, ownerviewid, ownerid,prpid, callback) {
    console.log('save new .. owner:' + ownerid + ',ownerview:' + ownerviewid);
    postandget('savenewobj', {
        Token: token,
        SaveObj: obj,
        OwnerViewId: ownerviewid,
        OwnerId: ownerid,
        Property:prpid
    }, callback
    )
}

exports.logout = function (token, callback){
    postandget('logout', { Token: token }, callback);
}

exports.getquerymodel = function (token, viewid, callback) {
 
    postandget('getmkqview', { Token: token , ViewId: viewid}, callback);
}

exports.exoperation = function (token, objid, viewid, opid, callback){

    postandget('runoperation', { Token: token , ViewId: viewid,ObjectId:objid,OperationId: opid}, callback);
     
}

exports.mkrpt = function (token, viewid, cols, pagesize, pageindex, exp,
     callback){
    postandget('getrpt',
         { Token: token, ViewId: viewid, ReportCols: cols, CurrentPage: pageindex, PageSize: pagesize, FilterExp: exp },
          callback);
}

exports.saverpt = function (token, viewid, cols, exp, name, callback) {
    postandget('saverpt', {
        Token: token, ViewId: viewid, ReportCols: cols,
        FilterExp: exp, ReportName: name
    }, callback);

}
exports.getmsg = function (token,callback){
    postandget('getmsg', {
        Token: token
    }, callback);
}