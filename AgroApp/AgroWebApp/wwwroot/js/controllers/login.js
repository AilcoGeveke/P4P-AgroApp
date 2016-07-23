angular.module('materialAdmin').controller('loginCtrl', function ($scope, $window, $http) {
    $scope.userDetails = {};

    $scope.Login = function () {
        swal({ title: "Bezig...", text: "Uw gegevens worden gecontroleerd", showConfirmButton: false, type: "" });

        $http.get('/api/user/login/' + $scope.userDetails.email + '/' + $scope.userDetails.password)
        .success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == "true") {
                $window.location.reload();
                swal({ title: "U bent nu ingelogd", text: "U wordt doorverwezen", showConfirmButton: false, type: "" });
            }
            else {
                swal("", "De opgegeven gegevens zijn incorrect!", "error");
            }
        }).error(function (data) {
            swal("Fout", "Er is iets misgegaan!", "error");
        });
    };
});