app.factory('dataService',['$q', '$http', function (q,$http) {
    var cache = { config: null, enumText: {} };
    var BASE = '/api/v1/';
    var pgSize = 24;
    var vm = {
        uri: function (ctrl) {
            return BASE + ctrl;
        },
        from: function (table) {
            return new query(table);
        },
        save: function (table, model, method) {
            return new query(table).save(model, method, model.Id ? 'post' : 'put');
        },
        pageSize: pgSize,
        util: {
            base: function () {
                return getUtil('base');
            },
            me: function () {
                return getUtil('me').then(function (d) {
                    vm.about = d;
                    return d;
                });
            },
            config: function () {
                return getUtil('config');
            }
        },
        enumText: function () {
            return cache.enumText;
        },
        enums: function (name, v) {
            return getUtil('enums').then(function (d) {
                for (var x in d) {
                    cache.enumText[x] = {};
                    for (var y in d[x]) {
                        d[x][y].Label = d[x][y].Label.replace(/([a-z])([A-Z])/g, '$1 $2');
                        cache.enumText[x][d[x][y].ID] = d[x][y].Label;
                    }
                }
                return !v ? d[name] : cache.enumText[name];
            });
        },
        get: function(url, page, pageSize, counts, orderBy, filter) {
            return get(BASE+url, page, pageSize, counts, orderBy, filter);
        },
        getPaged: getPaged,
        deleteByModel: function (table, method, model) {
            return new query(table).deleteByModel(method, model);
        },
        trash: function (table, id) {
            var deferred = q.defer();
            $.ajax({
                type: 'DELETE',
                url: BASE + table +'/'+ id,
                statusCode: {
                    400: function (e) {
                        deferred.reject(e.responseJSON);
                    }
                },
                error: function (e) {
                    deferred.reject(e.responseJSON);
                },
                success: function (d) {
                    if (d == 'Unauthorized') {
                        deferred.reject('Access Denied');
                    } else {
                        deferred.resolve(d);
                    }
                }
            });
            return deferred.promise;
        },
        createPageQuery: function (stateParams, filterOrSearchFunction, searchFunction) {
            var page = stateParams.page || 1;
            var size = stateParams.pageSize || pgSize;
            if (typeof (filterOrSearchFunction) == 'function') {
                searchFunction = filterOrSearchFunction;
            }
            var filter = searchFunction && stateParams.q ?
                        searchFunction(stateParams.q) :
                        stateParams.parent ?
                        (stateParams.parentMode || 'ParentID') + " eq " + stateParams.parent : "";

            if (filterOrSearchFunction && filterOrSearchFunction.filter) {
                if (filter!="") {
                    filter += " and ";
                }
                filter += filterOrSearchFunction.filter;
            }

            return {
                page: page,
                size: size,
                filter: filter,
                order: 'Id desc',
                counts:true
            }
        },
        postEx: function (url, model) {
            return post(url, model);
        },
        post: function (url, model) {
            return post(BASE + url, model);
        },
        put: function (url, model) {
            return post(BASE + url, model,'PUT');
        },
        getEx: function (url) {
            return get(url);
        },
        card: function (query, stateParams, scope, filterOrSearchFunction,searchFunction) {
            scope.refreshing = true;
            var that = this;
            var k = that.createPageQuery(stateParams,filterOrSearchFunction, searchFunction);
            scope.paging = k;

            scope.all = function (val) {
                scope.selectAll = val;
            }
            scope.toggleOne = function (one) {
                one.selected = !one.selected;
            }
            scope.$watch("selectAll", function (newVal) {
                selectAll(newVal);
            });

            function selectAll(what) {
                if (!scope.records) return;
                for (var i = 0; i < scope.records.length; i++) {
                    scope.records[i].selected = what;
                }
            }

            return this.get(query, k).then(function (result) {
                scope.selectAll = false;
                scope.refreshing = false;
                k.page = parseInt(k.page);
                k.size = parseInt(k.size);
                scope.currentPage = k.page || 1;
                scope.pages = Math.ceil(result.count / k.size);
                scope.records = result.items || result;
                scope.cards = result;
                scope.start = ((k.page - 1) * k.size) + 1;
                scope.end = (scope.start - 1) + (result.items || result).length;
                scope.total = result.count;
            });
        },
        paged: function (query, stateParams, scope, searchFunction) {
                scope.refreshing = true;
                var that = this;
                var k = that.createPageQuery(stateParams, searchFunction);
                scope.paging = k;
                return this.get(query, k).then(function (result) {
                    scope.selectAll = false;
                    scope.refreshing = false;
                    k.page = parseInt(k.page);
                    k.size = parseInt(k.size);
                    scope.currentPage = k.page || 1;
                    scope.pages = Math.ceil(result.Count / k.size);
                    scope.records = result.Items;
                    scope.start = ((k.page - 1) * k.size) + 1;
                    scope.end = (scope.start - 1) + result.Items.length;
                    scope.total = result.Count;
                });
            }
    };

    vm.me= {
        get:function() {
            return $http.get(BASE + 'account/me/profile');
        }
    }

    function post(url, model,httpMethod) {
        var deferred = q.defer();
        $.ajax({
            type: httpMethod||'POST',
            data: model,
            url: url,
            error: function (e) {
                if (e.status == 0) { e.responseText = "Connection refused by server."; }
                else if (e.status == 404) { e.responseText = "api not found"; }
                if (e.responseJSON) {
                    var errors = [];
                    for (var x in e.responseJSON) {
                        var Errors = e.responseJSON[x] && e.responseJSON[x].Errors;
                        if (Errors) {
                            for (var err in Errors) {
                                errors.push(Errors[err].ErrorMessage);
                            }
                        }
                    }
                    deferred.reject(errors.join("<br/>"));
                } else {
                    deferred.reject(e.responseText);
                }
            },
            success: function (d) {
                if (d === 'Unauthorized') {
                    console.error('Access Denied', url);
                    deferred.resolve();
                } else {
                    cache[url] = d;
                    deferred.resolve(d);
                }
            }
        });
        return deferred.promise;
    }

    function getCached(url) {
        return get(url);
    }

    function getUtil(url) {
        return getCached(BASE + 'util/' + url);
    }

    function defer(val) {
        var deferred = q.defer();
        setTimeout(function () {
            deferred.resolve(val);
        }, 100);
        return deferred.promise;
    }

    function getPaged(url,page,orderBy,filter) {
        return get(BASE+url, page, vm.pageSize, true, orderBy, filter);
    }

    function get(url, page, pageSize, counts, orderBy, filter) {
        if (typeof (page) == 'object') {
            pageSize = page.size;
            counts = page.counts;
            orderBy = page.order;
            filter = page.filter;
            page = page.page;
        }
        var deferred = q.defer();
        pageSize = pageSize || -1;
        page = page || 1;
        if (url.indexOf('?') == -1) {
            url += "?";
        }

        if (pageSize != -1) {
            url += "&$top=" + pageSize;
            if (page>1) {
                url += "&$skip=" + (pageSize*(page-1));
            }
            if (counts) {
                url += "&$inlinecount=allpages";
            }
        }
        if (orderBy) {
            url += "&$orderby=" + orderBy;
        }
        if (filter) {
            url += "&$filter=" + filter;
        }

        function runUrl() {
            $http.get(url).success(function (d) {
                if (d === 'Unauthorized') {
                    console.error('Access Denied', url);
                    deferred.resolve();
                } else {
                    cache[url] = d;
                    deferred.resolve(d);
                }
            }).error(function (e, status) {
                if (e && e.ExceptionMessage && e.ExceptionMessage.indexOf('A second operation started on this context before a previous asynchronous operation completed') > -1) {
                    setTimeout(runUrl, 200);
                } else if (status == 404) {
                    deferred.resolve();
                }else{
                    console.error('ERR!', status);
                    deferred.reject(e, status);
                }
            });
        }

        runUrl();

        return deferred.promise;
    }

    return vm;
}]);