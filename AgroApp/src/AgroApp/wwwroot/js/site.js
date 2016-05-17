var agroApp = angular.module('AgroApp');

agroApp.controller('Navigation', function ($scope, $window, $http, $rootScope) {
    $rootScope.showloading = false;

    $rootScope.changeView = function (view) {
        $rootScope.showLoading = true;
        window.location.href = "../../../" + view;
    }

    $scope.goLogout = function () {
        $rootScope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/login/logout',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).then(function successCallback(response) {
            window.location.href = '/';
        }, function errorCallback(response) {
            window.location.href = '/';
        });
    };
});