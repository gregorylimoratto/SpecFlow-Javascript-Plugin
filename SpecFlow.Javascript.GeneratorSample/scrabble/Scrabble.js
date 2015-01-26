(function(){

	var scrabble = function (){
		this.dictionnaire =[];
		this.chevalet = null;
		this.hashmap = {};
	};
	
	scrabble.prototype.parseDictionnaire = function(){
		for (var i=0; i< this.dictionnaire.length; i++){
			var map = this.hashmap;
			var tabLettre = this.dictionnaire[i].split("").sort();
			
			for(var j=0; j< tabLettre.length; j++) {
				var lettre = tabLettre[j];
				if (map[lettre] === undefined){
					map[lettre] = {};
					}
				map = map[lettre];
			}
			
			if (map.mots === undefined){ map.mots = []; }
			map.mots.push(this.dictionnaire[i]);
		}
	}
	
	scrabble.prototype.lireDictionnaire = function(){
	var fileContent = null;
			var xmlhttp = new XMLHttpRequest();
			xmlhttp.onreadystatechange = function()
			{
				if (xmlhttp.readyState==4 && xmlhttp.status==200)
				{
					fileContent = xmlhttp.responseText;
				}
			}.bind(this);
			xmlhttp.open("GET", this.urlDico, false);
			xmlhttp.send();
			
			this.dictionnaire = fileContent.split('\n');
	}
	
	scrabble.prototype.motPlusLong = function(){
		this.parseDictionnaire();
		
		var chaineSansDiese = this.chevalet.split('#');
		var nbDiese = chaineSansDiese.length - 1;
		this.chevalet = chaineSansDiese.join('');
		var tabLettre = this.chevalet.split("");
		
		tabLettre = tabLettre.sort();
		var map = this.hashmap;
		return this.findMots(tabLettre, map, nbDiese);
		
	};
	
	scrabble.prototype.findMots = function (tabLettre, map, nbDiese) {
	    if (tabLettre.length > 0 || nbDiese > 0) {
	        var motSansDiese = null;

	        if (tabLettre.length > 0 && map[tabLettre[0]] !== undefined) {
	            motSansDiese = this.findMots(tabLettre.slice(1), map[tabLettre[0]], nbDiese);
	        } 
	        for(var i=0; i<nbDiese; i++) {
	            for (hash in map) {
	                console.log(nbDiese);
	                console.log(map[hash].mots);
	                var newMot = this.findMots(tabLettre, map[hash], nbDiese - 1);
	                if (newMot && (motSansDiese == null || newMot[0].length > motSansDiese[0].length)) motSansDiese = newMot;
	            }
	        }

	        if (motSansDiese)
	            return motSansDiese;
			
		}
		return map.mots;
	}
	
	window.Scrabble = scrabble;
}());