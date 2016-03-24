app.controller('listIndexController', [
    '$scope', 'dataService', '$stateParams', '$state', 'Popup', 'logger', function ($scope, db, $stateParams, $state, Popup, logger) {
        var table = 'list';
        var more = 'Lists';
        var title = "Lists";

        $scope.title = "List";
        $scope.current = 'lists';
        $scope.icon = "th-list";
        $scope.action = "Action";
        $scope.actions = [];

        $scope.$watch("perm.list", function (newVal) {
            if (newVal) {
                $scope.perm.current = newVal;
            }
        }, true);

        $scope.addNew = function () {
            return Popup.open({ key: 'list/list.create', ctrl: 'listCreateController' }).then(loadRecords);
        }

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
            return db.trash(table, card.Id).then(function (result) {
                logger.log(title, 'Delete "' + card.Title + '" Successful', result);
            });
        }


        loadRecords();

        function loadRecords() {
            return db.card(table, $stateParams, $scope, $state.current.data, function (q) {
                return 'substringof(\'' + q + '\',Title)';
            });
        }
    }
]);