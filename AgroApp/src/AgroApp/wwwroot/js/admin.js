﻿var agroApp = angular.module('AgroApp');

agroApp.controller('LoginView', function ($scope, $http) {
    var originatorEv;
    $scope.openMenu = function ($mdOpenMenu, ev) {
        originatorEv = ev;
        $mdOpenMenu(ev);
    };
});

agroApp.controller('AppCtrl', function ($scope, $timeout, $mdSidenav, $log) {
    $scope.toggleLeft = buildDelayedToggler('left');
    /**
     * Supplies a function that will continue to operate until the
     * time is up.
     */
    function debounce(func, wait, context) {
        var timer;
        return function debounced() {
            var context = $scope,
                args = Array.prototype.slice.call(arguments);
            $timeout.cancel(timer);
            timer = $timeout(function () {
                timer = undefined;
                func.apply(context, args);
            }, wait || 10);
        };
    }
    /**
     * Build handler to open/close a SideNav; when animation finishes
     * report completion in console
     */
    function buildDelayedToggler(navID) {
        return debounce(function () {
            $mdSidenav(navID)
              .toggle()
              .then(function () {
                  $log.debug("toggle " + navID + " is done");
              });
        }, 200);
    }
    function buildToggler(navID) {
        return function () {
            $mdSidenav(navID)
              .toggle()
              .then(function () {
                  $log.debug("toggle " + navID + " is done");
              });
        }
    }
});

agroApp.controller('LeftCtrl', function ($scope, $timeout, $mdSidenav, $log) {
    $scope.close = function () {
        $mdSidenav('left').close()
          .then(function () {
              $log.debug("close LEFT is done");
          });
    };
});

agroApp.controller('UserEdit', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.rollen = ['Gebruiker', 'Admin'];

    $scope.showConfirmArchiveDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Gebruiker Verwijderen')
              .textContent('Als u doorgaat zal de machine definitief verwijderd worden!')
              .targetEvent(ev)
              .ok('Gebruiker Archiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ArchiveUser();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.showConfirmChangePasswordDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Als u doorgaat zal het wachtwoord van deze gebruiker gereset worden!')
              .textContent('Het nieuwe wachtwoord zal op het scherm getoond worden. Geef deze aan de medewerker door!')
              .targetEvent(ev)
              .ok('Reset')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            $rootScope.showLoading--;
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.showConfirmChangesDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditUser();
        }, function () {
            $rootScope.showLoading--;
        });
    };

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

agroApp.controller('VehicleEdit', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.types = ['Kraan', 'Shovel', 'Trekker', 'Dumper', 'Wagen', 'Tank', 'Ladewagen',
        'Strandreiniging', 'Gladheid', 'Auto', 'Apparaat', 'Trilplaat',
        'Meetapparatuur', 'Aanhanger', 'Hulpstuk', 'Overige'];

    $scope.showConfirmChangesDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditMachine();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.showConfirmDeleteDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Machine Verwijderen')
              .textContent('Als u doorgaat zal de machine definitief verwijderd worden!')
              .targetEvent(ev)
              .ok('Machine Verwijderen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            DeleteMachine();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.ArchiefMachines = [];
    $scope.getArchiefMachines = function () {
        $rootScope.showLoading++;
        $http({
            method: 'GET',
            url: '/api/werkbon/getarchiefmachine',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.ArchiefMachines = data;
            $rootScope.showLoading--;
        })
    }

    $scope.machineid = 0;
    $scope.ConfirmReAdd = function (machineid) {
        $scope.machineid = machineid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Machine dearchiveren')
              .textContent('Als u doorgaat zal deze machine gedearchiveerd worden!')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddMachine();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.AddMachine = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/addmachine/' + $scope.machineDetails.naam + '/' + $scope.machineDetails.nummer + '/' + $scope.machineDetails.kenteken + '/' + $scope.machineDetails.type,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/machinebeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Het machinenummer is al geregistreerd";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var EditMachine = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/editmachine/' + $scope.machineDetails.id + '/' + $scope.machineDetails.naam + '/' + $scope.machineDetails.nummer + '/' + $scope.machineDetails.kenteken + '/' + $scope.machineDetails.cato,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/machinebeheer');
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

    var DeleteMachine = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/deletemachine/' + $scope.machineDetails.id,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/machinebeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen machine geselecteerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ReAddMachine = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/machine/terughalen/' + $scope.machineid,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/machinebeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen machine geselecteerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    }
});

agroApp.controller('KlantEdit', function ($scope, $http, $rootScope, $mdDialog) {


    $scope.showConfirmChangesDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditKlant();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.showConfirmDeleteDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Klant Archiveren')
              .textContent('Als u doorgaat zal deze klant gearchiveerd worden!')
              .targetEvent(ev)
              .ok('Klant Archiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            DeleteKlant();
        }, function () {
            $rootScope.showLoading--;
        });
    };


    $scope.klantid = 0;
    $scope.ConfirmReAdd = function (klantid) {
        $scope.klantid = klantid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Klant Dearchiveren')
              .textContent('Als u doorgaat zal deze klant gedearchiveerd worden')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddKlant();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.AddKlant = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/addklant/' + $scope.klantDetails.naam + '/' + $scope.klantDetails.adres,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/klantbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Deze klant is al geregistreerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ReAddKlant = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/klant/terughalen/' + $scope.klantid,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/klantbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen klant geselecteerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    }

    $scope.selectedKlanten = [];
    $scope.klanten = [];
    $scope.getKlanten = function () {
        $rootScope.showLoading++;

        $http.get('/api/werkbon/getklanten')
        .success(function (data) {
            $scope.klanten = data;
            $rootScope.showLoading--;
        }).error(function (data) {
            $rootScope.showLoading--;
        });
    };


    $scope.ArchiefKlanten = [];
    $scope.getArchiefKlanten = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/getarchiefklanten',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.ArchiefKlanten = data;
        });
    };


    var EditKlant = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/editklant/' + $scope.klantDetails.id + '/' + $scope.klantDetails.naam + '/' + $scope.klantDetails.adres,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/klantbeheer');
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

    var DeleteKlant = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/deleteklant/' + $scope.klantDetails.id,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/klantbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen klant geselecteerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };
});

agroApp.controller('HulpstukEdit', function ($scope, $http, $rootScope, $mdDialog) {

    $scope.showConfirmChangesDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditHulpstuk();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.showConfirmDeleteDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Klant Archiveren')
              .textContent('Als u doorgaat zal deze hulpstuk gearchiveerd worden!')
              .targetEvent(ev)
              .ok('Hulpstuk Archiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            DeleteHulpstuk();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.hulpstukid = 0;
    $scope.ConfirmReAdd = function (hulpstukid) {
        $scope.hulpstukid = hulpstukid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Hulpstuk Dearchiveren')
              .textContent('Als u doorgaat zal dit hulpstuk gedearchiveerd worden')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddHulpstuk();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    $scope.AddHulpstuk = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/addhulpstuk/' + $scope.hulpstukDetails.naam + '/' + $scope.hulpstukDetails.nummer,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/hulpstukbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Deze hulpstuk is al geregistreerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ReAddHulpstuk = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/hulpstuk/terughalen/' + $scope.hulpstukid,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/hulpstukbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen hulpstuk geselecteerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    }

    $scope.ArchiefHulpstukken = [];
    $scope.getArchiefHulpstukken = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/getarchiefhulpstukken',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.ArchiefHulpstukken = data;
            $rootScope.showLoading--;
        }).error(function (data) {
            $rootScope.showLoading--;
        })
    };

    var EditHulpstuk = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/edithulpstuk/' + $scope.hulpstukDetails.id + '/' + $scope.hulpstukDetails.naam + '/' + $scope.hulpstukDetails.nummer,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/hulpstukbeheer');
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

    var DeleteHulpstuk = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/deletehulpstuk/' + $scope.hulpstukDetails.id,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/hulpstukbeheer');
            else {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen hulpstuk geselecteerd!";
            }
        }).error(function () {
            $rootScope.showLoading--;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };
});

agroApp.controller('WerkbonEdit', function ($scope, $rootScope, $http, $mdToast, $mdDialog, $timeout) {
    $rootScope.showLoading = 0;
    $scope.werktijd = [];
    $scope.werktijd.van = new Date(1970, 1, 1, 0, 0, 0, 0);
    $scope.werktijd.tot = new Date(1970, 1, 1, 0, 0, 0, 0);
    $scope.werktijd.urenTotaal = new Date(1970, 1, 1, 0, 0, 0, 0);
    $scope.werktijd.pauzeTotaal = new Date(1970, 1, 1, 0, 0, 0, 0);
    $scope.opdracht = [];
    $scope.opdracht.datum = new Date();

    $scope.checkRole = function () {
        if (User.IsInRole("Administrator"))
            changeView("admin/main");
        else
            changeView("werknemer/menu");
    }

    $scope.manKeuze = ['Man', 'Hovenier', 'Stratenmaker', 'Machinist', 'Onderhoud/Reparatie', 'Klaarzetten/omkoppelen/opbergen', 'Schoonmaken', 'Opruimen', 'Diverse werkzaamheden', 'Brandstof rondbrengen',
        'Compostering', 'Recycling', 'Terreinbeheer', 'Weegbonnen administratie', 'Groenonderhoud', 'Reistijd', 'Calculatie', 'Klantcontact/werving/service', 'Uitvoering', 'Werkvoorbereiding', 'Kantoor algemeen', 'Administratie', 'Financiële administratie'];

    $scope.
        ankeuze = [];
    
    $scope.selectedMachines = [];
    $scope.selectedUsers = [];
    $scope.machines = [];
    $scope.mankeuzes = [];
    $scope.selectedGewichten = [];
    $scope.gewichten = ['Zand', 'Gemengde grond', 'Gezeefde grond', 'Woudgrond', 'Compost', 'Menggranulaat', 'Kleischelpen', 'Schone schelpen', 'Houtchips'];
    $scope.getMachines = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werkbon/getmachines',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.machines = data;
            $rootScope.showLoading--;
        }).error(function (data) {
            $rootScope.showLoading--;
        });
    }


    $scope.increaseSelectedMachineList = function () {
        $scope.selectedMachines.push($scope.machines[0]);
    }
    $scope.decreaseSelectedMachineList = function () {
        $scope.selectedMachines.pop();
    }
    $scope.increaseMankeuze = function () {
        $scope.manKeuze.push($scope.mankeuzes[0]);
    }

    $scope.decreaseMankeuze = function () {
        $scope.manKeuze.pop();
    }

    $scope.increaseSelectedGewichtenList = function () {
        $scope.selectedGewichten.push($scope.gewichten[0]);
    }
    $scope.decreaseSelectedGewichtenList = function () {
        $scope.selectedGewichten.pop();
    }

    $scope.increaseSelectedUserList = function () {
        $scope.selectedUsers.push($scope.gebruikers[0]);
    }
    $scope.decreaseSelectedUsersList = function () {
        $scope.selectedUsers.pop();
    }


    $scope.selectedHulpstukken = [];
    $scope.hulpstukken = [];
    $scope.getHulpstukken = function () {
        $rootScope.showLoading++;

        $http.get('/api/werkbon/gethulpstukken').success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.hulpstukken = data;
            $rootScope.showLoading--;
        }).error(function (data) {
            $rootScope.showLoading--;
        });
    }

    $scope.selectedGebruikers = [];
    $scope.gebruikers = [];
    $scope.getAllUserData = function () {
        $rootScope.showLoading++;
        $http.get('/api/account/getfulllist').success(function (data) {
            $scope.gebruikers = data;
            $rootScope.showLoading--;
        }).error(function (data) {
            $rootScope.showLoading--;
        })
    }

    $scope.selectedKlant = "";
    $scope.klanten = [];
    $scope.getKlanten = function () {
        $rootScope.showLoading++;
        $http.get('/api/werkbon/getklanten').success(function (data) {
            $scope.klanten = data;
            $rootScope.showLoading--;
        }).error(function (data) {
            $mdToast.show($mdToast.simple().textContent('Kon klanten niet inladen!').action('Ververs pagina'));
            $rootScope.showLoading--;
        });
    };

    $scope.submitWerkbonAdd = function () {
        console.log($scope.selectedMachines);
    }

    $scope.submitWerkbon = function () {
        $rootScope.showLoading++;
        var sendData = JSON.stringify({
            Gebruiker: $scope.selectedGebruiker,
            Datum: $scope.opdracht.datum,
            Klant: $scope.klant,
            ManKeuze: $scope.manKeuze,
            Machines: $scope.selectedMachines,
            Hulpstukken: $scope.selectedHulpstukken,
            VanTijd: $scope.werktijd.van.getTime(),
            TotTijd: $scope.werktijd.tot.getTime(),
            TotaalTijd: $scope.werktijd.urenTotaal.getTime(),
            PauzeTijd: $scope.werktijd.pauzeTotaal.getTime(),
            verbruiktematerialen: $scope.werktijd.verbruikteMaterialen,
            Gewichten: $scope.selectedGewichten,
            Opmerking: $scope.werktijd.opmerking,
            IdOpdrachtWerknemer: $scope.werknemerOpdracht.id
        })

        $http.post('/api/werkbon/toevoegen', sendData)
        .success(function (data) {
            $rootScope.showLoading--;
            $rootScope.changeView('werknemer/opdrachten');
        }).error(function (data, ev) {
            $mdDialog.show(
                $mdDialog.alert()
                .parent(angular.element(document.querySelector('#popupContainer')))
                .clickOutsideToClose(true)
                .title('Er is iets misgegaan!')
                .textContent(data)
                .ariaLabel('Alert Dialog Demo')
                .ok('Begrepen')
                .targetEvent(ev)
            );
            $rootScope.showLoading--;
        });
    }

    $scope.submitOpdracht = function () {
        $rootScope.showLoading++;
        var sendData = JSON.stringify({
            klant: $scope.selectedKlant,
            gebruikers: $scope.selectedGebruikers,
            locatie: $scope.opdracht.adres,
            beschrijving: $scope.opdracht.omschrijving,
            datum: $scope.opdracht.datum
        });

        $http.post('/api/opdracht/toevoegen', sendData)
        .success(function (data, status, headers, config) {
            $rootScope.showLoading--;
            $rootScope.showMessage = true;
            $rootScope.message = "Opdracht succesvol toegevoegd!";
            $timeout(function () {
                $rootScope.showMessage = false;
            }, 5000);
            //$rootScope.changeView('admin/planning');
        }).error(function (data, ev) {
            $mdDialog.show(
                $mdDialog.alert()
                .parent(angular.element(document.querySelector('#popupContainer')))
                .clickOutsideToClose(true)
                .title('Er is iets misgegaan!')
                .textContent(data)
                .ariaLabel('Alert Dialog Demo')
                .ok('Begrepen')
                .targetEvent(ev)
            );
            $rootScope.showLoading--;
        })
    }

    $scope.onUrenChange = function ($isAantal) {
        if ($isAantal) {
            $scope.werktijd.van = new Date(1970, 1, 1, 0, 0, 0, 0);
            $scope.werktijd.tot = new Date(1970, 1, 1, 0, 0, 0, 0);
        }
        else {
            var millDiff = $scope.werktijd.tot - $scope.werktijd.van;
            var sec = millDiff / 1000;
            var min = sec / 60;
            var hours = min / 60;
            $scope.werktijd.urenTotaal = new Date(1970, 1, 1, hours % 24, min % 60, 0, 0);
        }
    }

    $scope.onGewichtChange = function ($isNetto) {
        if ($isNetto) {
            $scope.gewicht.vol = new Number(0);
            $scope.gewicht.leeg = new Number(0);
        }
        else {
            $scope.gewicht.netto = $scope.gewicht.vol - $scope.gewicht.leeg;
        }
    }

    $scope.querySearch = querySearch;

    $scope.machineSelectie = [];
    $scope.selectedMachine = "";

    function querySearch(criteria, targetArray) {
        return criteria ? targetArray.filter(createFilterFor(criteria)) : targetArray;
    };

    function createFilterFor(query) {
        var lowercaseQuery = angular.lowercase(query);
        return function filterFn(state) {
            return (state.Name.toLowerCase().indexOf(lowercaseQuery) === 0);
        };
    }

    $scope.querySearchNaam = function (criteria, targetArray) {
        return criteria ? targetArray.filter($scope.createFilterForNaam(criteria)) : targetArray;
    };

    $scope.createFilterForNaam = function (query) {
        var lowercaseQuery = angular.lowercase(query);
        return function filterFn(state) {
            return (state.Naam.toLowerCase().indexOf(lowercaseQuery) === 0);
        };
    }

});

agroApp.controller('WerknemerEdit', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.showConfirmChangePasswordDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading++;
        var confirm = $mdDialog.confirm()
              .title('Als u doorgaat zal het wachtwoord worden veranderd.')
              .targetEvent(ev)
              .ok('Wijzig')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            $rootScope.showLoading--;
        }, function () {
            $rootScope.showLoading--;
        });
    };

});

agroApp.controller('WerkbonView', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.werkbonnen = [];
    $scope.getWerkbonnen = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/werknemer/getwerkbonnen',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.werkbonnen = data;
        });
    };

});

agroApp.controller('OpdrachtView', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.WerknemerOpdrachten = [];
    $scope.getGebruikerOpdrachten = function () {
        $rootScope.showLoading++;
        $http.get(
            '/werknemer/getgebruikeropdrachten'
        ).success(function (data) {
            console.log(data);
            $scope.WerknemerOpdrachten = data;
            $rootScope.showLoading--;
        })
    }
    $scope.werknemeropdrachten = [];
    $scope.getOpdrachtWerknemer = function () {
        $rootScope.showLoading++;
        $http.get(
            '/werknemer/getopdrachtwerknemer/'
        ).success(function (data) {
            console.log(data);
            $scope.opdrachten = data;
            $rootScope.showLoading--;
        })
    }

    var EditAssignment = function () {
        $rootScope.showLoading++;

        $http({
            method: 'GET',
            url: '/api/werknemers/assignmentedit/' + $scope.assignmentDetails.locatie + '/' + $scope.assignmentDetails.beschrijving,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('werknemer/');
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

agroApp.controller('PlanningView', ['$scope', '$http', '$rootScope', '$timeout', '$mdDialog',
    function ($scope, $http, $rootScope, $timeout, $mdDialog) {
        $scope.gebruikerTijden = [];
        $scope.geselecteerdeDagDatum = new Date();
        $scope.geselecteerdeWeekDatum = new Date();
        $rootScope.showLoading = 0;

        $scope.showConfirmDeleteAll = function (ev) {
            // Appending dialog to document.body to cover sidenav in docs app
            $rootScope.showLoading++;
            var confirm = $mdDialog.confirm()
                  .title('Weet u zeker dat u alle opdrachten en werkbonnen wilt verwijderen?')
                  .textContent('LET OP: Het is niet mogelijk om de verijderde data hierna nog terug te halen!!')
                  .targetEvent(ev)
                  .ok('Verwijderen')
                  .cancel('Annuleer');
            $mdDialog.show(confirm).then(function () {
                $scope.showConfirmPermanentDelete();
            }, function () {
                $rootScope.showLoading--;
            });
        };

        $scope.showConfirmPermanentDelete = function (ev) {
            // Appending dialog to document.body to cover sidenav in docs app
            $rootScope.showLoading++;
            var confirm = $mdDialog.confirm()
                  .title('Verwijdering bevestigen')
                  .textContent('Alle opdrachten en werkbonnen zullen worden verwijderd!!')
                  .targetEvent(ev)
                  .ok('Definitief verwijderen')
                  .cancel('Annuleer');
            $mdDialog.show(confirm).then(function () {
                DeleteAllData();
            }, function () {
                $rootScope.showLoading--;
            });
        }

        $scope.updateGebruikersDag = function () {
            $rootScope.showLoading++;
            $http.get('/api/planning/getGebruikersWerktijden/' + $scope.geselecteerdeDagDatum.getTime())
                .success(function (response) { $scope.gebruikerTijden = response; $rootScope.showLoading--; })
                .error(function (response) {    
                    $rootScope.showLoading--;
                    setMessage($rootScope, $timeout, "Er is iets misgegaan bij het ophalen van de gebruikers");
                });
        }

        $scope.updateGebruikersWeek = function () {
            $scope.weekNummer = $scope.geselecteerdeWeekDatum.getWeek();
        }

        $scope.tijden = [];
        $scope.setTijdenRange = function () {
            $scope.tijden = [];
            for (uur = 6; uur <= 20; uur++) {
                $scope.tijden.push(uur + ":00");
                if (uur != 20)
                    $scope.tijden.push(uur + ":30");
            }
        }

        $scope.opdrachten = [];
        $scope.getOpdrachten = function () {
            $rootScope.showLoading++;
            $http.get('/api/opdracht/alle/false')
            .success(function (data) {
                $scope.opdrachten = data;
                $rootScope.showLoading--;
            }).error(function (data) {
                $rootScope.showLoading--;
                setMessage($rootScope, $timeout, "Er is iets misgegaan bij het ophalen van de opdrachten");
            })
        };

        var DeleteAllData = function () {
            $rootScope.showLoading++;
            $http({
                method: 'GET',
                url: '/api/werkbon/deleteall',
                params: 'limit=10, sort_by=created:desc',
                headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
            }).success(function (data) {
                // With the data succesfully returned, call our callback
                if (data == true)
                    $rootScope.changeView('admin/planning');
                else {
                    $rootScope.showLoading--;
                    $scope.showError = true;
                    $scope.errorMessage = "Er is niets om te verwijderen!";
                }
            }).error(function () {
                $rootScope.showLoading--;
                $scope.showError = true;
                $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
            });
        };

        $scope.opdrachtGeenDatumFilter = function (item) {
            return item.datum === null;
        }

        $scope.opdrachtGeenWerknemersFilter = function (item) {
            return item.gebruikerCount === 0;
        }

        $scope.opdrachtLopendFilter = function (item) {
            var date = new Date(item.datum);
            return item.datum != null && item.gebruikerCount != 0 && date.getTime() <= new Date().getTime();
        }

        $scope.opdrachtGeplandFilter = function (item) {
            var date = new Date(item.datum);
            return item.datum != null && item.gebruikerCount != 0 && date.getTime() > new Date().getTime();
        }
    }])

agroApp.controller('StatistiekenView', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.statistieken = [];
    $scope.getStatistieken = function () {
        $rootScope.showLoading++;
        $http.get(
            '/werknemer/getstatistieken/'
            //+ $scope.statistieken.vanaf + '/' + $scope.statistieken.tot
        ).success(function (data) {
            console.log(data);
            $scope.statistieken = data;
            $rootScope.showLoading--;
        })
    }
});
Date.prototype.getWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
}

setMessage = function ($rootScope, $timeout, mess) {
    $rootScope.message = mess;
    $rootScope.showMessage = true;
    $timeout(function () {
        $rootScope.showMessage = false;
    }, 5000);
}