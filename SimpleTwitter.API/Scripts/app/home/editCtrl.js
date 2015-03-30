
angular
    .module('SimpleTwitter.ctrl.edit', [])
    .controller('editCtrl', [
        '$scope',
        '$routeParams',
        '$templateCache',
        'service',
        function ($scope, $routeParams, $templateCache, service) {

            var editTemplates = {
                'followers': '/Home/Followers',
                'followings': '/Home/Followings',
                'email': '/Home/EditEmail'
            };

            $scope.user = {UserName: ''};

            $scope.returnToList = function() {
                $scope.navigationManager.goToListPage();
            };

            $scope.panelId = 'list';
            $scope.contactInfoPanelUrl = editTemplates[$scope.panelId];
            $scope.$watch('panelId', function(panelId) {
                $scope.contactInfoPanelUrl = editTemplates[panelId];
            });

            $scope.save = function () {
                
                if ($scope.isNew) {
                    service
                        .createUser($scope.user)
                        .success(function(data, status, headers, config) {
                            $scope.navigationManager.goToListPage();
                        })
                        .error(function(data, status, headers, config) {
                            $scope.errorMessage = (data || { message: "Create operation failed." }).message + (' [HTTP-' + status + ']');
                        });
                } else {
                    service
                        .updateUser($scope.user)
                        .success(function (data, status, headers, config) {
                            $scope.navigationManager.goToListPage();
                        })
                        .error(function (data, status, headers, config) {
                            $scope.errorMessage = (data || { message: "Update operation failed." }).message + (' [HTTP-' + status + ']');
                        });
                }
            };

            $scope.isNew = angular.isUndefined($routeParams.id);
            
            if (!$scope.isNew) {
                service
                    .readUser($routeParams.id)
                    .success(function (data, status, headers, config) {
                        $scope.user = data;
                    });
            }

        }]);
