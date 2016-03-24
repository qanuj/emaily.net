app.controller('customIndexController', [
    '$scope', 'dataService', '$stateParams', '$state', 'Popup', 'logger', '$rootScope', 'DTOptionsBuilder', function ($scope, db, $stateParams, $state, Popup, logger, $rootScope, DTOptionsBuilder) {

        var title = "Custom";
        var table = 'extra';
        var more = 'Custom Fields';
        $scope.title = title;
        $scope.name = 'custom';
        $scope.icon = "cubes";
        
        $scope.$watch("perm.custom", function (newVal) {
            if (newVal) {
                $scope.perm.current = newVal;
            }
        }, true);

        $scope.trash = function (card) {
            Popup.trash({ items: [card], title: title }).then(function () {
                return trashOne(card);
            }).then(loadRecords);
        }

        $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('paging', false).withOption('searching', false).withOption('info', false);
        $scope.columns = [
            { field: "Client" },
            { field: "Group" },
            { field: "Mode" },
            { field: "Title" },
            {
                field: "",
                template: '' +
                    '<a class="btn btn-danger" href=\\#click/delete?id=#: ID #><i class="fa fa-trash"></i></a>'
            }
        ];

        $scope.trashSelected=function() {
            Popup.trash({ items: $scope.records, filter: { selected: true }, title: more }).then(function () {
                var cards = $scope.records.filter(function (x) { return x.selected; });
                function trasher() {
                    var card = cards.pop();
                    if (!card) return loadRecords();
                    console.log('Deleting',card.Title,card);
                    return trashOne(card).then(trasher);
                }
                trasher();
            });
        }

        $scope.refresh = loadRecords;

        loadRecords();

        function loadRecords() {
            $stateParams.parentMode = 'ClientID';
            return db.card(table, $stateParams, $scope, function (q) {//card=grid
                return 'substringof(\'' + q + '\',Title)';
            });
        }

        function trashOne(card) {
            return db.trash(table, card.ID).then(function (result) {
                logger.log(title,'Delete "' + card.Title + '" Successful', result);
            });
        }
    }
]);