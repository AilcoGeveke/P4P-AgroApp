var agroApp = angular.module('AgroApp');

agroApp.controller('Navigation', function ($scope, $window, $http) {
    $scope.showloading = false;

    $scope.changeView = function (view) {
        $scope.showLoading = true;
        window.location.href = "../../../" + view;
    }

    $scope.goLogout = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/login/logout',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            window.location.href = '/';
        })
    };
});