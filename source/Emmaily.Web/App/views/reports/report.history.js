app.controller('historyController', [
     '$scope', 'dataService', '$stateParams', '$rootScope', function ($scope, db, $stateParams, $rootScope) {

         var title = "History";
         var table = 'history';
         $scope.title = title;
         $scope.name = 'history';
         $scope.icon = "history";
         $scope.actions = [];
         $scope.search = { ClientID: 0 };
         $scope.selectClient = function (client) {
             $scope.search.client = client;
             $scope.search.ClientID = client.ID;
         }

        $scope.colors = {
         Created:       {color:'',icon:''},
         Modified:      {color:'',icon:''},
         Deleted:       {color:'',icon:''},
         Uploaded:      {color:'',icon:''},
         Encoded:       {color:'',icon:''},
         Copied:        {color:'',icon:''},
         Moderated:     {color:'',icon:''},
         Approved:      {color:'',icon:''},
         Rejected:      {color:'',icon:''},
         Shared:        {color:'',icon:''},
         Published:     {color:'',icon:''},
         Viewed:        {color:'',icon:''},
         Failed:        {color:'',icon:''},
         Sent:          {color:'',icon:''},
         Moved:         {color:'',icon:''},
         Commented:     {color:'',icon:''},
         Downloaded:    {color:'',icon:''},
         Imported:      {color:'',icon:''},
         Added:         {color:'',icon:''},
         Unknown:       {color:'',icon:''}
        };

        function loadReport() {
            var filter = '';
            if ($scope.search.act) {
                filter = "&$filter=Action eq '" + $scope.search.act + "'";
            }
            return db.paged(table + '/clientfilter/' + $scope.search.client + '?$orderby=ID desc' + filter, $stateParams, $scope, function (q) {
                return 'substringof(\'' + q + '\',Title)';
            });
         }

        function loadRecords() {
            return db.get(table + '/clients?$orderby=Title&$select=ID,Title').then(function(clients) {
                $scope.clients = clients;
                $scope.search = {
                    client: $stateParams.client || clients[0].ID,
                    act: $stateParams.act
                };
            });
         }

        loadRecords().then(loadReport);
     }
]);