define('mkreport', ['swapp', 'setextype', 'showerror', 'jquery'], function (swapp) {
    swapp.regmodule('mkreport', ['showerror', 'setextype'], function (rpt, $) {


        rpt.controller("MakeReportController", function ($rootScope, $compile, $scope, $http, mkreport, setextype) {
            $rootScope.selectcols = [];
            $rootScope.pagesize = 10;
            $rootScope.pageindex = 1;
            $rootScope.viewid = mkreport.getviewid();
            $rootScope.totalpages = 0;
            $rootScope.totalrecords = 0;


            $rootScope.lbcandidate = '#rpt-candidate';
            $rootScope.lbselecttye = '#rtp-selecttype';
            $rootScope.lbselected = '#rtp-selected';
            $rootScope.mkdiag = '#rptdiag';
            $rootScope.rptdiag = '#rptresultdiag';
            $rootScope.resultTable = '#rptTable';
            $rootScope.rptcondition = '#tb-condition';

            var selected = " option:selected";
            var nameatr = "Name";
            var selectedindex = 0;
            var compareclass = "[data-input='compare']";
            var filterclass = "[data-input='filtercol']";
            var toucolspan = 1;
            var lenth = [];
            var tree = [];
            var Sequences = [];
            $scope.add = function () {

                var colselected = $($rootScope.lbcandidate + selected);
                if (colselected.length > 0) {
                    var colselecttype = $($rootScope.lbselecttye + selected);
                    if (colselecttype.length > 0) {


                        var i = 0;
                        while (i < $rootScope.selectcols.length) {
                            if ($rootScope.selectcols[i].ColId == colselected.val() &&
                                $rootScope.selectcols[i].SelectedTypeId == colselecttype.val()) {
                                return;
                            }
                            i++;
                        }
                        var addSelectCols = {
                            ColName: colselected.text() + '[' + colselecttype.text() + ']',
                            ColId: colselected.val(),
                            SelectedTypeId: colselecttype.val(),
                            Index: selectedindex,
                            OrderType: 2
                        };

                        $rootScope.selectcols.push
                            (addSelectCols);
                        $($rootScope.lbselected).append('<option value=' + selectedindex + ' data-selectcolname="' + addSelectCols.ColName + '">' + addSelectCols.ColName + '</option>');
                        selectedindex++;

                    }
                }
                
            };

            $scope.up = function () {
                var selectedcol = $($rootScope.lbselected + selected);
                if (selectedcol.length > 0) {

                    var index = parseInt(selectedcol.val());
                    if (index == 0)
                        return;
                    else {
                        var toup = $rootScope.selectcols[index];
                        var todown = $rootScope.selectcols[index - 1];
                        $rootScope.selectcols[index] = todown;
                        $rootScope.selectcols[index].Index++;
                        $rootScope.selectcols[index - 1] = toup;
                        $rootScope.selectcols[index - 1].Index--;
                        $($rootScope.lbselected + ' option[value=' + index + ']').text(todown.ColName);
                        $($rootScope.lbselected + ' option[value=' + index + ']').removeAttr('selected')
                        $($rootScope.lbselected + ' option[value=' + (index - 1) + ']').text(toup.ColName);
                        $($rootScope.lbselected + ' option[value=' + (index - 1) + ']').attr('selected', true);
                    }
                }
            };
            $scope.down = function () {
                var selectedcol = $($rootScope.lbselected + selected);
                if (selectedcol.length > 0) {

                    var index = parseInt(selectedcol.val());
                    if (index == $rootScope.selectcols.length - 1)
                        return;
                    else {
                        var toup = $rootScope.selectcols[index + 1];
                        var todown = $rootScope.selectcols[index];
                        $rootScope.selectcols[index] = toup;
                        $rootScope.selectcols[index].Index--;
                        $rootScope.selectcols[index + 1] = todown;
                        $rootScope.selectcols[index + 1].Index++;
                        $($rootScope.lbselected + ' option[value=' + index + ']').text(toup.ColName);
                        $($rootScope.lbselected + ' option[value=' + index + ']').removeAttr('selected');

                        $($rootScope.lbselected + ' option[value=' + (index + 1) + ']').text(todown.ColName);
                        $($rootScope.lbselected + ' option[value=' + (index + 1) + ']').attr('selected', true);
                    }
                }

            };
            $scope.del = function () {
                var selectedcol = $($rootScope.lbselected + selected);
                if (selectedcol.length > 0) {

                    var delindex = parseInt(selectedcol.val());
                    $rootScope.selectcols.removeAt(delindex);
                    var i = delindex;
                    while (i < $rootScope.selectcols.length) {

                        $($rootScope.lbselected + ' option[value=' + ($rootScope.selectcols[i].Index) + ']').attr('value', $rootScope.selectcols[i].Index - 1);
                        $rootScope.selectcols[i].Index--;
                        i++;

                    }
                    selectedindex = $rootScope.selectcols.length;
                    selectedcol.remove();
                    treeindex[delindex] ='';
                    alert(treeindex);
                }

            };
            $scope.mkrpt = function () {
                
                var tr = 0;
                for (var i = 0; i < lenth.length; i++) {
                    if (lenth[i] == 0) {
                        tr = i++;
                        for (var j = 0; j < lenth[i]; j++) {
                            Sequences[tree[tr]] = returndata(tree[tr]).concat(returndata(tree[tr + j]));  //合并后面数据然后和data最后赋值给data
                        }
                    } else {
                        tr = i;
                        for (var j = 0; j < lenth[i]; j++) {
                            Sequences[tree[tr]] = returndata(tree[tr]).concat(returndata(tree[j]));   //合并前面数据然后赋值给data
                        }
                    }
                }
                alert(Sequences);
                $($rootScope.mkdiag).modal('hide');
                $rootScope.mkreport(function () {
                    $($rootScope.rptdiag).modal('show');
                });
            }
            $scope.asc = function () {
                var selectedcol = $($rootScope.lbselected + selected);
                if (selectedcol.length > 0) {

                    var index = parseInt(selectedcol.val());
                    $rootScope.selectcols[index].OrderType = 0;
                    selectedcol.text(selectedcol.attr('data-selectcolname') + '[升序]');


                }
            }
            $scope.desc = function () {
                var selectedcol = $($rootScope.lbselected + selected);
                if (selectedcol.length > 0) {

                    var index = parseInt(selectedcol.val());
                    $rootScope.selectcols[index].OrderType = 1;
                    selectedcol.text(selectedcol.attr('data-selectcolname') + '[降序]');


                }
            }
            $scope.casc = function () {
                var selectedcol = $($rootScope.lbselected + selected);
                if (selectedcol.length > 0) {

                    var index = parseInt(selectedcol.val());
                    $rootScope.selectcols[index].OrderType = 2;
                    selectedcol.text(selectedcol.attr('data-selectcolname'));


                }

            }
          

            $scope.conditionindex = 0;
            var treeture = [];
            var treeindex = [];
            var treetou = [];
            var treetoulenth = [];
            var td2 = [],td2c=[];
            var td3 = [],td3c=[];
            var getcondition = function (callback) {
                var select = $rootScope.rptcondition + " tr[data-inputfor]";
                var Exp = {};
             
                Exp.Sequences = [];
                $(select).each(function () {

                    var andor = $('select[data-inputfor="andor"]', this).val();
                    var andorText = $('select[data-inputfor="andor"]', this).find("option:selected").text(); //与或
                    var col = $('select[data-inputfor="comparecol"]', this).val();
                    var colname = $('select[data-inputfor="comparecol"]', this).find("option:selected").text();
                    var compare = $('select[data-inputfor="compare"]', this).val();  //大于小于
                    var comparename = $('select[data-inputfor="compare"]', this).find("option:selected").text();

                    var obj = setextype.getval($('select[data-inputfor="comparecol"]', this).attr('data-inputid'));
                    var BoolOp = {
                        DBName: andor,
                        ShowName: andor

                    }
                    var exp = {
                        Col: {
                            ID: col,
                            Name: colname
                        },
                        CompareOp: {
                            ID: compare,
                            Name: comparename
                        },
                        ValueExp: obj.val,
                        ValueFmt: obj.text
                    };
                    if (andor == undefined) {
                        Exp.FirstExp = exp;
                    } else {
                        Exp.Sequences.push(
                            {
                                BoolOp: BoolOp,
                                AddedExp: exp

                            });
                    }
                   
                    
                    //alert(data);
                });

                callback(Exp)
            }
            $scope.addcondition = function (trid) {
                $rootScope.viewid = mkreport.getviewid();
                var str = '<tr data-inputfor="condition" id="con-' + $scope.conditionindex + '">';
                str += '<td><div  class =" glyphicon glyphicon-plus" ng-click="addcondition(\'con-' + $scope.conditionindex + '\')" ></div></td>';
                str += '<td><div class =" glyphicon glyphicon-remove" ng-click="removecondition(' + $scope.conditionindex + ')" ><span/></div></td>';
                str += '<td><input type="checkbox"  data-inputfor="mergecondition" id="chkbox' + $scope.conditionindex + '" /></td>'
                str += '<td style="width:22px" colspan='+toucolspan+' id="kong' + $scope.conditionindex + '"></td>'
                 
                str += '<td><select data-inputfor="andor" id="yu-' + $scope.conditionindex + '"><option value="and">与</option><option value="or">或</option></select></td>'; //add the option of AND or OR;
                str += '<td><select \
                        data-inputfor="comparecol" \
                        id="filtercol-' + $scope.conditionindex
                        + '" data-seltarget="#comparecol-' + $scope.conditionindex + '" \
                        data-inputfrom ="#input-' + $scope.conditionindex + '"\
                        data-inputid="input-control-'+ $scope.conditionindex + '">';
                str += '<option data-init></option>';

                for (var i = 0; i < $rootScope.candidatecols.length; i++) {
                    str += '<option value="' +
                        $rootScope.candidatecols[i].ID + '">' +
                        $rootScope.candidatecols[i].Name
                    + '</option>';

                }
                str += '</select></td>';
                str += '<td><select data-inputfor="compare" id="comparecol-' + $scope.conditionindex + '" ></select></td>';
                str += '<td id="input-' + $scope.conditionindex + '">';

                str += '</td>';
                str += '</tr>';

                treeindex.push($scope.conditionindex);
                if (trid == undefined) {
                    $($compile(str)($scope)).insertBefore(
                    $($rootScope.rptcondition + ' tr:last'));

                }
                else
                    $($compile(str)($scope)).insertBefore(
           $($rootScope.rptcondition + ' tr[id="' + trid + '"]'));
 
                $('#filtercol-' + $scope.conditionindex).change(function () {


                    //初始化的空去掉
                    $('option[data-init]', this).remove();
                    var selid = $(this).val();

                    for (i = 0; i < $rootScope.candidatecols.length; i++) {
                        //加入比较的类型
                        if (selid == $rootScope.candidatecols[i].ID) {

                            //
                            var tarid = $(this).attr("data-seltarget");
                            var inputcontext = $(this).attr("data-inputfrom");
                            var inputid = $(this).attr("data-inputid");
                            $(tarid + ' option').remove();
                            for (j = 0; j < $rootScope.candidatecols[i].CompareTypes.length; j++) {
                                $(tarid).append(
                                    '<option value="' +
                                     $rootScope.candidatecols[i].CompareTypes[j].ID
                                     + '">'
                                     + $rootScope.candidatecols[i].CompareTypes[j].Name
                                     + '</option>'
                                    );
                            }

                            setextype.makeinput($rootScope.candidatecols[i].Name,
                                $rootScope.candidatecols[i].PrpType,
                                $rootScope.candidatecols[i].ModelId,
                                $rootScope.candidatecols[i].States,
                                $(inputcontext),
                                inputid,
                                $rootScope.viewid);

                            //初始化input
                            return;
                        }

                    }
                });
                $($rootScope.rptcondition + ' tr[id]:first td select[data-inputfor="andor"]').remove();
                $scope.conditionindex++;
                 
             
            }
            
            $scope.removecondition = function (id) {
                $("#con-" + id).remove();
                $($rootScope.rptcondition + ' tr[id]:first td select[data-inputfor="andor"]').remove();

            }
           
            var removesame = function () {
                var result = [], hash = {};
                for (var i = 0, elem; (elem = tree[i]) != null; i++) {
                    if (!hash[elem]) {
                        result.push(elem);
                        hash[elem] = true;
                    }
                }
                return result;
            }//去掉数组相同元素
            $scope.mergs = function (count) {
  
                if (lenth.length == 0) {
                    lenth.push(count.length);
                    tree = count;
                     
                }         
                else {
                    lenth.push(0);
                    lenth.push(count.length);
                    tree = tree.concat(count);
                    
                }
                treeture = tree;
                treeture=removesame();
               // alert("树" + tree + "，真树" + treeture);
                
            };
            var returndata = function (i) {
                
                var andor = $('#' + 'yu-' + i).val();
                var andorText = $('#' + 'yu-' + i).text(); //与或
                var col = $('#' + 'filtercol-' + i).val();
                var colname = $('#' + 'filtercol-' + i).text();
                var compare = $('#' + 'comparecol-' + i).val();  //大于小于
                var comparename = $('#' + 'comparecol-' + i).text();
                var objval = $('#' + 'input-control-' + i).val();
                var objtext = $('#' + 'input-control-' + i).text();
                var BoolOp = {
                    DBName: andor,
                    ShowName: andorText
                }
                var exp = {
                    Col: {
                        ID: col,
                        Name: colname
                    },
                    CompareOp: {
                        ID: compare,
                        Name: comparename
                    },
                    ValueExp: objval,
                    ValueFmt: objtext
                };

                Sequences[i]=
                    {   
                        BoolOp: BoolOp,
                        AddedExp: exp
                    };
                return Sequences;
            }
            var geshu = function (count, tree) {
                var shu = 0;
                  
                for (var i = 0; i < count.length;i++){
                    for (var j = 0; j < tree.length; j++) {
                        if (count[i] == tree[j]) shu++;
                    }
                }
                return shu;

            }
            var zhen = function () {
                var n = 0;
                for(n=1;n<treetou.length;n++){
                    if(treetou[0]!=treetou[n]) return n; 
                }
                return n;
            }
            var weizhi = function (count, tree) {
                var i = 0;

                for (i = 0; i < count.length; i++) {
                    for (var j = 0; j < tree.length; j++) {
                        if (count[i] == tree[j]) return i; 
                    }
                }
                return 0;
            }
            var countgeshu = function (count, tree) {
                var shu = 0;

                for (var i = 0; i < count.length; i++) {
                    for (var j = 0; j < tree.length; j++) {
                        if (count[i] == tree[j]) {
                            shu++;
                            continue;
                        }
                    }
                }
                return shu;

            }
            var hebin = 0;
            var countlenth = 1;
            var he = [];
            
            $scope.removechange = function () {
                 
                //var str = '总共传了' + arguments.length + '个参数\n';
                //for (var i = 0; i < arguments.length; i++) {
                //    str += '第' + (i + 1) + '个参数值：' + arguments[i] + '\n';
                //}
                //alert(str);
                
                var i = lenth.pop();
                var shuzu = [];
                    for (var m = 0; m < i-1; m++) {
                        var j = tree.pop();
                        shuzu.push(j);
                         
                        if ($('#chkbox' + j).length == 0) {
                            $('#con-' + j + ' td:eq(2)').append('<input type="checkbox"  data-inputfor="mergecondition" id="chkbox' + j + '" />');
                        }
                        if ($('#kong' + j).length == 0) {
                            $('#con-' + j + ' td:eq(2)').after('<td style="width:22px"  id="kong' + j + '"></td>');
                        }
                        
                    }
                    var j = tree.pop();
                    shuzu.push(j);
                    treeture=removesame();
                    
                    if (treeture.length == 0 || geshu(shuzu, treeture) == 0) {
                        $('#con-' + j + ' td:eq(3)').attr("rowspan", 1);
                        $('#con-' + j + ' td:eq(3)').attr('bgcolor', ' #FFFFFF');
                        
                    }
                    else if (weizhi(shuzu, treeture) == 0) {
                        toucolspan--;
                        for (var i = 0; i < treetou.length - 1; i++) {
                            $('#kong' + treetou[i]).attr("colspan", toucolspan);
                        }
                    }
                    else if (weizhi(shuzu, treeture) == (shuzu.length-1)) {
                        toucolspan--;
                         
                    } else {
                        toucolspan--;
                        for (var i = 0; i < treetou.length - 1; i++) {
                            $('#kong' + treetou[i]).attr("colspan", toucolspan);
                        }
                    }

             
                    $('#tou th:eq(3)').attr("colspan", toucolspan);
                    $('#he' + he.pop()).remove();
                    hebin--;
                    
                    lenth.pop();
                    treetou.pop();
                    var newtree = a();
                    for (var n = 0; n < newtree.length; n++) {
                        $('#con-' + newtree[n] + ' td:eq(2)').attr("colspan", 1);
                        $('#con-' + newtree[n] + ' td:eq(3)').attr("colspan", toucolspan);
                       
                    }
            }
            var lastcount = [];
            var samegeshu = [];
            $scope.mergecondition = function () {
                treeture=removesame();
                var count = new Array();
                var j = 0;
                var contiues = false;
                var i = 0;
                var lastCon = 0;
                var firstCon = 0;
                var lastone = 0;
                var errorFlag = false;
                $($rootScope.rptcondition + ' tr td input[data-inputfor="mergecondition"]').
                each(function () {
                    if (errorFlag)
                        return;
                    
                    else if ($(this).is(':checked')) {
                        if (contiues == false) {
                            count[i] = $(this).parent().parent().attr("id");
                             
                            lastCon = j;
                            contiues = true;
                            i++;
                        } else {
                            if (j - lastCon != 1) {
                                alert('不连续不能合并');
                                errorFlag = true;
                                return;
                            } else {
                                count[i] = $(this).parent().parent().attr("id");
                                lastCon = j;
                                contiues = true;
                                 
                                i++;
                            }
                        }

                    }
                    j++;
                
                });
                if (count.length == 1) {
                    alert('不能合并单个');
                    return;
                }
                if (errorFlag == false) {

                    var laststr = count[count.length - 1];
                    var c = count.length - 1;
                    while (c) {
                        if (count[c].substr(count[c].length - 2, 2) >= 10) {
                            count[c] = count[c].substr(laststr.length - 2, 2);

                        }
                        else count[c] = count[c].substr(count[c].length - 1, 1);
                        c--;
                    }
                    if (count[0].substr(count[0].length - 2, 2) >= 10) {
                        count[0] = count[0].substr(laststr.length - 2, 2);

                    }
                    else count[0] = count[0].substr(count[0].length - 1, 1);

                    
                    firstCon = count[0];
                    lastone = count[count.length - 1];
                    treetou.push(firstCon);
                    treetoulenth[firstCon] = count.length;
                    var same = geshu(count, treeture);
                    var coun = count.toString();
                    if (same == 0 || countlenth == 1) {
                        c = count.length - 1;
                        while (c) {
                            $('#' + 'chkbox' + count[c] + '').remove();
                            $('#' + 'kong' + count[c] + '').remove();
                            lastone--;
                            c--;
                        }

                        $($compile('<div style="vertical-align:middle;" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '" rowspan="' + count.length + '" aria-hidden="true" class="glyphicon glyphicon-indent-right" ></div>')($scope))
                           .appendTo($('#con-' + firstCon + ' td:eq(3)'));
                        $('#con-' + firstCon + ' td:eq(3)').attr("rowspan", count.length);
                        $('#con-' + firstCon + ' td:eq(3)').css('vertical-align', 'middle');
                         $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#FFEBCD');

                        countlenth++;
                        } 
                    else if(weizhi(count,tree)==0){
                        c = count.length - 1;
                        toucolspan++;
                        $('#tou th:eq(3)').attr("colspan", toucolspan);
                        var t = tree;
                        for (var i = 0; i < count.length; i++) {
                            t.pop();
                        }
                        
                        if (zhen() < treetou.length) {
                            var row = count.length + lenth[lenth.length - 1] - same;

                            $($compile('<td style="width:22px; vertical-align:middle;" rowspan="' + row + '" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '"aria-hidden="true"><div  class="glyphicon glyphicon-indent-right" ></div></td>')($scope))
                                .insertBefore($('#con-' + firstCon + ' td:eq(3)'));
                            for (var i = 1; i < (treetou.length - 1) ; i++) {
                                $('#kong' + treetou[i]).attr("colspan", toucolspan);
                            }
                        } else {
                            var row = count.length + treeture.length - same;
                            $($compile('<td style="width:22px; vertical-align:middle;" rowspan="' + row + '" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '"aria-hidden="true"><div  class="glyphicon glyphicon-indent-right" ></div></td>')($scope))
                                .insertBefore($('#con-' + firstCon + ' td:eq(3)'));
                        }
                        while (c) {
                            $('#' + 'chkbox' + count[c] + '').remove();
                            //$('#con-' + count[c] + ' td:eq(3)').attr("colspan", toucolspan);
                            lastone--;
                            c--;
                        }


                        //for (var i = 1; i < treetou.length; i++) {
                        //    $('#kong' + treetou[i]).attr("colspan", toucolspan);
                        //}
                        // alert("树" + count + "，个数" + treeture + ",same:" + same);

                        if (toucolspan % 2 == 0) {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#B2DFEE');
                        } else {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#FFEBCD');
                        }
                       
                        } 
                    else if(same>=2){
                        c = count.length - 1;
                        toucolspan++;
                        $('#tou th:eq(3)').attr("colspan", toucolspan);
                        var row = treeture.length+count.length-same;
                         
                        $($compile('<td style="width:22px; vertical-align:middle;" rowspan="' + row + '" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '"aria-hidden="true"><div  class="glyphicon glyphicon-indent-right" ></div></td>')($scope))
                            .insertBefore($('#con-' + firstCon + ' td:eq(3)'));
                        while (c) {
                            $('#' + 'chkbox' + count[c] + '').remove();
                            //$('#con-' + count[c] + ' td:eq(3)').attr("colspan", toucolspan);
                            lastone--;
                            c--;
                        }
                         
                        // alert("树" + count + "，个数" + treeture + ",same:" + same);

                        if (toucolspan % 2 == 0) {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#B2DFEE');
                        } else {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#FFEBCD');
                        }
                    } else if (weizhi(count, lastcount) == (count.length-1)) {
                        c = count.length - 1;
                        toucolspan++;
                        $('#tou th:eq(3)').attr("colspan", toucolspan);
                        if (zhen() < treetou.length) {
                            var row = count.length + lenth[lenth.length - 1] - same;

                            $($compile('<td style="width:22px; vertical-align:middle;" rowspan="' + row + '" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '"aria-hidden="true"><div  class="glyphicon glyphicon-indent-right" ></div></td>')($scope))
                                .insertBefore($('#con-' + firstCon + ' td:eq(3)'));
                            for (var i = 1; i < (treetou.length - 1) ; i++) {
                                $('#kong' + treetou[i]).attr("colspan", toucolspan);
                            }
                        } else {
                            var row = count.length + treeture.length - same;

                            $($compile('<td style="width:22px; vertical-align:middle;" rowspan="' + row + '" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '"aria-hidden="true"><div  class="glyphicon glyphicon-indent-right" ></div></td>')($scope))
                                .insertBefore($('#con-' + firstCon + ' td:eq(3)'));


                        }
                        // alert("树" + count + "，个数" + treeture + ",same:" + same);

                        if (toucolspan % 2 == 0) {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#B2DFEE');
                        } else {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#FFEBCD');
                        }
                    } else {
                        c = count.length - 1;
                        toucolspan++;
                        $('#tou th:eq(3)').attr("colspan", toucolspan);
                        var row = count.length + lenth[lenth.length - 1] - same;

                        $($compile('<td style="width:22px; vertical-align:middle;" rowspan="' + row + '" ng-click="removechange(' + coun + ')" title="拆分分组" id="he' + hebin + '"aria-hidden="true"><div  class="glyphicon glyphicon-indent-right" ></div></td>')($scope))
                            .insertBefore($('#con-' + firstCon + ' td:eq(3)'));
                        while (c) {
                            $('#' + 'chkbox' + count[c] + '').remove();
                            $('#con-' + count[c] + ' td:eq(3)').attr("colspan", 1);
                            lastone--;
                            c--;
                        }
                        for (var i = 0; i < treetou.length - 2; i++) {
                            $('#kong' + treetou[i]).attr("colspan", toucolspan);
                        }

                        // alert("树" + count + "，个数" + treeture + ",same:" + same);

                        if (toucolspan % 2 == 0) {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#B2DFEE');
                        } else {
                            $('#con-' + firstCon + ' td:eq(3)').attr('bgcolor', '#FFEBCD');
                        }
                    }
                    lastcount = count;
                    samegeshu.push(same);
                    he.push(hebin);

                    hebin++;
                    $scope.mergs(count);
                    var newctree = a();
                    //alert(newctree);
                   
                        for (var n = 0; n < newctree.length; n++) {
                                $('#con-' + newctree[n] + ' td:eq(2)').attr("colspan", 1);
                                $('#con-' + newctree[n] + ' td:eq(3)').attr("colspan", toucolspan);
                        
                    }


                }
            }
            var a=function () {
                var arr1 = treeindex;
                var arr2 = treeture;
                var arr3 = [];
                var hash3 = {};
                for (var index in arr1) {
                    var i = arr1[index];
                    var temp = hash3["" + i];
                    if (!temp) {
                        hash3["" + i] = 2;
                    } else {
                        hash3["" + i] = temp * 2;
                    }
                }
                for (var index in arr2) {
                    var i = arr2[index];
                    var temp = hash3["" + i];
                    if (!temp) {
                        hash3["" + i] = 5;
                    } else {
                        hash3["" + i] = temp * 5;
                    }
                }
                for (var i in hash3) {
                    var temp = hash3["" + i];
                    if (temp % 10 != 0) {
                        arr3.push(i)
                    }
                }
                return arr3;


            }
            
            $scope.splitcondition = function () {

            }

            $rootScope.mkreport = function (callback) {

                $rootScope.viewid = mkreport.getviewid();
                getcondition(function (exp) {
                    $http.post('/report/mkrpt', {
                        viewid: $rootScope.viewid,
                        cols: $rootScope.selectcols,
                        pagesize: $rootScope.pagesize,
                        pageindex: $rootScope.pageindex,
                        exp: exp
                    }).success(function (data, status, headers, config) {

                        $($rootScope.resultTable + ' tr').remove();
                        var row;
                        for (i = 0; i < data.Cells.length; i++) {
                            if (data.Cells[i].Row != row) {
                                $($rootScope.resultTable).append('<tr></tr>');
                                row = data.Cells[i].Row;
                            }
                            var str = data.Cells[i].FmtValue;
                            if (str == '')
                                str = '&nbsp;';
                            $($rootScope.resultTable + ' tr:last').append('<td>' + str + '</td>');
                        }
                        $rootScope.totalpages = data.TotalPages;
                        if (callback != undefined) {
                            callback();
                        }

                    });
                });
            };


            require(['domReady'],
             function (domReady) {
                 domReady(function () {
                     var select = $('#rpt-candidate');
                     if (select.length > 0) {
                         $('#rpt-candidate').change(function () {
                             $('#rtp-selecttype').empty();
                             $('#rtp-selecttype').append($(this).children('option:selected').attr('data-selectvals'));
                             if ($('#rtp-selecttype option').length == 1) {
                                 $scope.add();

                             }
                         });
                     }





                     $('[data-toggle="tooltip"]').tooltip();
                 });

             });

        });

        rpt.controller("ShowReportController", function ($rootScope, $scope, $http, mkreport) {

            $scope.back = function () {
                $($rootScope.rptdiag).modal('hide');
                $rootScope.pageindex = 1;
                $($rootScope.mkdiag).modal('show');
            };
            $scope.next = function () {
                if ($rootScope.pageindex < $rootScope.totalpages) {
                    $rootScope.pageindex++;
                    $rootScope.mkreport();
                }
            }
            $scope.pre = function () {
                if ($rootScope.pageindex > 1) {
                    $rootScope.pageindex--;
                    $rootScope.mkreport();
                }
            }
        });

        rpt.provider('mkreport', function () {
            this.$get = function ($http, $rootScope) {

                var service = {}

                var viewid;

                service.getviewid = function () {
                    return viewid;
                };
                service.setviewid = function (vid) {
                    viewid = vid;
                };

                var page;
                service.getpage = function () {
                    return page;

                }
                service.initquery = function (viewid) {
                    service.setviewid(viewid);
                    $http.post('/report/mkqview',
                        {
                            viewid: viewid
                        }).success(function (data, status, headers, config) {

                            $('#rpt-candidate').empty();
                            $rootScope.candidatecols = [];
                            var str = '';
                            for (i = 0; i < data.length; i++) {



                                str += '<option value =  "' + data[i].ID + '" data-selectvals="';

                                var candidatecol = {};
                                candidatecol = data[i];
                                for (j = 0; j < data[i].QueryTypes.length; j++) {
                                    if (j == 0)
                                        str += '<option selected value=\'' + data[i].QueryTypes[j].ID + '\'>';
                                    else
                                        str += '<option value=\'' + data[i].QueryTypes[j].ID + '\'>';
                                    str += data[i].QueryTypes[j].Name;
                                    str += '</option>';
                                }
                                str += '">';
                                str += data[i].Name;
                                str += '</option>';
                                $rootScope.candidatecols.push(candidatecol);

                            }

                            $('#rpt-candidate').append(str);

                            $('#rptdiag').modal();
                        });
                }


                return service;
            }
        });
    });

});