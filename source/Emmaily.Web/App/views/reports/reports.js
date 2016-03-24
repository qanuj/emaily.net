app.controller('reportIndexController', [
    '$scope', 'dataService', '$stateParams', '$state', 'Popup', 'logger', function ($scope, db, $stateParams, $state, Popup, logger) {
        var table = 'campaign';
        var more = 'Companies';
        var title = "Companies";

        $scope.title = "Campaign Report";
        $scope.current = 'reports';
        $scope.icon = "pie-chart";
        $scope.action = "Action";
        $scope.actions = [];

        $scope.$watch("perm.report", function (newVal) {
            if (newVal) {
                $scope.perm.current = newVal;
            }
        }, true);

        $scope.trash = function (card) {
            Popup.trash({ items: [card], title: title }).then(function () {
                return trashOne(card);
            }).then(loadRecords);
        }

        $scope.trashSelected = function () {
            Popup.trash({ items: $scope.records, filter: { selected: true }, title: more }).then(function () {
                var cards = $scope.records.filter(function (x) { return x.selected; });
                function trasher() {
                    var card = cards.pop();
                    if (!card) return loadRecords();
                    return trashOne(card).then(trasher);
                }
                trasher();
            });
        }

        function trashOne(card) {
            return db.trash(table, card.ID).then(function (result) {
                logger.log(title, 'Delete "' + card.Title + '" Successful', result);
            });
        }


        loadRecords();

        function loadRecords() {
            return db.card(table+'/report', $stateParams, $scope, $state.current.data, function (q) {
                return 'substringof(\'' + q + '\',Title)';
            });
        }
    }
]);