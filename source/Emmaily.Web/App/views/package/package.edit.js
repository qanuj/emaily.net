app.controller('packageEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', 'Popup', function ($scope, db, $stateParams, $state, logger, popup) {
        var table = 'package', title = 'Package',index='packages';
        $scope.title = title;
        $scope.current = 'package';
        $scope.icon = "gift";
        $scope.action = "Save";
        $scope.breads = [
          { link: '#/packages', icon: $scope.icon, title: title }
        ];
        $scope.actions = [];

        $scope.update = function (record, keepPage) {
            var entity = angular.copy(record);
            entity.Published = moment(entity.Published).format(); //Needs to be done for each date value.
            return db.save(table, entity).then(function (result) {
                if (result.RowsAffected || result.ID) {
                    logger.info(title+' ' + record.Title, 'modification success.');
                    if (!keepPage) {
                        $state.go(index);
                    }
                } else {
                    logger.err(title + ' ' + record.Title, 'modification failed. ' + result.Error);
                }                                        
            });
        }

        function bindEntity(result) {
            result.Title = result.Name;
            result.picture = { url: result.Picture, email: '', title: result.Title };
            $scope.record = result;
        }

        function loadPayments() {
            return db.get('payment/all?$orderby=ID desc').then(function (payments) {
                $scope.payments = payments;
            });
        }

        function loadPackage() {
            if ($stateParams.id > 0) {
                return db.get(table + '/' + $stateParams.id).then(bindEntity);
            } else {
                bindEntity({Name:'New Package'});
            }
        }

        loadPayments().then(loadPackage);
    }
]);