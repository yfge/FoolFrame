define('setextype', ['swapp', 'bootstrap', 'jquerydatetime', 'typeahead', 'showerror', 'Handlebars'], function (swapp) {

    swapp.regmodule('setextype', [], function (setextype, $) {
        setextype.provider('setextype', function () {
            this.$get = function ($location, $http) {
                var Handlebars = require('Handlebars');

                var substringMatcher = function (viewitem, viewName, id, ownerid, opt) {
                    return function findMatches(q, cb, cba) {
                        var matches, substringRegex;
                        matches = [];


                        var addflag = false;
                        if (opt == 'new') {
                            addflag = true;

                        }


                        $http.post('/data/inputquery', {
                            viewid: viewName,
                            itemid: viewitem,
                            text: q,
                            objid: id,
                            newadd: addflag,
                            ownerid: ownerid

                        }).success(function (data, status, headers, config) {

                            if (data.error != undefined) {
                                showerror.showerrormsg(data.error);
                            }
                            for (i = 0; i < data.length; i++) {

                                matches.push(data[i]);
                            }
                            cba(matches);

                        });

                    };
                };
                var setedit = function (prType, property, idpre, objId, modelid, viewItem, text, viewName, jqueryDoc, id, ownerid, opt) {
                    if (prType == "15") {

                        //枚举
                        var obj = jqueryDoc;

                        $http.post('/model/getenum', {
                            modelid: modelid
                        }).success(function (data, status, headers, config) {

                            if (data.error != undefined) {
                                showerror.showerrormsg(data.error);
                            }
                            var appStr = '<select class="form-control"  id="' + idpre + property + '" name="' + property + '">';
                            //  $("[data-property='" + property + "']").append();
                            for (i = 0; i < data.length; i++) {

                                if (data[i].Value == objId)
                                    appStr += '<option value="' + data[i].Value + '" selected="selected">' + data[i].Name + '</option>';
                                else
                                    appStr += '<option value="' + data[i].Value + '">' + data[i].Name + '</option>';
                            }
                            appStr += '</select>';
                            jqueryDoc.append(appStr);
                        });

                    }
                    else if (prType == "8") {
                        //布尔
                        var checkStr = '<input type="checkbox" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "' + objId + '"';
                        if (objId.toLowerCase() == "true")
                            checkStr += " checked/>";
                        else
                            checkStr += "/>";
                        jqueryDoc.append(checkStr);

                    }
                    else if (prType == "12") {
                        var ipos = text.indexOf(" ");
                        var str1 = text.substring(0, ipos); //取前部分
                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "'
                    + '' + '" value="' + str1 + '"/>');
                    }
                    else if (prType == "13") {
                        var ipos = text.indexOf(" ");
                        var str1 = text.substring(ipos, text.length); //取后部分
                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "'
                    + '' + '" value="' + str1 + '"/>');
                    }
                    else if (prType == "1" || prType == "2") {
                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "'
                                 + objId + '" value="' + text + '"onkeyup="' + "value=value.replace(/[^0-9]/g,'') " +
                                  '"onbeforepaste="' + "clipboardData.setData('text',clipboardData.getData('text').replace(/[^0-9]/g,'')" + '"maxlength="4"'
                                  + '"/>');
                    }
                    else if (prType == "3" || prType == "4") {
                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "'
                                 + objId + '" value="' + text + '"onkeyup="' + "value=value.replace(/[^0-9]/g,'') " +
                                  '"onbeforepaste="' + "clipboardData.setData('text',clipboardData.getData('text').replace(/[^0-9]/g,'')" + '"maxlength="8"'
                                  + '"/>');

                    }
                    else if (prType == "5") {

                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "'
                                + objId + '" value="' + text + '"onkeyup="' + "value=value.replace(/[^0-9.]/g,'')" +
                                 '"onbeforepaste="' + "clipboardData.setData('text',clipboardData.getData('text').replace(/[^0-9.]/g,'')" + '"maxlength="4"'
                                 + '"/>');
                    }
                    else if (prType == "6") {

                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "'
                                + objId + '" value="' + text + '"onkeyup="' + "value=value.replace(/[^0-9.]/g,'')" +
                                 '"onbeforepaste="' + "clipboardData.setData('text',clipboardData.getData('text').replace(/[^0-9.]/g,'')" + '"maxlength="8"'
                                 + '"/>');
                    }
                    else
                        jqueryDoc.append('<input type="text" id="' + idpre + property + '" class="form-control" data-propertyinput="' + property + '" data-propertyinputvalue= "' + objId + '" value="' + text + '"/>');
                    if (prType == "12") {

                        $(function () {
                            $('#' + idpre + property).ymdateplugin({
                                changeMonth: true,
                                changeYear: true,
                                dateFormat: 'yy/m/d',
                                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                                monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],

                            });
                        });

                    }
                    if (prType == "13") {
                        $(function () {
                            $('#' + idpre + property).timepicki({

                                show_meridian: false,
                                min_hour_value: 0,
                                max_hour_value: 23,
                                min_minutes_value: 0,
                                max_minutes_value: 59,
                                step_size_minutes: 1,
                                overflow_minutes: true,
                                increase_direction: 'up',
                                disable_keyboard_mobile: false,
                            });
                        });
                    }
                    if (prType == "14") {

                        $(function () {


                            $('#' + idpre + property).ymdateplugin({
                                showTimePanel: true,
                                changeMonth: true,
                                changeYear: true,
                                dateFormat: 'yy/m/d',
                                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                                monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],

                            });
                        });

                        //var datetime = new Date();
                        //var str = '';
                        //str += datetime.getFullYear() + '/';
                        //str += datetime.getMonth() + 1 + '/';
                        //str += datetime.getDate() + ' ';
                        //if (datetime.getHours() < 10) str +='0'+ datetime.getHours() + ':';
                        //else str += datetime.getHours() + ':';
                        //if (datetime.getMinutes() < 10) str += '0' + datetime.getMinutes() + ':';
                        //else str += datetime.getMinutes() + ':';
                        //if (datetime.getSeconds() < 10) str += '0' + datetime.getSeconds() + ':';
                        //else str += datetime.getSeconds() ;

                        //if (str != "") {

                        //    $('#' + idpre + property).val(str);
                        //}

                    }
                    if (prType == "16") {
                        var oid = ownerid;
                        if (oid == undefined)
                            oid = '';
                        var postid = id;
                        if (postid == undefined)
                            postid = '';
                        $("#" + idpre + property).typeahead({
                            minLength: 1,
                            highlight: true
                        },
                            {

                                source: substringMatcher(viewItem, viewName, postid, oid, opt),
                                display: function (item) {

                                    return item.Text;

                                },
                                templates: {
                                    empty: [
                                      '<div class="empty-message">',
                                        '未找到匹配的选项',
                                      '</div>'
                                    ].join('\n'),
                                    suggestion: Handlebars.compile('<div><strong>{{Text}}</strong> – {{Id}}</div data-propertyvalue="{{Id}}">'),
                                    footer: [
                                        '<div>',
                                        '查找更多',
                                        '</div>'
                                    ],
                                    pending: [

                                        '<div>',
                                        '正在查询....',
                                        '</div>'
                                    ]

                                }
                            })
                        ;
                        $("#" + idpre + property).bind(
                    'typeahead:idle', function (ev) {
                        //if($('[data-property="' + property + '"]').attr("data-propertyvalue")
                        console.log("#" + idpre + property + ' idle ..');


                    });

                        $("#" + idpre + property).bind(
                        'typeahead:select', function (ev, suggestion) {
                            // $scope.typeaheadflag[idpre + property] = true;
                            $('[data-property="' + property + '"]').attr("data-propertyvalue", suggestion.Id);
                            $('[data-property="' + property + '"]').attr("data-fmtvalue", suggestion.Text);
                            $("#" + idpre + property).on('typeahead:changed').unbind();
                            $("#" + property).attr('data-propertyinputvalue', suggestion.Id);


                        });


                    }

                }


                    
                    /***
                    *prtype  控件的属性类型
                    *prModel 控件的modelid
                    *prStates 控件的状态值（当为枚举时)
                    *inputtype 输入类型，目前未启用
                    *contextappend 要加入控件的jquery上下文
                    *appendid 要加入的控件的id
                    *
                    *
                    */
                    var mkinput = function (prname, prType, prModel, prStates, contexttoappend, appendid, viewid, inputtype) {
                        var inputattr = 'data-inputtype=' + prType + ' data-inputmodel="' + prModel + '"';
                        var preInput = $('#' + appendid);
                        var toaddnew = true;
                        if (preInput.length > 0) {
                            //如果已经存在的情况 
                            toaddnew = false;
                            var pretype = preInput.attr("data-inputtype");
                            var premodel = preInput.attr("data-inputmodel");
                            preInput.attr("data-inputtype", prType);
                            preInput.attr("data-inputmodel", prModel);
                            if (pretype == prType) {
                                //输入类型相同，输入模型相同，返回
                                if (premodel == prModel)
                                    return;
                                //类型相同,模型不同，这时只有可能是枚举或是businessobj
                                if (prType == "15") {
                                    //枚举
                                    //1 移除原有值
                                    $('#' + appendid + ' option ').remove();
                                    //step2 加入新值
                                    for (i = 0; i < prStates.length; i++) {
                                        preInput.append('<option value="' + prStates[i].Value + '">' + prStates[i].String + '</option>');
                                    }
                                    return;
                                } else {
                                    //businessObj

                                preInput.typeahead('destroy');

                            }

                        } else {

                            if (pretype == "8" ||
                                prType == "8" ||
                                pretype == "15" ||
                                prType == "15") {
                                preInput.remove();
                                toaddnew = true;
                            }

                            //if (pretype == "12" || pretype == "14") {
                                
                            //}
                            
                            if (pretype == "16") {
                                preInput.typeahead('destroy');
                            }


                        }

                    }
                    if (prType != "14" || prType != "12") {

                        $('#' + appendid).remove();
                        contexttoappend.append('<input type="text" id="' + appendid + '" ' + inputattr + ' />');

                    }
                    if (toaddnew) {
                       
                        if (prType == "15") {

                            var appStr = '<select ' + inputattr + '  id="' + appendid + '">';
                            for (i = 0; i < prStates.length; i++) {
                                appStr += '<option value="' + prStates[i].Value + '">' + prStates[i].String + '</option>';
                            }
                            appStr += '</select>';

                            contexttoappend.append(appStr);

                        }
                        else if (prType == "8") {
                            //布尔
                            var checkStr = '<input type="checkbox" id="' + appendid + '" ' + inputattr + '/>';
                            contexttoappend.append(checkStr);

                        }
                        

                    }

                    if (prType == "12") {

                        $(function () {
                            $('#' + appendid).ymdateplugin({
                                changeMonth: true,
                                changeYear: true,
                                dateFormat: 'yy/m/d',
                                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                                monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],

                            });
                        });

                    }
                    if (prType == "13") {
                        $(function () {
                            $('#' + appendid).timepicki({

                                show_meridian: false,
                                min_hour_value: 0,
                                max_hour_value: 23,
                                min_minutes_value: 0,
                                max_minutes_value: 59,
                                step_size_minutes: 1,
                                overflow_minutes: true,
                                increase_direction: 'up',
                                disable_keyboard_mobile: false,
                            });
                        });
                    }
                    if (prType == "14") {

                        $(function () {


                            $('#' + appendid).ymdateplugin({
                                showTimePanel: true,
                                changeMonth: true,
                                changeYear: true,
                                dateFormat: 'yy/m/d',
                                dayNamesMin: ["日", "一", "二", "三", "四", "五", "六"],
                                monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],

                            });
                        });



                    }
                    if (prType == "16") {
                        $("#" + appendid).typeahead({

                            highlight: true
                        },
                            {
                                name: appendid,
                                source: substringMatcher(prname, viewid, '', '', ''),
                                display: function (item) {

                                    return item.Text;

                                },
                                limit: 5,
                                templates: {
                                    empty: [
                                      '<div class="empty-message">',
                                        '未找到匹配的选项',
                                      '</div>'
                                    ].join('\n'),
                                    suggestion: Handlebars.compile('<div><strong>{{Text}}</strong> – {{Id}}</div data-propertyvalue="{{Id}}">'),
                                    footer: [
                                        '<div>',
                                        '查找更多',
                                        '</div>'
                                    ],
                                    pending: [

                                        '<div>',
                                        '正在查询....',
                                        '</div>'
                                    ]

                                }
                            });


                        $("#" + appendid).bind(
                        'typeahead:select', function (ev, suggestion) {
                            // $scope.typeaheadflag[idpre + property] = true;
                            //  
                            $("#" + appendid).attr("data-propertyvalue", suggestion.Id);
                            // 
                            $("#" + appendid).attr("data-fmtvalue", suggestion.Text);
                            //$("#" + appendid).on('typeahead:changed').unbind();
                            $("#" + appendid).attr('data-propertyinputvalue', suggestion.Id);


                        });


                        }

                       

                    }
              
                    var getval = function (inputid) {
                        var input = $('#' + inputid);
                        if (input.length > 0) {
                            var type = input.attr("data-inputtype");
                            var model = input.attr("data-inputmodel");
                            if (type == "15") {
                                return {
                                    val: input.val(),
                                    text: input.find("option:selected").text()
                                };

                        } else if (type == "8") {

                            var val = false;
                            if ($('#' + inputid + ':checked').length > 0)
                                val = true;
                            var text = '否';
                            if (val)
                                text = '是';
                            return {
                                val: val,
                                text: text

                            };
                        } else if (type == "16") {
                            var val = input.attr('data-propertyvalue');
                            var text = input.val();
                            return {
                                val: val,
                                text: text
                            }
                        } else {
                            return {
                                val: input.val(),
                                text: input.val()
                            };
                        }

                    }
                    return undefined;


                }
                var service = {
                    setextype: setedit,
                    makeinput: mkinput,
                    getval: getval,
                };

                return service;

            };


        });

    })
});