app.controller('shareUiController', [
    '$scope', 'dataService', '$stateParams', '$state', 'Popup', '$rootScope', 'logger', '$location',
    function ($scope, db, $stateParams, $state, Popup, $rootScope, logger, $location) {
        var table = 'basket';
        var title = "List";
        var more = "Lists";

        var parms = $location.search();
        if (parms.message == 'Error' && $stateParams.mode == 'facebook') {
            $scope.fberror = parms.values;
        } else if (parms.message == 'Error' && $stateParams.mode == 'twitter') {
            $scope.twterror = parms.values;
        } else if (parms.message == 'Error' && $stateParams.mode == 'youtube') {
            $scope.yterror = parms.values;
        }

        $scope.mode = $stateParams.mode || 'email';
        $scope.source = "Share";
        $scope.current = 'lists';
        $scope.icon = "share-alt";
        $scope.title = "Share List";
        $scope.ytcategories = [
            { id: 2, label: "Autos & Vehicles" },
            { id: 23, label: "Comedy" },
            { id: 27, label: "Education" },
            { id: 24, label: "Entertainment" },
            { id: 1, label: "Film & Animation" },
            { id: 20, label: "Gaming" },
            { id: 26, label: "Howto & Style" },
            { id: 10, label: "Music" },
            { id: 25, label: "News & Politics" },
            { id: 29, label: "Nonprofits & Activism" },
            { id: 22, label: "People & Blogs" },
            { id: 15, label: "Pets & Animals" },
            { id: 28, label: "Science & Technology" },
            { id: 17, label: "Sports" },
            { id: 19, label: "Travel & Events" }
        ];
        $scope.ytprivacies = [
            { id: "private", label: "Private" },
            { id: "public", label: "Public" },
            { id: "unlisted", label: "Unlisted" }
        ];
        $scope.email = { to: [], message: '', clear: false, ispublic: false };
        $scope.screen = { to: [], message: '', clear: false, ispublic: false };
        $scope.facebook = { clear: false, ispublic: false };
        $scope.youtube = { category: 2, privacy: 'private', clear: false, ispublic: false };
        $scope.twitter = { clear: false, ispublic: false };
        $scope.loading = {
            email: false, screen: false, facebook: false, twitter: false, youtube: false
        }

        $scope.trash = function (card) {
            Popup.trash({ items: [card], title: title, trashName: 'Remove' }).then(function () {
                return trashOne(card);
            }).then(loadRecords);
        }

        $scope.trashSelected = function () {
            Popup.trash({ items: $scope.records, filter: { selected: true }, title: more, trashName: 'Remove' }).then(function () {
                var cards = $scope.records.filter(function (x) { return x.selected; });

                function trasher() {
                    var card = cards.pop();
                    if (!card) return loadRecords();
                    return trashOne(card).then(trasher);
                }
                trasher();
            });
        }

        $scope.refreshUsers = function (q) {
            db.get(table + '/all?$orderby=Name,Image,Email&$filter=indexof(Name,\'AutoCreated\') ne 0 and Email ne \'\'  and Email ne null and (substringof(\'' + q + '\',Name) or substringof(\'' + q + '\',Email))').then(function (result) {
                $scope.foundUsers = result;
            });
        }

        $scope.sendEmail = function (email) {
            $scope.loading.email = true;
            return db.post(table + '/share', { ShareIds: email.to, Message: email.message, IsPublic: email.ispublic, isClear: email.iclear }).then(function () {
                $scope.loading.email = false;
                logger.info("Email", "successful");
            });
        }

        $scope.sendScreening = function (mail) {
            $scope.loading.screen = true;
            return db.post(table + '/share', { ShareIds: mail.to, Message: mail.message, IsPublic: mail.ispublic, isClear: mail.iclear }).then(function () {
                $scope.loading.screen = false;
                logger.info("Email", "successful");
            }, function (err) {
                $scope.loading.email = false;
                logger.err("Email", "Failed", err);
            });
        }

        $scope.sendTweets = function (twt, tweets) {
            $scope.loading.twitter = true;
            return db.post(table + '/twitter', { Key: twt.account.Id, Message: tweets, isClear: twt.clear }).then(function () {
                $scope.loading.twitter = false;
                logger.info("Tweet", "successful");
            },function(err) {
                $scope.loading.twitter = false;
                logger.err("Tweet", "Failed", err);
            });
        }

        $scope.sendFacebook = function (fb) {
            $scope.loading.facebook = true;
            return db.post(table + '/facebook/' + fb.account.Id + '/' + (fb.page || 0) + '/' + fb.clear).then(function () {
                $scope.loading.facebook = false;
                logger.info("Facebook Share", "successful");
            }, function (err) {
                $scope.loading.facebook = false;
                logger.err("Facebook", "Failed", err);
            });
        }

        $scope.sendYouTube = function (yt) {
            $scope.loading.youtube = true;
            return db.post(table + '/youtube/' + yt.privacy + '/' + yt.category + '/' + yt.account.Id + '/' + yt.clear).then(function (f) {
                $scope.loading.youtube = false;
                logger.info("YouTube", "added to queue for uploads", f);
            }, function (err) {
                $scope.loading.youtube = false;
                logger.err("YouTube", "Failed", err);
            });
        }

        $scope.$watch("facebook.account", function (newVal) {
            if (!newVal || !newVal.Id) return;
            db.get(table + '/facebook/pages/' + newVal.Id).then(function (fbpages) {
                $scope.fbpages = fbpages;
                $scope.facebook.page = fbpages[0].ID;
            }, function (err) {
                if (err.ExceptionMessage) {
                    $scope.fberror = err.ExceptionMessage;
                } else {
                    console.log(err);
                }
            });
        });

        $scope.loadPages = function (item) {
            $scope.facebook.account = item;
        }

        function trashOne(card) {
            return db.trash(table, card.ID).then(function (result) {
                logger.log(title, 'Removed "' + card.Title + '" from list Successful', result);
            });
        }

        function findFirst(claims, provider) {
            for (var x in claims) {
                if (claims[x].Provider == provider) return claims[x];
            }
            return null;
        }

        function loadClaims() {
            return db.get(table + '/claims?$orderby=Provider,Client,Name&$select=Id,Name,Client,Provider').then(function (claims) {
                $scope.claims = claims;
                $scope.facebook.account = findFirst(claims, 'facebook');
                $scope.youtube.account = findFirst(claims, 'google');
                $scope.twitter.account = findFirst(claims, 'twitter');
            });
        }

        function loadTweets() {
            return db.get(table + '/tweet').then(function (tweets) {
                $scope.tweets = tweets;
            });
        }

        loadRecords()
        .then(loadClaims)
        .then(loadTweets);

        function loadRecords() {
            return db.card(table + '/card', $stateParams, $scope, function (q) {
                return 'substringof(\'' + q + '\',Title)';
            }).then(function () {
                $rootScope.cart = $scope.total;
            });
        }

    }]);