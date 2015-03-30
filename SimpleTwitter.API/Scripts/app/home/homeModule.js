
angular
    .module('SimpleTwitter', [
        'SimpleTwitter.ctrl.home',
        'SimpleTwitter.ctrl.list',
        'SimpleTwitter.ctrl.detail',
        'SimpleTwitter.ctrl.edit',
        'SimpleTwitter.Service']).config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {

            $routeProvider.when('/', {
                templateUrl: '/Home/List',
                controller: 'listCtrl'
            });
            $routeProvider.when('/create', {
                templateUrl: '/Home/Edit',
                controller: 'editCtrl'
            });
            $routeProvider.when('/detail/:id', {
                templateUrl: '/Home/Detail',
                controller: 'detailCtrl'
            });
            $routeProvider.otherwise({
                redirectTo: '/'
            });

            // Specify HTML5 mode (using the History APIs) or HashBang syntax.
            $locationProvider.html5Mode(true);
            //$locationProvider.html5Mode(false).hashPrefix('!');

        }]);
