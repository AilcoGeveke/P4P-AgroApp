angular.module('materialAdmin').controller('loginCtrl', function ($scope, $window, $http) {
    $scope.userDetails = {};

    $scope.Login = function () {
        $http.get('/api/user/login/' + $scope.userDetails.email + '/' + $scope.userDetails.password)
        .success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == "true")
                $window.location.reload();
            else {
                swal("", "De opgegeven gegevens zijn incorrect!", "error");
            }
        }).error(function (data) {
            swal("Fout", "Er is iets misgegaan!", "error");
        });
    };
});