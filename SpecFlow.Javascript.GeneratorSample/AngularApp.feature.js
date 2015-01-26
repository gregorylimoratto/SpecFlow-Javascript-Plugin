(function(){
	feature('AngularApp: Pour montrer l\'utilisation de specflow avec du JS et le fwk angular')
		.scenario('Faire un enchainement de service')
			.given ('I enter "Test" into input')
			.when ('I press validate')
			.then ('the result stored must be "Test"')
})();