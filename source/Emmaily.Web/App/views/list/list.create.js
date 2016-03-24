app.controller('listCreateController', [
    '$scope', 'dataService', '$uibModalInstance', 'opt', function ($scope, db, $uibModalInstance, opt) {

        var title = "List";
        $scope.title = title;
        $scope.name = 'list';
        $scope.icon = "th-list";
        $scope.record = { appId:$scope.brands && $scope.brands.length && $scope.brands[0].id };

        $scope.ok = function (record) {
            var entity = angular.copy(record);
            return db.put('list', entity).then(function (status) {
                if (status.Id) {
                    $uibModalInstance.close({ ok: true });
                }
            });
        };

        $scope.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        }
    }
]);