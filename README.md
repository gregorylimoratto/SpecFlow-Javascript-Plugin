SpecFlow-Javascript-Plugin
==========================

Plugin SpecFlow permettant de générer des tests Javascript à partir d'une syntaxe Gherkin

## Utilisation

+ Compiler le plugin, copier les dll générés dans le dossier des plugins SpecFlow
+ Ajouter la configuration du plugin dans le App.config de la lib de test :
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="specFlow" type="TechTalk.SpecFlow.Configuration.ConfigurationSectionHandler, TechTalk.SpecFlow" />
  </configSections>
  <specFlow>
    <!-- For additional details on SpecFlow configuration options see http://go.specflow.org/doc-config -->
    <plugins>
      <add name="Javascript" type="Generator" path="../libs"/>
    </plugins>
  </specFlow>
</configuration>
```
+ Ajouter un fichier .feature
+ Sauvegarder...
+ Les tests générés gère les backrounds, scenario outline et examples du langage Gherkin.
+ 2 fichiers sont générés : 
  + *.feature.js -> contient les tests exprimer dans la syntaxe du plugin karma-jasmine-cucumber
  + *.feature.generatedstep -> contient les implémentations des given / when / then. A copier / coller dans un vrai fichier js pour compléter les tests.

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
