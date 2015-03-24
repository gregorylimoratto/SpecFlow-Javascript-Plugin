Feature: AngularApp
	Pour montrer l'utilisation de specflow avec du JS et le fwk angular

@ignore
Scenario: Faire un enchainement de service
	Given I enter "Test" into input
	When I press validate
	Then the result stored must be "Test"

	 
Scenario Outline: mseltjk
	Given I enter "<truc>" into input
	When I press validate
	Then the result stored must be "<truc>"
	 
	Examples: 
	| truc |
	| truc |
	| truca |
	| trucb |
	| trucc |


