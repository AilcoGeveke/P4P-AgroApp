var agroApp = angular.module('materialAdmin');

agroApp.controller('UserManagement', function ($window, $scope, userManagement, tableService) {
    var um = this;

    um.userDetails = {};
    um.allUsers = {};

    um.registerUser = function () {
        userManagement.registerUser(um.userDetails)
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
            text: "Hierdoor zal het account geactiveerd worden. Het zal weer mogelijk zijn voor de gebruiker om in te loggen.",
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
        machineManagement.register(um.machineDetails)
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
        console.log(um.machineDetails);
        machineManagement.applyChanges(um.machineDetails)
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
            text: "Hierdoor zal deze machine gedeactiveerd worden. Het zal niet meer mogelijk zijn om deze machine te gebruiken.",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            machineManagement.archive(machine.IdMachine).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: machine.Name + " is gearchiveerd. De Machine kan niet meer worden gebruikt!", timer: 3000, showConfirmButton: false });
                        um.getAllMachines();
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
            machineManagement.restore(machine.IdMachine).then(
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

agroApp.controller('TimesheetController', function ($scope, userManagement, customerManagement, assignmentManagement, machineManagement, timesheetManagement, workTypeList) {
    var um = this;

    um.showMainView = true;
    um.showTaskOverview = true;
    um.showNewTaskCard = false;

    um.allTimesheets = [];
    um.allUsers = [];
    um.allCustomers = [];
    um.allAssignments = [];
    um.allMachines = [];
    um.allAttachments = [];
    um.allWorkTypes = workTypeList.data;
    um.selectedCoworkers = [];
    um.selectedMachines = [];
    um.selectedAttachments = [];
    um.selectedAssignment = {};

    var nulDate = new Date(1970, 1, 1, 0, 0, 0, 0);
    console.log(nulDate);
    um.timesheetDetails = {};
    um.timesheetDetails.WorkType = 'Machinist';
    um.timesheetDetails.StartTime = nulDate;
    um.timesheetDetails.EndTime = nulDate;
    um.timesheetDetails.TotalTime = nulDate;

    um.updateTime = function () {
        um.timesheetDetails.StartTime = nulDate;
        um.timesheetDetails.EndTime = nulDate;
        um.timesheetDetails.TotalTime = nulDate;
    }

    um.getAllTimesheets = function (id) {
        timesheetManagement.getAll(id).then(
            function successCallback(response) {
                console.log(response.data);
                um.allTimesheets = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    um.addTimesheet = function () {
        swal({
            title: "",
            text: "Taak word toegevoegd!",
            showConfirmButton: false
        });

        timesheetManagement.add(um.timesheetDetails)
        .then(function successCallback(response) {
            if (response.data != true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Taak is aangemaakt", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                um.timesheetDetails = {};
                $scope.showTaskOverview = true;
                $scope.showNewTaskCard = false;
                um.getAllTimesheets($scope.EmployeeAssignment.IdEmployeeAssignment);
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
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    var DeleteAllData = function () {
        $rootScope.showLoading++;
        $http({
            method: 'GET',
            url: '/api/assignment/deleteall',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == true)
                $rootScope.changeView('admin/overzicht');
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

    um.selectedDate = new Date();
    um.selectedEndDate = addDays(new Date(), 2);

    um.newAssignment = {};

    um.getAllUsers = function () {
        userManagement.getAllUsers().then(
            function successCallback(response) {
                console.log(response.data);
                um.allUsers = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllCustomers = function () {
        customerManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                um.allCustomers = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAssignment = function (id) {
        assignmentManagement.get(id).then(
            function successCallback(response) {
                um.selectedAssignment = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllAssignments = function (userSpecific) {
        assignmentManagement.getAll(um.selectedDate.getTime(), userSpecific).then(
            function successCallback(response) {
                um.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllAssignmentsPeriod = function (prepareForPlanning, user) {
        assignmentManagement.getAllPeriod(um.selectedDate.getTime(), um.selectedEndDate.getTime(), user, prepareForPlanning).then(
            function successCallback(response) {
                if (prepareForPlanning) {
                    console.log(response);

                    um.allAssignments = [];
                    for(val of response.data)
                    {
                        var containsCus = false;
                        var customerIndex = -1;
                        for(cus of um.allAssignments) {
                            if (cus.Name == val.Customer.Name) {
                                containsCus = true;
                                customerIndex = um.allAssignments.indexOf(cus);
                            }
                        }

                        if (!containsCus) {
                            um.allAssignments.push(val.Customer);
                            customerIndex = um.allAssignments.indexOf(val.Customer);
                            um.allAssignments[customerIndex].Assignments = [];
                        }

                        var Customer = val.Customer;
                        val.Customer = {};
                        val.opened = false;
                        um.allAssignments[customerIndex].Assignments.push(val);
                    }

                    console.log(um.allAssignments);
                }
                else
                    um.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    um.getAllEmployeeAssignments = function () {
        assignmentManagement.getAssignment(um.selectedDate.getTime()).then(
            function successCallback(response) {
                um.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.addAssignment = function () {
        swal({
            title: "",
            text: "Opdracht word toegevoegd!",
            showConfirmButton: false
        });
        um.newAssignment.Date = um.newAssignment.Date.getTime();
        assignmentManagement.add(um.newAssignment).then(
            function successCallback(response) {
                swal({
                    title: "Gelukt",
                    text: "Opdracht is met success opgeslagen",
                    timer: 3500,
                    showConfirmButton: false,
                    type: "success"
                });
                um.newAssignment = {};
                um.getAllAssignments();
                $scope.showNewAssignmentCard = false;
                $scope.showMainView = true;
            },
            function errorCallback(response) {
                swal({
                    title: "Fout",
                    type: "error",
                    text: response.data
                });
            });
    };

    //=======================
    // Datepicker
    //=======================
    $scope.open = function ($event, opened) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope[opened] = true;
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

agroApp.controller('CustomerManagement', function ($window, $scope, customerManagement, tableService) {
    var um = this;

    um.customerDetails = {};
    um.allCustomers = [];

    um.addCustomer = function () {
        customerManagement.register(um.customerDetails)
        .then(function successCallback(response) {
            if (response.data != "succes") {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Klant is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/klanten/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    um.getAllCustomers = function () {
        customerManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                um.allCustomers = response.data;
                tableService.data = um.allCustomers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllArchivedCustomers = function () {
        customerManagement.getAllArchivedCustomers().then(
            function successCallback(response) {
                um.allCustomers = response.data;
                tableService.data = um.allCustomers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    um.applyChangesToCustomer = function () {
        customerManagement.applyChanges(um.customerDetails)
        .then(function successCallback(response) {
            if (response.data != true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Klant is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/klanten/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met de ontwikkelaar!", "error");
        });
    };

    um.archiveCustomer = function (customer) {
        swal({
            title: "Weet u zeker dat u " + customer.Name + " wilt archiveren?",
            text: "",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            customerManagement.archiveCustomer(customer.IdCustomer).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: customer.Name + " is gearchiveerd.", timer: 3000, showConfirmButton: false });
                        um.getAllCustomers();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: customer.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: customer.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: customer.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
    um.restoreCustomer = function (customer) {
        swal({
            title: "Weet u zeker dat u " + customer.Name + " wilt dearchiveren?",
            text: "Hierdoor zal deze klant geactiveerd worden. Het zal weer mogelijk zijn om deze klant te gebruiken.",
            type: "info", 
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            customerManagement.restoreCustomer(customer.IdCustomer).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: customer.Name + " is gedearchiveerd. De klant kan weer worden gebruikt!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = '/admin/klanten/overzicht'; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: customer.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: customer.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: customer.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };

})

agroApp.controller('AttachmentManagement', function ($window, $scope, attachmentManagement, tableService) {
    var um = this;

    um.attachmentDetails = {};
    um.allAttachment = [];

    um.addAttachment = function () {
        attachmentManagement.register(um.attachmentDetails)
        .then(function successCallback(response) {
            if (response.data != "succes") {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Hulpstuk is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/hulpstuk/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    um.getAllAttachments = function () {
        attachmentManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                um.allAttachments = response.data;
                tableService.data = um.allAttachments;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.getAllArchivedAttachments = function () {
        attachmentManagement.getAllArchivedAttachments().then(
            function successCallback(response) {
                um.allAttachments = response.data;
                tableService.data = um.allAttachments;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    um.applyChangesToAttachment = function () {
        attachmentManagement.applyChanges(this.attachmentDetails)
        .then(function successCallback(response) {
            if (response.data != true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Hulpstuk is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = '/admin/hulpstuk/overzicht'; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met de ontwikkelaar!", "error");
        });
    };
    um.archiveAttachment = function (attachment) {
        swal({
            title: "Weet u zeker dat u " + attachment.Name + " wilt archiveren?",
            text: "",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            attachmentManagement.archiveAttachment(attachment.IdAttachment).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: attachment.Name + " is gearchiveerd.", timer: 3000, showConfirmButton: false });
                        um.getAllAttachments();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: attachment.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: attachment.Name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: Attachment.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };

    um.restoreAttachment = function (attachment) {
        swal({
            title: "Weet u zeker dat u " + attachment.Name + " wilt dearchiveren?",
            text: "Hierdoor zal het hulpstuk geactiveerd worden. Het zal weer mogelijk zijn om deze te gebruiken.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            attachmentManagement.restoreAttachment(attachment.IdAttachment).then(
                function successCallback(response) {
                    if (response.data == true) {
                        swal({ title: "Gelukt!", type: "success", text: attachment.Name + " is gedearchiveerd. Het hulpstuk kan weer gebruikt worden!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = '/admin/hulpstuk/overzicht'; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: attachment.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: attachment.Name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: Attachment.Name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };

})

agroApp.controller('CargoManagement', function ($window, $scope, userManagement, machineManagement, tableService) {
    var um = this;


});

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}