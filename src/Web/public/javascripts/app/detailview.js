define('detailview',
    ['swapp', 'jqueryUI', 'ngCookies', 'jquerydatetime', 'typeahead', 'savetext', 'setextype', 'showerror', 'querylistdata', 'operation'], function (swapp) {
    swapp.regmodule('detailview', ['ngCookies', 'showerror', 'savetext', 'setextype', 'querylistdata', 'operation'],
        function (detail, $) {
            detail.controller('DetailViewController',
                        function ($scope, $location, $compile, $http, $window, $cookies,
                            showerror, setextype, savetext, querylistdata, operation) {
                            var Handlebars = require('Handlebars');

                            $scope.obj = {};
                            $scope.obj.Id = '';
                            $scope.obj.Propertyies = [];
                            $scope.obj.Itemproperties = [];
                            $scope.opt = '';
                            $scope.ownerviewid = '';
                            $scope.ownerid = '';
                            $scope.viewid = '';
                            $scope.isedit = false;
                            $scope.prpid = '';
                            $scope.typeaheadflag = {};
                            $scope.editflag = [];
                            $scope.deleteitems = [];
                            $scope.detailviews = [];
                            $scope.currenteditview = '';
                            $scope.currentedititem = '';
                            $scope.currenteditoption = '';
                            $scope.currenteditprpid = '';
                            $scope.currenteditviewname = '';
                            $scope.lastclick = '';
                            $scope.queryviewId = '';
                            /***
                            *选择后设定值*/
                            var setSelect = function (item, id) {

                                $('[data-property="' + id + '"]').attr("data-propertyvalue", item.Id);
                                $('[data-property="' + id + '"]').attr("data-fmtvalue", item.Text);
                                return item.Text;
                            }

                            //开始编辑
                            $scope.beginedit = function () {
                                $scope.isedit = true;

                                if ($('#btn-edit').attr('disabled') == false ||
                                    $('#btn-edit').attr('disabled') == undefined
                                    ) {

                                    SetEdit('.col-md-4[data-propertyvalue]', 'p', $scope.viewName, '', $scope.obj.Id, $scope.ownerid, $scope.opt);
                                    $('#btn-edit').attr('disabled', true);
                                    $('#btn-save').removeAttr('disabled');

                                }
                            };
                            //保存
                            $scope.beginsave = function () {

                                if ($('#btn-save').attr('disabled') == false ||
                                    $('#btn-save').attr('disabled') == undefined) {
                                    $('#loading-title').text('保存中');
                                    $('#loading-msg').text('正在保存，请稍后....');
                                    $('#loading-dialog').modal('show');
                                    $('#loading-dialog').on('hidden.bs.modal', function (e) {
                                        // if($scope.opt == 'new')
                                        history.go(-1);
                                    });
                                    SaveEdit('.col-md-4[data-propertyvalue]', function (data) {
                                        $scope.obj.Propertyies = data;
                                        $('#btn-save').attr('disabled', true);
                                        $('#btn-edit').removeAttr('disabled');
                                        if ($scope.opt == 'new') {
                                            $http.post('/data/new',
                                                {
                                                    obj: $scope.obj,
                                                    ownerviewid: $scope.ownerviewid,
                                                    ownerid: $scope.ownerid,
                                                    prpid: $scope.prpid,
                                                }).success(function (data, status, headers, config) {
                                                    $('#loading-dialog').modal('hide');

                                                });
                                        } else {
                                            $http.post('/data/save',
                                            {
                                                obj: $scope.obj,
                                            }
                                            ).success(function (data, status, headers, config) {
                                                $('#loading-dialog').modal('hide');

                                            });
                                        }
                                    });

                                }
                                $scope.isedit = false;

                            };
                            //编辑子项
                            $scope.edititem = function (viewId, itemid, viewname, prpid, opt) {
                                var clickname = opt + "-" + viewname + '-' + itemid;

                                if ($scope.isedit == false)
                                    return;
                                if (viewId == 0) {

                                    if ($scope.lastclick != '' && $scope.lastclick != clickname) {
                                        $scope.edititem($scope.currenteditview, $scope.currentedititem, $scope.currenteditviewname, $scope.currenteditprpid, $scope.currenteditoption);
                                    }

                                    if ($scope.editflag[viewname + '-' + itemid] == undefined)
                                        $scope.editflag[viewname + '-' + itemid] = false;
                                    if ($scope.editflag[viewname + '-' + itemid] == false) {

                                        $scope.currentedititem = itemid;
                                        $scope.currenteditview = viewId;
                                        $scope.currenteditviewname = viewname;
                                        $scope.currenteditprpid = prpid;
                                        $scope.lastclick = clickname;
                                        $scope.currenteditoption = opt;
                                        $('#' + clickname).text('保存');

                                        SetEdit("tr[data-itemid='" + itemid + "'][data-itemview='" + viewname + "'][data-operation='" + opt + "'] td[data-propertyvalue]", 'p', viewname, viewname + '-' + itemid, itemid, $scope.obj.Id, opt);

                                        $scope.editflag[viewname + '-' + itemid] = true;

                                    } else {

                                        $scope.currenteditview = '';
                                        $scope.currentedititem = '';
                                        $scope.currenteditoption = '';
                                        $scope.currenteditprpid = '';
                                        $scope.currenteditviewname = '';
                                        $scope.lastclick = '';
                                        $('#' + clickname).text('编辑');
                                        $scope.editflag[viewname + '-' + itemid] = false;

                                        var saveitem = SaveEdit("tr[data-itemid='" + itemid + "'][data-itemview='" + viewname + "'][data-operation='" + opt + "'] td[data-property]");
                                        var itemArry = undefined;
                                        $scope.obj.Itemproperties.forEach(function (item) {
                                            if (item.Key == prpid) {
                                                itemArry = item;
                                                return;
                                            }
                                        });
                                        if (itemArry == undefined) {
                                            itemArry = {
                                                Key: prpid
                                            };
                                            itemArry.Items = [];
                                            itemArry.DelteItems = [];
                                            itemArry.AddedItems = [];
                                            $scope.obj.Itemproperties.push(itemArry);
                                        };

                                        var dataItem = undefined;
                                        if (opt == 'edit') {
                                            itemArry.Items.forEach(function (item) {
                                                if (item.ItemId == itemid) {
                                                    dataItem = item;
                                                    return;
                                                }
                                            });
                                            if (dataItem == undefined) {
                                                dataItem = {
                                                    ItemId: itemid
                                                };
                                                itemArry.Items.push(dataItem);
                                            }
                                        } else {
                                            itemArry.AddedItems.forEach(function (item) {
                                                if (item.ItemId == itemid) {
                                                    dataItem = item;
                                                    return;
                                                }
                                            });
                                            if (dataItem == undefined) {
                                                dataItem = {
                                                    ItemId: itemid
                                                };
                                                itemArry.AddedItems.push(dataItem);
                                            }

                                        }
                                        dataItem.Propertyies = saveitem;

                                    }
                                }
                                else
                                    $('#modaldiag').modal();
                            }
                            //删除子项
                            $scope.deleteitem = function (itemid, viewname, prpid) {
                                $("tr[data-itemid='" + itemid + "'][data-itemview='" + viewname + "']").fadeTo('slow', 0.01, function () {
                                    $(this).slideUp('slow', function () {


                                        var deleteItem;
                                        if ($scope.editflag[viewname + '-' + itemid] == true) {
                                            $scope.editflag[viewname + '-' + itemid] = false;
                                            deleteItem = SaveEdit("tr[data-itemid='" + itemid + "'][data-itemview='" + viewname + "'] td[data-propertyvalue]");
                                        } else {
                                            deleteItem = GetItemValue("tr[data-itemid='" + itemid + "'][data-itemview='" + viewname + "'] td[data-propertyvalue]");
                                        }
                                        var itemArry = undefined;
                                        $scope.obj.Itemproperties.forEach(function (item) {
                                            if (item.Key == prpid) {
                                                itemArry = item;
                                                return;
                                            }
                                        });
                                        if (itemArry == undefined) {
                                            itemArry = {
                                                Key: prpid
                                            };
                                            itemArry.Items = [];
                                            itemArry.DelteItems = [];
                                            itemArry.AddedItem = [];
                                            $scope.obj.Itemproperties.push(itemArry);
                                        };
                                        var dataItem = undefined;
                                        count = 0;
                                        itemArry.Items.forEach(function (item) {
                                            count++;
                                            if (item.ItemId == itemid) {
                                                dataItem = item;
                                                itemArry.Items.splice(count, 1);
                                                return;
                                            }
                                        });

                                        if (dataItem == undefined) {
                                            dataItem = {
                                                ItemId: itemid
                                            };
                                            itemArry.DelteItems.push(dataItem);
                                        }
                                        dataItem.Propertyies = deleteItem;
                                        $(this).remove();
                                    });
                                });
                            }
                            //撤消删除子项
                            $scope.undo = function (prpid) {

                            }

                            $scope.exuteop = function (opid) {

                                if ($scope.isedit) {
                                    showerror.showerror('请先保存当前信息', '10010', null, null);
                                } else {
                                    operation.runoperation($scope.viewid, $scope.obj.Id, opid);
                                }

                            }

                            $('#selectdialog').on('show.bs.modal', function (e) {

                            });
                            //增加子项
                            $scope.additem = function (viewid, viewname, prpid, select, listviewid) {


                                querylistdata.setEditMsg('选择');
                                if ($scope.opt == 'new') {
                                    showerror.showinfo('请先保存当前内容，再新建子项');
                                    return;
                                }
                                else {

                                    if (viewid != '0') {
                                        if (select) {
                                            $scope.queryviewId = listviewid;
                                            initQueryView($scope.queryviewId, function () {
                                                querylistdata.setclickcallback(function (viewid, dataid, dataitem) {
                                                    $('#selectdialog').modal('hide');
                                                    additem(prpid, viewname, viewid, true, dataitem);
                                                });
                                                $('#selectdialog').modal('show');
                                            });
                                        } else {
                                            window.location.href = '../new' + viewid + '/' + $scope.obj.Id + '&' + $scope.viewid + '&' + prpid;
                                            return;
                                        }

                                    } else {
                                        if (select) {
                                            $scope.queryviewId = listviewid;
                                            initQueryView($scope.queryviewId, function () {
                                                querylistdata.setclickcallback(function (viewid, dataid, dataitem) {
                                                    $('#selectdialog').modal('hide');
                                                    additem(prpid, viewname, viewid, true, dataitem);
                                                });
                                                $('#selectdialog').modal('show');
                                            });

                                        } else {
                                            additem(prpid, viewname, viewid);
                                        }


                                    }

                                }


                            }
                            var additem = function (prpid, viewname, viewid, select, items) {

                                var itemArry = undefined;
                                $scope.obj.Itemproperties.forEach(function (item) {
                                    if (item.Key == prpid) {
                                        itemArry = item;
                                        return;
                                    }
                                });
                                if (itemArry == undefined) {
                                    itemArry = {
                                        Key: prpid
                                    };
                                    itemArry.Items = [];
                                    itemArry.DelteItems = [];
                                    itemArry.AddedItems = [];
                                    $scope.obj.Itemproperties.push(itemArry);
                                };
                                var addId = itemArry.AddedItems.length + 1;
                                if (items != null) {
                                    addId = items.ItemId;
                                }
                                var str = '<tr data-operation="new" data-itemid="' + addId + '" data-itemview="' + viewname + '" ></tr>';
                                $('#' + viewname + '-table').append(str);
                                i = 0;
                                $('#' + viewname + '-table tr th[data-property]').each(function () {
                                    var str = '<td';
                                    str += ' data-property="' + $(this).attr("data-property");
                                    str += '" data-propertyModel="' + $(this).attr("data-propertyModel");
                                    str += '" data-viewitem="' + $(this).attr("data-viewitem");
                                    str += '" data-propertyType="' + $(this).attr("data-propertyType");
                                    str += '" data-edittype ="'+$(this).attr("data-edittype");
                                    str += '" data-readonly ="' + $(this).attr("data-readonly");
                                    str += '" data-propertyvalue ';
                                    if (!select)
                                        str += '="" >&nbsp;</td>';
                                    else if (items != undefined && i < items.Propertyies.length) {
                                        str += '="' + items.Propertyies[i].Value + '">' + items.Propertyies[i].FmtValue + '</td>'
                                        i++;
                                    } else
                                        str += '="" >&nbsp;</td>';

                                    $('#' + viewname + '-table tr[data-operation="new"][data-itemid="' + addId + '" ] ').append(str);
                                });
                                if (!select) {
                                    str = '<td> <a id="new-' + viewname + '-' + addId + '" ng-click="edititem(\'' + viewid + '\',\'' + addId + '\',\'' + viewname + '\',\'' + prpid + '\',\'new\')") > 编辑 </a></td>';
                                    $('#' + viewname + '-table tr[data-operation="new"][data-itemid="' + addId + '" ] ').append(
                                        $compile(str)($scope));
                                    $scope.edititem(viewid, addId, viewname, prpid, 'new');
                                } else {
                                    items.IsExist = true;
                                    itemArry.AddedItems.push(items);
                                }
                            }
                            //setEdit   SetEdit('.col-md-4[data-propertyvalue]', 'p', $scope.viewName, '', $scope.obj.Id, $scope.ownerid, $scope.opt);
                            var SetEdit = function (select, contentselect, viewName, idpre, objid, ownerid, opt) {
                                $(select).each(function () {
                                    var text = $(this).attr("data-fmtvalue");
                                    var property = $(this).attr("data-property");
                                    var objId = $(this).attr("data-propertyvalue");
                                    var prType = $(this).attr("data-propertyType");
                                    var modelid = $(this).attr("data-propertyModel");
                                    var viewItem = $(this).attr("data-viewitem");
                                    
                                    var readonly = $(this).attr("data-readonly");
                                    if (readonly == 'false') {
                                        $(contentselect, this).remove();
                                        setextype.setextype(prType, property, idpre, objId, modelid, viewItem, text, viewName, $(this), objid, ownerid, opt);
                                    }
                                });
                            };
                            var SaveEdit = function (select, callback) {

                                var savearry = savetext.savetext(select);

                             
                                if (callback != undefined) {
                                    callback(savearry)
                                }
                                return savearry;
                            }

                            var GetItemValue = function (select) {
                                var savearry = [];
                                $(select).each(function () {
                                    var property = $(this).attr("data-property");
                                    var objId = $(this).attr("data-propertyvalue");
                                    savearry.push({ Key: property, Value: objId });

                                });
                                return savearry;
                            }


                            require(['domReady'],
                            function (domReady) {
                                domReady(function () {
                                    if ($scope.opt == 'new')
                                        $scope.beginedit();
                                });
                            });


                            var initQueryView = function (id, callback) {
                                $('#loading-title').text('加载中');
                                $('#loading-msg').text('正在加载，请稍后....');
                                $('#loading-dialog').modal('show');
                                $http.post('/view', { id: id }).
                                    success(function (data, status, headers, config) {
                                        if (data.error != undefined) {
                                            showerror.showerrormsg(data.error);
                                        }
                                        querylistdata.setview(id);
                                        $('#datalist tbody tr').remove();

                                        $('#datalist tbody').append('<tr></tr>');
                                        for (i = 0; i < data.view.data.Items.length; i++) {

                                            $('#datalist tbody tr').append('<th>' + data.view.data.Items[i].Name + '</th>');

                                        }

                                        $('#info').text('记录未知 请查询');
                                        $('#pagenav li').remove();
                                        if (callback != undefined) {
                                            $('#loading-dialog').modal('hide');
                                            callback();
                                        }
                                    });


                            };
                        });
        }
    );
});