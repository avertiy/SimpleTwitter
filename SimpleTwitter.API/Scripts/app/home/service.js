angular.module('SimpleTwitter.Service', []).factory('service', ['$http', function ($http) {
    return {
        getUsers: function () {
            return $http({
                method: 'GET',
                url: '/api/twitter/users'
            });
        },
        createUser: function (user) {
            return $http({
                method: 'POST',
                url: '/api/twitter/users',
                data: user
            });
        },
        readUser: function(userId) {
            return $http({
                method: 'GET',
                url: '/api/twitter/users/' + userId
            });
        },

        deleteUser: function (userId) {
            return $http({
                method: 'DELETE',
                url: '/api/twitter/users/' + userId
            });
        },

        updateUser: function(user) {
            return $http({
                method: 'PUT',
                url: '/api/twitter/users',
                data: user
            });
        },
        tweet: function (post) {
            return $http({
                method: 'POST',
                url: '/api/twitter/tweet',
                data: post
            });
        },
        getFeed: function (userid) {
            return $http({
                method: 'GET',
                url: '/api/twitter/feeds/' + userid
            });
        },
        getGlobalFeed: function () {
            return $http({
                method: 'GET',
                url: '/api/twitter/globalfeed/0'
            });
        },
        follow: function (userId, followingId) {
            return $http({
                method: 'PUT',
                url: '/api/twitter/follow',
                data: { UserId:userId, FollowingId:followingId }
        });
        },
        unfollow: function (userId, followingId) {
            return $http({
                method: 'PUT',
                url: '/api/twitter/unfollow',
                data: { UserId: userId, FollowingId: followingId }
            });
        }
    };
}]);