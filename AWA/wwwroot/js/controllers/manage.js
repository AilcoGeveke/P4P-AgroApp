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
            showLoaderOnConfirm: true
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
            showLoaderOnConfirm: true
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
        });
    };
});

agroApp.factory("MachineService", function ($resource) {
    return $resource("/api/machines/:id", { id: "@_id" }, {
        update: {
            method: "PUT"
        }
    });
});

agroApp.factory("AttachmentService", function ($resource) {
    return $resource("/api/attachments/:id", { id: "@_id" }, {
        update: {
            method: "PUT"
        }
    });
});

agroApp.controller("MachineManagement", function (tableService, MachineService) {
    var ctrl = this;

    ctrl.types = ["Kraan", "Shovel", "Trekker", "Dumper", "Wagen", "Tank", "Ladewagen",
        "Strandreiniging", "Gladheid", "Auto", "Apparaat", "Trilplaat",
        "Meetapparatuur", "Aanhanger", "Hulpstuk", "Overige"];
    ctrl.statusTypes = ["Operationeel", "In onderhoud"];

    ctrl.machineDetails = {};
    ctrl.allMachines = {};

    ctrl.showEditCard = false;
    ctrl.showMainCard = true;

    ctrl.getAllMachines = function () {
        var machines = MachineService.query(function () {
            ctrl.allMachines = machines;
            tableService.data = machines;
        });
    }

    ctrl.addMachine = function () {
        MachineService.save(ctrl.machineDetails,
            function () {
                swal({ title: "Voertuig is aangemaakt", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.getAllMachines();
            },
            function () {
                swal("Fout", "Er is iets misgegaan. Probeer het later opnieuw.", "error");
            });
    }

    ctrl.putMachine = function () {
        MachineService.update({ id:ctrl.machineDetails.machineId }, ctrl.machineDetails,
            function () {
                swal({ title: "Voertuig is aangepast", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.getAllMachines();
            },
            function () {
                swal("Fout", "Er is iets misgegaan. Probeer het later opnieuw.", "error");
            });
    }
});

agroApp.controller("TimesheetController", function ($scope, $http, userManagement, customerManagement, assignmentManagement, MachineService, timesheetManagement, workTypeList) {
    var ctrl = this;

    ctrl.showMainCard = true;
    ctrl.showTaskOverview = true;
    ctrl.showNewTaskCard = false;

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
    ctrl.timesheet = {};

    ctrl.timesheetDetails = {};
    ctrl.timesheetDetails.workType = "Machinist";
    ctrl.timesheetDetails.startTime = moment().startOf("d").add(7, "h").toDate();
    ctrl.timesheetDetails.endTime = moment().startOf("h").toDate();

    ctrl.updateTime = function (calcTotal) {
        if (calcTotal) {
            if (ctrl.timesheetDetails.endTime < ctrl.timesheetDetails.startTime)
                ctrl.timesheetDetails.endTime = angular.copy(ctrl.timesheetDetails.startTime);

            var hours = ctrl.timesheetDetails.endTime.getHours() - ctrl.timesheetDetails.startTime.getHours();
            var minutes = ctrl.timesheetDetails.endTime.getMinutes() - ctrl.timesheetDetails.startTime.getMinutes();

            var dateNow = moment().startOf("d").toDate();
            dateNow.setHours(hours);
            dateNow.setMinutes(minutes);

            ctrl.timesheetDetails.totalTime = dateNow;
        }
        else {
            ctrl.timesheetDetails.startTime = moment().startOf("d").toDate();
            ctrl.timesheetDetails.endTime = moment().startOf("d").toDate();
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

    ctrl.addTimesheetRecord = function () {
        ctrl.timesheetDetails.startTime = fixTime(ctrl.timesheetDetails.startTime).getTime();
        ctrl.timesheetDetails.endTime = fixTime(ctrl.timesheetDetails.endTime).getTime();
        ctrl.timesheetDetails.totalTime = fixTime(ctrl.timesheetDetails.totalTime).getTime();
        console.log(ctrl.timesheetDetails);

        //if the list doesn't exist, create empty list
        if (ctrl.timesheet.records === undefined)
            ctrl.timesheet.records = [];

        //add task to list
        ctrl.timesheet.records.push(ctrl.timesheetDetails);

        //switch view
        ctrl.showTaskOverview = true;
        ctrl.showNewTaskCard = false;

        //reset timesheetDetails
        ctrl.timesheetDetails = {};
        ctrl.timesheetDetails.workType = "Machinist";
        ctrl.timesheetDetails.startTime = moment().startOf("d").add(7, "h").toDate();
        ctrl.timesheetDetails.endTime = moment().startOf("h").toDate();
        ctrl.updateTime(true);
    };

    ctrl.saveTimesheet = function () {
        swal({
            title: "",
            text: "Bezig met opslaan!",
            showConfirmButton: false
        });

        $http.patch("/api/assignment/").then(
            function successCallback(response) {
                if (response.data !== true) {
                    swal("", response.data, "error");
                }
                else {
                    swal({ title: "Taak is aangemaakt", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                    ctrl.showTaskOverview = true;
                    ctrl.showNewTaskCard = false;
                    ctrl.getAllTimesheets(ctrl.timesheet.timesheetId);

                    ctrl.timesheetDetails = {};
                    ctrl.timesheetDetails.workType = "Machinist";
                    ctrl.timesheetDetails.startTime = moment().startOf("d").add(7, "h").toDate();
                    ctrl.timesheetDetails.endTime = moment().startOf("h").toDate();
                    ctrl.updateTime(true);
                }
            }, function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan, neem contact op met een ontwikkelaar!", "error");
            });
    };

    ctrl.getAllMachines = function () {
        var machines = MachineService.query(function () {
            ctrl.allMachines = machines;
        });
    };

    ctrl.selectedDate = moment().startOf("day").valueOf();
    ctrl.selectedEndDate = moment().add(2, "days").startOf("day").valueOf();
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
                        if (response.data.hasOwnProperty(x)) {
                            var containsCus = false;
                            var customerIndex = -1;

                            for (var cus in ctrl.allAssignments) {
                                if (ctrl.allAssignments.hasOwnProperty(cus)) {
                                    if (ctrl.allAssignments[cus].name === response.data[x].customer.name) {
                                        containsCus = true;
                                        customerIndex = cus;
                                    }
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
                }
                else
                    ctrl.allAssignments = response.data;
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };
    ctrl.getAllUnverifiedAssignments = function () {
        $http.get("/api/assignment/getallunverified/" + moment().startOf("d").toDate().getTime()).then(
            function successCallback(response) {
                ctrl.allAssignments = [];
                for (var x in response.data) {
                    if (response.data.hasOwnProperty(x)) {
                        var containsCus = false;
                        var customerIndex = -1;

                        for (var cus in ctrl.allAssignments) {
                            if (ctrl.allAssignments.hasOwnProperty(cus)) {
                                if (ctrl.allAssignments[cus].name === response.data[x].customer.name) {
                                    containsCus = true;
                                    customerIndex = cus;
                                }
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
            },
            function errorCallback(response) {
                swal("Fout", "Er is iets misgegaan bij het ophalen van de lijst. Ververs de pagina en probeer het opnieuw.", "error");
            });
    };

    ctrl.getAllTimesheets = function () {
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
                swal({ title: "Klant is aangemaakt", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.customerDetails = {};
                ctrl.getAllCustomers();
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
                swal({ title: "Klant is aangepast", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.customerDetails = {};
                ctrl.getAllCustomers();
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
            showLoaderOnConfirm: true
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
            showLoaderOnConfirm: true
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

}),

agroApp.controller("AttachmentManagement", function ($window, $scope, AttachmentService, tableService) {
    var ctrl = this;

    ctrl.attachmentDetails = {};
    ctrl.allAttachments = [];

    ctrl.showEditCard = false;
    ctrl.showMainCard = true;

    ctrl.getAllAttachments = function () {
        var machines = AttachmentService.query(function () {
            ctrl.allAttachments = machines;
        });
    }

    ctrl.addAttachment = function () {
        AttachmentService.save(ctrl.attachmentDetails,
            function () {
                swal({ title: "Hulpstuk is aangemaakt", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.getAllAttachments();
            },
            function () {
                swal("Fout", "Er is iets misgegaan. Probeer het later opnieuw.", "error");
            });
    }

    ctrl.putAttachment = function () {
        AttachmentService.update({ id: ctrl.attachmentDetails.attachmentId }, ctrl.attachmentDetails,
            function () {
                swal({ title: "Hulpstuk is aangepast", text: "", timer: 3000, showConfirmButton: false, type: "success" });
                ctrl.showEditCard = false;
                ctrl.showMainCard = true;
                ctrl.getAllAttachments();
            },
            function () {
                swal("Fout", "Er is iets misgegaan. Probeer het later opnieuw.", "error");
            });
    }
}),

agroApp.directive("assignmentItem", function () {
    return {
        templateUrl: "template/assignmentitem.html"
    };
});

function fixTime(now) {
    return new Date(now.getTime() + (now.getTimezoneOffset() * -60000));
}

function scrollTo(div) {
    $("html, body").animate({
        scrollTop: $("#" + div).offset().top - 15
    }, 500);
};
