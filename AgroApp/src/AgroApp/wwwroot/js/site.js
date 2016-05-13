var agroApp = angular.module('AgroApp', ['ngMaterial']);

agroApp.controller('Navigation', function ($scope, $window) {
    $scope.changeView = function (view) {
        $scope.showLoading = true;
        window.location.href = "../../../" + view;
    }
});