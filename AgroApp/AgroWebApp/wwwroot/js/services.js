materialAdmin

    // =========================================================================
    // Service for handling user data
    // =========================================================================

    .service('userManagement', function ($http) {
        this.registerUser = function (user) {
            return $http.post('/api/user/register', user);
        }

        this.getAllUsers = function(){
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
    // Data Table
    // =========================================================================
    
    .service('tableService', [function(){
        this.data = {};
    }])


    // =========================================================================
    // Malihu Scroll - Custom Scroll bars
    // =========================================================================
    .service('scrollService', function() {
        var ss = {};
        ss.malihuScroll = function scrollBar(selector, theme, mousewheelaxis) {
            $(selector).mCustomScrollbar({
                theme: theme,
                scrollInertia: 100,
                axis:'yx',
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

    .service('growlService', function(){
        var gs = {};
        gs.growl = function(message, type) {
            $.growl({
                message: message
            },{
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
