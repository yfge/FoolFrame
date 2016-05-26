$(function () { 
    $('#detail-tab a:first').tab('show');
    $('#detail-tab a').click(function (e) {
        e.preventDefault()
        $(this).tab('show')
    });
});