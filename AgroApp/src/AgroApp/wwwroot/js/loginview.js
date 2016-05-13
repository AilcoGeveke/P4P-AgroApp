﻿var agroApp = angular.module('AgroApp');

agroApp.controller('loginController', function ($scope, $location, $http) {
    $scope.userDetails = {};

    $scope.showloading = false;
    $scope.showError = false;

    $scope.goLogin = function () {
        $scope.showloading = true;

        $http({
            method: 'GET',
            url: '/api/login/' +  $scope.userDetails.email + '/' + $scope.userDetails.password,
            params: 'limit=10, sort_by=created:desc',
            headers: { 'Authorization': 'Token token=xxxxYYYYZzzz' }
        }).success(function (data) {
            // With the data succesfully returned, call our callback
            if (data == "true")
                window.location.href = '/admin/main/' + $scope.userDetails.email;
            else {
                $scope.showloading = false;
                $scope.showError = true;
                $scope.errorMessage = "De opgegeven combinatie is incorrect";
            }
        }).error(function () {
            $scope.showloading = false;
            $scope.showError = true;
            $scope.errorMessage = "Er is iets misgegaan! Probeer het opnieuw of neem contact op met een administrator";
        });
    };
})