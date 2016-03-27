app.controller('listEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', function ($scope, db, $stateParams, $state, logger) {
        $scope.title = "List";
        $scope.current = 'lists';
        $scope.icon = "th-list";
        $scope.action = "Save";
        $scope.breads = [
          { link: 'lists', icon: 'th-list', title: 'List' }
        ];
        $scope.readonly = $state.current.data.readonly;

        $scope.update = function (record, keepPage) {
            return db.post('list', record).then(function (result) {
                if (result.id>0) {
                    logger.info('List ' + record.name, 'modification success.');
                    if (!keepPage) {
                        $state.go('lists',{id:$stateParams.id});
                    }
                } else {
                    logger.err('List ' + record.name, 'modification failed. '+result.Error);
                }
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.attachment = { url: '' };
            $scope.record.picture = { url: result.picture, email: result.email, title: result.name };
        }

        function loadList() {
            return db.get('list/' + $stateParams.id).then(bindEntity);
        }


        function fetchRecord() {
            return loadList();
        }

        fetchRecord();
    }
]);