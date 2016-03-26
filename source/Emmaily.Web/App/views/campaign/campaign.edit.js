app.controller('campaignEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', function ($scope, db, $stateParams, $state, logger) {
        $scope.title = "Campaign";
        $scope.current = 'campaigns';
        $scope.icon = "code";
        $scope.action = "Save";
        $scope.breads = [
          {link:'campaigns',icon:'code',title:'Campaign'}
        ];
        $scope.readonly = $state.current.data.readonly;

        $scope.update = function (record, keepPage) {
            return db.post('campaign', record).then(function (result) {
                if (result.id>0) {
                    logger.info('Campaign ' + record.name, 'modification success.');
                    if (!keepPage) {
                        $state.go('campaigns');
                    }
                } else {
                    logger.err('Campaign ' + record.name, 'modification failed. '+result.Error);
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
            $scope.record.picture = { url: result.picture, email: result.email, title: result.name };
        }

        function loadCampaign() {
            return db.get('campaign/' + $stateParams.id).then(bindEntity);
        }


        function fetchRecord() {
            return loadCampaign()
                .then(loadAttachments);
        }

        fetchRecord();
    }
]);