var agroApp = angular.module('AgroApp');

agroApp.controller('PlanningView', function ($scope) {
    
});

agroApp.factory('getUsers', function () {
    return function () {
        $http({
            method: 'GET',
            url: '/api/user/getallegebruikers',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            return data;
        })
    }
});

agroApp.factory('getCustomers', function () {
    return function () {
        $http({
            method: 'GET',
            url: '/api/werkbon/getklanten',
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            return data;
        })
    }
});