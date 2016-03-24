app.controller('templateCreateController', [
    '$scope', 'dataService', '$uibModalInstance', 'opt', function ($scope, db, $uibModalInstance, opt) {

        var title = "Template";
        $scope.title = title;
        $scope.name = 'template';
        $scope.icon = "code";
        $scope.record = { appId:$scope.brands && $scope.brands.length && $scope.brands[0].id };

        $scope.ok = function (record) {
            var entity = angular.copy(record);
            return db.put('template', entity).then(function (status) {
                if (status.id) {
                    $uibModalInstance.close({ ok: true });
                }
            });
        };

        $scope.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        }
    }
]);