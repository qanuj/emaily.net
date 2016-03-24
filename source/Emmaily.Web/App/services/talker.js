app.factory('Talker', [function () {
    var hub = $.connection.notificationHub;
    var responders = {
        broadcast: [],
        que: [],
        recording:[]
    };

    function callHanders(name) {
        return function (msg) {
            console.log('MSG', name, msg);
            for (var x in responders[name]) {
                var caller = responders[name][x];
                if (caller && typeof (caller) === typeof (Function)) {
                    caller(msg);
                }
            }
        }
    }

    var vm = {
        on: function (name, cb) {
            responders[name] = responders[name] || [];
            responders[name].push(cb);
            if (!hub.client[name]) {
                hub.client[name] = callHanders(name);
            }
        },
        received: function (msg) {
            //console.log('msg-received', msg);
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
        connect: function (room) {
            var that = this;
            if (!that.connected) {
                $.connection.hub.start().done(function () {
                    that.connected = true;
                    that.join(room);
                    that.join('lobby');
                })
                .fail(function (a) {
                    console.log('failed to connect signalr : ' + a);
                });
            } else {
                console.trace('connecting twice? : ', room);
            }
            return true;
        },
        talk: function (msg) {
            if (!msg) return;
            var that = this;
            var talkWatcher = setInterval(function () {
                if (that.connected) {
                    //console.log('sending', msg);
                    hub.server.talk(msg);
                    clearTimeout(talkWatcher);
                }
            }, 2000);

        }
    };

    //default handlers;
    vm.on('broadcast', function (msg) {
        //console.log('in room', msg);
    });
    vm.on('recording', function (msg) {
        //console.log('recording-msg', msg);
    });
    vm.on('que', function (msg) {
        //console.log('que-msg', msg);
    });
    vm.on('any', function (msg) {
        //console.log('que-msg', msg);
    });

    return vm;

}]);