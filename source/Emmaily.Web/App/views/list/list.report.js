app.controller('listReportController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', 'Popup', function ($scope, db, $stateParams, $state, logger, Popup) {
        $scope.title = "Subscriber";
        $scope.current = 'lists';
        $scope.icon = "th-list";
        $scope.action = "Action";
        $scope.breads = [
          { link: 'lists', icon: 'th-list', title: 'List' }
        ];
        $scope.readonly = $state.current.data.readonly;
        $scope.currentTab = $stateParams.tab;

        $scope.chartOption = {
            bezierCurve: true,
            showTooltips: true,
            useUtc: true,
            maintainAspectRatio: false,
            responsive: true,
            height: 450,
            scaleDateFormat: "mmm-yyyy",
            scaleDateTimeFormat: "mmm-yyyy",
            scaleShowHorizontalLines: true,
            scaleType: "date"
        }

        $scope.$watch("perm.list", function (newVal) {
            if (newVal) {
                $scope.perm.current = newVal;
            }
        }, true);


        $scope.addNew = function () {
            Popup.create({ key: 'list/subscriber.create' }).then(function (result) {
                if (result.ok && result.data) {
                    result.data.listId = $stateParams.id;
                    return db.put('subscriber/' + $stateParams.id + '/create', result.data);
                }
            }).then(loadRecords);
        }

        $scope.edit = function (id) {
            db.get('subscriber/' + $stateParams.id + '/' + id).then(function(result) {
                Popup.create({ key: 'list/subscriber.edit',record:result }).then(function(result) {
                    if (result.ok && result.data) {
                        result.data.listId = $stateParams.id;
                        return db.post('subscriber/' + $stateParams.id, result.data);
                    }
                }).then(loadRecords);
            });
        }

        $scope.import = function () {
            return Popup.open({ key: 'list/list.import', ctrl: 'listImportController' }).then(loadRecords);
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


        function parseReport(report) {
            if (!report.length) {
                report = [];
                for (var i = 0; i < 12; i++) {
                    report.push({ x: moment().add(0 - i, 'month')._d, y: 0 });
                }
            }
            for (var x in report) {
                report[x].x = moment(report[x].x)._d;
            }
            $scope.data = [{
                label: 'Subscribers',
                strokeColor: '#26C281',
                data: report
            }];
        }

        function bindEntity(result) {
            $scope.list = result;
        }

        function loadList() {
            return db.get('list/' + $stateParams.id).then(bindEntity);
        }

        function findFilter(tab) {
            if (tab == 'active') return 'IsActive eq true';
            if (tab == 'spam') return 'IsComplaint eq true';
            if (tab == 'unconfirmed') return 'IsConfirmed eq false';
            if (tab == 'unsubscribed') return 'IsUnsubscribed eq true';
            if (tab == 'bounced') return 'IsBounced eq true';
            return null;
        }

        function loadSubscribers() {
            return db.card('subscriber/' + $stateParams.id, $stateParams, $scope, { filter: findFilter($stateParams.tab) }, function (q) {
                return 'substringof(\'' + q + '\',Name)';
            });
        }

        function loadReport() {
            return db.get('subscriber/' + $stateParams.id + '/report').then(parseReport);
        }

        function countReports() {
            return db.get('subscriber/' + $stateParams.id + '/count').then(function (counts) {
                $scope.modes = [
                    {
                        title: 'All',
                        count: counts.all,
                        link:'all',
                        css: 'info'
                    },
                    {
                        title: 'Active',
                        count: counts.active,
                        link: 'active',
                        css: 'success'
                    },
                    {
                        title: 'Unconfirmed',
                        count: counts.unconfirmed,
                        link: 'unconfirmed',
                        css: 'default'
                    },
                    {
                        title: 'Unsubscribed',
                        count: counts.unsubscribed,
                        link: 'unsubscribed',
                        css: 'danger'
                    },
                    {
                        title: 'Bounced',
                        count: counts.bounced,
                        link: 'bounced',
                        css: 'inverse'
                    },
                    {
                        title: 'Marked as spam',
                        count: counts.spam,
                        link: 'spam',
                        css: 'inverse'
                    }
                ];
            });
        }

        function loadRecords() {
            return loadList()
                .then(loadSubscribers)
                .then(countReports)
                .then(loadReport);
        }

        loadRecords();
    }
]);