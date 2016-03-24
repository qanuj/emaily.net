app.controller('packageIndexController', [
    '$scope', 'dataService', '$stateParams', '$state', 'Popup', 'logger', '$rootScope', 'DTOptionsBuilder', function ($scope, db, $stateParams, $state, Popup, logger, $rootScope, DTOptionsBuilder) {

        var title = "Package";
        var table = 'package';
        var more = 'Packages';
        $scope.title = title;
        $scope.name = 'package';
        $scope.icon = 'package';
        
        $scope.$watch("perm.sales", function (newVal) {
            if (newVal) {
                $scope.perm.current = newVal;
            }
        }, true);

        $scope.addNew=function() {
            $state.go('createPackage');
        }

        $scope.trash = function (card) {
            Popup.trash({ items: [card], title: title }).then(function () {
                return trashOne(card);
            }).then(loadRecords);
        }

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

        $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('paging', false).withOption('searching', false).withOption('info', false);

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