app.factory('Talker', [function () {
    //$.connection.hub.logging = true;
    var hub = $.connection.notificationHub;
    var responders = {
        any: []
    };
    hub.client.any = callHanders('any');

    function callHanders(name) {
        return function (mode, msg) {
            //console.log.apply(console, arguments);
            for (var x in responders[name]) {
                var caller = responders[name][x];
                if (caller && typeof (caller) === typeof (Function)) {
                    caller(mode, msg);
                }
            }
        }
    }

    var vm = {
        on: function (name, cb) {
            responders[name] = responders[name] || [];
            responders[name].push(cb);
        },
        join: function (room) {
            var that = this;
            if (!room) return;
            var roomWatcher = setInterval(function () {
                if (that.connected) {
                    //console.log('joining room', room);
                    hub.server.connect(room);
                    clearTimeout(roomWatcher);
                }
            }, 2000);
        },
        connect: function (userid) {
            var that = this;
            if (!that.connected) {
                $.connection.hub.qs = "userid="+userid;
                $.connection.hub.start().done(function () {
                    that.connected = true;
                })
                .fail(function (a) {
                    console.log('failed to connect signalr : ' + a);
                });
            } else {
                console.trace('connecting twice? : ', userid);
            }
            return true;
        },
        talk: function (msg) {
            if (!msg) return;
            var that = this;
            var talkWatcher = setInterval(function () {
                if (that.connected) {
                    hub.server.talk(msg);
                    clearTimeout(talkWatcher);
                }
            }, 2000);
        }
    };

    return vm;

}]);