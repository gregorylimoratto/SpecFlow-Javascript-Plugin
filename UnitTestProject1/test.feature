Feature: Example feature
  As a user of cucumber.js
  I want to have documentation on cucumber
  So that I can concentrate on building awesome applications

  Background: 
	Given I do truc
	Then Machin

  Scenario: Reading documentation
    Given I am on the Cucumber.js GitHub repository
	Given there are users:
| username | password | email               |
| everzet  | 123456   | everzet@knplabs.com |
| fabpot   | 22@222   | fabpot@symfony.com  |
    When I go to the README file
    Then I should see "Usage" as the page title

Scenario Outline: controlling order
  Given there are <start> cucumbers
  When I eat <eat> cucumbers
  Then I should have <left> cucumbers

Examples:
    | start | eat | left |
    |  12   |  5  |  7   |
	|  1    |  1  |  0   |
	|  10   |  5  |  5   |
	|  1    |  9  | -8   |