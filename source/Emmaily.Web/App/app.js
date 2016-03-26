var app = angular.module('app', [
    // Angular modules 
    'ngAnimate', // animations
    'ngRoute', // routing
    'ngSanitize', // sanitizes html bindings (ex: sidebar.js)
    "ui.router",
    "oc.lazyLoad",
    // 3rd Party Modules
    'ui-rangeSlider',
    'ngTagsInput',
    'angularMoment',
    'ui.tinymce',
    'theaquaNg',
    'daterangepicker',
    'ui.bootstrap.datetimepicker',
    'frapontillo.bootstrap-switch',
    'ui.slimscroll',
    'nya.bootstrap.select',
    'humenize',
    'toaster',
    'SignalR',
    'ui.gravatar', //gravtaar for user
    'ui.select', //ui-select for dropdown and multi values.
    'ui.bootstrap', // ui-bootstrap (ex: carousel, pagination, dialog)
    'blueimp.fileupload', //jQuery File Uploader Component
    'rzModule', //slider module
    'angular.filter',
    'angular-google-analytics',
    'datatables',
    'chart.js',
    'xeditable',
    'ngJsTree',
    'colorpicker.module',
    'ui.tinymce',
    'mwl.confirm',
    'ngPasswordStrength',
    'ui.codemirror',
    'color.picker'
]);
app.constant('angularMomentConfig', { preprocess: 'utc' });
app.config([
    '$ocLazyLoadProvider', '$controllerProvider', '$httpProvider', 'fileUploadProvider', 'AnalyticsProvider', function ($ocLazyLoadProvider, $controllerProvider, $httpProvider, fileUploadProvider, AnalyticsProvider) {
        AnalyticsProvider
            .setAccount({
                tracker: 'UA-15078156-55',
                crossDomainLinker: true,
                fields: {
                    cookieDomain: 'emaily.xyz',
                    cookieName: 'emaily',
                    cookieExpires: 200000
                },
                crossLinkDomains: ['emaily.xyz'],
                displayFeatures: true,
                trackEvent: true,
                enhancedLinkAttribution: true,
                set: {
                    forceSSL: true
                }
            })
            .setDomainName('emaily.xyz')
            .logAllCalls(true)
            .startOffline(true);

        $ocLazyLoadProvider.config({
            // global configs go here
        });
        $controllerProvider.allowGlobals();
        $httpProvider.interceptors.push('redirectInterceptor');
        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        fileUploadProvider.defaults.redirect = window.location.href.replace(/\/[^\/]*$/, '/cors/result.html?%s');
        if (1) {
            // Demo settings:
            angular.extend(fileUploadProvider.defaults, {
                disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
                maxFileSize: Math.pow(1024, 3) * 200, // 200GB
                maxChunkSize: Math.pow(1024, 2) * 2,
                acceptFileTypes: /(^Video\/(x-ms-wmv|mp4|avi|x-flv)+$)|(\.|\/)(264|787|3gp2|3gpp|3p2|aaf|aep|aetx|ajp|amv|amx|arf|avb|axm|bdmv|bin|bmk|camrec|clpi|cmmp|cmmtpl|cmproj|cmrec|cst|cvc|d2v|d3v|dat|dce|dck|dir|dmb|dmsd|dmsd3d|dmss|dpa|dpg|dv|dv-avi|dvr|dvx|dxr|dzt|evo|eye|f4p|fbz|fcp|flc|flh|fli|gfp|gts|hkm|ifo|imovieproject|ismc|ivf|ivr|izz|izzy|jts|jtv|m1pg|m21|m21|m2t|m2ts|m2v|mgv|mj2|mjp|mnv|mp21|mp21|mpgindex|mpl|mpls|mpv|mqv|msdvd|mswmm|mtv|mvb|mvd|mve|mvp|mvp|mvy|ncor|nsv|nuv|nvc|ogm|ogx|pgi|photoshow|piv|pmf|ppj|prel|pro|prtl|pxv|qtl|qtz|rdb|rec|rmd|rmp|rms|roq|rsx|rum|rv|sbk|scc|screenflow|seq|sfvidcap|smi|smk|ssm|stl|svi|swt|tda3mt|tivo|tod|tp|tp0|tpd|tpr|tsp|tvs|vc1|vcpf|vcv|vdo|vdr|vep|vfz|vgz|viewlet|vlab|vp6|vp7|vpj|vsp|wcp|wmd|wmmp|wmx|wp3|wpl|wvx|xej|xel|xesc|xfl|xlmv|zm1|zm2|zm3|zmv|aepx|ale|avp|avs|bdm|bik|bsf|camproj|cpi|divx|dmsm|dream|dvdmedia|dvr-ms|dzm|dzp|edl|f4v|fbr|fcproject|hdmov|imovieproj|ism|ismv|m2p|m4v|mkv|mod|moi|mpeg|mts|mxf|ogv|pds|prproj|psh|r3d|rcproject|rmvb|scm|smil|sqz|srt|stx|swi|tix|trp|ts|veg|vf|vro|webm|wlmp|wtv|xvid|yuv|anim|bix|dsy|gl|grasp|gvi|ivs|lsf|m15|m4e|m75|mmv|mob|mpeg4|mpf|mpg2|mpv2|msh|rmd|rts|scm|sec|tdx|viv|vivo|vp3|3gpp2|3mm|60d|aet|avd|avs|bnp|box|bs4|byu|dav|ddat|dif|dlx|dmsm3d|dnc|dv4|fbr|flx|gvp|h264|irf|iva|k3g|lrec|lsx|m1v|m2a|m4u|meta|mjpg|modd|moff|moov|movie|mp2v|mp4v|mpe|mpsub|mvc|mys|osp|par|playlist|pns|pssd|pva|pvr|qt|qtch|qtm|rp|rts|sbt|scn|sfd|sml|smv|spl|str|vcr|vem|vft|vfw|vid|video|vs4|vse|w32|wm|wot|3g2|3gp|asf|asx|avi|flv|mov|mp4|mpg|rm|swf|vob|wmv|gif|svg|jpe?g|png|tif|bmp|xls|xlsx|ppt|pptx|txt|rtf|pdf|doc|docx|html|htm|mp3|mp4|wav|3gp|aac|m4a|m4p|ogg|wma|vox)$/i,
                maxNumberOfFiles: 20,
                maxRetries: 100,
                retryTimeout: 500
            });
        }
    }
]);
app.factory('redirectInterceptor', function () {
    return {
        'response': function (response) {
            if (typeof response.data === 'string' && response.data.indexOf("action=\"/account/login\"") > -1) {
                return 'Unauthorized';
            } else {
                return response;
            }
        }
    }
});

app.factory('settings', [
    '$rootScope', function ($rootScope) {
        // supported languages
        var settings = {
            layout: {
                pageSidebarClosed: false, // sidebar menu state
                pageBodySolid: false, // solid body color state
                pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
            },
            layoutImgPath: Metronic.getAssetsPath() + 'admin/layout/img/',
            layoutCssPath: Metronic.getAssetsPath() + 'admin/layout/css/'
        };

        $rootScope.settings = settings;

        return settings;
    }
]);
app.controller('AppController', [
    '$scope', function ($scope) {
        $scope.$on('$viewContentLoaded', function () {
            Metronic.initComponents();
        });
    }
]);
app.factory('about', ['dataService',function (db) {
    var appName = document.querySelector('html').dataset.appName;
    var theme = document.querySelector('html').dataset.appTheme;

    var factory = {
        name: appName,
        theme: theme,
        logo: theme,
        get: function () {
            return db.get('app');
        },
        topNav:function(code) {
            return db.get("nav/items/" + code);
        },
        me: function () {
            return db.get('account/me/profile');
        },
        logout: function () {
            return db.postEx('/account/logoff',{}).then(function() {
                window.location = "/account/login";
            });
        },
        client: function (clientId) {
            return db.get('client/'+clientId);
        },
        enums: function () {
            return db.get('util/enums');
        }
    }
    return factory;
}]);
app.controller('HeaderController', ['$scope',function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initHeader(); // init header
    });
}]);

/* Setup Layout Part - Sidebar */
app.controller('SidebarController', ['$scope', function ($scope) {
    $scope.$on('$includeContentLoaded', function () {
        Layout.initSidebar(); // init sidebar
    });
}]);

/* Setup Layout Part - Footer */
app.controller('FooterController', ['$scope', 'about', function ($scope, about) {
    $scope.name = about.name;
    $scope.year = new Date().getFullYear();
    $scope.$on('$includeContentLoaded', function () {
        Layout.initFooter(); // init footer
    });
}]);

/* Init global settings and run the app */
app.run(["$rootScope", "settings", "$state", 'about', '$urlRouter', 'Talker', 'editableOptions', '$http', function ($rootScope, settings, $state, about, $urlRouter, talker, editableOptions, $http) {

    editableOptions.theme = 'bs3';
    var enums;
    var role = document.querySelector('html').dataset.appRoles.toLowerCase();

    var n = 24;
    $rootScope.sizes = [n * 1, n * 2, n * 3, n * 4, n * 5];
    $rootScope.canImpersonate = role == 'admin';
    $rootScope.canPermission = role == 'admin';

    $rootScope.encoder = '/upload';
    $rootScope.cols = 'col-xl-2 col-lg-3 col-md-4 col-sm-6 col-xs-12';
    $rootScope.uploadList = {};
    $rootScope.maxAttachmentSize = 10485760;
    $rootScope.fields = [
        { id: 1, name: 'Text' },
        { id: 3, name: 'Number' },
        { id: 7, name: 'Email' },
        { id: 9, name: 'Telephone' },
        { id: 10, name: 'Url' },
        { id: 4, name: 'Date' },
        { id: 8, name: 'Time' },
        { id: 12, name: 'Month' },
        { id: 2, name: 'Checkbox' },
        { id: 6, name: 'Color' },
        { id: 13, name: 'Picture' },
        { id: 14, name: 'Html' },
        { id: 15, name: 'Multiline' }
    ];

    $rootScope.navigations = [
        { icon: 'paper-plane-o', title: 'Campaigns', route: 'campaigns', childs: [] },
        { icon: 'code', title: 'Templates', route: 'templates', childs: [] },
        { icon: 'th-list', title: 'Lists', route: 'lists', childs: [] },
        { icon: 'pie-chart', title: 'Reports', route: 'reports', childs: [] }
    ];

    $rootScope.search = function (q) {
        var d = angular.copy($state.current.data); d.q = q;
        $state.go($state.current.name, d);
    }

    $rootScope.refreshAddresses = function (address) {
        var params = {
            address: address,
            sensor: false
        };
        return $http.get(
            'http://maps.googleapis.com/maps/api/geocode/json', {
                params: params
            }
        ).then(function (response) {
            $rootScope.addresses = response.data.results;
        });
    };

    function findPermission(me, name) {
        return { write: me.write.indexOf(name) > -1, read: me.read.indexOf(name) > -1 || me.write.indexOf(name) > -1 };
    }
    function buildPermission(me) {
        var perms = {}
        for (var x in enums.apiaccessenum) {
            perms[enums.apiaccessenum[x].toLowerCase()] = findPermission(me, enums.apiaccessenum[x]);
        }
        return perms;
    }
    
    function onAnyMessage(mode,msg) {
        console.log('Next', mode,msg);
    }

    function getMySelf() {
        return about.me().then(function (me) {
            $rootScope.user = me.id;
            $rootScope.me = me;    
            $rootScope.brands = me.apps;
            if (me.role == 'Admin') {
                me.Write = enums.apiaccessenum.join(' ');//HACK?
            }
            $rootScope.perm = buildPermission(me);
            talker.on('any', onAnyMessage);
            talker.connect(me.id);
        });
    }

    function getEnum() {
        return about.enums().then(function (d) {
            enums = d;
            $rootScope.enums = d;
        });
    }

    $rootScope.tinymceOptions = {
        onChange: function (e) {
            // put logic here for keypress and cut/paste changes
        },
        body_class:'container',
        menubar: false,
        height: 500,
        convert_urls: false,
        inline: false,
        verify_html: false,
        content_css: [
            '//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css',
            '//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css',
            '//maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css'
        ],
        plugins: [
                "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                "searchreplace wordcount visualblocks visualchars code fullscreen",
                "insertdatetime media nonbreaking save table contextmenu directionality",
                "emoticons template textcolor colorpicker textpattern imagetools codemirror"
        ],
        skin: 'lightgray',
        theme: 'modern',
        codemirror: {
            indentOnInit: true, // Whether or not to indent code on init. 
            path: '/scripts/code-mirror', // Path to CodeMirror distribution
            config: {           // CodeMirror config object
                mode: 'text/html',
                lineNumbers: true
            }
        },
        toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image media | forecolor backcolor | preview code",
        image_advtab: true
    };

    $rootScope.logout = about.logout;
    $rootScope.about = about.me;

    getEnum().then(getMySelf);

    $rootScope.$state = $state; // state to be accessed from view

}]);