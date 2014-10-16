Feature: Merge
	Merging users from one repository to my local database.


Background:
    Given a global administrator named "Greg"
	Given there are users:
| username | password | email               |
| everzet  | 123456   | everzet@knplabs.com |
| fabpot   | 22@222   | fabpot@symfony.com  |
    And a blog named "Greg's anti-tax rants"
    And a customer named "Wilson"
    And a blog named "Expensive Therapy" owned by "Wilson"

@Author:John.Smith
@Jira:SMG-1375
Scenario: Merge one user
	Given database is empty
	And database does not have a user with Id 1 
    When Merge User with Id 1  
    Then database has user with Id 1  
	  
Scenario Outline: controlling order
  Given there are <start> cucumbers
  When I eat <eat> cucumbers
  Then I should have <left> cucumbers

Examples:
    | start | eat | left |
    |  12   |  5  |  7   |

Scenario: Reuse given
Given database is empty
When I eat <eat> cucumbers
Then I should have <left> cucumbersEEEEE

Examples: 
	|eat |left|
	|1	|2|


Scenario Outline: Simple Test
    When I am on the "<property>" 
    Then I should see "Welcome"

  @LA
  Examples:
    | property | user       |
    | LATimes  | TribTester |

  @CT
  Examples:
    | property       | user       |
    | ChicagoTribune | TribTester |

  @SS
  Examples:
    | property    | user       |
    | SunSentinel | TribTester |

  @OS
  Examples:
    | property        | user       |
    | OrlandoSentinel | TribTester |