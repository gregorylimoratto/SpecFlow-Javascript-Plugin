﻿(function () {
	featureSteps('test: avec des exemple\\(s\\)')
	.given('je suis là', function () {
		/* Set test logic here */
	})
	.when('je vais là-bas', function () {
		/* Set test logic here */
	})
	.then('j\'y suis', function () {
		/* Set test logic here */
	})
	.given('je renseigne (.*)', function (p0/* String */) {
		console.log(p0);
	});
})();