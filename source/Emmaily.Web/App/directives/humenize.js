'use strict';
(function () {

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

    function isEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    function fixPicture(value, all) {
        var size = 150, email = null;
        if (all && !!all.size) { size = unwrap(all.size); }
        value = unwrap(value);
        if (typeof (value) === 'object') {
            if (!value) { return value; }
            email = value.Email;
            value = value.Picture || value.Email;
        }
        value = unwrap(value);
        if (isEmail(value)) { return '//www.gravatar.com/avatar/' + md5(email) + '?s=' + size + '&d=mm'; }
        if (!!value) {
            if (value.indexOf('youtube.com') > -1) return value;
            if (value.indexOf('http://') === -1) value = 'http://images.vidzapper.com/' + value;
            if (value.indexOf('www.gravatar.com') === -1 && value.indexOf("notfound") === -1 && value.indexOf('placehold.it/') === -1 && value !== "http://images.vidzapper.com/") return '//uplive.vzconsole.com/img/' + (value.replace('http://', '').replace('https://', '')) + '?h=' + size;
        }
        email = unwrap(email);
        if (!!email) return '//www.gravatar.com/avatar/' + md5(email) + '?s=' + size + '&d=mm';
        return null;
    }

    var funcs={
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

    angular
     .module('humenize', [])
    .filter('uncamel', function () {
        function decamelize(str, sep) {
            if (typeof str !== 'string') {
                throw new TypeError('Expected a string');
            }
            return str.replace(/([a-z\d])([A-Z])/g, '$1' + (sep || '_') + '$2').toLowerCase();
        }
        return function (input, allLower) {

            if (typeof input !== "string") {
                return input;
            }

            var result = decamelize(input, ' ');

            if (!allLower) {
                result = result.charAt(0).toUpperCase() + result.slice(1);
            }

            return result;
        };
    })
    .filter('nohash', function () {
        return function (val) {
            if (!val) return val;
            function camelize(str) {
                return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (letter, index) {
                    return index == 0 ? letter.toLowerCase() : letter.toUpperCase();
                }).replace(/\s+/g, '');
            }
            return camelize(val.replace('#', '').split("/").join(" "));
        }
    })
    .filter('plaintext', function () {
        return function (html) {
            var div = document.createElement("div");
            div.innerHTML = html;
            return div.textContent || div.innerText || "";
        };
    })
    .filter('unitsofseconds', function () {
        return function () {
            return (new Date()).getTime();
        };
    })
    .directive('relativeTime',['$timeout', function ($timeout) {

        function getRelativeDateTimeString(date) {
            if (date && date.indexOf('+') == -1 && date.indexOf('Z') == -1) date += '+00:00';
            return moment(date).utc().fromNow();
        }

        function update(scope, element) {
            element.text(getRelativeDateTimeString(scope.actualTime));
            $timeout(function() { update(scope, element); }, 60000);
        }

        return {
            scope: {
                actualTime: '=relativeTime'
            },
            link: function(scope, element) {
                update(scope, element);
            }
        };
    }])
    .filter('fromNow', function () {
        return function (date) {
            if (date && date.indexOf('+') == -1 && date.indexOf('Z') == -1) date += '+00:00';
            return moment(date).utc().fromNow();
        }
    })
    .filter('amHumanize', function () {
        return function (date, to) {
            if (date && date.indexOf('+') == -1) date += '+00:00';
            if (to && to.indexOf('+') == -1) to += '+00:00';
            return moment.duration(moment(date).diff(to)).humanize();
        }
    })
    .filter('amFormat', function () {
        return function (date, format) {
            if (date && date.indexOf && date.indexOf('+') == -1) date += '+00:00';
            return moment(date).format(format);
        }
    })
    .filter('comma', function () {
        return function (values, seperator) {
            seperator = seperator || ',';
            return (values || "").split(seperator).filter(function (n) { return n != '' && n!='x'; });
        }
    })
    .filter('colorize', function () {
        var colors = ['info', 'warning', 'danger', 'success'];
        return function (x) {
            return colors[x];
        }
    })
    .filter('amDate', function () {
        return function (date) {
            if (date && date.indexOf('+') == -1) date += '+00:00';
            return moment(date).format('DD MMM YYYY');
        }
    })
    .filter('time', function () {
        return function (duration, ms) {
            var num = parseInt(duration);
            if (isNaN(num)) num = 0;
            var hours = Math.floor(num / 3600) < 10 ? ("00" + Math.floor(num / 3600)).slice(-2) : Math.floor(num / 3600);
            var minutes = ("00" + Math.floor((num % 3600) / 60)).slice(-2);
            var seconds = ("00" + (num % 3600) % 60).slice(-2);
            var val = (hours != '00' ? hours + ":" : '') + minutes + ":" + seconds;
            if (ms) {
                var msValue = ("000" + parseInt((duration - parseInt(duration)) * 1000)).slice(-3);
                val += "." + msValue;
            }
            return val;
        }
    })
    .filter('plaintext', function () {
        return function (html) {
            var div = document.createElement("div");
            div.innerHTML = html;
            return div.textContent || div.innerText || "";
        };
    })
    .filter('gravtaar', ['md5', function (md5) {
        return function (value, email, size, missing) {
            size = size || 150;
            if (!!value) {
                if (value.indexOf('youtube.com') > -1) return value;
                if (value.indexOf('http://') === -1) value = 'http://images.vidzapper.com/' + value;
                if (value.indexOf('www.gravatar.com') === -1 && value.indexOf("notfound") === -1
                    && value.indexOf('placehold.it/') === -1
                    && value !== "http://images.vidzapper.com/")
                    return '//uplive.vzconsole.com/img/' + (value.replace('http://', '').replace('https://', '')) + '?h=' + size;
            }
            if (isEmail(email)) { return '//www.gravatar.com/avatar/' + md5(email) + '?s=' + size + '&d=' + (missing || 'mm'); }
            if (missing != 'notfound') { return '//www.gravatar.com/avatar/' + md5(value || '') + '?s=' + size + '&d=' + (missing || 'mm'); }
            return null;
        };
    }])
    .filter('notfound', [function () {
        return function (value) {
            return !value || value == 'notfound';
        };
    }])
    .filter('eta', function () {
        return function (val) {
            var output = "0:00:00";
            if (val && !isNaN(val)) {
                output = funcs.format.time(val);
            }
            return output;
        }
    })
    .filter('notfound', function () {
        return function (picture) {
            return !picture || picture == 'notfound';
        }
    })
    .filter('icon', function () {
        return function (faicon, size, videos, audios, pictures, documents) {
            if (!faicon) return '';
            if (videos) return 'fa fa-video-camera' + (!!size ? ' fa-' + size : '');
            if (audios) return 'fa fa-music' + (!!size ? ' fa-' + size : '');
            if (pictures) return 'fa fa-picture-o' + (!!size ? ' fa-' + size : '');
            if (documents) return 'fa fa-file-o' + (!!size ? ' fa-' + size : '');
            return 'fa fa-' + faicon + (!!size ? ' fa-' + size : '');
        }
    })
    .filter('fileIcon', function () {
        return function (faicon, size, contentType) {
            if (!faicon) return '';
            if (contentType && contentType.length) {
                if (contentType.indexOf('.presentation') > -1) return 'fa fa-file-powerpoint-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('.document') > -1) return 'fa fa-file-word-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('.sheet') > -1) return 'fa fa-file-excel-o' + (!!size ? ' fa-' + size : '');
                if (contentType == 'text/html') return 'fa fa-file-code-o' + (!!size ? ' fa-' + size : '');
                if (contentType == 'text/plain') return 'fa fa-file-text-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('zip') > -1) return 'fa fa-file-archive-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('pdf') > -1) return 'fa fa-file-pdf-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('image') > -1) return 'fa fa-file-image-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('audio') > -1) return 'fa fa-file-audio-o' + (!!size ? ' fa-' + size : '');
                if (contentType.indexOf('video') > -1) return 'fa fa-file-video-o' + (!!size ? ' fa-' + size : '');
            }
            return 'fa fa-' + faicon + (!!size ? ' fa-' + size : '');
        }
    })
    .filter('resize', function () {
        return function (value, size,missing) {
            size = size || 150;
            if (!!value) {
                if (value.indexOf('youtube.com') > -1) return value;
                if (value.indexOf('http://') === -1) value = 'http://images.vidzapper.com/' + value;
                if (value.indexOf('www.gravatar.com') === -1 && value.indexOf("notfound") === -1
                    && value.indexOf('placehold.it/') === -1
                    && value !== "http://images.vidzapper.com/")
                    return (value.replace('http://', '').replace('https://', '')) + encodeURIComponent('?h=' + size);
            }
            return missing || "mm";
        }
    })
    .filter('gb', function () {
        return function (bytes) {
            return funcs.format.gb(bytes).toFixed(2);
        };
    })
    .filter('filesize', function () {
        return function (bytes) {
            return funcs.format.size(bytes);
        };
    })
    .filter('encode', function () {
        return function (value) {
            return encodeURIComponent(value);
        };
    })
    .filter('actIcon', function () {
        return function (act) {
            if (act == "Modified") return "fa-pencil";
            if (act == "Added") return "fa-plus";
            if (act == "Deleted") return "fa-trash";
            return 'fa-info';
        };
    })
    .filter('actStatus', function () {
        return function (act) {
            if (act == "Modified") return "warning";
            if (act == "Added") return "success";
            if (act == "Deleted") return "danger";
            return 'info';
        };
    })
    .filter('nonEmpty', function () {
        return function (object) {
            return !!(object && Object.keys(object).length > 0);
        };
    })
    .filter('objCount', function () {
        return function (object) {
            return !!object && Object.keys(object).length;
        };
    })
    .filter('years', function () {
        return function (value, inbox) {
            if (isNaN(value)) return value;
            if (value == 0) return '';
            var years = Math.floor(value / 12);
            var months = value % 12;
            var val = years + (months > 0 ? "." + months : "") + 'y';
            if (inbox) return "(" + val + ")";
            return val;
        };
    });
})();
