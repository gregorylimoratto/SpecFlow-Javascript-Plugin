Feature: Préciser la tranche de revenu, US01-Sélectionner la tranche revenu du client
En tant que Distributeur,
Je dois saisir l'estimation des revenus mensuels nets de mon client,
Afin d'être conforme à la loi sur la Lutte Contre le Blanchiment (LCB-FT). 

#MMF01-F4-US01-RG01 : Je dois sélectionner l’une des valeurs suivantes : 
#- Moins de 1500€
#- Entre 1500€ et 2500€
#- Entre 2500€ et 5000€
#- Entre 5000€ et 10000€
#- Plus de 10000€

#MMF01-F4-US01-RG02 : Le champ est initialisé à "Choisir un revenu mensuel net"lorsque je réalise un GPS pour la 1ère fois avec ce client (c’est-à-dire qu’aucune donnée sur ce client n’est stockée dans la BDD GPS ou la Brique Conformité LME).
#MMF01-F4-US01-RG03 : Je dois saisir obligatoirement une valeur parmi les valeurs proposées ("Choisir un revenu mensuel net" n'est pas une valeur acceptable). En l’absence de réponse, un message bloquant s’affiche. (cf. N° de maquette à préciser ici)
#MMF01-F4-US01-RG04 : En complément, je peux saisir la valeur exacte dans un champ de saisie acceptant uniquement les caractères numériques sans décimales (9 caractères maximum - champ borné au délà de cette limite).


@CasPassant
  Scenario: Visualisation de revenus
    Given Je suis sur l'écran "Connaissance Client", "5 Profession", "Vos revenus"
     When Je sélectionne la tranche de revenus
     Then Je dispose de deux champs
      | Nom du champ           | Valeur par défaut             | 
      | Revenus mensuels net * | Choisir un revenu mensuel net | 
      | Préciser (€)           | Vide                          | 
      And La liste des revenus apparaît dans l'ordre suivant
      | Revenus                       | 
      | Choisir un revenu mensuel net | 
      | Moins de 1500€                | 
      | Entre 1500€ et 2500€          | 
      | Entre 2500€ et 5000€          | 
      | Entre 5000€ et 10000€         | 
      | Plus de 10000€                | 
  
  Scenario: Saisie de revenus
    Given Je suis sur l'écran "Connaissance Client", "5 Profession", "Vos revenus"
     When Je renseigne les valeurs suivantes
      | Revenus mensuels net * | Préciser (€) | 
      | Moins de 1500€         | 1200         | 
      | Moins de 1500€         |              | 
      | Entre 1500€ et 2500€   | 1700         | 
      | Entre 1500€ et 2500€   |              | 
      | Entre 2500€ et 5000€   | 5000         | 
      | Entre 2500€ et 5000€   |              | 
      | Entre 5000€ et 10000€  | 5001         | 
      | Entre 5000€ et 10000€  |              | 
      | Plus de 10000€         | 123456789    | 
     Then La saisie est possible
  
  @CasNonPassant
  Scenario: Saisie incohérente de revenus
    Given Je suis sur l'écran "Connaissance Client", "5 Profession", "Vos revenus"
     When Je renseigne les valeurs suivantes
      | Revenus mensuels net *        | Préciser (€)     | message                                | 
      | Choisir un revenu mensuel net |                  | Champ obligatoire Revenus mensuels net | 
      | Choisir un revenu mensuel net | 12000            | Champ obligatoire Revenus mensuels net | 
      | Entre 1500€ et 2500€          | 0                | Champ en erreur                        | 
      | Entre 2500€ et 5000€          | 1000             | Champ en erreur                        | 
      | Plus de 10000€                | 1234567891       | Champ en erreur                        | 
      | Moins de 1500€                | -500             | Champ en erreur                        | 
      | Moins de 1500€                | 10^3             | Champ en erreur                        | 
      | Entre 5000€ et 10000€         | fdsfuiosdfisxudf | Champ en erreur                        | 
      | Entre 5000€ et 10000€         | ff               | Champ en erreur                        | 
      | Moins de 1500€                | " "              | Champ en erreur                        | 
      | Entre 1500€ et 2500€          | %                | Champ en erreur                        | 
      | Entre 2500€ et 5000€          | $                | Champ en erreur                        | 
      | Entre 2500€ et 5000€          | 3000 €           | Champ en erreur                        | 
      | Moins de 1500€                | " 10"            | Champ en erreur                        | 
      | Moins de 1500€                | 1000,2           | Champ en erreur                        | 
      | Entre 1500€ et 2500€          | 1803,5           | Champ en erreur                        | 
     Then Le message bloquant apparaît
      | Message                                | 
      | Champ obligatoire Revenus mensuels net | 
      | Champ en erreur                        | 
      And Les messages d'erreurs sont sur fond rouge. Ils sont placés entre le bandeau bleu et la zone de contenu. Ils se ferment via petite croix en haut à droite de la zone d'erreur. La fermeture du message se fait à l'initiative de l'utilisateur 
  
  @CasTechnique
  Scenario: BDD technique
    Given Je suis sur l'écran "Connaissance Client", "5 Profession", "Vos revenus"
     When je regarde le code de la page
     Then je constate que les id de champs sont bien conformes au tableau suivant :
      | Enuméré                | Valeur | Libellé                  | 
      | CmbMontants_Revenu     | RM1    | Moins de 1 500 €         | 
      | EnumereMontants_Revenu | RM2    | Entre 1 500 € et 2 499 € | 
      | EnumereMontants_Revenu | RM3    | Entre 2 500 € et 4 999 € | 
      | EnumereMontants_Revenu | RM4    | Entre 5 000 € et 9 999 € | 
      | EnumereMontants_Revenu | RM5    | Plus de 10 000 €         | 
  #Champ/Question ID LME
  #Revenus mensuels nets 191
  #Préciser (€) 190
  
  
  
  
  
  
