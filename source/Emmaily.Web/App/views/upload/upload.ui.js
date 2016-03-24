app.controller('uploadUiController', ['$scope', 'dataService','$state', '$stateParams', function ($scope, db,$state, $stateParams) {

    $scope.source = "Uploding";
    $scope.current = 'sources';
    $scope.icon = "cloud-upload";
    $scope.title = $state.current.data.title;

    $scope.containerTitle = $state.current.data.containerTitle;
    $scope.maxFileSize = Math.pow(1024, 3) * 200; // 200GB
    $scope.uploadConfig = { container: $stateParams.container || 0 };
    $scope.hideContainer = $stateParams.container;

    $scope.$watch("perm.upload.read", function (newVal) {
        if (newVal == false) {
            $state.go('home');
        }
    });

    $scope.refreshIntellect = function (q) {
        db.get('ip/all?$filter=(substringof(\'' + q + '\',Title) or ID eq ' + $scope.uploadConfig.container + ') and IpType eq \'Programme\'&$top=20&$orderby=ID desc').then(function (result) {
            $scope.intellects=result.Items;
        });
    };

    function getStorages() {
        return db.get('ftp/all?$select=ServiceProvider,Client,ClientID,ID&$orderby=ClientID,ServiceProvider&$filter=substringof(%27blob.core%27,Path)').then(function (storages) { //only Azure.
            $scope.storages = storages;
            $scope.uploadConfig.storage = $scope.storages[0].ID;
        });
    }

    getStorages();

}]);