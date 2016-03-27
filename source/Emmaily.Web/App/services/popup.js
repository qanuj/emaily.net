app.factory('Popup', ['$uibModal', function ($uibModal) {
   function open(opt) {
        opt.size = opt.size || 'lg';
        var modalInstance = $uibModal.open({
            animation: opt.animation,
            templateUrl: '/App/views/' + (opt.inPopup ? 'popup/' : '') + opt.key + '.html',
            controller: opt.ctrl,
            size: opt.size,
            resolve: {
                opt: function () {
                    return opt;
                }
            }
        });
        return modalInstance.result;
    }
    return {
        video: function (opt) {
            opt.key = 'video';
            opt.ctrl = 'VideoPlayerController';
            opt.inPopup = true;
            return open(opt);
        },
        embed: function (opt) {
            opt.key = 'embed';
            opt.ctrl = 'EmbedBuilderController';
            opt.inPopup = true;
            return open(opt);
        },
        history: function (opt) {
            opt.key = 'history';
            opt.ctrl = 'HistoryController';
            opt.inPopup = true;
            return open(opt);
        },
        comment: function (opt) {
            opt.key = 'comment';
            opt.ctrl = 'CommentController';
            opt.inPopup = true;
            return open(opt);
        },
        usage: function (opt) {
            opt.key = 'usage';
            opt.ctrl = 'UsageController';
            opt.inPopup = true;
            return open(opt);
        },
        trash: function (opt) {
            opt.key = 'trash';
            opt.ctrl = 'TrashController';
            opt.inPopup = true;
            return open(opt);
        },
        picker: function (opt) {
            opt.key = 'image.picker';
            opt.ctrl = 'ImagePickerController';
            opt.inPopup = true;
            return open(opt);
        },
        moderate: function (opt) {
            opt.key = 'moderate';
            opt.ctrl = 'ModerateController';
            opt.inPopup = true;
            return open(opt);
        },
        create: function (opt) {
            opt.ctrl = 'AnythingController';
            opt.inPopup = false;
            return open(opt);
        },
        open:function(opt) {
            opt.inPopup = false;
            return open(opt);;
        }
    }
}])
.controller('VideoPlayerController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    var url = '/watch?v=7&width=600&height=337&autostart=true&' + opt.mode + '=' + (opt.playID || opt.info.ID);
    $scope.url = url;
    $scope.info = opt.info;
    $scope.dismiss = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.openTab = function (url) {
        window.open(url, "_blank");
    };
}])
.controller('EmbedBuilderController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.info = opt.info;
    $scope.width = 480;
    $scope.height = 270;
    $scope.skin = 'lulu';
    $scope.autostart = true;
    $scope.isIframe = opt.isIframe;
    if (opt.code && typeof(opt.code)=='function') {
        $scope.code = function() {
            return opt.code($scope.width, $scope.height);
        }
    }else{
        $scope.code = function () {
            return '<iframe class="vz-player" allowfullscreen height="' + $scope.height +
                '" width="' + $scope.width + '"' +
                ' src="' + window.location.origin +
                '/watch?v=7&' + opt.mode + '=' + opt.info.id +
                '&height=' + $scope.height +
                '&width=' + $scope.width +
                ($scope.skin ? "&skin=" + $scope.skin : "") +
                ($scope.autostart == true ? "&autostart=true" : "") +
                '" frameborder="0" scrolling="no"></iframe>';
        }
    }
    $scope.dismiss = function () {
        $uibModalInstance.dismiss('cancel');
    }
}])
.controller('CommentController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.info = opt.info;

    function loadItems() {
        if (typeof (opt.items) == 'function') {
            opt.items().then(function (result) {
                $scope.comments = result;
            });
        } else {
            $scope.comments = opt.items;
        }
    }
   
    $scope.add = function(message) {
        opt.add(message).then(function() {
            $scope.message = '';
        }).then(loadItems);
    }
    $scope.dismiss = function () {
        $uibModalInstance.dismiss('cancel');
    }

    loadItems();
}])
.controller('AnythingController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.info = opt.info;
    $scope.usages = opt.items;
    $scope.record = opt.record || {};
    $scope.ok = function (info) {
        $uibModalInstance.close({ ok: true, data: info });
    };
    $scope.dismiss = function () {
        $uibModalInstance.close();
    }
}])
.controller('UsageController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.info = opt.info;
    $scope.usages = opt.items;
    $scope.dismiss = function () {
        $uibModalInstance.close();
    }
}])
.controller('ModerateController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.items = opt.items;
    $scope.filter = opt.filter;
    $scope.title = opt.title;
    $scope.check = opt.check;
    $scope.trashName = opt.trashName || 'Moderate';
    $scope.$watch("reason", function(newVal) {
        if (newVal) {
            $scope.reasonRequired = false;
        }
    });
    $scope.reject = function (reason) {
        if (!reason) {
            $scope.reasonRequired = true;
        } else {
            $uibModalInstance.close({ ok: true, reason: reason });
        }
    };
    $scope.approve = function () {
        $uibModalInstance.close({ ok: false });
    };
    $scope.dismiss = function () {
        $uibModalInstance.dismiss('cancel');
    }
}])
.controller('HistoryController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.info = opt.info;
    $scope.history = opt.items;
    $scope.dismiss = function () {
        $uibModalInstance.close();
    }
}])
.controller('TrashController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.items = opt.items;
    $scope.filter = opt.filter;
    $scope.title = opt.title;
    $scope.check = opt.check;
    $scope.trashName = opt.trashName || 'Delete';
    $scope.ok = function (deleteCheck) {
        $uibModalInstance.close({ ok: true, check: deleteCheck });
    };
    $scope.dismiss = function () {
        $uibModalInstance.dismiss('cancel');
    }
}])
.controller('ImagePickerController', ['$scope', '$uibModalInstance', 'opt', function ($scope, $uibModalInstance, opt) {
    $scope.items = opt.items;
    $scope.title = opt.title;
    $scope.ok = function (url) {
        $uibModalInstance.close({ ok: true, url: url });
    };
    $scope.dismiss = function () {
        $uibModalInstance.dismiss('cancel');
    }
}]);