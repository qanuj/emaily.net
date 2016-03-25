app.controller('listImportController', [
    '$scope', 'dataService', '$state', '$stateParams', function ($scope, db, $state, $stateParams) {

        var title = "List";
        $scope.title = title;
        $scope.name = 'list';
        $scope.icon = "th-list";
        $scope.record = { appId:$scope.brands && $scope.brands.length && $scope.brands[0].id };

        $scope.$watch("record.file", function(newVal) {
            if (newVal && newVal.key && newVal.url) {
                db.get('subscriber/' + $stateParams.id + '/import/' + newVal.key).then(function (result) {
                    if (result.started) {
                        $state.go('lists', { id: $stateParams.id });
                    }
                });
           }
        },true);

    }
]);