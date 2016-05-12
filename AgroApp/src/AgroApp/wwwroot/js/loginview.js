angular.module('AgroApp').
    controller('loginController', function ($scope, $location) {
        $scope.goLogin = function () {
            $scope.showloading = true;
            window.location.href = '/admin/main';
        };
    })