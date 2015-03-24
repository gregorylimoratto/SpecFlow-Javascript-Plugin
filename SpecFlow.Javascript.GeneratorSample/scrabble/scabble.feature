Feature: scabble
	Dans le but de marquer le plus de points,
	En tant que personne nul au scrabble,
	Je souhaite qu'on me donne la meilleur combinaison

@mytag
Scenario: Récupérer la meilleur combinaison de lettre avec un tirage défini
	Given Je dispose d'un chevalet avec les lettres "ABCD"
	And Le dictionnaire contient les mots 
	| mots  |
	| BA    |
	| SOJDF |
	| BAC   |
	When Je demande le mot le plus long
	Then Le resultat doit être 
	| mots  |
	| BAC |

Scenario: Récupérer les meilleurs combinaisons de lettre avec un tirage défini dans deux arbos differentes
	Given Je dispose d'un chevalet avec les lettres "ABCDD"
	And Le dictionnaire contient les mots 
	| mots  |
	| BA    |
	| SOJDF |
	| BAC   |
	| BDD   |
	When Je demande le mot le plus long
	Then Le resultat doit être 
	| mots  |
	| BAC |
	| BDD |


Scenario: Récupérer la meilleur combinaison de lettre avec un tirage défini et avec plus de lettres
	Given Je dispose d'un chevalet avec les lettres "TOCAABS"
	And Le dictionnaire contient les mots 
	| mots  |
	| TABASCO    |
	| CABOTAS |
	| ABACOST   |
	| APOZAER   |
	| ZIMBABUE   |
	| TUC |
	When Je demande le mot le plus long
	Then Le resultat doit être
	| mots  |
	| TABASCO |
	| CABOTAS |
	| ABACOST |

Scenario: Lire un dictionnaire stoqué dans un fichier
	Given Je dispose d'un fichier contenant le dictionnaire à l'url "/base/SpecFlow.Javascript.GeneratorSample/scrabble/ListeMots.txt"
	When Je lit le fichier
	Then J'ai stoqué "59232" mots

Scenario: Rechercher le mot le plus long dans un dictionnaire en fonction d'un chevalet
	Given Je dispose d'un fichier contenant le dictionnaire à l'url "/base/SpecFlow.Javascript.GeneratorSample/scrabble/ListeMots.txt"
	And Je dispose d'un chevalet avec les lettres "TOCAABS"
	When Je lit le fichier
	And Je demande le mot le plus long
	Then Le resultat doit être
	| mots  |
	| TABASCO |
	| CABOTAS |
	| ABACOST |


Scenario: Récupérer la meilleur combinaison de lettre avec un tirage défini mais une lettre joker
	Given Je dispose d'un chevalet avec les lettres "AB#D"
	And Le dictionnaire contient les mots 
	| mots  |
	| BA    |
	| SOJDF |
	| BAC   |
	When Je demande le mot le plus long
	Then Le resultat doit être 
	| mots  |
	| BAC |


	
Scenario: Récupérer la meilleur combinaison de lettre avec un tirage défini mais deux lettres joker
	Given Je dispose d'un chevalet avec les lettres "AB#D#CD"
	And Le dictionnaire contient les mots 
	| mots  |
	| BA    |
	| SOJDF |
	| BAC   |
	| BAGDAD   |
	When Je demande le mot le plus long
	Then Le resultat doit être 
	| mots  |
	| BAGDAD |


Scenario: Rechercher le mot le plus long dans un dictionnaire en fonction d'un chevalet avec des joker
	Given Je dispose d'un fichier contenant le dictionnaire à l'url "/base/SpecFlow.Javascript.GeneratorSample/scrabble/ListeMots.txt"
	And Je dispose d'un chevalet avec les lettres "TOCAABS##"
	When Je lit le fichier
	And Je demande le mot le plus long
	Then Le resultat doit être
	| mots    |
	| ABACOST |
	| ASSAUTS|
	|CABOTAS| 
	|CACABAS| 
	|CACABAT|
	|ENTUBEZ|
	|TABASCO|
	
