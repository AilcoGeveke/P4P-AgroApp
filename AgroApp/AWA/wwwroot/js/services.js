materialAdmin

    // =========================================================================
    // Service for handling user data
    // =========================================================================

    .service('userManagement', function ($http) {
        this.registerUser = function (user) {
            return $http.post('/api/user/register', user);
        }

        this.getUsers = function (archived) {
            return $http.get('/api/user/getall/' + archived);
        }

        this.applyChangesToUser = function (user) {
            return $http.post('/api/user/edit', user);
        }

        this.archiveUser = function (id) {
            return $http.get('/api/user/archive/' + id);
        }

        this.restoreUser = function (id) {
            return $http.get('/api/user/restore/' + id);
        }
    })

    // =========================================================================
    // Service for handling user data
    // =========================================================================

    .service('machineManagement', function ($http) {
        this.register = function (machine) {
            return $http.post('/api/machine/add', machine);
        }

        this.getAll = function () {
            return $http.get('/api/machine/getall');
        }

        this.getAllArchived = function () {
            return $http.get('/api/machine/getallarchived');
        }

        this.applyChanges = function (machine) {
            return $http.post('/api/machine/change', machine);
        }

        this.archive = function (id) {
            return $http.get('/api/machine/archive/' + id);
        }

        this.restore = function (id) {
            return $http.get('/api/machine/restore/' + id);
        }
    })



    // =========================================================================
    // Service for handling customer data
    // =========================================================================

    .service('customerManagement', function ($http) {
        this.register = function (customer) {
            return $http.post('/api/customer/add', customer);
        }

        this.getAll = function (archived) {
            return $http.get('/api/customer/getall/' + archived);
        }

        this.applyChanges = function (id) {
            return $http.post('/api/customer/edit', id);
        }

        this.setArchiveState = function (id, state) {
            return $http.get('/api/customer/setarchivestate/' + id + '/' + state);
        }
    })

        // =========================================================================
    // Service for handling user data
    // =========================================================================

    .service('attachmentManagement', function ($http) {
        this.register = function (attachment) {
            return $http.post('/api/attachment/add', attachment);
        }

        this.getAll = function () {
            return $http.get('/api/attachment/getall');
        }

        this.getAllArchivedAttachments = function () {
            return $http.get('/api/attachment/getallarchived');
        }

        this.restoreAttachment = function (id) {
            return $http.get('/api/attachment/restoreattachment/' + id)
        }

        this.archiveAttachment = function (id) {
            return $http.get('/api/attachment/archive/' + id);
        }

        this.applyChanges = function (attachment) {
            return $http.post('/api/attachment/change', attachment);
        }

        this.archive = function (id) {
            return $http.get('/api/attachment/archive/' + id);
        }

        this.restore = function (id) {
            return $http.get('/api/attachment/restore/' + id);
        }
    })
    // =========================================================================
    // Service for handling cargo data
    // =========================================================================

    .service('cargoManagement', function ($http) {
        this.add = function (machine) {
            return $http.post('/api/cargo/add', machine);
        }

        this.getAll = function () {
            return $http.get('/api/cargo/getall');
        }

        this.applyChanges = function (machine) {
            return $http.post('/api/machine/change', machine);
        }

        this.archive = function (id) {
            return $http.get('/api/machine/archive/' + id);
        }

        this.restore = function (id) {
            return $http.get('/api/machine/restore/' + id);
        }
    })

    // =========================================================================
    // Service for handling assignments data
    // =========================================================================

    .service('assignmentManagement', function ($http) {
        this.add = function (data) {
            return $http.post('/api/assignment/add', data);
        }

        this.get = function (id) {
            return $http.get('/api/assignment/get/' + id);
        }

        this.getAll = function (date, user) {
            var api = '/api/assignment/getall/';

            if (!angular.isUndefined(user) && user === "True")
                api += "userspecific/";

            api += date;

            return $http.get(api);
        }

        this.getAllPeriod = function (date1, date2, user) {
            var api = '/api/assignment/getallperiod/' + date1 + "/" + date2;

            if (!angular.isUndefined(user))
                api += "/" + user;

            return $http.get(api);
        }

        this.applyChanges = function (data) {
            return $http.post('/api/assignment/change', data);
        }
    })

    // =========================================================================
    // Service for handling worktype data
    // =========================================================================

    .service('workTypeList', function () {
        this.data = ['Man', 'Hovenier', 'Stratenmaker', 'Machinist', 'Onderhoud/Reparatie', 'Klaarzetten/omkoppelen/opbergen', 'Schoonmaken', 'Opruimen', 'Diverse werkzaamheden', 'Brandstof rondbrengen',
        'Compostering', 'Recycling', 'Terreinbeheer', 'Weegbonnen administratie', 'Groenonderhoud', 'Reistijd', 'Calculatie', 'Klantcontact/werving/service', 'Uitvoering', 'Werkvoorbereiding', 'Kantoor algemeen', 'Administratie', 'Financi\xEBle administratie'];
    })

        // =========================================================================
    // Service for handling timesheet data
    // =========================================================================

    .service('timesheetManagement', function ($http) {
        this.add = function (data) {
            var copy = angular.copy(data);

            copy.startTime = data.startTime;
            copy.endTime = data.endTime;
            copy.totalTime = data.totalTime;

            return $http.post('/api/timesheet/add', copy);
        }

        this.getAll = function (id) {

            return $http.get('/api/timesheet/getall/' + id);
        }
    })


    // =========================================================================
    // Data Table
    // =========================================================================

    .service('tableService', [function () {
        this.data = [];
    }])


    // =========================================================================
    // Malihu Scroll - Custom Scroll bars
    // =========================================================================
    .service('scrollService', function () {
        var ss = {};
        ss.malihuScroll = function scrollBar(selector, theme, mousewheelaxis) {
            $(selector).mCustomScrollbar({
                theme: theme,
                scrollInertia: 100,
                axis: 'yx',
                mouseWheel: {
                    enable: true,
                    axis: mousewheelaxis,
                    preventDefault: true
                }
            });
        }

        return ss;
    })


    //==============================================
    // BOOTSTRAP GROWL
    //==============================================

    .service('growlService', function () {
        var gs = {};
        gs.growl = function (message, type) {
            $.growl({
                message: message
            }, {
                type: type,
                allow_dismiss: false,
                label: 'Cancel',
                className: 'btn-xs btn-inverse',
                placement: {
                    from: 'top',
                    align: 'right'
                },
                delay: 2500,
                animate: {
                    enter: 'animated bounceIn',
                    exit: 'animated bounceOut'
                },
                offset: {
                    x: 20,
                    y: 85
                }
            });
        }

        return gs;
    })
