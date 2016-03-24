app.factory('reader', ['$http', function ($http) {
    return {
        parse: function (list) {
            return $http.jsonp('//ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=50&callback=JSON_CALLBACK&q=' + encodeURIComponent(list[0].url));
        }
    }
}]);