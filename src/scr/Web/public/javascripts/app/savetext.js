define('savetext', ['swapp', 'ngCookies', 'bootstrap','operation'], function (swapp) {

    swapp.regmodule('savetext', ['ngCookies','operation'], function (savetext, $) {
        savetext.provider('savetext', function () {
            this.$get = function () {
                var save = function (select) {
                    var savearry = [];
                    $(select).each(function () {
                        var text, property, objId;
                        if ($(this).attr("data-readonly") =='false'){
                        property = $(this).attr("data-property");
                        if ($(this).attr("data-propertyType") != "16") {

                            if ($('input[type="checkbox"]', this).length > 0) {
                                if ($('input[type="checkbox"]:checked', this).length > 0) {
                                    objId = true;
                                    text = objId;
                                } else {
                                    objId = false;
                                    text = objId.toString();
                                }


                            } else if ($('input', this).length > 0) {
                                //text
                                text = $('input', this).val();
                                objId = text;//$('input', this).attr("data-propertyinputvalue");
                            }
                            if ($('select', this).length > 0) {
                                objId = $('select', this).val();
                                text = $('select', this).find("option:selected").text()
                            }
                            $(this).attr("data-fmtvalue", text);
                            $(this).attr("data-propertyvalue", objId);

                        } else {

                            text = $('input[id]', this).val();
                            if (text.toString() == '') {
                                text = '';
                                objId = '';
                            } else {
                                text = $(this).attr("data-fmtvalue");
                                objId = $(this).attr("data-propertyvalue");
                            }
                            $('input', this).typeahead('destroy');
                        }

                        $('input', this).remove();
                        $('select', this).remove();
                        savearry.push({ Key: property, Value: objId });

                        if (text == null ||text == '')
                            text = '&nbsp;';
                        $(this).append('<p>' + text + '</p>');

                    }
                    });
                    return savearry;
                    
                }

                var service = {
                    savetext: save
                };

                return service;

            };


        });

    })
});