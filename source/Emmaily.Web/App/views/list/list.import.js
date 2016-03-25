app.controller('listImportController', [
    '$scope', 'dataService', '$state', '$stateParams', function ($scope, db, $state, $stateParams) {

        var title = "List";
        $scope.title = title;
        $scope.name = 'list';
        $scope.icon = "th-list";
        $scope.record = { appId:$scope.brands && $scope.brands.length && $scope.brands[0].id };

        $scope.$watch("record.file", function(newVal) {
            if (newVal && newVal.key && newVal.url) {
                db.get('list/' + $stateParams.id).then(function (result) {
                    if (result.added>=0) {
                        $state.go('lists', { id: $stateParams.id });
                    }
                });
           }
        },true);

    }
]);