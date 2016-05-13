var agroApp = angular.module('AgroApp');

agroApp.controller('Navigation', function ($scope, $window) {
    $scope.showloading = false;

    $scope.changeView = function (view) {
        $scope.showLoading = true;
        window.location.href = "../../../" + view;
    }
});