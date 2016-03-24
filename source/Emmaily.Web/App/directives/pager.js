app.directive('ccPager', pager);
pager.$inject = [];

function pager() {
    var directive = {
        link: link,
        restrict: 'E',
        replace: true,
        templateUrl: "App/views/pager.html",
        scope: {
            maxSize: "=",
            totalPages: "=",
            currentPage: "=",
            pageAction: "="
        }
    };

    return directive;

    function link(scope) {
        scope.pages = [];
        scope.linkMode = !scope.pageAction;
        scope.$watch('totalPages + currentPage', function () {
            createPageArray(scope.pages, scope.totalPages, scope.currentPage, scope.maxSize || 5);
        });
    }

    function createPageArray(pages, totalPages, currentPage, maxPages) {
        var i;
        pages.length = 0;
        var start = currentPage - ((maxPages - 1) / 2);
        if (start <= 1) start = 1;
        for (i = start ; i <= start + maxPages && i <= totalPages; i++) {
            pages.push(i);
        }
    }
}