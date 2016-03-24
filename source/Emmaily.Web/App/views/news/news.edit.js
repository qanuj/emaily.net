app.controller('newsEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', 'Popup', function ($scope, db, $stateParams, $state, logger, popup) {
        var table = 'blog', title = 'News';
        $scope.title = "News";
        $scope.current = 'news';
        $scope.icon = "newspaper-o";
        $scope.action = "Save";
        $scope.breads = [
          { link: '#/blogs', icon: 'newspaper-o', title: 'News' }
        ];
        $scope.actions = [];

        $scope.update = function (record, keepPage) {
            var entity = angular.copy(record);
            entity.Published = moment(entity.Published).format(); //Needs to be done for each date value.
            return db.save(table, entity).then(function (result) {
                if (result.RowsAffected || result.ID) {
                    logger.info('News ' + record.Title, 'modification success.');
                    if (!keepPage) {
                        $state.go('blogs');
                    }
                } else {
                    logger.err('News ' + record.Title, 'modification failed. ' + result.Error);
                }
            });
        }

        function bindEntity(result) {
            result.picture = { url: result.Picture, email: '', title: result.Title };
            $scope.record = result;
        }

        function loadNews() {
            if ($stateParams.id > 0) {
                return db.get(table + '/' + $stateParams.id).then(bindEntity);
            } else {
                bindEntity({Title:'New News Update'});
            }
        }

        function loadLibraries() {
            return db.get('library/all?$orderby=Title&$select=ID,Title').then(function (libraries) {
                $scope.libraries = libraries;
            });
        }

        function fetchRecord() {
            return loadLibraries()
                .then(loadNews);
        }

        fetchRecord();
    }
]);