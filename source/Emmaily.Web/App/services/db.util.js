app.factory('db.util', [function() {

    function calculatePaging(page, pageSize) {
        pageSize = pageSize || factory.pageSize;
        var pg = "&$top=" + pageSize;
        if (page > 1) {
            pg += "&$skip=" + ((page - 1) * pageSize);
        }
        return pg;
    }

    function orderBy(order) {
        return '&$orderby=' + order;
    }

    var _formatBitrate = function (bits) {
        if (typeof bits !== 'number') {
            return '';
        }
        if (bits >= 1000000000) {
            return (bits / 1000000000).toFixed(2) + ' Gbit/s';
        }
        if (bits >= 1000000) {
            return (bits / 1000000).toFixed(2) + ' Mbit/s';
        }
        if (bits >= 1000) {
            return (bits / 1000).toFixed(2) + ' kbit/s';
        }
        return bits.toFixed(2) + ' bit/s';
    };

    var _formatTime = function (seconds) {
        var date = new Date(seconds * 1000),
            days = Math.floor(seconds / 86400);
        days = days ? days + 'd ' : '';
        return days +
        ('0' + date.getUTCHours()).slice(-2) + ':' +
        ('0' + date.getUTCMinutes()).slice(-2) + ':' +
        ('0' + date.getUTCSeconds()).slice(-2);
    };

    var _formatPercentage = function (floatValue) {
        return (floatValue * 100).toFixed(2) + ' %';
    };

    function _formatFileSize(bytes) {
        var exp = bytes === 0 ? 0 : Math.round(Math.log(bytes) / Math.log(1024) || 0);
        var result = (bytes / Math.pow(1024, exp)).toFixed(2);
        return result + ' ' + (exp === 0 ? 'bytes' : 'KMGTPEZY'[exp - 1] + 'B');
    }


    function _inKB(bytes) {
        return bytes / 1024;
    }

    function _inMB(bytes) {
        return _inKB(bytes) / 1024;
    }

    function _inGB(bytes) {
        return _inMB(bytes) / 1024;
    }

    function findCurrentIP() {
        var tmp = window.location.hash.split('#');
        return tmp[tmp.length - 1].split('?')[0].split('/')[0];
    }

    function getIP(ips) {
        if (!!ips) {
            var o = {};
            for (var i = 0; i < ips.length; i++) {
                var x = {
                    name: ips[i].Name,
                    plural: ips[i].Plural,
                    id: ips[i].ID,
                    icon: ips[i].Icon
                };
                o[makeWebSafe(x.name)] = x;
                o[makeWebSafe(x.plural)] = x;
            }
            this.IP = o;
        }
        return this.IP[findCurrentIP()];
    }
    function getAllIP() {
        return this.allIP;
    }

    function getAllIntellectNav() {
        return this.allIntellectNavs;
    }

    function setAllIP(ips) {
        this.allIP = ips;
        this.allIntellectNavs = JSON.parse(JSON.stringify(ips));
        this.allIntellectNavs.push({ Mode: 'a', Name: "Video", Plural: "Videos", Icon: "film" });
        this.allIntellectNavs.push({ Mode: 'a', Name: "Audio", Plural: "Audios", Icon: "music" });
        this.allIntellectNavs.push({ Mode: 'a', Name: "Image", Plural: "Images", Icon: "picture-o" });
        this.allIntellectNavs.push({ Mode: 'a', Name: "Document", Plural: "Documents", Icon: "file-o" });
        this.allIntellectNavs.unshift({ Mode: 'c', Name: "Company", Plural: "Companies", Icon: "building" });
    }
    function makeWebSafe(val) {
        if (val == null) return "";
        return val.replace(/[^a-zA-Z0-9]/g, '').toLowerCase();
    }

    return {
        pageSize: 10,
        calculatePaging: calculatePaging,
        orderBy: orderBy,
        ip: getIP,
        setAllIP: setAllIP,
        getAllIP: getAllIP,
        getAllIntellectNav: getAllIntellectNav,
        safe: makeWebSafe,
        format: {
            bit: _formatBitrate,
            time: _formatTime,
            cent: _formatPercentage,
            size: _formatFileSize,
            gb: _inGB
        },
        capitalize: function (val) {
            return val.replace(/^./, function (match) {
                return match.toUpperCase();
            });
        }
    };
}]);