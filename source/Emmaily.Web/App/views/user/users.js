app.controller('userIndexController', [
    '$scope', 'dataService', '$stateParams', '$state', 'Popup', 'logger', '$rootScope', function ($scope, db, $stateParams, $state, Popup, logger, $rootScope) {

        var title = "User";
        var table = 'user';
        var more = "Users";
        $scope.title = title;
        $scope.name = 'user';
        $scope.icon = "user";
        $scope.actions = [];

        $scope.$watch("perm.users", function (newVal) {
            if (newVal) {
                $scope.perm.current = newVal;
            }
        }, true);

        $scope.history = function (card) {
            db.get('history/user?$filter=ItemId eq ' + card.ID).then(function (result) {
                Popup.history({ info: card, items: result });
            });
        }

        $scope.addNew = function () {
            return Popup.open({ key: 'user/user.create',ctrl:'userCreateController' }).then(loadRecords);
        }

        $scope.comment = function (card) {
            Popup.comment({
                info: card,
                items: function () {
                    return db.get('comment/by/chat/' + card.ID + '?approval=any');
                },
                add: function (message) {
                    return db.save("comment/on/user", { message: message, receiverID: card.ID });
                }
            });
        }

        $scope.usage = function (card) {
            db.get(table + '/usage/' + card.ID).then(function (result) {
                Popup.usage({ info: card, items: result });
            });
        }

        $scope.download = function (card) {
            window.location = '/download/' + card.ID;
        }

        $scope.copy = function (card) {
            db.create(table + '/copy/' + card.ID, card.ID).then(loadRecords);
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
                    console.log('Deleting', card.Title, card);
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

        $scope.refresh = loadRecords;


        loadRecords();

        function loadRecords() {
            return db.card(table + '/card', $stateParams, $scope, $state.current.data, function (q) {
                return 'substringof(\'' + q + '\',Title) or substringof(\'' + q + '\',Email) ';
            });
        }
    }
]);