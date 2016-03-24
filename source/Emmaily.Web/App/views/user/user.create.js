app.controller('userCreateController', [
    '$scope', 'dataService', '$uibModalInstance', 'opt', function ($scope, db,$uibModalInstance, opt) {

        var title = "User";
        $scope.title = title;
        $scope.name = 'user';
        $scope.icon = "user";
        $scope.record = {Role:[],Notify:false};

        function loadClients() {
            return db.get('client/all?$orderby=Name&$select=ID,Title').then(function (resellers) {
                $scope.clients = resellers;
            });
        }

        function getRoles() {
            return db.get('user/roles').then(function (roles) {
                $scope.roles = roles;
            });
        }

        function toName(val) {
            if (!!val && !!val[0]) return val[0].toUpperCase() + val.substring(1);
            return val;
        }

        $scope.ok = function (record,notifyUser) {
            var entity = angular.copy(record);
            entity.Notify = notifyUser;
            entity.RegistrationUrl = window.location.host;
            var name = entity.Email.split('@')[0];
            entity.firstName = toName(name.split('.')[0]);
            entity.lastName = toName(name.split('.')[1] || '');
            return db.save('user', entity).then(function (status) {
                if (status.created) {
                    $uibModalInstance.close({ ok: true });
                }
            });
        };

        $scope.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        }

        loadRecords();

        function loadRecords() {
            return getRoles()
                .then(loadClients);
        }
    }
]);