app.controller('systemLogsController', ['$scope', 'dataService', '$stateParams', '$state', function ($scope, db, $stateParams, $state) {

    $scope.title = "System";
    $scope.current = 'Logs';
    $scope.icon = "bug";
    $scope.action = "Save";

    function bindEntity(result) {
        result.logo = { url: result.General.Logo, email: '', title: result.Title };
        result.banner = { url: result.General.Banner, email: '', title: 'Banner' };
        $scope.record = result;
    }

    function loadRecord() {
        //return db.create('app').then(bindEntity);
    }

    loadRecord();

}]);