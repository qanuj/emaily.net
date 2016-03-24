app.factory('logger', ['Talker', 'toaster', function (talker, toaster) {

    var vm = {
        log: function (title,message) {
            toaster.pop({
                type: 'success',
                title: title,
                body: message
            });
            console.log.apply(console, arguments);
        },
        info: function (title, message) {
            toaster.pop({
                type: 'info',
                title: title,
                body: message
            });
            console.info.apply(console, arguments);
        },
        err: function (title, message) {
            toaster.pop({
                type: 'error',
                title: title,
                body: message
            });;
            console.error.apply(console, arguments);
        }
    }
    return vm;
}]);