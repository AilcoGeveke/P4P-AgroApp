agroApp.controller('ManageUser', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.rollen = ['Gebruiker', 'Admin'];

    $scope.selectedGebruikers = [];
    $scope.gebruikers = [];
    $scope.getAllUserData = function () {
        $http.get('/api/user/getall').success(function (data) {
            $scope.gebruikers = data;
        }).error(function (data) {
        })
    }

    $scope.ArchiefGebruikers = [];
    $scope.getArchiefUserData = function () {
        $rootScope.showLoading++;
        $http({
            method: 'GET',
            url: '/api/account/getarchiefusers',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.ArchiefGebruikers = data;
            $rootScope.showLoading--;
        })
    }

    $scope.gebruikerid = 0;
    $scope.ConfirmReAdd = function (gebruikerid) {
        $scope.gebruikerid = gebruikerid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Gebruiker dearchiveren')
              .textContent('Als u doorgaat zal deze gebruiker gedearchiveerd worden!')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddUser();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    var EditUser = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/user/wijzigen/' + $scope.userDetails.id + '/' + $scope.userDetails.naam + '/' + $scope.userDetails.email + '/' + $scope.userDetails.rol,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/gebruikerbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ArchiveUser = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/user/archiveren/' + $scope.userDetails.id,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/gebruikerbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ReAddUser = function () {
        $http({
            method: 'GET',
            url: '/api/user/gebruikerbeheer/terughalen/' + $scope.gebruikerid,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/gebruikerbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };
});