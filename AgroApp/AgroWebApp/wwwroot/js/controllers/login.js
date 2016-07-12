angular.module('materialAdmin').controller('loginCtrl', function ($scope, $http) {
    $scope.userDetails = {};

    $scope.Login = function () {
        $http.get('/api/user/login/' + $scope.userDetails.email + '/' + $scope.userDetails.password)
        .success(function (data) {
            // With the data succesfully returned, call our callback
            if (data != "false")
                window.location.href = data;
            else {
                swal("", "De opgegeven gegevens zijn incorrect!", "error");
            }
        }).error(function (data) {
            swal("Fout", "Er is iets misgegaan!", "error");
        });
    };

    $scope.goRegister = function () {
        $http({
            method: 'GET',
            url: '/api/user/register/' + $scope.userDetails.email + '/' + $scope.userDetails.password + '/' + $scope.userDetails.fullname + '/' + $scope.userDetails.rol,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == "true")
                window.location.href = '/admin/gebruikerbeheer';
            else {
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven combinatie is incorrect";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een administrator";
        });
    };
});