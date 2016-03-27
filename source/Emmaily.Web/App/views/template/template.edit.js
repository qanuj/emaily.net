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

        $scope.removeAttachment = function (att) {
            db.trash('attachment/' + $stateParams.id, att.id).then(loadAttachments);
        }

        $scope.$watch("record.attachment.url", function (newVal) {
            if (newVal) {
                console.log('Url', newVal);
                loadAttachments();
            }
        });

        function buildTotalSize(attachments) {
            var totalSize = 0;
            for (var x in attachments) {
                totalSize += attachments[x].size;
            }
            $scope.totalSize = totalSize;
        }

        function loadAttachments() {
            return db.get('attachment/' + $stateParams.id).then(function (attachments) {
                $scope.attachments = attachments;
                buildTotalSize(attachments);
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.attachment = { url: '' };
            $scope.record.picture = { url: result.picture, email: result.sender.email, title: result.name };
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