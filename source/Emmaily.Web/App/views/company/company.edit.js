app.controller('companyEditController', [
    '$scope', 'dataService', '$stateParams', '$state', 'logger', function ($scope, db, $stateParams, $state, logger) {
        $scope.title = "Company";
        $scope.current = 'companies';
        $scope.icon = "building";
        $scope.action = "Save";
        $scope.breads = [
          {link:'companies',icon:'building',title:'Company'}
        ];
        $scope.actions = [
            {link:'companies'}
        ];
        $scope.readonly = $state.current.data.readonly;

        $scope.update = function (record, keepPage) {
            record.Picture = record.picture.url || record.Picture;
            record.NavLogo = record.navLogo.url || record.NavLogo;
            record.Custom.Data = JSON.stringify(record.Custom.Bag);
            return db.save('client', record).then(function (result) {
                if (result.RowsAffected) {
                    logger.info('Company ' + record.Name, 'modification success.');
                    if (!keepPage) {
                        $state.go('companies');
                    }
                } else {
                    logger.err('Company ' + record.Name, 'modification failed. '+result.Error);
                }
            });
        }

        $scope.newTag = { clientId: $stateParams.id, tag: '' };
        $scope.saveTag = function (item) {
            return db.save('companytag', item).then(loadTags).then(function () {
                $scope.newTag.tag = '';
            });
        }
        $scope.trashTag = function (item) {
            return db.trash('companytag/' + item.ID).then(loadTags);
        }

        $scope.newSite = { clientId: $stateParams.id, website: '' };
        $scope.saveWebsite = function (item) {
            return db.save('companywebsite', item).then(loadWebSites).then(function () {
                $scope.newSite.website = '';
            });
        }
        $scope.trashWebsite = function (item) {
            return db.trash('companywebsite/'+item.ID).then(loadWebSites);
        }

        $scope.newKeyword = { clientId: $stateParams.id, word: '' };
        $scope.saveKeyword = function (item) {
            return db.save('keyword', item).then(loadKeywords).then(function () {
                $scope.newKeyword.word = '';
            });
        }
        $scope.trashKeyword = function (item) {
            return db.trash('keyword/' + item.ID).then(loadKeywords);
        }

        $scope.categories = {
            url: 'api/v2/my/category/categorytree/v2/' + $stateParams.id,
            'data': function (node) {
                return { 'id': node.id };
            }
        }

        $scope.categoryConfig = {
            core: {
                'check_callback': function (o, n, p, i, m) {
                    if(m && m.dnd && m.pos !== 'i') { return false; }
                    if(o === "move_node" || o === "copy_node") {
                        if(this.get_node(n).parent === this.get_node(p).id) { return false; }
                    }
                    return true;
                },
                'force_text': true,
                'sort': function (a, b) {
                    return this.get_type(a) === this.get_type(b) ? (this.get_text(a) > this.get_text(b) ? 1 : -1) : (this.get_type(a) >= this.get_type(b) ? 1 : -1);
                } 
            },
            'plugins': ["wholerow","types", "contextmenu", "dnd", "mapper"]
        };

        $scope.createRootCategory=function() {
            db.save('category', { title: "Default", description: "Defaut", clientId: $stateParams.id }).then(function (result) {
                $scope.categoryTree.refresh();
            }, function (errr) {
                console.log('Unable to add category', errr);
                $scope.categoryTree.refresh();
            });
        }

        $scope.loadedCategories = function (e,data) {
            $scope.hasCategories = data.instance._model.data['#'].children.length;
            $scope.categoryTree=data.instance;
        }

        $scope.createCategory = function (e, data) {
            db.save('category', { parentId: data.node.parent, title: data.node.text, description: data.node.text, clientId: $stateParams.id }).then(function (result) {
                data.instance.set_id(data.node, result.ID);
            }, function (errr) {                         
                console.log('Unable to add category',errr);
                data.instance.refresh();
            });
        }
        $scope.moveCategory = function (e, data) {
            db.save('category/move', { ID: data.node.id, parentId: data.parent,clientId: $stateParams.id }).then(function (result) {
                data.instance.refresh();
            }, function (errr) {
                console.log('Unable to move category', errr);
                data.instance.refresh();
            });
        }
        $scope.renameCategory = function (e, data) {
            db.save('category/rename', { ID: data.node.id, title: data.node.text, clientId: $stateParams.id }).then(function (result) {
                data.instance.refresh();
            }, function (errr) {
                console.log('Unable to add category', errr);
                data.instance.refresh();
            });
        }
        $scope.trashCategory = function (e, data) {
            db.trash('category',data.node.id).then(null, function (errr) {
                console.log('Unable to delete category', errr.ExceptionMessage || errr);
                data.instance.refresh();
            });
        }

        $scope.createCloudStorage = function () {
            $scope.creatingCloudStorage = true;
            db.save('client/azure', {clientID:$stateParams.id}).then(loadStorages, function (errr) {
                $scope.creatingCloudStorage = false;
                logger.err('Failed to create cloud storage', errr);
            });
        }

        $scope.addExtra = function (mode, name) {
            db.create('extra', { clientid: $stateParams.id, mode: mode, title: name, group: 'client' }).then(loadCustom);
        }

        function loadTags() {
            return db.get('companytag/all?$orderby=ID desc&$filter=ClientID eq ' + $stateParams.id).then(function (tags) {
                $scope.tags = tags;
            });
        }

        function loadAddresses() {
            return db.get('location/all/' + $stateParams.id).then(function (addresses) {//
                $scope.addresses = addresses;
            });
        }

        function loadWebSites() {
            return db.get('companywebsite/all?$orderby=ID desc&$filter=ClientID eq ' + $stateParams.id).then(function (websites) {
                $scope.websites = websites;
            });
        }

        function loadKeywords() {
            return db.get('keyword/all?$orderby=ID desc&$filter=ClientID eq ' + $stateParams.id).then(function (keywords) {
                $scope.keywords = keywords;
            });
        }

        function loadStorages() {
            return db.get('ftp/all?$orderby=ID desc&$filter=ClientID eq ' + $stateParams.id).then(function (storages) {
                $scope.storages = storages;
                for (var x in storages) {
                    if (storages[x].Path.indexOf('blob.core.windows.net')>-1) {
                        $scope.hasCloudStorage = true;
                        break;
                    }
                }
            });
        }

        function loadEncoders() {
            return db.get('rawencoder/all?$orderby=ID desc&$filter=ClientID eq 0 or ClientID eq ' + $stateParams.id).then(function (encoders) {
                $scope.encoders = encoders;
            });
        }


        $scope.updatePayment=function(payment,gateway) {
            var entity = angular.copy(payment);
            entity.ClientId = $stateParams.id;
            if (gateway) {
                entity.PayPal = gateway == 'PayPal';
                entity.Stripe = gateway == 'Stripe';
                entity.BusinessEmail = entity.Email;
                entity.APIPassword = 'not specified';
                entity.APISignature = 'not specified';
                entity.APIUserName = 'not specified';
                entity.PublisherKey = 'not specified';
                entity.PublisherSecret = 'not specified';
                entity.Currency = 'USD';
            }
            db.save(gateway, entity).then(function (result) {

            }).then(loadPayments);
        }
        function loadPayments() {
            return db.get('payment/all?$orderby=ID desc&$filter=ClientID eq ' + $stateParams.id).then(function (payments) {
                $scope.payments = payments;
            });
        }

        function loadCustom() {
            return db.get('extra/client/' + $stateParams.id).then(function (extra) {
                $scope.extra = extra;
                $scope.newField = '';
            });
        }

        function loadSocial() {
            return db.get('account/userclaims/' + $stateParams.id).then(function (socials) {
                $scope.socials = socials;
            });
        }

        function bindEntity(result) {
            $scope.record = result;
            $scope.record.picture = { url: result.Picture, email: result.Email, title: result.Title };
            $scope.record.navLogo = { url: result.NavLogo, email: '', title: 'Small Logo' };
        }

        function loadCompany() {
            return db.get('client/' + $stateParams.id).then(bindEntity);
        }

        function loadResellers() {
            return db.get('client/all?$orderby=Name').then(function (resellers) {
                $scope.resellers = resellers;
            });
        }

        $scope.addNewHandshake = function () {
            $scope.insertedHandshake = { ID: 0, PartyID: 0, Relation: '' };
            $scope.handshakes.push($scope.insertedHandshake);
        }

        $scope.saveHandshake = function (data, id) {
            var entity = angular.copy(data);
            entity.ID = id;
            entity.ClientID = $stateParams.id;
            return db.save('clienthandshake', entity).then(loadHandshakes);
        }

        $scope.trashHandshaker = function (data) {
            return db.trash('clienthandshake', data.ID).then(loadHandshakes);
        }


        function loadHandshakes() {
            return db.get('clienthandshake/all?$orderby=ID desc&$filter=ClientID eq ' + $stateParams.id).then(function (handshakes) {
                $scope.handshakes = handshakes;
            });
        }

        var defaultEncoding = '-i "{!URL!} live=1" -acodec copy -c:v libx264 -pix_fmt yuv420p -profile:v main ' +
        '-level 3.1 -movflags +faststart -preset ultrafast -crf 2 -c:a libfdk_aac -sample_fmt s16 ' +
        '"{!OUT!}"';

        $scope.addNewStream = function () {
            $scope.insertedStream = { ID: 0, Command: defaultEncoding};
            $scope.streams.push($scope.insertedStream);
        }

        $scope.saveStream = function (data, id) {
            var entity = angular.copy(data);
            entity.ID = id;
            entity.ClientID = $stateParams.id;
            console.log('Saving', entity);
            return db.save('recordinglink', entity).then(loadStream);
        }

        $scope.trashStream = function (data) {
            return db.trash('recordinglink', data.ID).then(loadStream);
        }

        function loadStream() {
            return db.get('recordinglink/all?$filter=ClientID eq ' + $stateParams.id).then(function (streams) {
                $scope.streams = streams;
            });
        }


        function loadCountries() {
            return db.get('country/all?$orderby=Name').then(function(countries) {
                $scope.countries = countries;
            });
        } 

        function fetchRecord() {
            return loadCountries()
                .then(loadResellers)
                .then(loadTags)
                .then(loadWebSites)
                .then(loadAddresses)
                .then(loadKeywords)
                .then(loadStorages)
                .then(loadEncoders)
                .then(loadPayments)
                .then(loadStream)
                .then(loadHandshakes)
                .then(loadCustom)
                //.then(loadSocial) //TODO:Not loading correctly.
                .then(loadCompany);
        }

        fetchRecord();
    }
]);