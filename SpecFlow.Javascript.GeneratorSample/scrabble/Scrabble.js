(function () {

    var scrabble = function () {
        this.dictionnaire = [];
        this.chevalet = null;
        this.hashmap = {};
    };

    scrabble.prototype.parseDictionnaire = function () {
        for (var i = 0; i < this.dictionnaire.length; i++) {
            var map = this.hashmap;
            var tabLettre = this.dictionnaire[i].split("").sort();

            for (var j = 0; j < tabLettre.length; j++) {
                var lettre = tabLettre[j];
                if (map[lettre] === undefined) {
                    map[lettre] = {};
                }
                map = map[lettre];
            }

            if (map.mots === undefined) { map.mots = []; }
            map.mots.push(this.dictionnaire[i]);
        }
    }

    scrabble.prototype.lireDictionnaire = function () {
        var fileContent = null;
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                fileContent = xmlhttp.responseText;
            }
        }.bind(this);
        xmlhttp.open("GET", this.urlDico, false);
        xmlhttp.send();

        this.dictionnaire = fileContent.split('\n');
    }

    scrabble.prototype.motPlusLong = function () {
        this.parseDictionnaire();

        var chaineSansDiese = this.chevalet.split('#');
        var nbDiese = chaineSansDiese.length - 1;
        this.chevalet = chaineSansDiese.join('');
        var tabLettre = this.chevalet.split("");

        tabLettre = tabLettre.sort();

        var map = this.hashmap;

        var result = this.findMots(tabLettre, map, nbDiese);

        var maxSize = 0;
        for (var i=0; i< result.length; i++)
        {
            if (result[i].length > maxSize) maxSize = result[i].length;
        }

        var toReturn = [];
        for (var i = 0; i < result.length; i++) {
            if (result[i].length === maxSize) toReturn.push(result[i]);
        }

        return toReturn;
    };


    scrabble.prototype.findMots = function (tabLettre, map, nbDiese) {
        var result = [];
        var mot = null;
        for (hash in map) {
            var tmp = hash;
            if (tmp != 'mots') {
                var index = tabLettre.indexOf(tmp);
                
                if (index > -1) {
                    var spl = tabLettre.splice(index, 1);
                    mot = this.findMots(tabLettre, map[spl[0]], nbDiese);
                    tabLettre.push(spl[0]);
                    addAll(mot, result);
                }
                if (nbDiese > 0) {
                    mot = this.findMots(tabLettre, map[tmp], nbDiese--);
                    addAll(mot, result);
                }
            }
        }

        if (mot == null && map.mots != undefined) {
            return map.mots;
        }
        return result;
       
    }

    function addAll(tabIn, result) {
        for (var i = 0; i < tabIn.length; i++) {
            if (result.indexOf(tabIn[i]) == -1)
                result.push(tabIn[i]);
        }
    }

    window.Scrabble = scrabble;
}());