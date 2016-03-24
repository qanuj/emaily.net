app.controller('companyCreateController', [
    '$scope', 'dataService', '$uibModalInstance', 'opt', function ($scope, db, $uibModalInstance, opt) {

        var title = "Company";
        $scope.title = title;
        $scope.name = 'company';
        $scope.icon = "building";
        $scope.record = {};

        function loadClients() {
            return db.get('client/all?$orderby=Name&$select=ID,Title').then(function (resellers) {
                $scope.clients = resellers;
            });
        }

        $scope.ok = function (record, notifyUser) {
            var entity = angular.copy(record);
            entity.Notify = notifyUser;
            return db.save('client', entity).then(function (status) {
                if (status.ID) {
                    $uibModalInstance.close({ ok: true });
                }
            });
        };

        $scope.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        }

        loadRecords();

        function loadRecords() {
            return loadClients();
        }
    }
]);