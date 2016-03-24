app.controller('userEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', function ($scope, db, $stateParams, $state, logger) {
        $scope.title = "User";
        $scope.current = 'users';
        $scope.icon = "user";
        $scope.action = "Save";
        $scope.breads = [
          {link:'users',icon:'user',title:'User'}
        ];
        $scope.actions = [
            {link:'users'}
        ];
        $scope.readonly = $state.current.data.readonly;

        var clientId = 0;

        $scope.update = function (record, keepPage) {
            var entity = angular.copy(record);
            entity.ImageFile = entity.picture.url || entity.Picture;
            entity.Custom.Data = JSON.stringify(entity.Custom.Bag);
            entity.Birthday = moment(entity.Birthday).format(); //Needs to be done for each date value.
            return db.save('user', entity).then(function (result) {
                if (result.RowsAffected) {
                    logger.info('User ' + entity.FirstName, 'modification success.');
                    if (!keepPage) {
                        $state.go('users');
                    }
                } else {
                    logger.err('User ' + entity.FirstName, 'modification failed. ' + result.Error);
                }
            });
        }

        $scope.newTag = { userId: $stateParams.id, tag: '' };
        $scope.saveTag = function (item) {
            return db.save('usertag', item).then(loadTags).then(function () {
                $scope.newTag.tag = '';
            });
        }
        $scope.trashTag = function (item) {
            return db.trash('usertag/' + item.ID).then(loadTags);
        }

        $scope.addExtra = function (mode, name) {
            db.create('extra', { clientid: clientId, mode: mode, title: name, group: 'user' }).then(loadCustom);
        }

        function loadTags() {
            return db.get('usertag/all?$orderby=ID desc&$filter=UserID eq ' + $stateParams.id).then(function (tags) {
                $scope.tags = tags;
            });
        }

        function loadAddresses() {
            return db.get('location/all/' + $stateParams.id).then(function (addresses) {//
                $scope.addresses = addresses;
            });
        }

        function loadCustom() {
            return db.get('extra/user/' + clientId).then(function (extra) {
                $scope.extra = extra;
                $scope.newField = '';
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.picture = { url: result.ImageFile, email: result.Email, title: result.FirstName + ' '+ result.Surname,size:200 };
            clientId = result.ClientID;
        }

        function loadUser() {
            return db.get('user/' + $stateParams.id).then(bindEntity);
        }

        function loadResellers() {
            return db.get('client/all?$orderby=Name').then(function (resellers) {
                $scope.resellers = resellers;
            });
        }

        function fetchRecord() {
            return loadUser()
                .then(loadResellers)
                .then(loadTags)
                .then(loadAddresses)
                .then(loadCustom);
        }

        fetchRecord();
    }
]);