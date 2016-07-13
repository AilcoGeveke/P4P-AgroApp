var agroApp = angular.module('materialAdmin');

agroApp.controller('UserManagement', function ($window, $scope, userManagement, tableService) {
    var um = this;

    um.userDetails = {};
    um.allUsers = {};

    um.registerUser = function () {
        userManagement.registerUser(this.userDetails)
        .then(function successCallback(response) {
            if (response.data != "succes") {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Gebruiker is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/gebruikers/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    um.getAllUsers = function () {
        userManagement.getAllUsers().then(
            function successCallback(response) {
                console.log(response.data);
                um.allUsers = response.data;
                tableService.data = um.allUsers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllArchivedUsers = function () {
        userManagement.getAllArchivedUsers().then(
            function successCallback(response) {
                um.allUsers = response.data;
                tableService.data = um.allUsers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.applyChangesToUser = function () {
        userManagement.applyChangesToUser(this.userDetails)
        .then(function successCallback(response) {
            if (response.data != true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Gebruiker is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/gebruikers/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met de ontwikkelaar!", "error");
        });
    };
    um.archiveUser = function (user) {
        swal({
            title: "Weet u zeker dat u " + user.Name + " wilt archiveren?",
            text: "Hierdoor zal het account gedeactiveerd worden. Het zal niet meer mogelijk zijn voor de gebruiker om in te loggen.",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            userManagement.archiveUser(user.IdEmployee).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: user.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 3000, showConfirmButton: false });
                        um.getAllUsers();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: user.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: user.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: user.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
    um.restoreUser = function (user) {
        swal({
            title: "Weet u zeker dat u " + user.Name + " wilt dearchiveren?",
            text: "Hierdoor zal de account geactiveerd worden. Het zal weer mogelijk zijn voor de gebruiker om in te loggen.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            userManagement.restoreUser(user.IdEmployee).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: user.Name + " is gedearchiveerd. De gebruiker kan weer inloggen!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = '/admin/gebruikers/overzicht'; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: user.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: user.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: user.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
});

agroApp.controller('MachineManagement', function ($window, $scope, machineManagement, tableService) {
    var um = this;

    um.types = ['Kraan', 'Shovel', 'Trekker', 'Dumper', 'Wagen', 'Tank', 'Ladewagen',
        'Strandreiniging', 'Gladheid', 'Auto', 'Apparaat', 'Trilplaat',
        'Meetapparatuur', 'Aanhanger', 'Hulpstuk', 'Overige'];

    um.machineDetails = {};
    um.allMachines = {};

    um.registerMachine = function () {
        machineManagement.register(this.machineDetails)
        .then(function successCallback(response) {
            if (response.data != true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Machine is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/machines/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    um.getAllMachines = function () {
        machineManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                um.allMachines = response.data;
                tableService.data = um.allMachines;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllArchivedMachines = function () {
        machineManagement.getAllArchived().then(
            function successCallback(response) {
                um.allMachines = response.data;
                tableService.data = um.allMachines;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.applyChangesToMachine = function () {
        machineManagement.applyChanges(this.machineDetails)
        .then(function successCallback(response) {
            if (response.data != true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Machine is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/machines/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    um.archiveMachine = function (machine) {
        swal({
            title: "Weet u zeker dat u " + machine.Name + " wilt archiveren?",
            text: "Hierdoor zal de account gedeactiveerd worden. Het zal niet meer mogelijk zijn voor de gebruiker om in te loggen.",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            machineManagement.archiveMachine(machine.IdEmployee).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: machine.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 3000, showConfirmButton: false });
                        um.getAll();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: machine.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: machine.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: machine.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
    um.restoreMachine = function (machine) {
        swal({
            title: "Weet u zeker dat u " + machine.Name + " wilt dearchiveren?",
            text: "Hierdoor zal de machine geactiveerd worden. Het zal weer mogelijk worden om de machine te gebruiken.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            machineManagement.restore(machine.IdEmployee).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: machine.Name + " is gedearchiveerd. De machine kan weer gebruikt worden!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = '/admin/machines/overzicht'; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: machine.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: machine.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
        });
    };
});

agroApp.controller('ManageUser2', function ($scope, $http, $rootScope, $mdDialog) {
    $scope.rollen = ['Gebruiker', 'Admin'];

    $scope.selectedUsers = [];
    $scope.users = [];
    $scope.getAllUserData = function () {
        $http.get('/api/user/getall').success(function (data) {
            $scope.users = data;
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

agroApp.controller('CustomerManagement', function ($window, $scope, machineManagement, tableService) {
    um.getAllCustomers = function () {
        CustomerManagement.getAllCustomers().then(
            function successCallback(response) {
                console.log(response.data);
                um.getAllCustomers = response.data;
                tableService.data = um.AllCustomers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    }

    um.customerDetails = {};
    um.allCustomers = {};

    um.registerCustomer = function () {
        customerManagement.registerCustomer(this.customerDetails)
        .then(function successCallback(response) {
            if (response.data != "succes") {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Gebruiker is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/klanten/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };

})

agroApp.controller('CargoManagement', function ($window, $scope, userManagement, machineManagement,tableService) {
    var um = this;


});