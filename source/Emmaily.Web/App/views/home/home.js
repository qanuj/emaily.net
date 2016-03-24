app.controller('homeController', ['$scope', 'dataService', 'reader', 'Popup', function ($scope, db, reader, Popup) {
    $scope.title = "Welcome";

    var height = 350;

    $scope.height = height;
    $scope.height2 = height-30;

    $scope.$watch("dashboardNavCode", function(newVal) {
       if (newVal) {
           getDashboard(newVal);
       }
    });

    function getCompanies() {
        return db.get('client/card?$top=20&$orderby=ID desc&$inlinecount=allpages').then(function (companies) {
            $scope.companies = companies;
        });
    }

    function getProgrammes() {
        return db.get('content/card?mode=3&$top=10&$orderby=ID desc&$inlinecount=allpages').then(function (programmes) {
            $scope.programmes = programmes;
        });
    }

    function getDashboard(nav) {
        return db.get('nav/items/'+nav).then(function (dashboard) {
            $scope.dashboard = dashboard;
        });
    }

    function getNews() {
        return db.get('message/all?$orderby=ID desc&$top=20').then(function (news) {
            $scope.news = news;
        });
    }

    function getNewsFeed() {
        return db.get('livefeed/all').then(function (d) {
            if (!d.length) { d = [{ url: 'http://vidzapper.blogspot.com/feeds/posts/default?alt=rss' }]; }
            return reader.parse(d).then(function (rss) {
                $scope.rss = rss.data.responseData.feed.entries;
            });
        });
    }

    function getHistory() {
        return db.get('history/all?$orderby=ID%20desc&$top=10&$inlinecount=allpages').then(function (history) {
            $scope.history = history;
        });
    }

    function getBlog(id) {
        db.get('blog/'+id).then(function(result) {
            console.log(result);
        });
    }

    getCompanies().then(getProgrammes).then(getNews).then(getHistory);

}]);