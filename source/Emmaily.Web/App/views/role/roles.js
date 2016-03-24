app.controller('roleEditController', [
    '$scope', 'dataService', '$stateParams', 'Talker', '$state', '$rootScope','logger', function ($scope, db, $stateParams, talker, $state, $rootScope,logger) {

        var title = 'Roles & Permissions';

        $scope.title = title;
        $scope.current = 'Roles & Permissions';
        $scope.icon = "lock";
        $scope.breads = [
            { link: 'users', icon: 'users', title: 'Users' }
        ];
        $scope.selectRole = selectRole;
        $scope.createRole = createRole;
        $scope.removeUserFromRole = removeUserFromRole;
        $scope.addUserToRole = addUserToRole;
        $scope.refreshUsers = refreshUsers;
        $scope.usr = {};
        $scope.$watch('role', getUsers);
        $scope.$watch('usr', validateUserInRole, true);
        $scope.applyPermission = applyPermission;
        $scope.permission = { read: [], write: [] };

        function validateUserInRole(usr) {
            if (!usr || (usr.selected && usr.error) || !$scope.users) return;
            $scope.usr.error = '';
            for (var i = 0; i < $scope.users.length; i++) {
                if ($scope.users[i].Label == usr.selected) {
                    $scope.usr.error = '"'+usr.selected+'" already added in role "'+$scope.role.Label+'"';
                    break;
                }
            }
        }

        function refreshUsers(q) {
            db.get('user/all?$top=20&$select=Title,UserName,ImageFile,Reseller&$filter=substringof(\''+q+'\',UserName)').then(function(result) {
                $scope.foundUsers = result;
            });
        }

        function createRole(roleName) {
            roleName = roleName.replace(/[^a-z0-9\-\_]/gi, '-');
            db.create('user/role/' + roleName).then(function (d) {
                if (d.success) {
                    $scope.roles = d.roles;
                    $scope.roleName = '';
                }
            });
        }

        function addUserToRole(user, role) {
            db.create('user/' + user + '/role/' + role.Label).then(function (d) {
                if (d.success) {
                    $scope.roles = d.roles;
                    $scope.users = d.users;
                    $scope.usr = {};
                    logger.log('Add Success', 'added "' + user + '" to role "' + role.Label + '"');
                } else {
                    logger.error('Add Failed', d.error || d.err);
                }
            });
        }

        function getRoles() {
            return db.get('user/roles').then(function (roles) {
                $scope.roles = roles;
                $scope.role = roles[0];
            });
        }

        function getUsers(role) {
            if (!role || !role.Label) return;
            return db.get('user/by/role/' + role.Label).then(function (userInfo) {
                $scope.users = userInfo.Users;
                $scope.selectedUser = null;
                $scope.permission = {
                    read: userInfo.ApisReadOnly.split(','),
                    write: userInfo.ApisWritable.split(',')
                };
            });
        }

        function applyPermission(role, permision) {
            db.update('user/role/' + role.Label, {
                ApisReadOnly: permision.read.join(','),
                ApisWritable: permision.write.join(',')
            }).then(function (d) {
                if (d.success) {
                    logger.log('Permissions', 'applied for role ' + role.Label);
                } else {
                    logger.error('Permissions', 'Failed saving for role ' + role.Label);
                }
            });
        }

        function selectRole(role) {
            $scope.role = role;
        }

        function removeUserFromRole(role, user) {
            db.trash('user/'+ user.Label + '/role',role.Label).then(function (d) {
                if (d.success) {
                    $scope.roles = d.roles;
                    $scope.users = d.users;
                    logger.log('Removed Success', 'removed "' + user.Label + '" from role "' + role.Label + '"');
                } else {
                    logger.error('Removed Failed', d.error || d.err);
                }
            });
        }

        return getRoles();
    }]);