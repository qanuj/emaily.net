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
        .state('users', {
            url: "/contacts?page&parent&pageSize&q&mode",
            templateUrl: root + "user/user.cards.html",
            data: { title: 'Contacts' },
            controller: "userIndexController",
            resolve: {}
        })
        .state('editUser', {
            url: "/user/edit/:id",
            templateUrl: root + "user/user.edit.html",
            data: { title: 'User' },
            controller: "userEditController",
            resolve: {}
        })
        .state('teams', {
            url: "/teams?page&parent&pageSize&q&mode",
            templateUrl: root + "team/team.cards.html",
            data: { title: 'Teams', filter: 'IsAutoCreated eq false' },
            controller: "teamIndexController",
            resolve: {}
        })
        .state('editTeam', {
            url: "/team/edit/:id",
            templateUrl: root + "team/team.edit.html",
            data: { title: 'Team' },
            controller: "teamEditController",
            resolve: {}
        })
        .state('titles', {
            url: "/titles?page&parent&pageSize&q&mode",
            templateUrl: root + "title/title.cards.html",
            data: { title: 'Titles' },
            controller: "titleIndexController",
            resolve: {}
        })
        .state('editTitle', {
            url: "/title/edit/:id",
            templateUrl: root + "title/title.edit.html",
            data: { title: 'Title' },
            controller: "titleEditController",
            resolve: {}
        })
        .state('viewTitle', {
            url: "/title/:id",
            templateUrl: root + "title/title.view.html",
            data: { title: 'Title', readonly:true },
            controller: "titleEditController",
            resolve: {}
        })
        .state('versions', {
            url: "/versions?page&parent&pageSize&q&mode",
            templateUrl: root + "version/version.cards.html",
            data: { title: 'Versions' },
            controller: "versionIndexController",
            resolve: {}
        })
        .state('programmes', {
            url: "/programmes?page&parent&pageSize&q&mode",
            templateUrl: root + "programme/programme.cards.html",
            data: { title: 'Programme' },
            controller: "programmeIndexController",
            resolve: {}
        })
        .state('editProgramme', {
            url: "/programme/edit/:id",
            templateUrl: root + "programme/programme.edit.html",
            data: { title: 'Programme' },
            controller: "programmeEditController",
            resolve: {}
        })
        .state('viewProgramme', {
            url: "/programme/:id",
            templateUrl: root + "programme/programme.view.html",
            data: { title: 'Programme', readonly: true },
            controller: "programmeEditController",
            resolve: {}
        })
        .state('videos', {
            url: "/videos?page&parent&pageSize&q&mode",
            templateUrl: root + "video/video.cards.html",
            data: { title: 'Video', filter: 'IsRejected eq false' },
            controller: "videoIndexController",
            resolve: {}
        })
        .state('videosModerate', {
            url: "/videos/moderate?page&parent&pageSize&q&mode",
            templateUrl: root + "video/video.cards.html",
            data: { title: 'Video', filter: 'IsModerated eq false and IsRejected eq false' },
            controller: "videoIndexController",
            resolve: {}
        })
        .state('videosRejected', {
            url: "/videos/rejected?page&parent&pageSize&q&mode",
            templateUrl: root + "video/video.cards.html",
            data: { title: 'Video', filter: 'IsRejected eq true' },
            controller: "videoIndexController",
            resolve: {}
        })
        .state('videosLive', {
            url: "/videos/live?page&parent&pageSize&q&mode",
            templateUrl: root + "video/video.cards.html",
            data: { title: 'Video', filter: 'IsLive eq true' },
            controller: "videoIndexController",
            resolve: {}
        })
        .state('editVideo', {
            url: "/video/edit/:id",
            templateUrl: root + "video/video.edit.html",
            data: { title: 'Video' },
            controller: "videoEditController",
            resolve: {}
        })
        .state('viewVideo', {
            url: "/video/:id",
            templateUrl: root + "video/video.view.html",
            data: { title: 'Video',readonly:true },
            controller: "videoEditController",
            resolve: {}
        })
        .state('audios', {
            url: "/audios?page&parent$pageSize&q&mode",
            templateUrl: root + "audio/audio.cards.html",
            data: { title: 'Audio', filter: 'IsRejected eq false' },
            controller: "audioIndexController",
            resolve: {}
        })
        .state('audiosModerate', {
            url: "/audios/moderate?page&parent&pageSize&q&mode",
            templateUrl: root + "audio/audio.cards.html",
            data: { title: 'Audio', filter: 'IsModerated eq false and IsRejected eq false' },
            controller: "audioIndexController",
            resolve: {}
        })
        .state('audiosRejected', {
            url: "/audios/rejected?page&parent&pageSize&q&mode",
            templateUrl: root + "audio/audio.cards.html",
            data: { title: 'Audio', filter: 'IsRejected eq true' },
            controller: "audioIndexController",
            resolve: {}
        })
        .state('editAudio', {
            url: "/audio/edit/:id",
            templateUrl: root + "audio/audio.edit.html",
            data: { title: 'audio' },
            controller: "audioEditController",
            resolve: {}
        })
        .state('viewAudio', {
            url: "/audio/:id",
            templateUrl: root + "audio/audio.view.html",
            data: { title: 'audio',readonly:true },
            controller: "audioEditController",
            resolve: {}
        })
        .state('images', {
            url: "/images?page&parent$pageSize&q&mode",
            templateUrl: root + "image/image.cards.html",
            data: { title: 'Image', filter: 'IsRejected eq false' },
            controller: "imageIndexController",
            resolve: {}
        })
        .state('imagesModerate', {
            url: "/images/moderate?page&parent&pageSize&q&mode",
            templateUrl: root + "image/image.cards.html",
            data: { title: 'Unapproved Images', filter: 'IsModerated eq false and IsRejected eq false' },
            controller: "imageIndexController",
            resolve: {}
        })
        .state('imagesRejected', {
            url: "/images/rejected?page&parent&pageSize&q&mode",
            templateUrl: root + "image/image.cards.html",
            data: { title: 'Rejected Images', filter: 'IsRejected eq true' },
            controller: "imageIndexController",
            resolve: {}
        })
        .state('editImage', {
            url: "/image/edit/:id",
            templateUrl: root + "image/image.edit.html",
            data: { title: 'Image' },
            controller: "imageEditController",
            resolve: {}
        })
        .state('viewImage', {
            url: "/image/:id",
            templateUrl: root + "image/image.view.html",
            data: { title: 'Image',readonly:true },
            controller: "imageEditController",
            resolve: {}
        })
        .state('documents', {
            url: "/documents?page&parent$pageSize&q&mode",
            templateUrl: root + "document/document.cards.html",
            data: { title: 'Document' },
            controller: "documentIndexController",
            resolve: {}
        })
        .state('documentsModerate', {
            url: "/documents/moderate?page&parent&pageSize&q&mode",
            templateUrl: root + "document/document.cards.html",
            data: { title: 'Unapproved Documents', filter: 'IsModerated eq false and IsRejected eq false' },
            controller: "documentIndexController",
            resolve: {}
        })
        .state('documentsRejected', {
            url: "/documents/rejected?page&parent&pageSize&q&mode",
            templateUrl: root + "document/document.cards.html",
            data: { title: 'Rejected Documents', filter: 'IsRejected eq true' },
            controller: "documentIndexController",
            resolve: {}
        })
        .state('editDocument', {
            url: "/document/edit/:id",
            templateUrl: root + "document/document.edit.html",
            data: { title: 'Document' },
            controller: "documentEditController",
            resolve: {}
        })
        .state('viewDocument', {
            url: "/document/:id",
            templateUrl: root + "document/document.view.html",
            data: { title: 'Document',reaadonly:true },
            controller: "documentEditController",
            resolve: {}
        })
        .state('sources', {
            url: "/sources?page&parent$pageSize&q&mode",
            templateUrl: root + "source/source.cards.html",
            data: { title: 'Source' },
            controller: "sourceIndexController",
            resolve: {}
        })
        .state('editSource', {
            url: "/source/edit/:id",
            templateUrl: root + "source/source.edit.html",
            data: { title: 'Source' },
            controller: "sourceViewController",
            resolve: {}
        })
        .state('viewSource', {
            url: "/source/:id",
            templateUrl: root + "source/source.view.html",
            data: { title: 'Source',readonly:true },
            controller: "sourceViewController",
            resolve: {}
        })
        .state('editor', {
            url: "/editor/:id",
            templateUrl: root + "source/source.editor.html",
            data: { title: 'Editor' },
            controller: "sourceEditorController",
            resolve: {}
        })
        .state('libraries', {
            url: "/libraries?page&parent$pageSize&q&mode",
            templateUrl: root + "library/library.cards.html",
            data: { title: 'Web Channels' },
            controller: "libraryIndexController",
            resolve: {}
        })
        .state('editLibrary', {
            url: "/library/edit/:id",
            templateUrl: root + "library/library.edit.html",
            data: { title: 'Web Channel Edit' },
            controller: "libraryEditController"
        })
        .state('playlists', {
            url: "/playlists?page&parent$pageSize&q&mode",
            templateUrl: root + "playlist/playlist.cards.html",
            data: { title: 'Playlists' },
            controller: "playlistIndexController",
            resolve: {}
        })
        .state('editPlaylist', {
            url: "/playlist/:mode/edit/:id",
            templateUrl: root + "playlist/playlist.edit.html",
            data: { title: 'Playlist Edit' },
            controller: "playlistEditController",
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
        .state('record', {
            url: "/record",
            templateUrl: root + "record/rtmp.html",
            data: { title: 'Record' },
            controller: "recordingController",
            resolve: {}
        })
        .state('roles', {
            url: "/roles",
            templateUrl: root + "role/role.editor.html",
            data: { title: 'Roles' },
            controller: "roleEditController",
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
        })
        .state('history', {
            url: "/history?page&parent$pageSize&q&mode&client&act",
            templateUrl: root + "reports/report.history.html",
            data: { title: 'History Report' },
            controller: "historyController",
            resolve: {}
        })
        .state('analytics', {
            url: "/analytics?page&parent$pageSize&q&mode&client",
            templateUrl: root + "reports/report.analytics.html",
            data: { title: 'Analytics Report' },
            controller: "analyticsController",
            resolve: {}
        })
        .state('commentModerationApprove', {
            url: "/comments/moderate?page&parent&pageSize&q&mode",
            templateUrl: root + "comment/comments.index.html",
            data: { title: 'Unapproved Comments', mode: 'pending' },
            controller: "commentIndexController",
            resolve: {}
        })
        .state('commentModerationRejected', {
            url: "/comments/rejected?page&parent&pageSize&q&mode",
            templateUrl: root + "comment/comments.index.html",
            data: { title: 'Rejected Comments', mode: 'approval/false' },
            controller: "commentIndexController"
        })
        .state('tests', {
            url: "/console",
            templateUrl: root + "console/console.tests.html",
            data: { title: 'Test Console Api' },
            controller: "consoleTestController"
        });
    
}]);