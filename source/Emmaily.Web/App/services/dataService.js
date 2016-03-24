app.factory('dataService',['$q', '$http', function (q,$http) {
    var cache = { config: null, enumText: {} };
    var BASE = '/api/v2/my/';
    var pgSize = 24;
    var vm = {
        uri: function (ctrl) {
            return BASE + ctrl;
        },
        from: function (table) {
            return new query(table);
        },
        save: function (table, model, method) {
            return new query(table).save(model, method, model.ID || model.id || model.Id ? 'post' : 'put');
        },
        pageSize: pgSize,
        que: {
            status: function (id) {
                return get(BASE + 'que/status/' + id);
            },
            re: function (id) {
                return get(BASE + 'que/reque/' + id);
            }
        },
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
        ddMany: function (allDds) {
            return new query().ddMany(allDds);
        },
        create: function (table, model, method) {
            return new query(table).save(model, method, 'put');
        },
        update: function (table, model, method) {
            return new query(table).save(model, method, 'post');
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
        saveData: function (table, form, model, errors, next) {
            var validator = form.kendoValidator().data("kendoValidator");
            if (validator.validate()) {
                var data = map.toJS(model());
                return this.save(table, data).then(function (d) {
                    if (next && next.closed) {
                        next.closed(d, data);
                    } else {
                        next(d.ID, data);
                    }
                }).catch(function (d) {
                    errors(map.fromJS(d, {}));
                });
            }
        },
        clientBread: function (scope, clientId) {
            if (!clientId) return;
            return this.get('client/all?$select=ID,Title&$filter=ID eq ' + clientId).then(function (breads) {
                if (breads && breads.length) {
                    var bread = breads[0];
                    scope.breads = [{
                        link: '#/company/edit/=' + bread.ID,
                        icon: 'building',
                        title: bread.Title
                    }];
                }
            });
        },
        titleBread: function (scope, titleId) {
            if (!titleId) return;
            return this.get('title/all?$select=ID,ClientID,Title,Client&$filter=ID eq ' + titleId).then(function (breads) {
                if (breads && breads.length) {
                    var bread = breads[0];
                    scope.breads = [{
                        link: '#/titles?parent=' + bread.ClientID,
                        icon: 'building',
                        title: bread.Client
                    }, {
                        link: '#/title/edit/' + bread.ID,
                        icon: 'desktop',
                        title: bread.Title
                    }];
                }
            });
        },
        titleEditBread: function (scope, titleId) {
            if (!titleId) return;
            return this.get('title/all?$select=ID,ClientID,Title,Client&$filter=ID eq ' + titleId).then(function (breads) {
                if (breads && breads.length) {
                    var bread = breads[0];
                    scope.breads = [{
                        link: '#/client/edit/' + bread.ClientID,
                        icon: 'building',
                        title: bread.Client
                    }, {
                        link: '#/title/edit/' + bread.ID,
                        icon: 'desktop',
                        title: bread.Title
                    }];
                }
            });
        },
        contentBread: function (scope, contentId) {
            if (!contentId) return;
            return this.get('content/all?$select=ID,ClientID,ChannelID,Title,Client,Channel&$filter=ID eq ' + contentId).then(function (breads) {
                if (breads && breads.length) {
                    var bread = breads[0];
                    scope.breads = [{
                        link: '#/titles?parent=' + bread.ClientID,
                        icon: 'building',
                        title: bread.Client
                    }, {
                        link: '#/programmes?parent=' + bread.ChannelID,
                        icon: 'desktop',
                        title: bread.Channel
                    }, {
                        link: '#/programme/edit/' + bread.ID,
                        icon: 'video-camera',
                        title: bread.Title
                    }];
                }
            });
        },
        contentEditBread: function (scope, contentId) {
            if (!contentId) return;
            return this.get('content/all?$select=ID,ClientID,ChannelID,Title,Client,Channel&$filter=ID eq ' + contentId).then(function (breads) {
                if (breads && breads.length) {
                    var bread = breads[0];
                    scope.breads = [{
                        link: '#/company/edit/' + bread.ClientID,
                        icon: 'building',
                        title: bread.Client
                    }, {
                        link: '#/title/edit/' + bread.ChannelID,
                        icon: 'desktop',
                        title: bread.Channel
                    }, {
                        link: '#/programme/edit/' + bread.ID,
                        icon: 'video-camera',
                        title: bread.Title
                    }];
                }
            });
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
                order: 'ID desc',
                counts:true
            }
        },
        postEx: function (url, model) {
            return post(url, model);
        },
        post: function (url, model) {
            return post(BASE+url, model);
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
                scope.pages = Math.ceil(result.Count / k.size);
                scope.records = result.Items || result;
                scope.cards = result;
                scope.start = ((k.page - 1) * k.size) + 1;
                scope.end = (scope.start - 1) + (result.Items || result).length;
                scope.total = result.Count;
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

    function post(url, model) {
        var deferred = q.defer();
        $.ajax({
            type: 'POST',
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

    function groupBy(array, f) {
        var groups = {};
        array.forEach(function (o) {
            var group = JSON.stringify(f(o));
            groups[group] = groups[group] || [];
            groups[group].push(o);
        });
        return Object.keys(groups).map(function (group) {
            return groups[group];
        });
    }


    var query = function (table) {
        var base = '/api/v2/my/' + table;

        var fillTo = function (fld, source) {
            var items = source.Items || source;
            fld.items.removeAll();
            for (var x in items) {
                fld.items.push(items[x]);
            }
            if (!!fld.count) {
                fld.count(source.Count || items.length);
            }
            return true;
        };
        var fillAsGroup = function (fld, source) {
            var items = source.Items || source;
            var result = groupBy(items, function (item) {
                return [item[fld.by]];
            });
            fld.items.removeAll();
            for (var x in result) {
                if (result[x].length > 0) {
                    fld.items.push(ko.observable({ id: ko.observable(result[x][0][fld.by]), name: ko.observable(result[x][0][fld.nest]), items: ko.observableArray(result[x]) }));
                }
            }
            if (!!fld.count) {
                fld.count(result.length);
            }
            return true;
        };
        var allRecords = function (qry) {
            var deferred = q.defer();
            $.ajax({
                type: 'get',
                url: qry.base,
                error: function (e) {
                    deferred.reject(e.responseText);
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
        };

        var kendoSource = function (page, source, pageSize, filter) {
            var uri = base + (source || '') + (typeof (filter) === 'string' ? '&$filter=' + filter : '');
            return new kendo.data.DataSource({
                parameterMap: function (options) {
                    var paramMap = kendo.data.transports.odata.parameterMap(options);
                    delete paramMap.format;
                    return paramMap;
                },
                page: page || 1,
                filter: typeof (filter) === 'string' ? {} : filter,
                schema: {
                    data: function (data) {
                        if (data === 'Unauthorized') { console.error('Access Denied - Kendo', uri); }
                        return (data && data.Items) || [];
                    }, total: function (data) {
                        return (data && data.Count) || 0;
                    }
                },
                pageSize: pageSize || 20,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                type: 'odata',
                transport: {
                    read: {
                        url: uri,
                        dataType: "json"
                    }
                }
            });
        };
        var trash = function (idOrIDs, method) {
            

        };

        var parseError = function (eJson) {
            var e = {};
            var tmp = {};
            try {
                if (typeof (eJson) === 'object') { e = eJson; }
                else e = JSON.parse(eJson);

                for (var x in e) {
                    tmp[x] = [];
                    for (var err in e[x].Errors) {
                        var msg = e[x].Errors[err].ErrorMessage;
                        if (!msg || msg === "") { msg = e[x].Errors[err].Exception && e[x].Errors[err].Exception.Message; }
                        tmp[x].push(msg);
                    }
                }
            } catch (ex) {
                return ex;
            }
            return tmp;
        };

        var saveData = function (model, method, mode) {
            var deferred = q.defer();
            //q.stopUnhandledRejectionTracking();TODO:this would not work on Angular;

            $.ajax({
                type: mode,
                url: base + (!!method ? '/' + method : ''),
                data: model,
                statusCode: {
                    400: function (e) {
                        // deferred.reject(parseError(e.responseJSON));
                    }
                },
                error: function (e) {
                    deferred.reject(parseError(e.responseJSON));
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
        };



        var deleteByModel = function (method, model) {
            var deferred = q.defer();
            q.stopUnhandledRejectionTracking();

            $.ajax({
                type: 'DELETE',
                url: base + (!!method ? '/' + method : ''),
                data: model,
                statusCode: {
                    400: function (e) {
                        // deferred.reject(parseError(e.responseJSON));
                    }
                },
                error: function (e) {
                    deferred.reject(parseError(e.responseJSON));
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
        };

        var createTreeList = function (box, roots, name, fix) {
            fix = (fix || '');
            for (var x in roots) {
                roots[x][name] = fix + roots[x][name];
                box.push(roots[x]);
                if (roots[x].children.length > 0) {
                    createTreeList(box, roots[x].children, name, fix + '__');
                }
            }
            return box;
        };

        var parseAsTree = function (data, fld) {
            var group = fld.tree, name = fld.name, id = fld.id;

            data.sort(function (a, b) { return a[group] - b[group]; });
            var obj = {}, node, roots = [];
            for (var i = 0; i < data.length; i += 1) {
                node = data[i];
                node.children = [];
                obj[node[id]] = i; // use map to look-up the parents
                if (!!node[group]) {
                    var p = data[obj[node[group]]];
                    if (!p) {
                        roots.push(node);
                    } else {
                        p.children.push(node);
                    }
                } else {
                    roots.push(node);
                }
            }
            return createTreeList([], roots, name.split(',')[0], '|_');
        };

        var dd = function (fld) {
            var fi = fillTo;
            base = !!table ? base : ('/api/v2/my/' + fld.from);
            if (!fld.all) {
                base = base + '/all';
            }
            var orderBy = fld.name, select = fld.id + ',' + fld.name;
            if (fld.by) {
                orderBy = fld.by + ',' + orderBy;
                select += ',' + fld.nest + ',' + fld.by;
                fi = fillAsGroup;
            }
            return allRecords({
                base: base + '?$orderby=' + orderBy + '&$select=' + select + (!!fld.filter ? '&$filter=' + fld.filter : '')
            }).then(function (d) { return fi(fld, fld.tree ? parseAsTree(d, fld) : d); });
        };

        return {
            dd: dd,
            ddMany: function (dds) {
                var i = -1;
                var next = function () {
                    i++;
                    if (!!dds[i]) {
                        if (dds[i].enum) {
                            return vm.enums(dds[i].enum).then(function (l) {
                                dds[i].items(l);
                            });
                        } else {
                            return dd(dds[i]).then(next);
                        }
                    }
                };
                return next();
            },
            all: function (page, size, filter, mode, parentId) {
                return kendoSource(page, (mode ? '?$orderby=ID desc&mode=' + mode : '?$orderby=ID desc') + (!!parentId ? '&parentId=' + parentId : ''), size || 20, filter); //index view is default
            },
            tree: function (act, id, field) {
                return get(table + '/' + act + (id ? '?' + (field || 'id') + '=' + id : ''));
            },
            cards: function (page, size, filter, mode, parentId) {
                return kendoSource(page, '/card?$orderby=ID desc' + (!!mode ? '&mode=' + mode : '') + (!!parentId ? '&parentId=' + parentId : ''), size || 24, filter); //card is second view
            },
            odata: function (page, size) {
                return kendoSource(page, '/odata?$orderby=ID desc', size || 20);
            },
            save: saveData,
            trash: trash,
            deleteByModel: deleteByModel,
            byId: function (id) {
                var deferred = q.defer();
                $.ajax({
                    type: 'get',
                    url: base + '/' + id,
                    error: function (e) {
                        deferred.reject(e.responseText);
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
            }
        };
    };
    return vm;
}]);