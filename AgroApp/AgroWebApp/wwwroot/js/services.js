materialAdmin

    // =========================================================================
    // Service for handling user data
    // =========================================================================

    .service('userManagement', function ($http) {
        this.registerUser = function (user) {
            return $http.post('/api/user/register', user);
        }

        this.getAllUsers = function () {
            return $http.get('/api/user/getall');
        }

        this.getAllArchivedUsers = function () {
            return $http.get('/api/user/getallarchived');
        }

        this.applyChangesToUser = function (user) {
            return $http.post('/api/user/wijzigen', user);
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
    // Service for handling user data
    // =========================================================================

    .service('customerManagement', function ($http) {
        this.register = function (customer) {
            return $http.post('/api/customer/add', customer);
        }

        this.getAll = function () {
            return $http.get('/api/customer/getall');
        }

        this.getAllArchived = function () {
            return $http.get('/api/customer/getallarchived');
        }

        this.applyChanges = function (customer) {
            return $http.post('/api/customer/change', customer);
        }

        this.archive = function (id) {
            return $http.get('/api/customer/archive/' + id);
        }

        this.restore = function (id) {
            return $http.get('/api/customer/restore/' + id);
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

        this.getAll = function (date, user) {
            var api = '/api/assignment/getall/' + date;

            console.log(user);
            if (!angular.isUndefined(user))
                api += "/" + user;

            return $http.get(api);
        }

        this.getAllPeriod = function (date1, date2, user, includeEmployees) {
            var api = '/api/assignment/getallperiod/' + date1 + "/" + date2;

            if (!angular.isUndefined(user))
                api += "/" + user;
            if (!angular.isUndefined(includeEmployees))
                api += "/" + includeEmployees;

            return $http.get(api);
        }

        this.applyChanges = function (data) {
            return $http.post('/api/assignment/change', data);
        }
    })

    // =========================================================================
    // Data Table
    // =========================================================================

    .service('tableService', [function () {
        this.data = {};
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
