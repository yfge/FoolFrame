//config file ..

requirejs.config({
    baseUrl: '../javascripts/lib',

    paths: {
        //swapp -- app init .
        swapp: '../app/swapp',
        //appmodule
        
        showerror: '../app/showerror',
        serverUtil:'../app/ServerUtil',
        login: '../app/login',
        menu: '../app/menuinfo',
        timer: '../app/timer',
        querylistdata: '../app/querylistdata',
        detailview: '../app/detailview',
        operation: '../app/operation',
        groupview:'../app/groupview',
        subitem:'../app/subitem',
        Handlebars: 'handlebars-v4.0.5',
        Sudoku: '../app/Sudoku',
        mkreport:'../app/mkreport',
        setextype: '../app/setextype',
        message: '../app/message',
        savetext: '../app/savetext',
        mapview: '../app/mapview',
        swchartline: '../app/swchartline',
        navbar:'../app/navbar',
        optionSet :'../app/optionSet',
        viewWithChart:'../app/viewWithChart',

        //util
        baidumaputil:'../app/baidumaputil',
        //libs
        jquery: 'jquery-1.10.2',
        bootstrap:'bootstrap.min',
        angular: 'angular',
        ngRoute: 'angular-route',
        ngCookies: 'angular-cookies.min',
        typeahead: 'typeahead.jquery',
        domReady: 'domReady',
        jqueryUI: 'jquery-ui.min',
        jquerytime: 'timepicki',
        jquerydatetime: 'jquery-ym-datePlugin-0.1' ,
        echarts: 'http://echarts.baidu.com/dist/echarts.min',
        
        baidumap: 'http://api.map.baidu.com/api?v=2.0&ak=d93uy5PGxv8udM93irHVF9GY'
        
       
    
    },

    shim: {
        'setextype': {
            deps: ['jquerytime'],
            exports: 'setextype'
        },
        'savetext': {
            deps: ['jquery'],
            exports: 'savetext'
        },
        'jquerydatetime': {
            deps: ['jquery'],
            exports: 'jquerydatetime'
        },
        
        'jquerytime': {
            deps: ['jquery'],
            exports: 'jquerytime'
        },
        'jqueryUI': {
            deps: ['jquery'],
            exports: 'jqueryUI'
        },
        'showerror':{
            deps:['swapp'],
            exports:'showerror'
        },
        'login':{
            deps:['showerror','swapp','ngRoute','ngCookies'],
            exports:'login'
        },
        'menu': {
            deps: ['showerror', 'swapp', 'ngRoute', 'ngCookies'],
            exports: 'menu'
        },
        'timer': {
            deps: ['angular'],
            exports: 'timer'
        },
        'querylistdata': {
            deps:['angular','timer','showerror','ngCookies','menu','mkreport'],
            exports: 'querylistdata'
        },
        'Sudoku': {
            deps: ['angular', 'querylistdata', 'message', 'detailview', 'groupview',
                'subitem', 'timer', 'showerror', 'ngCookies', 'menu', 'mapview', 'swchartline'],
            exports: 'Sudoku'
        },
        'angular': {
            exports: 'angular'
        },
        'jquery': {
            
            exports:'$'
        },
        'bootstrap':{
            deps:['jquery'],
            exports:'bootstrap'
        },
        'swapp': {
            exports:'swapp'
        },
        'ngRoute': {
            deps: ['angular'],
            exports:'ngRoute'
        },
        'ngCookies': {
            deps: ['angular'],
            exports:'ngCookies'
        },
        'detailview': {
            deps: ['swapp', 'ngCookies', 'jquerytime', 'savetext', 'jquerydatetime',
                'typeahead', 'showerror', 'querylistdata', 'operation', 'Handlebars'],
            exports: 'detailview'
        },
        'Handlebars': {
            deps: ['jquery'],
            exports: 'Handlebars'
        },
        'typeahead': {
            deps: ['jquery'],
            exports:'typeahead'
        },
        'message': {
            deps: ['swapp', 'angular', 'showerror'],
            exports:'message'

        },
        'serverUtil': {
            deps: ['swapp', 'angular', 'showerror'],
            exports:'serverUtil'
        },
        'swchartLine': {
            deps:['angular','showerror','swapp','ngRoute','ngCookies'],
            exports: 'swchartLine'
        },
        'viewWithChart': {
            deps: ['swapp', 'querylistdata', 'domReady', 'echarts', 'navbar',
    'showerror', 'mkreport', 'timer', 'ngCookies', 'mkreport', 'serverUtil', 'swchartline'],
            exports: 'viewWithChart'
        }
    

    }
});
 