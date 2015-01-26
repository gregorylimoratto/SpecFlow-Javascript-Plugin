(function () {
    featureSteps('AngularApp:')
        .before(function () {
            module('application');

            this.ctx = getContext();
            this.scope = this.ctx.$rootScope.$new();

            this.ctx.$controller('sampleController', { $scope: this.scope });
        })
	    .given('I enter "(.*)" into input', function (p0/* String */) {
	        this.scope.input = p0;
	        
	    })
	    .when('I press validate', function () {
	        this.scope.valider();
	    })
	    .then('the result stored must be "(.*)"', function (p0/* String */) {
	        var service = this.ctx.$injector.get('dependencyService');
	        expect(service.get()).toBe(p0);
	    });
})();

