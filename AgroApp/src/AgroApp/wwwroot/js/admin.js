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

agroApp.controller('UserView', function ($scope, $http, $rootScope) {
    $scope.gebruikers = [];
    $scope.getAllUserData = function () {
        $rootScope.showLoading = true;
        $http({
            method: 'GET',
            url: '/api/account/getfulllist',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.gebruikers = data;
            $rootScope.showLoading = false;
        })
    }

    $scope.tijden = [];
    $scope.setTijdenRange = function () {
        for (uur = 6; uur < 20; uur++)
        {
            $scope.tijden.push(uur + ":00");
            $scope.tijden.push(uur + ":30");
        }
    }
});

agroApp.controller('UserEdit', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.rollen = ['Gebruiker', 'Admin'];

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
            $rootScope.changeView('admin/gebruikers');
        }, function () {
            $rootScope.showLoading = false;
        });
    };
});


agroApp.controller('VehicleEdit', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.types = ['Kranen', 'Shovels', 'Trekkers', 'Dumpers', 'Wagens', 'Tanks', 'Ladewagens',
        'Strandreinigen', 'Gladheid', 'Auto s', 'Apparaten', 'Trilplaten',
        'Meetapparatuur', 'Aanhangers', 'Hulpstukken', 'Overige'];

    $scope.showloading = false;
    $scope.showError = false;

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
            $rootScope.changeView('admin/machinebeheer');
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
            $rootScope.changeView('admin/machinebeheer');
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
            if (data == "true")
                window.location.href = '/admin/machinebeheer/machinebeheer';
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
   
});

agroApp.controller('WerkbonEdit', function ($scope, $rootScope, $http) {
    $scope.manKeuze = [];
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

    $scope.selectedMachines = [];
    $scope.machines = [];
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

    $scope.gebruikers = [];
    $scope.getAllUserData = function () {
        $rootScope.showLoading = true;
        $http({
            method: 'GET',
            url: '/api/account/getfulllist',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            $scope.gebruikers = data;
            $rootScope.showLoading = false;
        })
    }

    $scope.submitWerkbonAdd = function () {
        console.log($scope.selectedMachines);
    }

    $scope.onUrenChange = function ($isAantal) {
        if($isAantal)
        {
            $scope.tijd.van = new Date( 1970, 1, 1, 0, 0, 0, 0);
            $scope.tijd.tot = new Date(1970, 1, 1, 0, 0, 0, 0);
        }
        else
        {
            var millDiff = $scope.tijd.tot - $scope.tijd.van;
            var sec = millDiff / 1000;
            var min = sec / 60;
            var hours = min / 60;
            $scope.tijd.aantal = new Date( 1970, 1, 1, hours % 24, min % 60, 0, 0 );
        }
    }

    $scope.onGewichtChange = function ($isNetto) {
        if ($isNetto) {
            $scope.gewicht.vol = new Number(0);
            $scope.gewicht.leeg = new Number(0);            
        }
        else {
            $scope.gewicht.netto =($scope.gewicht.vol - $scope.gewicht.leeg);
        }
    }
});