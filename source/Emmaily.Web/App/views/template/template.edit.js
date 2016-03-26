app.controller('templateEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', function ($scope, db, $stateParams, $state, logger) {
        $scope.title = "Template";
        $scope.current = 'templates';
        $scope.icon = "code";
        $scope.action = "Save";
        $scope.breads = [
          {link:'templates',icon:'code',title:'Template'}
        ];
        $scope.readonly = $state.current.data.readonly;

        $scope.update = function (record, keepPage) {
            return db.post('template', record).then(function (result) {
                if (result.id>0) {
                    logger.info('Template ' + record.name, 'modification success.');
                    if (!keepPage) {
                        $state.go('templates');
                    }
                } else {
                    logger.err('Template ' + record.name, 'modification failed. '+result.Error);
                }
            });
        }

        function loadAttachments() {
            return db.get('attachment/'+$stateParams.id).then(function (attachments) {
                $scope.attachments = attachments;
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.picture = { url: result.picture, email: result.email, title: result.name };
        }

        function loadTemplate() {
            return db.get('template/' + $stateParams.id).then(bindEntity);
        }


        function fetchRecord() {
            return loadTemplate()
                .then(loadAttachments);
        }

        fetchRecord();
    }
]);