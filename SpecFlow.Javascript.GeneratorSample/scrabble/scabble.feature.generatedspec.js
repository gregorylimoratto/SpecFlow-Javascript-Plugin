(function(){
	featureSteps('scabble: Dans le but de marquer le plus de points,\n'+
'En tant que personne nul au scrabble,\n'+
'Je souhaite qu\'on me donne la meilleur combinaison')
    
	.given('Je dispose d\'un chevalet avec les lettres "(.*)"', function(p0/* String */) {
	    this.scrabble.chevalet = p0;
	})
	.given('Le dictionnaire contient les mots', function (table /* json */) {
	    var mots = [];
	    for (var i = 0; i < table.length; i++) {
	        mots.push(table[i].mots);
	    }
	    this.scrabble.dictionnaire = mots;
	})
	.given('Je dispose d\'un fichier contenant le dictionnaire à l\'url "(.*)"', function (p0/* String */) {
	    this.scrabble.urlDico = p0;
	})
	.when('Je demande le mot le plus long', function() {
	    this.motLePlusLong = this.scrabble.motPlusLong();
	})
	.when('Je lit le fichier', function() {
	    this.scrabble.lireDictionnaire();
	})
	.then('J\'ai stoqué "(.*)" mots', function(p0/* Int32 */) {
	    expect(this.scrabble.dictionnaire.length).toBe(parseInt(p0));
	})
    .then('Le resultat doit être', function (table /* json */) {
        var mots = [];
        for (var i = 0; i < table.length; i++) {
            mots.push(table[i].mots);
        }
        expect(this.motLePlusLong.sort()).toEqual(mots.sort());
    });
})();