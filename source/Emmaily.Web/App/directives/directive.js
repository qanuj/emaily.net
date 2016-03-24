app.directive('loading', [
    '$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };
    }
]);
app.directive('ionslider',['$timeout', function ($timeout) {
    return {
        restrict: 'E',
        scope: {
            min: '@',
            max: '@',
            fromMin: '@',
            fromMax: '@',
            toMin: '@',
            toMax: '@',
            ngModel: '=',
            type: '@',
            prefix: '@',
            maxPostfix: '@',
            prettify: '&',
            grid: '@',
            keyboard: '@',
            gridMargin: '@',
            postfix: '@',
            step: '@',
            hideMinMax: '@',
            hideFromTo: '@',
            from: '=',
            to: '=',
            disable: '=',
            dragInterval: '@',
            fromShadow: '@',
            toShadow: '@'
        },
        template: '<div></div>',
        replace: true,
        link: function ($scope, $element, attrs) {
            (function init() {

                $element.ionRangeSlider({
                    min: $scope.min,
                    max: $scope.max,
                    from_min: $scope.fromMin,
                    from_max: $scope.fromMax,
                    drag_interval: $scope.dragInterval,
                    from_shadow: $scope.fromShadow,
                    to_shadow: $scope.toShadow,
                    to_min: $scope.toMin,
                    to_max: $scope.toMax,
                    type: $scope.type,
                    keyboard: $scope.keyboard,
                    prefix: $scope.prefix,
                    maxPostfix: $scope.maxPostfix,
                    prettify: function (val) {
                        if (angular.isDefined(attrs.prettify)) {
                            return $scope.prettify({value:val});
                        }
                        return val;
                    },
                    grid: $scope.grid,
                    gridMargin: $scope.gridMargin,
                    postfix: $scope.postfix,
                    step: $scope.step,
                    hideMinMax: $scope.hideMinMax,
                    hideFromTo: $scope.hideFromTo,
                    from: $scope.from,
                    to: $scope.to,
                    disable: $scope.disable,
                    onChange: onChange,
                    onFinish: onFinish,
                    values_separator: " → "
                });
            })();
            function onChange(data) {
                //console.log('Change',data);
            }                              
            function onFinish(data) {
                $scope.from = data.from;
                $scope.to = data.to;
                if ($scope.type == 'double') {
                    $scope.ngModel = { from: data.from, to: data.to };
                } else {
                    $scope.ngModel = data.from;
                }
                $scope.$apply();
            }
            $scope.$watch('ngModel', function (value) {
                if (!value) return;
                if ($scope.type == 'double') {
                    $timeout(function () { $element.data("ionRangeSlider").update(value); });
                } else {
                    $timeout(function () { $element.data("ionRangeSlider").update({ from: value }); });
                }
            }, true);
            var toWatch = ['min', 'max', 'from', 'step', 'to', 'disable'];
            var toWatch2 = ['fromMin', 'fromMax', 'toMin', 'toMax','fromShadow','toShadow'];
            function toUnderscore(name){
                return name.replace(/([A-Z])/g, function ($1) { return "_" + $1.toLowerCase(); });
            }
            toWatch.forEach(function (name, i) {
                $scope.$watch(name, function (value) {
                    var opt = {};opt[name] = value;
                    $timeout(function () { $element.data("ionRangeSlider").update(opt); });
                }, true);
            });
            toWatch2.forEach(function (name, i) {
                $scope.$watch(name, function (value) {
                    var opt = {}; opt[toUnderscore(name)] = value;
                    $timeout(function () { $element.data("ionRangeSlider").update(opt); });
                }, true);
            });
        }
    }
}]);
app.filter('propsFilter', function () {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            items.forEach(function (item) {
                var itemMatches = false;

                var keys = Object.keys(props);
                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
});
app.directive('nagPrism', [
    function () {
        return {
            restrict: 'A',
            scope: {
                source: '@'
            },
            link: function (scope, element, attrs) {
                scope.$watch('source', function (v) {
                    if (v) {
                        Prism.highlightElement(element.find("code")[0]);
                    }
                });
            },
            template: "<code ng-bind='source'></code>"
        };
    }
]);
app.directive('ngSpinnerBar', [
    '$rootScope',
    function ($rootScope) {
        return {
            link: function (scope, element, attrs) {
                // by defult hide the spinner bar
                element.addClass('hide'); // hide spinner bar by default

                // display the spinner bar whenever the route changes(the content part started loading)
                $rootScope.$on('$stateChangeStart', function () {
                    element.removeClass('hide'); // show spinner bar
                });

                // hide the spinner bar on rounte change success(after the content loaded)
                $rootScope.$on('$stateChangeSuccess', function () {
                    element.addClass('hide'); // hide spinner bar
                    $('body').removeClass('page-on-load'); // remove page loading indicator
                    Layout.setSidebarMenuActiveLink('match'); // activate selected link in the sidebar menu

                    // auto scorll to page top
                    setTimeout(function () {
                        Metronic.scrollTop(); // scroll to the top on content load
                    }, $rootScope.settings.layout.pageAutoScrollOnLoad);
                });

                // handle errors
                $rootScope.$on('$stateNotFound', function () {
                    element.addClass('hide'); // hide spinner bar
                });

                // handle errors
                $rootScope.$on('$stateChangeError', function () {
                    element.addClass('hide'); // hide spinner bar
                });
            }
        };
    }
]);
app.directive('a', function () {
    return {
        restrict: 'E',
        link: function (scope, elem, attrs) {
            if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                elem.on('click', function (e) {
                    e.preventDefault(); // prevent link click for above criteria
                });
            }
        }
    };
});
app.directive('dropdownMenuHover', function () {
    return {
        link: function (scope, elem) {
            elem.dropdownHover();
        }
    };
});

app.directive('toggle', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            // prevent directive from attaching itself to everything that defines a toggle attribute
            if (!element.hasClass('selectpicker')) {
                return;
            }

            var target = element.parent();
            var toggleFn = function () {
                target.toggleClass('open');
            };
            var hideFn = function () {
                target.removeClass('open');
            };

            element.on('click', toggleFn);
            element.next().on('click', hideFn);

            scope.$on('$destroy', function () {
                element.off('click', toggleFn);
                element.next().off('click', hideFn);
            });
        }
    };
});
app.directive('uploadzone', function () {
    return function (scope, element, attrs) {
        var config, dropzone;

        config = scope[attrs.dropzone];

        // create a Dropzone for the element with the given options
        dropzone = new FileUpload(element[0], config.options);

        // bind the given event handlers
        angular.forEach(config.eventHandlers, function (handler, event) {
            dropzone.on(event, handler);
        });
    };
});
app.directive('selectpicker', [
        '$parse', function($parse) {
            return {
                restrict: 'A',
                require: '?ngModel',
                priority: 10,
                compile: function(tElement, tAttrs, transclude) {
                    tElement.selectpicker($parse(tAttrs.selectpicker)());
                    tElement.selectpicker('refresh');
                    return function(scope, element, attrs, ngModel) {
                        if (!ngModel) return;

                        scope.$watch(attrs.ngModel, function(newVal, oldVal) {
                            scope.$evalAsync(function() {
                                if (!attrs.ngOptions || /track by/.test(attrs.ngOptions)) element.val(newVal);
                                element.selectpicker('refresh');
                            });
                        });

                        ngModel.$render = function() {
                            scope.$evalAsync(function() {
                                element.selectpicker('refresh');
                            });
                        }
                    };
                }

            };
        }
    ])
    .filter('highlight', [
        '$sce', function($sce) {
            return function(text, phrase) {
                if (phrase)
                    text = text.replace(new RegExp('(' + phrase + ')', 'gi'),
                        '<span class="highlighted">$1</span>');

                return $sce.trustAsHtml(text);
            }
        }
    ])
    .directive('backgroundImage', function() {
        return {
            priority: 99, // it needs to run after the attributes are interpolated
            link: function(scope, element, attr) {
                attr.$observe('backgroundImage', function(value) {
                    element.css({
                        'background-image': 'url(' + value + ')'
                    });
                });
            }
        };
    })
    .directive('vzSize', function() {
        return function(scope, element, attrs) {
            element.css({
                'min-height': attrs.vzSize,
                'max-height': attrs.vzSize,
                'background-size': '100%',
                'background-repeat': 'no-repeat'
            });
        };
    });
app.directive('applyUniform',['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attr, ngModel) {
            element.uniform({ useID: false });
            scope.$watch(function () { return ngModel.$modelValue }, function () {
                $timeout(jQuery.uniform.update, 0);
            });
        }
    };
}]);
app.directive('vzEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.vzEnter);
                });
                event.preventDefault();
            }
        });
    };
});
app.directive('fillHeight', ['$window', '$document', '$timeout', function ($window, $document, $timeout) {
    return {
        restrict: 'A',
        scope: {
            footerElementId: '@',
            additionalPadding: '@',
            debounceWait: '@'
        },
        link: function (scope, element, attrs) {
            if (scope.debounceWait === 0) {
                angular.element($window).on('resize', onWindowResize);
            } else {
                // allow debounce wait time to be passed in.
                // if not passed in, default to a reasonable 250ms
                angular.element($window).on('resize', debounce(onWindowResize, scope.debounceWait || 250));
            }

            onWindowResize();

            // returns a fn that will trigger 'time' amount after it stops getting called.
            function debounce(fn, time) {
                var timeout;
                // every time this returned fn is called, it clears and re-sets the timeout
                return function () {
                    var context = this;
                    // set args so we can access it inside of inner function
                    var args = arguments;
                    var later = function () {
                        timeout = null;
                        fn.apply(context, args);
                    };
                    $timeout.cancel(timeout);
                    timeout = $timeout(later, time);
                };
            }

            function onWindowResize() {
                var footerElement = angular.element($document[0].getElementById(scope.footerElementId));
                var footerElementHeight;

                if (footerElement.length === 1) {
                    footerElementHeight = footerElement[0].offsetHeight
                          + getTopMarginAndBorderHeight(footerElement)
                          + getBottomMarginAndBorderHeight(footerElement);
                } else {
                    footerElementHeight = 0;
                }

                var elementOffsetTop = element[0].offsetTop;
                var elementBottomMarginAndBorderHeight = getBottomMarginAndBorderHeight(element);

                var additionalPadding = scope.additionalPadding || 0;

                var elementHeight = $window.innerHeight
                                    - elementOffsetTop
                                    - elementBottomMarginAndBorderHeight
                                    - footerElementHeight
                                    - additionalPadding;
                element.css('height', elementHeight + 'px');
            }

            function getTopMarginAndBorderHeight(element) {
                var footerTopMarginHeight = getCssNumeric(element, 'margin-top');
                var footerTopBorderHeight = getCssNumeric(element, 'border-top-width');
                return footerTopMarginHeight + footerTopBorderHeight;
            }

            function getBottomMarginAndBorderHeight(element) {
                var footerBottomMarginHeight = getCssNumeric(element, 'margin-bottom');
                var footerBottomBorderHeight = getCssNumeric(element, 'border-bottom-width');
                return footerBottomMarginHeight + footerBottomBorderHeight;
            }

            function getCssNumeric(element, propertyName) {
                return parseInt(element.css(propertyName), 10) || 0;
            }
        }
    };
}]);
app.filter("toArray", function () {
    return function (obj) {
        var result = [];
        angular.forEach(obj, function (val, key) {
            result.push(val);
        });
        return result;
    };
});
app.directive("btnLoading", function () {
    return function (scope, element, attrs) {
        scope.$watch(function () {
            return scope.$eval(attrs.ngDisabled);
        }, function (newVal) {
            if (newVal) {
                return;
            } else {
                return scope.$watch(function () {
                    return scope.$eval(attrs.btnLoading);
                },function (loading) {
                    if (loading)
                        return element.button("loading");
                    element.button("reset");
                });
            }
        });
    };
});