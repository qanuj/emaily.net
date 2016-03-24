app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/");
    var root = "App/views/";

    $stateProvider
        .state('account', {
            url: "/account",
            templateUrl: root + "account/account.html",
            data: { title: 'Account' },
            controller: "accountController",
            resolve: {}
        }).state('password', {
            url: "/account/password",
            templateUrl: root + "account/account.html",
            data: { title: 'Account', mode: 'password' },
            controller: "accountController",
            resolve: {}
        })
        .state('send', {
            url: "/send",
            templateUrl: root + "share/share.ui.html",
            data: { title: 'Send' },
            controller: "shareUiController",
            resolve: {}
        })
        .state('companies', {
            url: "/companies?page&parent&pageSize&q&mode",
            templateUrl: root + "company/company.cards.html",
            data: { title: 'Companies' },
            controller: "companyIndexController",
            resolve: {}
        })
        .state('editCompany', {
            url: "/company/edit/:id",
            templateUrl: root + "company/company.edit.html",
            data: { title: 'Company' },
            controller: "companyEditController",
            resolve: {}
        })
        .state('createCompany', {
            url: "/company/create",
            templateUrl: root + "company/company.create.html",
            data: { title: 'Company' },
            controller: "companyCreateController",
            resolve: {}
        })
        .state('campaigns', {
            url: "/campaigns?page&parent&pageSize&q&mode",
            templateUrl: root + "campaign/campaign.cards.html",
            data: { title: 'Campaigns' },
            controller: "campaignIndexController",
            resolve: {}
        })
        .state('editCampaign', {
            url: "/company/edit/:id",
            templateUrl: root + "campaign/campaign.edit.html",
            data: { title: 'Campaign' },
            controller: "campaignEditController",
            resolve: {}
        })
        .state('createCampaign', {
            url: "/company/create",
            templateUrl: root + "campaign/campaign.create.html",
            data: { title: 'Campaign' },
            controller: "campaignCreateController",
            resolve: {}
        })
        .state('reports', {
            url: "/reports?page&parent&pageSize&q&mode",
            templateUrl: root + "reports/report.cards.html",
            data: { title: 'Campaign Reports' },
            controller: "reportIndexController",
            resolve: {}
        })
        .state('viewReport', {
            url: "/report/:id",
            templateUrl: root + "reports/report.view.html",
            data: { title: 'View Campaign Reports' },
            controller: "reportViewController",
            resolve: {}
        })
        .state('lists', {
            url: "/lists?page&parent&pageSize&q&mode",
            templateUrl: root + "list/list.cards.html",
            data: { title: 'Lists' },
            controller: "listIndexController",
            resolve: {}
        })
        .state('editList', {
            url: "/list/edit/:id",
            templateUrl: root + "list/list.edit.html",
            data: { title: 'List' },
            controller: "listEditController",
            resolve: {}
        })
        .state('templates', {
            url: "/templates?page&parent&pageSize&q&mode",
            templateUrl: root + "template/template.cards.html",
            data: { title: 'Templates' },
            controller: "templateIndexController",
            resolve: {}
        })
        .state('editTemplate', {
            url: "/template/edit/:id",
            templateUrl: root + "template/template.edit.html",
            data: { title: 'List' },
            controller: "templateEditController",
            resolve: {}
        })
        .state('customs', {
            url: "/customs?page&parent$pageSize&q&mode",
            templateUrl: root + "custom/custom.index.html",
            data: { title: 'Customs' },
            controller: "customIndexController",
            resolve: {}
        })
        .state('packages', {
            url: "/packages?page&parent$pageSize&q&mode",
            templateUrl: root + "package/packages.index.html",
            data: { title: 'Packages' },
            controller: "packageIndexController",
            resolve: {}
        })
        .state('createPackage', {
            url: "/package/create",
            templateUrl: root + "package/package.edit.html",
            data: { title: 'Create Package', mode: 'create' },
            controller: "packageEditController",
            resolve: {}
        })
        .state('editPackage', {
            url: "/package/edit/:id",
            templateUrl: root + "package/package.edit.html",
            data: { title: 'Edit Package'},
            controller: "packageEditController",
            resolve: {}
        })
        .state('blogs', {
            url: "/blogs?page&parent$pageSize&q&mode",
            templateUrl: root + "news/news.index.html",
            data: { title: 'Blogs' },
            controller: "newsIndexController",
            resolve: {}
        })
        .state('editBlog', {
            url: "/blog/edit/:id",
            templateUrl: root + "news/news.edit.html",
            data: { title: 'Edit News' },
            controller: "newsEditController",
            resolve: {}
        })
        .state('news', {
            url: "/news/:id",
            templateUrl: root + "news/news.read.html",
            data: { title: 'Read News' },
            controller: "newsReadController",
            resolve: {}
        })
        .state('createBlog', {
            url: "/blog/create",
            templateUrl: root + "news/news.edit.html",
            data: { title: 'Create News',mode:'create' },
            controller: "newsEditController",
            resolve: {}
        })
        .state('usage', {
            url: "/usage",
            templateUrl: root + "reports/report.usage.html",
            data: { title: 'Usage Report' },
            controller: "usageReportController",
            resolve: {}
        })
        .state('share', {
            url: "/share?mode",
            templateUrl: root + "share/share.ui.html",
            data: { title: 'Share - List' },
            controller: "shareUiController",
            resolve: {}
        });
    
}]);