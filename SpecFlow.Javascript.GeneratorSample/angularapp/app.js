(function () {
    var module = angular.module("application", []);

    module.controller('sampleController', ['$scope', 'testService', function ($scope, testService) {
        $scope.input = '';
        $scope.service = testService;
        $scope.valider = function () {
            testService.modifier($scope.input);
        };
    }]);

    module.factory('testService', ['dependencyService', function (dependencyService) {
        return {
            valeur: function () { return dependencyService.f(); },
            modifier: function (input) { return dependencyService.set(input); },
            recuperer: function () { return dependencyService.get(); }
        }
    }])

    module.factory('dependencyService', function () {
        var cst = null;
        return {
            f: function () {
                return "sample"
            },
            set: function (input) {
                cst = input;
            },
            get: function(){
                return cst;
            }
        }

    })
}());