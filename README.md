SpecFlow-Javascript-Plugin
==========================

Plugin SpecFlow permettant de générer des tests Javascript à partir d'une syntaxe Gherkin

## Dependences

Ce plugin génère des tests basés sur le plugin Karma [karma-jasmine-cucumber](https://github.com/DealerDotCom/karma-jasmine-cucumber).
Le but étant de mutualiser l'écriture des TU et BDD javascript avec jasmine. (la syntaxe cucumber étant assez différente).
Aucun problème d'intégration avec le framework angularjs. 

exemple :
configuration.service.js
```
(function () {
    'use strict';

    var mainModule = angular.module('app', []);

    /** Provider angular permettant de stocker la configuration au lancement de l'application */
    mainModule.provider("configuration", function () {
        var parameters = null;

        function initialise() {
            parameters = {};
        }

        this.setConfiguration = function (params) {
            initialise();
            angular.extend(parameters, params);
        };
        
        function configuration() {

            /**
             * fourni la configuration de l'application
             * (set au démarrage de l'appli)
             *
             * @returns {Object} parameters - Parametres de l'application
             * @returns {string} parameters.baseUrl - Url de base de l'application
             */
            this.getConfiguration = function () {
                return parameters;
            };
        }
        
        this.$get = function () {
            return new configuration();
        };
    });
})();
```

generatedSpec.js
```
feature('configurationService: init')
    .scenario('should init config with specific configuration')
        .given('I init "propA" to "valueA" for configuration')
        .when('I apply configuration')
        .then('I should get "valueA" in configuration for "propA"')
    .scenario('should init config with specific configuration')
        .given('I init "propB" to "valueB" for configuration')
        .when('I apply configuration')
        .then('I should get "valueB" in configuration for "propB"');
```

feature.js
```
(function () {
    var injector;
    var confProvider;
    var config = {};
    featureSteps('configurationService: init')
        .before(module("app", function (configurationProvider, $provide) {
            confProvider = configurationProvider;
        }))
        .before(inject(function ($injector) {
            injector = $injector;
        }))
        .given('I init "(.*)" to "(.*)" for configuration', function (key, value) {
            config[key] = value;  
        })
        .when('I apply configuration', function () {
            confProvider.setConfiguration(config);
        })
        .then('I should get "(.*)" in configuration for "(.*)"', function (value, key) {
            var confService = injector.get("configuration");
            expect(confService.getConfiguration()[key]).toBe(value);
        });
}())

```

## Utilisation
