app.controller('campaignEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', function ($scope, db, $stateParams, $state, logger) {
        $scope.title = "Campaign";
        $scope.current = 'campaigns';
        $scope.icon = "paper-plane-o";
        $scope.action = "Save";
        $scope.breads = [
          { link: 'campaigns', icon: 'paper-plane-o', title: 'Campaign' }
        ];
        $scope.readonly = $state.current.data.readonly;

        $scope.$watch("record.attachment.url", function(newVal) {
            if (newVal) {
                console.log('Url', newVal);
                loadAttachments();
            }
        });

        $scope.removeAttachment=function(att) {
            db.trash('attachment/' + $stateParams.id, att.id).then(loadAttachments);
        }

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

        function buildTotalSize(attachments) {
            var totalSize = 0;
            for (var x in attachments) {
                totalSize += attachments[x].size;
            }
            $scope.totalSize = totalSize;
        }
           
        function loadAttachments() {
            return db.get('attachment/'+$stateParams.id).then(function (attachments) {
                $scope.attachments = attachments;
                buildTotalSize(attachments);
            });
        }

        function loadLists() {
            return db.get('list/all').then(function (lists) {
                $scope.lists = lists;
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.attachment = { url: '' };
            $scope.record.picture = { url: result.picture, email: result.sender.email, title: result.name };
        }

        function loadCampaign() {
            return db.get('campaign/' + $stateParams.id).then(bindEntity);
        }


        function fetchRecord() {
            return loadLists()
                .then(loadAttachments)
                .then(loadCampaign);
        }

        fetchRecord();
    }
]);