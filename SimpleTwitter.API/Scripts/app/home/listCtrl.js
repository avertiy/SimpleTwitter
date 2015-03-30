
angular
    .module('SimpleTwitter.ctrl.list', [])
    .controller('listCtrl', [
        '$scope',
        '$location',
        'service',
        function ($scope, $location, service) {

            $scope.users = [];
            $scope.viewUser = function(id) {
                $location.path("/detail/" + id);
            };
            $scope.createUser = function() {
                $location.path("/create");
            };

            service
                .getUsers()
                .success(function(data, status, headers, config) {
                    $scope.users = data;
                });

            service
                .getGlobalFeed()
                .success(function (data, status, headers, config) {
                    $scope.globalfeed = data;
                });
            $scope.navigationManager.setListPage();

        }]);
