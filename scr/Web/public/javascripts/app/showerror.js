
define('showerror', ['swapp','bootstrap'], function (swapp) {
    swapp.regmodule('showerror', [], function (showerror,$) {
       

        showerror.provider('showerror', function () {
            this.$get = function ($window, $location) {
 
                var show = function (errorMsg, errorCode, callback, redirecturl) {
                    if (callback != null) {
                        $('#error-dialog').on('hidden.bs.modal', function (e) {

                            callback();
                        });
                    }
                    if (redirecturl != undefined) {
                        $('#error-dialog').on('hidden.bs.modal', function (e) {
                            $location.path(redirecturl).replace();
                            $window.$location = $location;
                            $window.location.reload();
                        });
                    }
                    if (errorCode != undefined) {
                        $('#errorCode').text('错误代码:' + errorCode);
                        $('#errorInfo').text('错误信息:' + errorMsg);
                    } else {
                        $('#errorInfo').text(errorMsg);
                    }
                    $('#error-dialog').modal().on();

                };
                var showalert = function (divid, msg, type) {
                    if (type == undefined)
                        type = 'warning';
                    var appmsg = '<div class="alert fade in alert-'+type+' alert-dismissible" role="alert"> \
                                    <button type="button" class="close" data-dismiss="alert" aria-label="关闭"> \
                                    <span aria-hidden="true">&times;</span></button> '
                                    + msg + '</div>';
                 
                    $('#' + divid).append(appmsg);
                }
                var showInfo = function (isSuccess, infoTitle, infomsg, infoCode) {
                    $('#info-title').text(infoTitle);
                    if (isSuccess)
                        $('#inforesult').text('操作成功');
                    else
                        $('#inforesult').text('操作失败');
                    $('#infoMsg').text(infomsg);
                    $('#info-dialog').modal().on();
                }

                var showmsg = function (msgdatetime,msgcontent,view,objid) {
                     
                    $('#msgdatetime').text(msgdatetime);
                    $('#msgcontent').text(msgcontent);
                    if (view != undefined && view != '' &&view != 0) {
                        if (objid != undefined && objid != '') {
                            $('#btnmsgview').attr('href','../view' + view + '/' + objid);
                        } else {
                            $('#btnmsgview').attr('href','../view'+view);
                        }
                    } else {
                        $('#btnmsgview').attr('href', '#');
                    }
                    $('#msg-dialog').modal().on();
                }

                var service = {

                    showmsg: function (errorMsg, errorCode, callback, redirecturl) {
                        show(errorMsg, errorCode, callback, redirecturl);

                    },
                    showerrormsg: function (error) {

                        show(error.Message, error.Code, null, '');
                    },
                    showinfo: function (msg) {

                        showInfo(true, '操作提示', msg);
                    },
                    showdetailinfo: function (isSuccess, infoTitle, infomsg, infoCode) {
                        showInfo(isSuccess, infoTitle, infomsg, infoCode);
                    },
                    showalert: showalert,
                    showevtmsg:showmsg

                };

                return service;
            };


        });

    })
});