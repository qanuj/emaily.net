app.controller('analyticsController', ['$scope', 'dataService', 'Popup', function ($scope, db, Popup) {

    var title = "Analytics";
    $scope.title = title;
    $scope.name = 'analytics';
    $scope.icon = "area-chart";
    $scope.actions = [];
    $scope.search = { ChannelID: 0, date: { startDate: moment().add(-1, 'month')._d, endDate: new Date() } };

    $scope.daterangeOptions = {
        timePicker: true,
        timePickerIncrement: 1,
        timePicker12Hour: true,
        applyClass: 'green',
        max:new Date(),
        cancelClass: 'default',
        format: 'MM/DD/YYYY',
        separator: ' to ',
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
            'Last 7 Days': [moment().subtract('days', 6), moment()],
            'Last 30 Days': [moment().subtract('days', 29), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
        }
    };

    $scope.chartOption = {
        scaleLabel: function (valuePayload) {
            return Number(valuePayload.value) + 'GB';
        }
    }

    $scope.viewChartOptions = {
        bezierCurve: true,
        showTooltips: true,
        useUtc: true,
        scaleDateFormat: "mmm d",
        scaleTimeFormat: "h:MM",
        scaleDateTimeFormat: "mmm d, yyyy, hh:MM",
        scaleShowHorizontalLines: true,
        scaleType: "date"
    };
    $scope.timeChartOptions = {
        bezierCurve: true,
        showTooltips: true,
        useUtc: true,
        scaleDateFormat: "mmm d",
        scaleTimeFormat: "h:MM",
        scaleDateTimeFormat: "mmm d, yyyy, hh:MM",
        scaleShowHorizontalLines: true,
        scaleLabel: function (valuePayload) {
            return _formatTime(valuePayload.value);
        },
        scaleType: "date"
    };

    function _formatTime(seconds) {
        var date = new Date(seconds * 1000),
            days = Math.floor(seconds / 86400);
        days = days ? days + 'd ' : '';
        return days +
        ('0' + date.getUTCHours()).slice(-2) + ':' +
        ('0' + date.getUTCMinutes()).slice(-2) + ':' +
        ('0' + date.getUTCSeconds()).slice(-2);
    };

    $scope.onClick = function (points) {
        if (points.length && points[0].label != 'Start') {
            $scope.current = map[points[0].label];
        }
    };

    $scope.$watch("search", function (newVal) {
        if (newVal && newVal.ChannelID > 0) {
            loadReport(newVal);
        }
    }, true);

    $scope.refreshIntellect = function (q) {
        db.get('ip/all?$filter=substringof(\'' + q + '\',Title)&$top=10&$orderby=Title').then(function (result) {
            $scope.intellects = result.Items;
        });
    };

    var lines = {
        SessionPerViewer: [],
        Sessions: [],
        Time: [],
        TimePerSession: [],
        TimePerViewer: [],
        Videos: [],
        VideosPerSession: [],
        VideosPerViewer: [],
        Views: [],
        Viewers:[]
    };

    var map = {};

    $scope.play = function (card) {
        Popup.video({ mode: 'epkey', info: card });
    }


    function parseReport(report) {
        $scope.topvideos = report.TopVideos;
        $scope.totals = report.Total[0];

        var time = report.length==1 ? moment(new Date(report[0].Created)).add(-1,'month')._d : undefined;
        for (var x in lines) {
            lines[x] = [];
            if (time) lines[x].push({ x: time ,y:0});
        }

        for (var i = 0; i < report.Items.length; i++) {
            var k = report.Items[i];
            k.time = new Date(k.Created);
            lines.SessionPerViewer.push({ x: k.time, y: k.SessionPerViewer });
            lines.Sessions.push({ x: k.time, y: k.Sessions });
            lines.Time.push({ x: k.time, y: k.Time });
            lines.TimePerSession.push({ x: k.time, y: k.TimePerSession });
            lines.TimePerViewer.push({ x: k.time, y: k.TimePerViewer });
            lines.Videos.push({ x: k.time, y: k.Videos });
            lines.VideosPerSession.push({ x: k.time, y: k.VideosPerSession });
            lines.VideosPerViewer.push({ x: k.time, y: k.VideosPerViewer });
            lines.Views.push({ x: k.time, y: k.Views });
            lines.Viewers.push({ x: k.time, y: k.Viewers });
            map[k.time] = k;
            $scope.current = k;
        }
        $scope.data = [{
            label: 'Views',
            strokeColor: '#26C281',
            data: lines.Views
        }, {
            label: 'Sessions',
            strokeColor: '#5C9BD1',
            data: lines.Sessions
        }, {
            label: 'Videos',
            strokeColor: '#22313F',
            data: lines.Videos
        }, {
            label: 'Unique Viewers',
            strokeColor: '#D91E18',
            data: lines.Viewers
        }, {
            label: 'Avg Videos per viewer',
            strokeColor: '#F7CA18',
            data: lines.VideosPerSession
        }, {
            label: 'Avg Session per viewer',
            strokeColor: '#8E44AD',
            data: lines.SessionPerViewer
        }, {
            label: 'Avg Videos per viewer',
            strokeColor: '#2AB4C0',
            data: lines.VideosPerViewer
        }];
        $scope.dataTime = [
            {
                label: 'Viewing Time',
                strokeColor: '#D91E18',
                data: lines.Time
            }, {
                label: 'Avg Time per viewer',
                strokeColor: '#9A12B3',
                data: lines.TimePerViewer
            }, {
                label: 'Avg Time per session',
                strokeColor: '#3598DC',
                data: lines.TimePerSession
            }
        ];
    }

    function loadReport(newVal) {
        db.post('pulse/analytics', { ChannelID: newVal.ChannelID, FromDate: moment(newVal.date.startDate).format(), ToDate: moment(newVal.date.endDate).format(), TopVidoes: 10 }).then(parseReport);
    }

    function loadChannels() {
        return db.get('library/all?$orderby=Title&$select=ID,Title').then(function (channels) {
            $scope.channels = channels;
            $scope.search.ChannelID = channels[0].ID;
        });
    }

    loadChannels();
}]);