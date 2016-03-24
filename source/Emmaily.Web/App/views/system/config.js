app.controller('systemConfigController', ['$scope', 'dataService', '$stateParams', '$state', function ($scope, db, $stateParams, $state) {
    $scope.title = "System";
    $scope.current = 'Configurations';
    $scope.icon = "cogs";
    $scope.action = "Save";
    $scope.breads = [
      { link: 'companies', icon: 'building', title: 'Company' }
    ];
    $scope.actions = [
        { link: 'companies' }
    ];

    $scope.addDomain=function(domain) {
        db.update('app/host/' + domain).then(function (o) {
            if (o.created) {
                $scope.record.General.Hosts.push(model.General.NewHost());
                $scope.record.General.NewHost='';
            }
        });
    }

    $scope.removeDomain=function(domain) {
        db.trash('app/host/' + domain).then(function (o) {
            domain.deleted = true;
        });
    }


    $scope.update = function (record, keepPage) {
        record.Logo = record.logo.url || record.Logo;
        record.Banner = record.banner.url || record.Banner;
        return db.update('app', record).then(function () {
            if (!keepPage) {
                window.location.reload(true);
            }
        });
    }

    function bindEntity(result) {
        result.logo = { url: result.General.Logo, email: '', title: result.Title };
        result.banner = { url: result.General.Banner, email: '', title: 'Banner' };
        $scope.record = result;
    }

    function loadRecord() {
        return db.create('app').then(bindEntity);
    }

    loadRecord();

}]);