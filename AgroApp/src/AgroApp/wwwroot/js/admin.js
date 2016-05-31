var agroApp = angular.module('AgroApp');

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
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Gebruiker Verwijderen')
              .textContent('Als u doorgaat zal de machine definitief verwijderd worden!')
              .targetEvent(ev)
              .ok('Gebruiker Archiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ArchiveUser();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.showConfirmChangePasswordDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Als u doorgaat zal het wachtwoord van deze gebruiker gereset worden!')
              .textContent('Het nieuwe wachtwoord zal op het scherm getoond worden. Geef deze aan de medewerker door!')
              .targetEvent(ev)
              .ok('Reset')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            $rootScope.showLoading = false;
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.showConfirmChangesDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditUser();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.ArchiefGebruikers = [];
    $scope.getArchiefUserData = function () {
        $rootScope.showLoading = true;
        $http({
            method: 'GET',
            url: '/api/account/getarchiefusers',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.ArchiefGebruikers = data;
            $rootScope.showLoading = false;
        })
    }

    $scope.gebruikerid = 0;
    $scope.ConfirmReAdd = function (gebruikerid) {
        $scope.gebruikerid = gebruikerid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Gebruiker dearchiveren')
              .textContent('Als u doorgaat zal deze gebruiker gedearchiveerd worden!')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddUser();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    var EditUser = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/user/wijzigen/' + $scope.userDetails.id + '/' + $scope.userDetails.naam + '/' + $scope.userDetails.email + '/' + $scope.userDetails.rol,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/gebruikers');
            else {
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ArchiveUser = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $scope.showloading = false;
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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $scope.showloading = false;
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
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditMachine();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.showConfirmDeleteDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Machine Verwijderen')
              .textContent('Als u doorgaat zal de machine definitief verwijderd worden!')
              .targetEvent(ev)
              .ok('Machine Verwijderen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            DeleteMachine();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.ArchiefMachines = [];
    $scope.getArchiefMachines = function () {
        $rootScope.showLoading = true;
        $http({
            method: 'GET',
            url: '/api/werkbon/getarchiefmachine',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.ArchiefMachines = data;
            $rootScope.showLoading = false;
        })
    }

    $scope.machineid = 0;
    $scope.ConfirmReAdd = function (machineid) {
        $scope.machineid = machineid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Machine dearchiveren')
              .textContent('Als u doorgaat zal deze machine gedearchiveerd worden!')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddMachine();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.AddMachine = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "Het machinenummer is al geregistreerd";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var EditMachine = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var DeleteMachine = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen machine geselecteerd!";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ReAddMachine = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen machine geselecteerd!";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    }
});

agroApp.controller('KlantEdit', function ($scope, $http, $rootScope, $mdDialog) {


    $scope.showConfirmChangesDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Wijzigingen Toepassen?')
              .textContent('Als u doorgaat zullen de wijzigingen opgeslagen worden!')
              .targetEvent(ev)
              .ok('Wijzigingen Toepassen')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            EditKlant();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.showConfirmDeleteDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Klant Archiveren')
              .textContent('Als u doorgaat zal deze klant gearchiveerd worden!')
              .targetEvent(ev)
              .ok('Klant Archiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            DeleteKlant();
        }, function () {
            $rootScope.showLoading = false;
        });
    };


    $scope.klantid = 0;
    $scope.ConfirmReAdd = function (klantid) {
        $scope.klantid = klantid;
        $scope.showConfirmReAddDialog();
    }

    $scope.showConfirmReAddDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Klant Dearchiveren')
              .textContent('Als u doorgaat zal deze klant gedearchiveerd worden')
              .targetEvent(ev)
              .ok('Dearchiveren')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            ReAddKlant();
        }, function () {
            $rootScope.showLoading = false;
        });
    };

    $scope.AddKlant = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "Deze klant is al geregistreerd!";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var ReAddKlant = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen klant geselecteerd!";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    }

    $scope.selectedKlanten = [];
    $scope.klanten = [];
    $scope.getKlanten = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/werkbon/getklanten',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.klanten = data;
        });
    };


    $scope.ArchiefKlanten = [];
    $scope.getArchiefKlanten = function () {
        $scope.showloading = true;

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
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };

    var DeleteKlant = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "Er is geen klant geselecteerd!";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };
});

agroApp.controller('WerkbonEdit', function ($scope, $rootScope, $http) {
    'use strict';
    var self = this;
    $rootScope.showloading = false;
    $scope.getManKeuzeData = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/werkbon/getmankeuze',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.man = data;
        });
    };


    $scope.checkRole = function () {
        if (User.IsInRole("Administrator"))
            changeView("admin/main");
        else
            changeView("werknemer/menu");
    }


    $scope.selectedMachines = [];
    $scope.selectedUsers = [];
    $scope.machines = [];
    $scope.selectedGewichten = [];
    $scope.gewichten = ['Zand', 'Gemengde grond', 'Gezeefde grond', 'Woudgrond', 'Compost', 'Menggranulaat', 'Kleischelpen', 'Schone schelpen', 'Houtchips'];
    $scope.getMachines = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/werkbon/getmachines',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.machines = data;
        });
    };


    $scope.increaseSelectedMachineList = function () {
        $scope.selectedMachines.push($scope.machines[0]);
    }
    $scope.decreaseSelectedMachineList = function () {
        $scope.selectedMachines.pop();
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
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/werkbon/gethulpstukken',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            $scope.hulpstukken = data;
        });
    };

    self.selectedGebruiker = [];
    self.gebruikers = [];
    $scope.getAllUserData = function () {
        $rootScope.showLoading = true;
        $http({
            method: 'GET',
            url: '/api/account/getfulllist',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            self.gebruikers = data;
            $rootScope.showLoading = false;
        })
    }

    self.selectedKlant = "";
    self.klanten = [];
    $scope.getKlanten = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/werkbon/getklanten',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            self.klanten = data;
        });
    };

    $scope.submitWerkbonAdd = function () {
        console.log($scope.selectedMachines);
    }

    $scope.submitWerkbon = function () {
        $rootScope.showLoading = true;
        var sendData = JSON.stringify({
            Gebruiker: $scope.selectedGebruiker,
            Datum: $scope.opdracht.datum,
            Klant: $scope.klant.naam,
            ManKeuze: $scope.manKeuze,
            Machines: self.selectedMachines,
            Hulpstukken: self.selectedHulpstukken,
            VanTijd: $scope.werktijd.van,
            TotTijd: $scope.werktijd.tot,
            TotaalTijd: $scope.werktijd.urenTotaal,
            verbruiktematerialen: $scope.werktijd.verbruikteMaterialen,
            opmerking: $scope.werktijd.opmerking
        })

        var config = { headers: { 'Content-Type': 'application/json' } }

        $http.post('/api/werkbon/toevoegen', sendData)
        .success(function (data, status, headers, config) {
            $rootScope.showLoading = false;
            $rootScope.changeView('admin/planning');
        })
    }

    $scope.submitOpdracht = function () {
        $rootScope.showLoading = true;
        var sendData = JSON.stringify({
            klant: self.selectedKlant,
            gebruikers: self.selectedGebruikers,
            locatie: $scope.opdracht.adres,
            beschrijving: $scope.opdracht.omschrijving,
            datum: $scope.opdracht.datum
        });

        var config = { headers: { 'Content-Type': 'application/json' } }

        $http.post('/api/opdracht/toevoegen', sendData)
        .success(function (data, status, headers, config) {
            $rootScope.showLoading = false;
            $rootScope.changeView('admin/planning');
        })
    }

    $scope.onUrenChange = function ($isAantal) {
        if ($isAantal) {
            $scope.tijd.van = new Date(1970, 1, 1, 0, 0, 0, 0);
            $scope.tijd.tot = new Date(1970, 1, 1, 0, 0, 0, 0);
        }
        else {
            var millDiff = $scope.tijd.tot - $scope.tijd.van;
            var sec = millDiff / 1000;
            var min = sec / 60;
            var hours = min / 60;
            $scope.tijd.aantal = new Date(1970, 1, 1, hours % 24, min % 60, 0, 0);
        }
    }

    $scope.onGewichtChange = function ($isNetto) {
        if ($isNetto) {
            $scope.gewicht.vol = new Number(0);
            $scope.gewicht.leeg = new Number(0);
        }
        else {
            $scope.gewicht.netto = ($scope.gewicht.vol - $scope.gewicht.leeg);
        }
    }

    self.querySearch = querySearch;

    self.machineSelectie = [];
    self.selectedMachine = "";

    function querySearch(criteria, targetArray) {
        return criteria ? targetArray.filter(createFilterFor(criteria)) : targetArray;
    };

    function createFilterFor(query) {
        var lowercaseQuery = angular.lowercase(query);
        return function filterFn(state) {
            return (state.Name.toLowerCase().indexOf(lowercaseQuery) === 0);
        };
    }

});

agroApp.controller('WerknemerEdit', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.showConfirmChangePasswordDialog = function (ev) {
        // Appending dialog to document.body to cover sidenav in docs app
        $rootScope.showLoading = true;
        var confirm = $mdDialog.confirm()
              .title('Als u doorgaat zal het wachtwoord worden veranderd.')
              .targetEvent(ev)
              .ok('Wijzig')
              .cancel('Annuleer');
        $mdDialog.show(confirm).then(function () {
            $rootScope.showLoading = false;
        }, function () {
            $rootScope.showLoading = false;
        });
    };

});

agroApp.controller('Query', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.print = [];
    $scope.getArchiefMachines = function () {
        $rootScope.showLoading = true;
        $http({
            method: 'GET',
            url: '/api/werkbon/getdata',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.print = data;
            $rootScope.showLoading = false;
        })
    }

});

agroApp.controller('OpdrachtView', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.WerknemerOpdrachten = [];
    $scope.getGebruikerOpdrachten = function () {
        $rootScope.showLoading = true;
        $http.get(
            '/werknemer/getgebruikeropdrachten'
        ).success(function (data) {
            console.log(data);
            $scope.WerknemerOpdrachten = data;
            $rootScope.showLoading = false;
        })
    }
    $scope.werknemeropdrachten = [];
    $scope.getOpdrachtWerknemer = function () {
        $rootScope.showLoading = true;
        $http.get(
            '/werknemer/getopdrachtwerknemer/'
        ).success(function (data) {
            console.log(data);
            $scope.opdrachten = data;
            $rootScope.showLoading = false;
        })
    }

    var EditAssignment = function () {
        $scope.showloading = true;

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
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven waardes zijn ongeldig";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een beheerder";
        });
    };
});

agroApp.controller('PlanningView', function ($scope, $http, $rootScope) {
    $scope.gebruikerTijden = [];
    $scope.geselecteerdeDagDatum = new Date();
    $scope.geselecteerdeWeekDatum = new Date();

    $scope.updateGebruikersDag = function () {
        $rootScope.showLoading = true;
        $http.get('/api/planning/getGebruikersWerktijden/' + $scope.geselecteerdeDagDatum.getTime())
        .success(function (response) { $scope.gebruikerTijden = response; $rootScope.showLoading = false; })
        .error(function (response) { console.log(response) });
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
        $rootScope.showLoading = true;
        $http.get('/api/opdracht/alle/false')
        .success(function (data) {
            $scope.opdrachten = data;
            $rootScope.showLoading = false;
        })
    }
})

Date.prototype.getWeek = function () {
    var onejan = new Date(this.getFullYear(), 0, 1);
    return Math.ceil((((this - onejan) / 86400000) + onejan.getDay() + 1) / 7);
}