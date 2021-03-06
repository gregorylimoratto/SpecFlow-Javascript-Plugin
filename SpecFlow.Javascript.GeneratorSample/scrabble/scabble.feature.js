﻿(function(){
	feature('scabble: Dans le but de marquer le plus de points,\n'+
'En tant que personne nul au scrabble,\n'+
'Je souhaite qu\'on me donne la meilleur combinaison')
		.scenario('Récupérer la meilleur combinaison de lettre avec un tirage défini')
			.given ('Je dispose d\'un chevalet avec les lettres "ABCD"')
			.and ('Le dictionnaire contient les mots', [{"mots":"BA"},{"mots":"SOJDF"},{"mots":"BAC"}])
			.when ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"BAC"}])
		.scenario('Récupérer les meilleurs combinaisons de lettre avec un tirage défini dans deux arbos differentes')
			.given ('Je dispose d\'un chevalet avec les lettres "ABCDD"')
			.and ('Le dictionnaire contient les mots', [{"mots":"BA"},{"mots":"SOJDF"},{"mots":"BAC"},{"mots":"BDD"}])
			.when ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"BAC"},{"mots":"BDD"}])
		.scenario('Récupérer la meilleur combinaison de lettre avec un tirage défini et avec plus de lettres')
			.given ('Je dispose d\'un chevalet avec les lettres "TOCAABS"')
			.and ('Le dictionnaire contient les mots', [{"mots":"TABASCO"},{"mots":"CABOTAS"},{"mots":"ABACOST"},{"mots":"APOZAER"},{"mots":"ZIMBABUE"},{"mots":"TUC"}])
			.when ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"TABASCO"},{"mots":"CABOTAS"},{"mots":"ABACOST"}])
		.scenario('Lire un dictionnaire stoqué dans un fichier')
			.given ('Je dispose d\'un fichier contenant le dictionnaire à l\'url "/base/SpecFlow.Javascript.GeneratorSample/scrabble/ListeMots.txt"')
			.when ('Je lit le fichier')
			.then ('J\'ai stoqué "59232" mots')
		.scenario('Rechercher le mot le plus long dans un dictionnaire en fonction d\'un chevalet')
			.given ('Je dispose d\'un fichier contenant le dictionnaire à l\'url "/base/SpecFlow.Javascript.GeneratorSample/scrabble/ListeMots.txt"')
			.and ('Je dispose d\'un chevalet avec les lettres "TOCAABS"')
			.when ('Je lit le fichier')
			.and ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"TABASCO"},{"mots":"CABOTAS"},{"mots":"ABACOST"}])
		.scenario('Récupérer la meilleur combinaison de lettre avec un tirage défini mais une lettre joker')
			.given ('Je dispose d\'un chevalet avec les lettres "AB#D"')
			.and ('Le dictionnaire contient les mots', [{"mots":"BA"},{"mots":"SOJDF"},{"mots":"BAC"}])
			.when ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"BAC"}])
		.scenario('Récupérer la meilleur combinaison de lettre avec un tirage défini mais deux lettres joker')
			.given ('Je dispose d\'un chevalet avec les lettres "AB#D#CD"')
			.and ('Le dictionnaire contient les mots', [{"mots":"BA"},{"mots":"SOJDF"},{"mots":"BAC"},{"mots":"BAGDAD"}])
			.when ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"BAGDAD"}])
		.scenario('Rechercher le mot le plus long dans un dictionnaire en fonction d\'un chevalet avec des joker')
			.given ('Je dispose d\'un fichier contenant le dictionnaire à l\'url "/base/SpecFlow.Javascript.GeneratorSample/scrabble/ListeMots.txt"')
			.and ('Je dispose d\'un chevalet avec les lettres "TOCAABS##"')
			.when ('Je lit le fichier')
			.and ('Je demande le mot le plus long')
			.then ('Le resultat doit être', [{"mots":"ABACOST"},{"mots":"ASSAUTS"},{"mots":"CABOTAS"},{"mots":"CACABAS"},{"mots":"CACABAT"},{"mots":"ENTUBEZ"},{"mots":"TABASCO"}])
})();