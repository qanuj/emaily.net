app.controller('reportViewController', [
     '$scope', 'dataService',  function ($scope, db) {

         var title = "Usage";
         var table = 'usage';
         $scope.title = title;
         $scope.name = 'usage';
         $scope.icon = "area-chart";
         $scope.actions = [];
         $scope.search = { ClientID: 0 };
         $scope.selectClient = function (client) {
             $scope.search.ClientID = client.ClientID;
         }

         $scope.chartOption = {
             bezierCurve: true,
             showTooltips: true,
             useUtc: true,
             scaleDateFormat: "mmm-yyyy",
             scaleDateTimeFormat: "mmm-yyyy",
             scaleShowHorizontalLines: true,
             scaleType: "date",
             scaleLabel: function (valuePayload) {
                 return _inGB(Number(valuePayload.value)).toFixed(2)+ 'GB';
             }
         }

         var lines = {
             Ingress: [],
             Egress: [],
             BillableRequests: [],
             Items: [],
             Size: [],
             Encodes: [],
             EncodeOuput: [],
             EncodeInputSize: [],
             EncodeOutputSize: []
         };

         var map = {};

         $scope.series = ["Input", "Output", "Size"];
         $scope.series2 = ["Ouputs", "Inputs", "Items"];
         $scope.seriesBandwith = ["Egress", "Ingress"];

         $scope.onClick = function (points) {
             if (map[points[0].label]) {
                 $scope.current = map[points[0].label];
             }
         };

         $scope.$watch("search", function (newVal) {
             if (newVal && newVal.ClientID > 0) {
                 loadReport(newVal.ClientID);
             }
         }, true);

         function _inKB(bytes) {
             return bytes / 1024;
         }

         function _inMB(bytes) {
             return _inKB(bytes) / 1024;
         }

         function _inGB(bytes) {
             return parseFloat((_inMB(bytes) / 1024).toFixed(2));
         }
         function parseReport(report) {

             var time = report.length == 1 ? moment(new Date(report[0].Year, report[0].Month - 1, 1)).add(-1,'month')._d : undefined;
             for (var x in lines) {
                 lines[x] = [];
                 if (time) lines[x].push({ x: time ,y:0});
             }

             var d = new Date();
             for (var i = 0; i < report.length; i++) {
                 k = report[i];
                 k.time = new Date(k.Year, k.Month - 1, 1);
                 lines.BillableRequests.push({ x: k.time, y: k.BillableRequests });
                 lines.EncodeOuput.push({ x: k.time, y: k.EncodeOuput});
                 lines.Encodes.push({ x: k.time, y: k.Encodes});
                 lines.Items.push({ x: k.time, y: k.Items});

                 lines.Egress.push({ x: k.time, y: k.Egress});
                 lines.EncodeInputSize.push({ x: k.time, y: k.EncodeInputSize});
                 lines.EncodeOutputSize.push({ x: k.time, y: k.EncodeOutputSize});
                 lines.Ingress.push({ x: k.time, y: k.Ingress});
                 lines.Size.push({ x: k.time, y: k.Size });
                 map[k.time] = k;
                 $scope.current = k;
             }
             $scope.data = [
                 {
                     label: 'Inputs',
                     strokeColor: '#26C281',
                     data: lines.EncodeInputSize
                 }, {
                    label: 'Outputs',
                    strokeColor: '#5C9BD1',
                    data: lines.EncodeOutputSize
                }, {
                    label: 'Size',
                    strokeColor: '#22313F',
                    data: lines.Size
                }
             ];
             $scope.dataBandwith = [
                 {
                     label: 'Egress',
                     strokeColor: '#26C281',
                     data: lines.Egress
                 },
                 {
                     label: 'Ingress',
                     strokeColor: '#22313F',
                     data: lines.Ingress
                 }
             ];
         }

         function loadReport(clientID) {
             db.get(table + '/client/all/' + clientID+'?$orderby=Year desc,Month desc').then(parseReport);
         }

         function loadRecords() {
             return db.get(table + '/clients?$orderby=Title').then(function (clients) {
                 $scope.clients = clients;
                 $scope.search.ClientID = clients[0].ClientID;
             });
         }

         loadRecords();
     }
]);