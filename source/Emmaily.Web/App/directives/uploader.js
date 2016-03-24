(function () {
    'use strict';


    var fileTypes= {
        picture: /(\.|\/)(gif|jpe?g|png|svg)$/i,
        video: /\.(264|787|3gp2|3gpp|3p2|aaf|aep|aetx|ajp|amv|amx|arf|avb|axm|bdmv|bin|bmk|camrec|clpi|cmmp|cmmtpl|cmproj|cmrec|cst|cvc|d2v|d3v|dat|dce|dck|dir|dmb|dmsd|dmsd3d|dmss|dpa|dpg|dv|dv-avi|dvr|dvx|dxr|dzt|evo|eye|f4p|fbz|fcp|flc|flh|fli|gfp|gts|hkm|ifo|imovieproject|ismc|ivf|ivr|izz|izzy|jts|jtv|m1pg|m21|m21|m2t|m2ts|m2v|mgv|mj2|mjp|mnv|mp21|mp21|mpgindex|mpl|mpls|mpv|mqv|msdvd|mswmm|mtv|mvb|mvd|mve|mvp|mvp|mvy|ncor|nsv|nuv|nvc|ogm|ogx|pgi|photoshow|piv|pmf|ppj|prel|pro|prtl|pxv|qtl|qtz|rdb|rec|rmd|rmp|rms|roq|rsx|rum|rv|sbk|scc|screenflow|seq|sfvidcap|smi|smk|ssm|stl|svi|swt|tda3mt|tivo|tod|tp|tp0|tpd|tpr|tsp|tvs|vc1|vcpf|vcv|vdo|vdr|vep|vfz|vgz|viewlet|vlab|vp6|vp7|vpj|vsp|wcp|wmd|wmmp|wmx|wp3|wpl|wvx|xej|xel|xesc|xfl|xlmv|zm1|zm2|zm3|zmv|aepx|ale|avp|avs|bdm|bik|bsf|camproj|cpi|divx|dmsm|dream|dvdmedia|dvr-ms|dzm|dzp|edl|f4v|fbr|fcproject|hdmov|imovieproj|ism|ismv|m2p|m4v|mkv|mod|moi|mpeg|mts|mxf|ogv|pds|prproj|psh|r3d|rcproject|rmvb|scm|smil|sqz|srt|stx|swi|tix|trp|ts|veg|vf|vro|webm|wlmp|wtv|xvid|yuv|anim|bix|dsy|gl|grasp|gvi|ivs|lsf|m15|m4e|m75|mmv|mob|mpeg4|mpf|mpg2|mpv2|msh|rmd|rts|scm|sec|tdx|viv|vivo|vp3|3gpp2|3mm|60d|aet|avd|avs|bnp|box|bs4|byu|dav|ddat|dif|dlx|dmsm3d|dnc|dv4|fbr|flx|gvp|h264|irf|iva|k3g|lrec|lsx|m1v|m2a|m4u|meta|mjpg|modd|moff|moov|movie|mp2v|mp4v|mpe|mpsub|mvc|mys|osp|par|playlist|pns|pssd|pva|pvr|qt|qtch|qtm|rp|rts|sbt|scn|sfd|sml|smv|spl|str|vcr|vem|vft|vfw|vid|video|vs4|vse|w32|wm|wot|3g2|3gp|asf|asx|avi|flv|mov|mp4|mpg|rm|swf|vob|wmv)$/i,
        any: /(^Video\/(x-ms-wmv|mp4|avi|x-flv)+$)|(\.|\/)(264|787|3gp2|3gpp|3p2|aaf|aep|aetx|ajp|amv|amx|arf|avb|axm|bdmv|bin|bmk|camrec|clpi|cmmp|cmmtpl|cmproj|cmrec|cst|cvc|d2v|d3v|dat|dce|dck|dir|dmb|dmsd|dmsd3d|dmss|dpa|dpg|dv|dv-avi|dvr|dvx|dxr|dzt|evo|eye|f4p|fbz|fcp|flc|flh|fli|gfp|gts|hkm|ifo|imovieproject|ismc|ivf|ivr|izz|izzy|jts|jtv|m1pg|m21|m21|m2t|m2ts|m2v|mgv|mj2|mjp|mnv|mp21|mp21|mpgindex|mpl|mpls|mpv|mqv|msdvd|mswmm|mtv|mvb|mvd|mve|mvp|mvp|mvy|ncor|nsv|nuv|nvc|ogm|ogx|pgi|photoshow|piv|pmf|ppj|prel|pro|prtl|pxv|qtl|qtz|rdb|rec|rmd|rmp|rms|roq|rsx|rum|rv|sbk|scc|screenflow|seq|sfvidcap|smi|smk|ssm|stl|svi|swt|tda3mt|tivo|tod|tp|tp0|tpd|tpr|tsp|tvs|vc1|vcpf|vcv|vdo|vdr|vep|vfz|vgz|viewlet|vlab|vp6|vp7|vpj|vsp|wcp|wmd|wmmp|wmx|wp3|wpl|wvx|xej|xel|xesc|xfl|xlmv|zm1|zm2|zm3|zmv|aepx|ale|avp|avs|bdm|bik|bsf|camproj|cpi|divx|dmsm|dream|dvdmedia|dvr-ms|dzm|dzp|edl|f4v|fbr|fcproject|hdmov|imovieproj|ism|ismv|m2p|m4v|mkv|mod|moi|mpeg|mts|mxf|ogv|pds|prproj|psh|r3d|rcproject|rmvb|scm|smil|sqz|srt|stx|swi|tix|trp|ts|veg|vf|vro|webm|wlmp|wtv|xvid|yuv|anim|bix|dsy|gl|grasp|gvi|ivs|lsf|m15|m4e|m75|mmv|mob|mpeg4|mpf|mpg2|mpv2|msh|rmd|rts|scm|sec|tdx|viv|vivo|vp3|3gpp2|3mm|60d|aet|avd|avs|bnp|box|bs4|byu|dav|ddat|dif|dlx|dmsm3d|dnc|dv4|fbr|flx|gvp|h264|irf|iva|k3g|lrec|lsx|m1v|m2a|m4u|meta|mjpg|modd|moff|moov|movie|mp2v|mp4v|mpe|mpsub|mvc|mys|osp|par|playlist|pns|pssd|pva|pvr|qt|qtch|qtm|rp|rts|sbt|scn|sfd|sml|smv|spl|str|vcr|vem|vft|vfw|vid|video|vs4|vse|w32|wm|wot|3g2|3gp|asf|asx|avi|flv|mov|mp4|mpg|rm|swf|vob|wmv|gif|jpe?g|png|tif|bmp|xls|xlsx|ppt|pptx|txt|rtf|pdf|psd|doc|docx|html|htm|mp3|mp4|wav|3gp|aac|m4a|m4p|ogg|wma|vox)$/i
    }

    var bogusMessages = [
            "Please wait..",
            "Taking longer then expected, Please wait..",
            "Validation taking longer then expected.."
    ];

    var MSG = {
        connectionRetry: 'Server Connection Failed, retrying',
        uploading: 'Processing uploads',
        uploaderNotAvailable: 'Uploader not available, please try again',
        hasMaster: 'Checking to see if master already exists',
        resumeUploading: 'Existing master found, resuming upload',
        fileInProgress: 'File already in progress to publishing',
        profileStart: 'Fetching encode profiles',
        profileSuccess: 'Fetching encode profiles',
        profileError: 'Cannot find upload profiles, please reload and start again',
        queCreated: 'Adding encode to queue',
        queFailed: 'Unable to add files to queue, please reload and start again, if problem persist, please contact tech support',
        estimateUploadTime: 'Estimating encode time',
        createRecord: 'Creating database entries',
        createRecordRetry: 'Retrying to create database entries',
        createRecordFailed: 'Unable to create database entry, please contact technical support',
        masterPrepare: 'Preparing master for upload',
        masterUploadProgress: 'Uploading master',
        masterUploadRetry: 'Upload failed, retrying',
        masterUploadFailed: 'Upload failed, process aborted, please check your internet connection and try again',
        uploadComplete: 'Upload complete',
        storageReachError: 'Encoder storage not available, retrying',
        encoderMessageUnavailable: 'Encoder message not available, encode terminated',
        loadingFileForHive: 'Loading  file to encoder',
        loadingFailForHive: 'Unable to load file to encoding network',
        sentToHive: 'File sent to encoder',
        sendRetryToHive: 'Encoder failed, retrying',
        sendFailedHive: 'Encoder failed, please contact technical support',
        thumbStarted: 'Thumbnails being created',
        thumbProgress: 'X thumbnails created',
        thumbRetry: 'Thumbnails not created, retrying',
        thumbFailed: 'Thumbnails not created, please create manually',
        encodingStarted: 'Encoding started (est time and %)',
        encodingCompleted: 'Encode completed',
        viewAsset: 'View'
    };

    var app = angular.module('app');

    app.directive('ngUploadForm', ['$rootScope', 'fileUpload', function () {
        return {
            restrict: 'E',
            templateUrl: function (elem, attrs) {
                return attrs.templateUrl || '/App/views/upload/file.html';
            },
            scope: {
                progress:'@',
                acceptFileTypes: '@',
                dropzone: '@',
                storage: '@',
                container: '@',
                containerkey: '@',
                titlekey: '@',
                master: '@',
                user: '@',
                resumeUpload: '@',
                icon:'@',
                readonly:'@',
                url: '@',
                size: '@',
                fileList:'=',
                autoUpload: '@',
                sizeLimit: '@',
                ngModel: '=',
                title: '@',
                name: '@',
                quick:'@'
            },
            controller: ['$rootScope', '$scope', '$element', 'fileUpload', '$http', 'Talker', 'dataService', function ($rootScope, $scope, $element, fileUpload, $http, talker, db) {

                if (typeof ($scope.ngModel) == 'undefined') {
                    $scope.ngModel = { url: '', email: '', title: $scope.title };
                }
                var talking = {};
                var savingTimeout;

                var map = {};

                function findMode(d) {
                    if (d === 'image' || d === 'picture') return 'images';
                    if (d === 'document') return 'documents';
                    if (d === 'audio') return 'audios';
                    return 'videos';
                }

                function onMeta(f, filename) {
                    f.meta = f.meta || {};
                    if (f.programme > 0) {
                        if (savingTimeout) return;
                        db.get('content/' + f.programme)
                            .then(function (o) {
                                f.meta.title = o.Title;
                                f.meta.description = o.Description || o.Title;
                                f.meta.keywords = o.Keywords || o.Title;
                            });
                    } else {
                        f.meta.title = filename;
                        f.meta.description = filename;
                        f.meta.keywords = filename;
                    }
                }

                function onMessage(queue) {

                    var res = queue.Result.Bag || {};

                    var original = queue.Request.Bag.original || '';
                    var taskResult = !!original ? map[original] : map[queue.ID];

                    if (!taskResult) return;

                    if (queue.State === -1) {
                        if (!res.status) res.status = 'code-BLNK';
                        taskResult.err = typeof (res.status) == 'string' ? res.status : JSON.stringify(res.status);
                        return;
                    }
                    if (res.next) {
                        map[res.next] = taskResult;
                        var nextTalk = 'que-' + res.next;
                        if (!talking[nextTalk]) {
                            talking[nextTalk] = true;
                            talker.on(nextTalk, onMessage);
                        }
                    }
                    taskResult.uploaded = true;
                    taskResult.fileLength = res.size || taskResult.fileLength;
                    taskResult.originalname = res.originalname || res.original || taskResult.originalname;
                    taskResult.state = queue.State;
                    taskResult.status = res.status;
                    taskResult.remaining = res.eta;

                    if (!!res.received) taskResult.received = res.received;
                    if (!!res.captured) taskResult.captured = res.captured;
                    if (!!res.encoded) {
                        taskResult.encoded = res.encoded;
                        taskResult.error = '';
                        taskResult.msg = MSG.encodingCompleted;
                        taskResult.encoded = true;
                        taskResult.uploaded = true;
                        taskResult.valid = true; //turn all green.
                    }
                    taskResult.progress = queue.Progress || (res.percent) || 0;
                    if (res.hive || res.ready) {
                        taskResult.tasks = res.tasks;
                        if (res.percent == 100) {
                            taskResult.bx = findMode(queue.Request.Bag.mode || 'video');
                            taskResult.ready = true;
                        }
                    }
                    taskResult.hive = true;

                    if (res.where) {
                        taskResult.bx = findMode(res.mode);
                        taskResult.link = res.where;
                        taskResult.progress = 100;
                        taskResult.loadChanges = res.where;
                    }

                    console.log('Task', taskResult);
                    $scope.$apply();

                }

                function parseFileData(fl) {
                    if (fl.vmodel && fl.vmodel.fileInfo) {
                        fl.vmodel.fileInfo.partial = true;
                        return fl.vmodel.fileInfo;
                    }
                    var tmp = {
                        name: fl.name,
                        size: fl.size,
                        user: $scope.user,
                        storage: $scope.storage,
                        container: $scope.container || 0,
                        master: fl.master || $scope.master || 0,
                        que: fl.que
                    };
                    if (!!$scope.quick) {
                        tmp.quick = true;
                    }
                    if ($scope.containerkey) {
                        tmp.containerkey = $scope.containerkey;
                    }
                    return tmp;
                }

                function checkCurrentSize(retry, data) {
                    var file = data.files[0];
                    var tmp = parseFileData(file);
                    file.validating = true;
                    return $http.post($scope.url + 'broken', tmp).then(function (result, status) {
                        var x = result.data;
                        clearInterval(data.bogusWatcher);
                        data.uploadedBytes = parseInt(x.uploadedBytes) || 0; //resume uploads by setting uploaded bytes to whats recieved.
                        if (x.error) {
                            if (x.error.code === 'ECONNRESET' || x.error.code === 'EPIPE') {
                                file.error = MSG.uploaderNotAvailable;
                                data.sizeChecker = setTimeout(checkCurrentSize, retry * 1000, ++retry, data); /// retry;
                            } else {
                                file.error = x.error.code || x.error;
                            }
                        } else {
                            if (file.error) { delete file.error; }
                            tmp.que = x.que;
                            tmp.master = x.master;
                            file.que = x.que;
                            file.master = x.master;
                            file.valid = true;
                            file.validating = false;
                            if (data.uploadedBytes === tmp.size) {
                                file.error = MSG.fileInProgress;
                                file.progress = 100;
                            } else {
                                if (data.uploadedBytes > 0) {
                                    file.msg = MSG.resumeUploading;
                                } else {
                                    file.msg = MSG.uploading;
                                }
                                file.programme = x.programme || 0;
                                file.title = x.title;
                                map[file.name] = file;
                                onMeta(file, x.title);
                                data.scope.option('formData', tmp);
                                data.submit();
                            }

                        }
                    }, function (err) {
                        clearInterval(data.bogusWatcher);
                        file.error = MSG.connectionRetry;
                        console.log('Error', err);
                        data.sizeChecker = setTimeout(checkCurrentSize, retry * 1000, ++retry, data); /// retry;
                    });
                }



                $scope.$on('fileuploadadd', function (e, data) {
                    console.log('ADD',e, data);
                    data.scope.option('url', $scope.url);
                    if ($scope.quick) {
                        data.scope.option('formData', parseFileData(data.files[0]));
                        data.submit();
                    } else {
                        checkCurrentSize(0, data);
                    }
                });
                $scope.$on('fileuploaddone', function (e, data) {
                    var f = data._response.result.files[0];
                    $scope.ngModel.url = f.url;
                    $scope.result = f;
                    $scope.title = f.originalname;
                    $scope.error = f.error;
                    f.uploaded = true;
                    f.old = map[f.originalname];
                    $scope.current = f;
                    $scope.$apply();
                    data.scope.$apply();
                    fileUpload.addFieldData($scope.name, f);
                });
                $scope.$on('fileuploadprogress', function (e, data) {
                    var pg = data.progress();
                    var prg = pg.loaded * 100 / pg.total;
                    data.scope.prg = prg;
                    data.files[0].msg = "uploading";
                    data.files[0].progress = prg;
                    data.files[0].uploaded = prg == 100;
                    data.files[0].uploading = prg < 100;
                    $scope.current = data.files[0];
                    data.scope.$apply();
                });
                $scope.$on('fileuploadchunkdone', function (e, data) {
                    if (data.files && data.files.length) {
                        for (var i in data.files) {
                            var xb = data.files[i];
                            if (xb) {
                                try {
                                    var xt = JSON.parse(data.result);
                                    xb.remaining=xt.files[0].eta;
                                } catch (e) {

                                }
                            }
                        }
                        $scope.current = data.files[0];
                    }
                    data.scope.$apply();
                });
                $scope.$on('fileuploadprogressall', function(e, data) {
                    data.progress=Math.floor(data.loaded / data.total * 100);
                    data.remainingTime = (data.total - data.loaded) * 8 / data.bitrate;
                    $scope.overall = data;
                    $scope.$apply();
                });
                $scope.$on('fileuploadfail', function (e, data) {
                    $scope.current = data.files[0];
                    data.scope.$apply();
                });

                $scope.options = {
                    url: $scope.url,
                    dropZone: $element,
                    maxFileSize: $scope.sizeLimit,
                    autoUpload: $scope.autoUpload,
                    acceptFileTypes: fileTypes[$scope.acceptFileTypes || 'picture'],
                    maxNumberOfFiles: 20,
                    minFileSize: 1, //1 Bytes min upload size.
                    maxRetries: 100,
                    retryTimeout: 500,
                    messages: {
                        maxNumberOfFiles: 'Maximum number of files exceeded, allowed 20 at once.',
                        acceptFileTypes: 'File type not allowed, only video / audio / document types allowed.',
                        maxFileSize: 'File is too large, max upload limit is 200GB',
                        minFileSize: 'File is too small, file should be atleast 1 Byte'
                    }
                };

                if (!$scope.queue) {
                    $scope.queue = [];
                }

                var generateFileObject = function generateFileObjects(objects) {
                    var fileNames = [];
                    angular.forEach(objects, function (value, key) {

                        var f = {
                            name: value.name,
                            originalname: value.originalname,
                            size: value.fileLength,
                            url: value.url,
                            thumbnailUrl: value.url,
                            deleteUrl: value.url,
                            deleteType: 'DELETE',
                            result: value,
                            programme:value.where,
                            old: value.old || map[value.originalname]
                        };

                        if (value.where) {
                            f.old.programme = value.where;
                        }
                        if (value.old && value.old.preview) {
                            f.preview = value.old.preview;
                        }

                        if (f.url && f.url.charAt(0) !== '/') {
                            f.url = '/' + f.url;
                        }

                        if (f.deleteUrl && f.deleteUrl.charAt(0) !== '/') {
                            f.deleteUrl = '/' + f.deleteUrl;
                        }

                        if (f.thumbnailUrl && f.thumbnailUrl.charAt(0) !== '/') {
                            f.thumbnailUrl = '/' + f.thumbnailUrl;
                        }

                        if(fileNames.indexOf(f.name)==-1) { fileNames.push(f.name);}
                        $scope.queue[fileNames.indexOf(f.name)] = f;
                        $scope.current = f;
                    });
                };
                fileUpload.registerField($scope.name);
                $scope.filequeue = fileUpload.fieldData[$scope.name];

                $scope.$watchCollection('filequeue', function (newval) {
                    generateFileObject(newval);
                });
                talker.on('que', onMessage);
            }]
        };
    }])
    .controller('FileDestroyController', ['$rootScope', '$scope', '$http', 'fileUpload', function ($rootScope, $scope, $http, fileUpload) {
        var file = $scope.file,
          state;

        if ($scope.$parent && $scope.$parent.$parent && $scope.$parent.$parent.$parent.name) {
            $scope.fieldname = $scope.$parent.$parent.$parent.name;
        }

        if (!fileUpload.fieldData[$scope.name]) {
            fileUpload.fieldData[$scope.name] = [];
        }

        $scope.filequeue = fileUpload.fieldData;

        if (file.url) {
            file.$state = function () {
                return state;
            };
            file.$destroy = function () {
                state = 'pending';
                return $http({
                    url: file.deleteUrl,
                    method: file.deleteType
                }).then(
                  function () {
                      state = 'resolved';
                      fileUpload.removeFieldData($scope.fieldname, file.result._id);
                      $scope.clear(file);
                  },
                  function () {
                      state = 'rejected';
                      fileUpload.removeFieldData($scope.fieldname, file.result._id);
                      $scope.clear(file);
                  }
                );


            };
        } else if (!file.$cancel && !file._index) {
            file.$cancel = function () {
                $scope.clear(file);
            };
        }
    }
    ]);

})();