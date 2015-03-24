(function(){
	feature('SpecFlowFeature1: In order to avoid silly mistakes\n'+
'As a math idiot\n'+
'I want to be told the sum of two numbers')
		.scenario('Add two numbers')
			.given ('I have entered 50 into the calculator')
			.and ('I have entered 70 into the calculator')
			.when ('I press add')
			.and ('I do truc')
			.then ('the result should be 120 on the screen')
})();