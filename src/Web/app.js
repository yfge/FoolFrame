
/**
 * Module dependencies.
 */

var express = require('express');
var routes = require('./routes');
var http = require('http');
var path = require('path');
var app = express();
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var logger = require('morgan');


// all environments
app.set('port', process.env.PORT || 3000);
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

app.use(bodyParser.json());
app.use(require('stylus').middleware(path.join(__dirname, 'public')));
app.use(express.static(path.join(__dirname, 'public')));
app.use(logger('dev'));

app.get('/', routes.index);
app.get('/about', routes.about);
app.get('/contact', routes.contact);
app.post('/user/login', routes.login);
app.post('/user/logout', routes.checkauth,routes.logout);
app.post('/user/getmenu', routes.checkauth, routes.getmenu);
app.post('/user/getchk', routes.refrechchkcode);
app.get('/main',routes.checkauth, routes.main,routes.redirectToLogin);
app.get('/view:id', routes.checkauth, routes.getview, routes.redirectToLogin);
app.post('/view', routes.checkauth, routes.getqueryview, routes.redirectToLogin);
app.get('/view:id/:objid', routes.checkauth,routes.getItem, routes.redirectToLogin);
app.get('/itemview:id', routes.checkauth, routes.getreaditemview, routes.redirectToLogin);
app.post('/itemview', routes.checkauth, routes.getItemPost, routes.redirectToLogin);
app.post('/data/querylist', routes.checkauth,routes.querylistdata, routes.redirectToLogin);
app.post('/data/inputquery', routes.checkauth, routes.inputquery);
app.post('/data/save', routes.checkauth, routes.saveobj, routes.redirectToLogin);
app.post('/data/new', routes.checkauth, routes.savenew, routes.redirectToLogin);
app.post('/data/exoperation',routes.checkauth,routes.runoperation,routes.redirectToLogin);
app.post('/model/getenum', routes.checkauth, routes.getenum);
app.get('/new:id', routes.checkauth, routes.initnew, routes.redirectToLogin);
app.get('/new:id/:objid&:ownerviewid&:prpid', routes.checkauth, routes.initnew, routes.redirectToLogin);

//report 
app.post('/report/mkqview', routes.checkauth, routes.getquerymodel, routes.redirectToLogin);
app.post('/report/mkrpt', routes.checkauth, routes.mkrpt, routes.redirectToLogin);
app.post('/report/saverpt', routes.checkauth, routes.saverpt, routes.redirectToLogin);
app.post('/getmsg', routes.checkauth, routes.getmsg, routes.redirectToLogin);

http.createServer(app).listen(app.get('port'), function () {
    console.log('Express server listening on port ' + app.get('port'));  
});
