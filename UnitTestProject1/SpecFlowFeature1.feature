Feature: test
avec des exemple(s)

Scenario: faire un test
Given je suis là
When je vais là-bas
Then j'y suis

Scenario Outline: faire un gros test
Given je renseigne <A>
When je renseigne <B'()éz"poià"'=)éç'->
Then je renseigne <C>

Examples: 
| A | B'()éz"poià"'=)éç'- | C |
| a | b | c |