app.controller('accountController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger','$rootScope', function ($scope, db, $stateParams, $state, logger,$rootScope) {
        var table = 'account/me/profile';
        $scope.title = "Account";
        $scope.current = 'account';
        $scope.icon = "user";
        $scope.action = "Save";

        $scope.update = function (record, keepPage) {
            var entity = angular.copy(record);
            entity.Picture = record.picture.url || record.Picture;
            if (entity.Birthday) {
                entity.Birthday = moment(record.Birthday).format();
            }
            return db.save(table, entity).then(function (result) {
                if (result.RowsAffected) {
                    logger.info('Account', 'modification success.');
                    if (!keepPage) {
                        $state.go('home');
                    }
                } else {
                    logger.err('Account', 'modification failed. ' + result.Error);
                }
            }).then(loadMySelf);
        }

        $scope.changePassword = function (pwd) {
            var entity = angular.copy(pwd);
            return db.post('account/change/password', entity).then(function (result) {
                if (result.Succeeded) {
                    logger.info('Account', 'Password Changed.');
                } else {
                    logger.err('Account', 'Password Change Failed. ' + result.Errors.join('<br/>'));
                }
            },function(err) {
                logger.err('Password Error',err);
            });
        }

        function loadMySelf() {
            return db.get('account/me/profile').then(function(me) {
                $rootScope.me = me;
            });
        }

        function loadSocial() {
            return db.get('account/userclaims/' + $stateParams.id).then(function (socials) {
                $scope.socials = socials;
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.picture = { url: result.picture, email: result.email, title: result.name, missing: 'mm' };
        }

        function loadProfile() {
            return db.get(table).then(bindEntity);
        }

        function fetchRecord() {
            $scope.args = $state.current.data;
            return loadProfile();
        }

        fetchRecord();
    }
]);