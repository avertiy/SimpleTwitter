
angular
    .module('SimpleTwitter.ctrl.detail', [])
    .controller('detailCtrl', [
        '$scope',
        '$routeParams',
        '$route',
        '$timeout',
        'service',
        function ($scope, $routeParams, $route,$timeout, service) {

            $scope.user = {UserName: '',Id:''};
            $scope.post = { UserId: '', MessageText: '' };
            $scope.follow = { UserId: '', FollowingId: ''};

            $scope.isDeleteRequested = !!$route.current.isDeleteRequested;

            $scope.deleteUser = function () {
                service
                    .deleteUser($routeParams.id)
                    .success(function (data, status, headers, config) {
                        $scope.navigationManager.goToListPage();
                    })
                    .error(function (data, status, headers, config) {
                        $scope.errorMessage = "Delete operation failed." + (' [HTTP-' + status + ']');
                    });
            };

            $scope.returnToList = function () {
                $scope.navigationManager.goToListPage();
            };
            $scope.reloadUserDetails = function () {
                $timeout(function () { $route.reload(); }, 500);
            };

            $scope.tweet = function () {
                service
                    .tweet($scope.post)
                    .success(function (data, status, headers, config) {
                        $scope.reloadUserDetails();
                    })
                    .error(function (data, status, headers, config) {
                        $scope.errorMessage = "Tweet operation failed." + (' [HTTP-' + status + ']');
                    });
            };
            $scope.followUser = function () {
                service.follow($scope.follow.UserId,$scope.follow.FollowingId)
                           .success(function () {
                               $scope.reloadUserDetails();
                           })
                           .error(function (data, status) {
                               $scope.errorMessage = "Follow operation failed."+ (' [HTTP-' + status + ']');
                           });
            };
            $scope.unfollowUser = function (followingId) {
                service.unfollow($scope.follow.UserId, followingId)
                           .success(function () {
                               $scope.reloadUserDetails();
                           })
                           .error(function (data, status) {
                               $scope.errorMessage = "Unfollow operation failed." + (' [HTTP-' + status + ']');
                           });
            };

            service
                .readUser($routeParams.id)
                .success(function(data, status, headers, config) {
                    $scope.user = data;
                    $scope.post.UserId = data.Id;
                    $scope.follow.UserId = data.Id;
                });
            service
                .getFeed($routeParams.id)
                .success(function (data, status, headers, config) {
                    $scope.tweets = data;
                });
        }]);

