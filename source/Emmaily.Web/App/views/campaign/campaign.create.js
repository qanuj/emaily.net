app.controller('campaignCreateController', [
    '$scope', 'dataService', '$uibModalInstance', 'opt', function ($scope, db, $uibModalInstance, opt) {

        var title = "Campaign";
        $scope.title = title;
        $scope.name = 'campaign';  
        $scope.icon = 'paper-plane-o';
        $scope.record = { appId:$scope.brands && $scope.brands.length && $scope.brands[0].id };

        $scope.ok = function (record) {
            var entity = angular.copy(record);
            return db.put('campaign', entity).then(function (status) {
                if (status.id) {
                    $uibModalInstance.close({ ok: true });
                }
            });
        };

        $scope.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        }

        function loadTemplates() {
            return db.get('template/all').then(function (templates) {
                $scope.templates = templates;
            });
        }

        loadTemplates();
    }
]);