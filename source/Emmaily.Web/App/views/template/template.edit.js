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
            record.Picture = record.picture.url || record.Picture;
            record.NavLogo = record.navLogo.url || record.NavLogo;
            record.Custom.Data = JSON.stringify(record.Custom.Bag);
            return db.post('template', record).then(function (result) {
                if (result.RowsAffected) {
                    logger.info('Template ' + record.Name, 'modification success.');
                    if (!keepPage) {
                        $state.go('templates');
                    }
                } else {
                    logger.err('Template ' + record.Name, 'modification failed. '+result.Error);
                }
            });
        }

        function loadAttachments() {
            return db.get('attachment/all?$orderby=ID desc&$filter=CampaignId eq ' + $stateParams.id).then(function (attachments) {
                $scope.attachments = attachments;
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.picture = { url: result.picture, email: result.email, title: result.title };
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