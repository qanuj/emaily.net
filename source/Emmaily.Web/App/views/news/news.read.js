app.controller('newsReadController', [
    '$scope', 'dataService', '$stateParams', '$state', function ($scope, db, $stateParams, $state) {
        var table = 'blog';
        $scope.title = "News";
        $scope.current = 'news';
        $scope.icon = "newspaper-o";
        $scope.breads = [];

        function bindEntity(result) {
            $scope.news = result;
        }

        function loadNews() {
            return db.get(table + '/' + $stateParams.id).then(bindEntity);
        }

        loadNews();
    }
]);