"use strict";
var agroApp = angular.module("materialAdmin");

agroApp.controller("UserManagement", function ($window, $scope, userManagement, tableService) {
    var ctrl = this;

    ctrl.userDetails = { Role: 0 };
    ctrl.allUsers = [];
    ctrl.availableRoles = [{ id: 0, name: "Gebruiker" }, { id: 1, name: "Admin" }];

    ctrl.showEditCard = false;
    ctrl.showMainCard = true;

    ctrl.registerUser = function () {
        userManagement.registerUser(ctrl.userDetails)
        .then(function successCallback(response) {
            if (response.data === true) {
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.userDetails = {};
                ctrl.getUsers(false);
                swal({ title: "Gebruiker is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
            }
            else {
                swal("Fout", response.data, "error");
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    ctrl.getUsers = function (archived) {
        userManagement.getUsers(archived).then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allUsers = response.data;
                tableService.data = ctrl.allUsers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.applyChangesToUser = function () {
        userManagement.applyChangesToUser(this.userDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.userDetails = {};
                ctrl.getUsers(false);
                swal({ title: "Gebruiker is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met de ontwikkelaar!", "error");
        });
    };
    ctrl.archiveUser = function (user) {
        swal({
            title: "Weet u zeker dat u " + user.name + " wilt archiveren?",
            text: "Hierdoor zal het account gedeactiveerd worden. Het zal niet meer mogelijk zijn voor de gebruiker om in te loggen.",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            userManagement.archiveUser(user.userId).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: user.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 3000, showConfirmButton: false });
                        ctrl.getUsers();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: user.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: user.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: user.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
    ctrl.restoreUser = function (user) {
        swal({
            title: "Weet u zeker dat u " + user.name + " wilt dearchiveren?",
            text: "Hierdoor zal het account geactiveerd worden. Het zal weer mogelijk zijn voor de gebruiker om in te loggen.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            userManagement.restoreUser(user.userId).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: user.name + " is gedearchiveerd. De gebruiker kan weer inloggen!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = "/admin/gebruikers/overzicht"; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: user.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: user.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: user.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
});

agroApp.controller("MachineManagement", function ($window, $scope, machineManagement, tableService) {
    var ctrl = this;

    ctrl.types = ["Kraan", "Shovel", "Trekker", "Dumper", "Wagen", "Tank", "Ladewagen",
        "Strandreiniging", "Gladheid", "Auto", "Apparaat", "Trilplaat",
        "Meetapparatuur", "Aanhanger", "Hulpstuk", "Overige"];

    ctrl.machineDetails = {};
    ctrl.allMachines = {};

    ctrl.registerMachine = function () {
        machineManagement.register(ctrl.machineDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Machine is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = "/admin/machines/overzicht"; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    ctrl.getAllMachines = function () {
        machineManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allMachines = response.data;
                tableService.data = ctrl.allMachines;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.getAllArchivedMachines = function () {
        machineManagement.getAllArchived().then(
            function successCallback(response) {
                ctrl.allMachines = response.data;
                tableService.data = ctrl.allMachines;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.applyChangesToMachine = function () {
        console.log(ctrl.machineDetails);
        machineManagement.applyChanges(ctrl.machineDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Machine is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = "/admin/machines/overzicht"; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    ctrl.archiveMachine = function (machine) {
        swal({
            title: "Weet u zeker dat u " + machine.name + " wilt archiveren?",
            text: "Hierdoor zal deze machine gedeactiveerd worden. Het zal niet meer mogelijk zijn om deze machine te gebruiken.",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            machineManagement.archive(machine.IdMachine).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: machine.name + " is gearchiveerd. De Machine kan niet meer worden gebruikt!", timer: 3000, showConfirmButton: false });
                        ctrl.getAllMachines();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: machine.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: machine.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: machine.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
    ctrl.restoreMachine = function (machine) {
        swal({
            title: "Weet u zeker dat u " + machine.name + " wilt dearchiveren?",
            text: "Hierdoor zal de machine geactiveerd worden. Het zal weer mogelijk worden om de machine te gebruiken.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            machineManagement.restore(machine.IdMachine).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: machine.name + " is gedearchiveerd. De machine kan weer gebruikt worden!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = "/admin/machines/overzicht"; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: machine.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: machine.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
        });
    };
});

agroApp.controller("TimesheetController", function ($scope, userManagement, customerManagement, assignmentManagement, machineManagement, timesheetManagement, workTypeList) {
    var ctrl = this;

    ctrl.showMainCard = true;
    ctrl.showTaskOverview = true;
    ctrl.showNewTaskCard = false;

    ctrl.allTimesheets = [];
    ctrl.allUsers = [];
    ctrl.allCustomers = [];
    ctrl.allAssignments = [];
    ctrl.allMachines = [];
    ctrl.allAttachments = [];
    ctrl.allWorkTypes = workTypeList.data;
    ctrl.selectedCoworkers = [];
    ctrl.selectedMachines = [];
    ctrl.selectedAttachments = [];
    ctrl.selectedAssignment = {};
    ctrl.employeeAssignment = {};

    ctrl.timesheetDetails = {};
    ctrl.timesheetDetails.workType = "Machinist";
    ctrl.timesheetDetails.startTime = moment().startOf('d').add(7, 'h').toDate();
    ctrl.timesheetDetails.endTime = moment().startOf('h').toDate();

    ctrl.updateTime = function (calcTotal)
    {
        if(calcTotal)
        {
            if (ctrl.timesheetDetails.endTime < ctrl.timesheetDetails.startTime)
                ctrl.timesheetDetails.endTime = angular.copy(ctrl.timesheetDetails.startTime);

            var hours = ctrl.timesheetDetails.endTime - ctrl.timesheetDetails.startTime;
            ctrl.timesheetDetails.totalTime = new Date(hours);
        }
        else
        {
            ctrl.timesheetDetails.startTime = moment().startOf('d').toDate();
            ctrl.timesheetDetails.endTime = moment().startOf('d').toDate();
        }
    }
    ctrl.updateTime(true);

    ctrl.getAllTimesheets = function (id) {
        timesheetManagement.getAll(id).then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allTimesheets = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.addTimesheet = function () {
        swal({
            title: "",
            text: "Taak word toegevoegd!",
            showConfirmButton: false
        });
        console.log(ctrl.timesheetDetails);
        ctrl.timesheetDetails.employeeAssignmentId = ctrl.employeeAssignment.employeeAssignmentId;
        timesheetManagement.add(ctrl.timesheetDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Taak is aangemaakt", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showTaskOverview = true;
                ctrl.showNewTaskCard = false;
                ctrl.getAllTimesheets(ctrl.employeeAssignment.employeeAssignmentId);

                ctrl.timesheetDetails = {};
                ctrl.timesheetDetails.workType = "Machinist";
                ctrl.timesheetDetails.startTime = moment().startOf('d').add(7, 'h').toDate();
                ctrl.timesheetDetails.endTime = moment().startOf('h').toDate();
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });


    };

    ctrl.getAllMachines = function () {
        machineManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allMachines = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.selectedDate = moment().startOf('day').valueOf();
    ctrl.selectedEndDate = moment().add(2, 'days').startOf('day').valueOf();
    ctrl.newAssignment = {};

    ctrl.getUsers = function () {
        userManagement.getUsers(false).then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allUsers = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getCustomers = function () {
        customerManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allCustomers = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getAssignment = function (id) {
        assignmentManagement.get(id).then(
            function successCallback(response) {
                ctrl.selectedAssignment = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getAllAssignments = function (userSpecific) {
        assignmentManagement.getAll(ctrl.selectedDate.valueOf(), userSpecific).then(
            function successCallback(response) {
                ctrl.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getAllAssignmentsPeriod = function (prepareForPlanning, user) {
        assignmentManagement.getAllPeriod(ctrl.selectedDate.valueOf(), ctrl.selectedEndDate.valueOf(), user).then(
            function successCallback(response) {
                if (prepareForPlanning) {
                    ctrl.allAssignments = [];
                    for (var x in response.data) {
                        var containsCus = false;
                        var customerIndex = -1;

                        for (var cus in ctrl.allAssignments) {
                            if (ctrl.allAssignments[cus].name === response.data[x].customer.name) {
                                containsCus = true;
                                customerIndex = cus;
                            }
                        }

                        if (!containsCus) {
                            ctrl.allAssignments.push(response.data[x].customer);
                            customerIndex = ctrl.allAssignments.indexOf(response.data[x].customer);
                            ctrl.allAssignments[customerIndex].assignments = [];
                        }

                        var customer = response.data[x].customer;
                        delete response.data[x].customer;
                        response.data[x].opened = false;
                        ctrl.allAssignments[customerIndex].assignments.push(response.data[x]);
                    }
                }
                else
                    ctrl.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.getAllEmployeeAssignments = function () {
        assignmentManagement.getAssignment(ctrl.selectedDate.valueOf()).then(
            function successCallback(response) {
                ctrl.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.addAssignment = function () {
        swal({
            title: "",
            text: "Opdracht word toegevoegd!",
            showConfirmButton: false
        });

        var data = angular.copy(ctrl.newAssignment);

        data.date = data.date.getTime();

        //only send the customer id
        data.customerId = data.customer.customerId;
        delete data.customer;

        assignmentManagement.add(data).then(
            function successCallback(response) {
                swal({
                    title: "Gelukt",
                    text: "Opdracht is met success opgeslagen",
                    timer: 3500,
                    showConfirmButton: false,
                    type: "success"
                });
                ctrl.newAssignment = {};
                ctrl.getAllAssignments();
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
            },
            function errorCallback(response) {
                swal({
                    title: "Fout",
                    type: "error",
                    text: "Er is iets misgegaan met het opslaan van de opdracht, probeer het opnieuw of neem contact op met een administrator"
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

agroApp.controller("ManageUser2", function ($scope, $http, $rootScope, $mdDialog) {
    $scope.rollen = ["Gebruiker", "Admin"];

    $scope.selectedUsers = [];
    $scope.users = [];
    $scope.getAllUserData = function () {
        $http.get("/api/user/getall").success(function (data) {
            $scope.users = data;
        }).error(function (data) {
        })
    }

    $scope.ArchiefGebruikers = [];
    $scope.getArchiefUserData = function () {
        $rootScope.showLoading++;
        $http({
            method: "GET",
            url: "/api/account/getarchiefusers",
            params: "limit=10, sort_by=created:desc",
            headers: { 'Authorization': "Token token=xxxxYYYYZzzz" }
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
              .title("Gebruiker dearchiveren")
              .textContent("Als u doorgaat zal deze gebruiker gedearchiveerd worden!")
              .targetEvent(ev)
              .ok("Dearchiveren")
              .cancel("Annuleer");
        $mdDialog.show(confirm).then(function () {
            ReAddUser();
        }, function () {
            $rootScope.showLoading--;
        });
    };

    var EditUser = function () {
        $rootScope.showLoading++;

        $http({
            method: "GET",
            url: "/api/user/wijzigen/" + $scope.userDetails.id + "/" + $scope.userDetails.naam + "/" + $scope.userDetails.email + "/" + $scope.userDetails.rol,
            params: "limit=10, sort_by=created:desc",
            headers: { 'Authorization': "Token token=xxxxYYYYZzzz" }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data === true)
                $rootScope.changeView("admin/gebruikerbeheer");
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
            method: "GET",
            url: "/api/user/archiveren/" + $scope.userDetails.id,
            params: "limit=10, sort_by=created:desc",
            headers: { 'Authorization': "Token token=xxxxYYYYZzzz" }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data === true)
                $rootScope.changeView("admin/gebruikerbeheer");
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
            method: "GET",
            url: "/api/user/gebruikerbeheer/terughalen/" + $scope.gebruikerid,
            params: "limit=10, sort_by=created:desc",
            headers: { 'Authorization': "Token token=xxxxYYYYZzzz" }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data === true)
                $rootScope.changeView("admin/gebruikerbeheer");
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

agroApp.controller("CustomerManagement", function ($window, $scope, customerManagement, tableService) {
    var ctrl = this;

    ctrl.customerDetails = {};
    ctrl.allCustomers = [];

    ctrl.showEditCard = false;
    ctrl.showMainCard = true;

    ctrl.addCustomer = function () {
        customerManagement.register(ctrl.customerDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Klant is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.customerDetails = {};
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    ctrl.getAllCustomers = function () {
        customerManagement.getAll(false).then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allCustomers = response.data;
                tableService.data = ctrl.allCustomers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getAllArchivedCustomers = function () {
        customerManagement.getall(true).then(
            function successCallback(response) {
                ctrl.allCustomers = response.data;
                tableService.data = ctrl.allCustomers;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.applyChangesToCustomer = function () {
        customerManagement.applyChanges(ctrl.customerDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Klant is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.customerDetails = {};
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met de ontwikkelaar!", "error");
        });
    };

    ctrl.archiveCustomer = function (customer) {
        swal({
            title: "Weet u zeker dat u " + customer.name + " wilt archiveren?",
            text: "",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            customerManagement.setArchiveState(customer.IdCustomer, true).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: customer.name + " is gearchiveerd.", timer: 3000, showConfirmButton: false });
                        ctrl.getAllCustomers();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: customer.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: customer.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: customer.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };
    ctrl.restoreCustomer = function (customer) {
        swal({
            title: "Weet u zeker dat u " + customer.name + " wilt dearchiveren?",
            text: "Hierdoor zal deze klant geactiveerd worden. Het zal weer mogelijk zijn om deze klant te gebruiken.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            customerManagement.setArchiveState(customer.IdCustomer, false).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: customer.name + " is gedearchiveerd. De klant kan weer worden gebruikt!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = "/admin/klanten/overzicht"; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: customer.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: customer.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: customer.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };

})

agroApp.controller("AttachmentManagement", function ($window, $scope, attachmentManagement, tableService) {
    var ctrl = this;

    ctrl.attachmentDetails = {};
    ctrl.allAttachment = [];

    ctrl.addAttachment = function () {
        attachmentManagement.register(ctrl.attachmentDetails)
        .then(function successCallback(response) {
            if (response.data !== "succes") {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Hulpstuk is aangemaakt", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = "/admin/hulpstuk/overzicht"; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
        });
    };
    ctrl.getAllAttachments = function () {
        attachmentManagement.getAll().then(
            function successCallback(response) {
                console.log(response.data);
                ctrl.allAttachments = response.data;
                tableService.data = ctrl.allAttachments;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getAllArchivedAttachments = function () {
        attachmentManagement.getAllArchivedAttachments().then(
            function successCallback(response) {
                ctrl.allAttachments = response.data;
                tableService.data = ctrl.allAttachments;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.applyChangesToAttachment = function () {
        attachmentManagement.applyChanges(this.attachmentDetails)
        .then(function successCallback(response) {
            if (response.data !== true) {
                swal("", response.data, "error");
            }
            else {
                swal({ title: "Hulpstuk is aangepast", text: "U wordt doorverwezen", timer: 3000, showConfirmButton: false, type: "success" });
                setTimeout(function () { $window.location.href = "/admin/hulpstuk/overzicht"; }, 3500);
            }
        }, function errorCallback(response) {
            swal("Fout", "Er is iets misgegaan, neem contact op met de ontwikkelaar!", "error");
        });
    };
    ctrl.archiveAttachment = function (attachment) {
        swal({
            title: "Weet u zeker dat u " + attachment.name + " wilt archiveren?",
            text: "",
            type: "warning",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            attachmentManagement.archiveAttachment(attachment.IdAttachment).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: attachment.name + " is gearchiveerd.", timer: 3000, showConfirmButton: false });
                        ctrl.getAllAttachments();
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: attachment.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: attachment.name + " is niet gearchiveerd. Er is iets misgegaan!", timer: 4000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: Attachment.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };

    ctrl.restoreAttachment = function (attachment) {
        swal({
            title: "Weet u zeker dat u " + attachment.name + " wilt dearchiveren?",
            text: "Hierdoor zal het hulpstuk geactiveerd worden. Het zal weer mogelijk zijn om deze te gebruiken.",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
        }, function () {
            attachmentManagement.restoreAttachment(attachment.IdAttachment).then(
                function successCallback(response) {
                    if (response.data === true) {
                        swal({ title: "Gelukt!", type: "success", text: attachment.name + " is gedearchiveerd. Het hulpstuk kan weer gebruikt worden!", timer: 3000, showConfirmButton: false });
                        setTimeout(function () { $window.location.href = "/admin/hulpstuk/overzicht"; }, 3500);
                    }
                    else
                        swal({ title: "Fout!", type: "error", text: attachment.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                }, function errorCallback(response) {
                    swal({ title: "Fout!", type: "error", text: attachment.name + " is niet gedearchiveerd. Er is iets misgegaan!", timer: 3000, showConfirmButton: false });
                });
            //setTimeout(function () {
            //    swal({ title: "Gelukt!", type: "success", text: Attachment.name + " is gearchiveerd. De gebruiker kan niet meer inloggen!", timer: 4000, showConfirmButton: false });
            //}, 3000);
        });
    };

})

agroApp.controller("CargoManagement", function ($window, $scope, userManagement, machineManagement, tableService) {
    var ctrl = this;


});

agroApp.directive('assignmentItem', function () {
    return {
        templateUrl: 'template/assignmentitem.html'
    };
});