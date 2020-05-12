using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;

namespace ProjetProgAvENSC1A.Views
{
    /*
     * Ici on crée notre première page en utilisant le renderer
     *
     * On commence par définir la class qui prendra la convention de nommage suivante : XxxPage
     * et qui dérive nécessairement de ContentPage
     */

    class HomeViewWithComments : ContentView
    {

        /*
         * Définition du constructeur :
         * Le nombre de paramètre que prend le constructeur est à définir en
         * fonction des besoins :
         *      - a t'on besoin de lui passer des ressources partégées
         *      - a t'on besoin de lui passer des valeurs de variables
         *      - etc..
         *
         * La définition du layout et des LocalResources doit se faire dans le
         * constucteur ou au moins une fois que l'objet a été instancié vu que
         * ContentPage nécessite une instance pour posséder ces attributs
         */

        public HomeViewWithComments(
            Dictionary<string, string> sharedResources,
            Dictionary<string, string> pageModifiers)
            :base(sharedResources)
        {
            // Rendre les paramètres accessibles à l'instance de la class mère ContentPage
            SharedResources = sharedResources;
            PageModifiers = pageModifiers;

            
            /*
             * Ici on définit le layout de notre écran en question
             * On peut faire une analogie avec le html --> ici on construit la structure
             * de notre page (avec les balises)
             * Du haut vers le bas, cela correspond à l'ordre d'affichage
             */ 
            Layout = new List<string>()
            {
                "topBar", 
                "[emptyLine]", 
                "[intro]",
                "3*[test]",
                /*
                 * Si on regarde "intro", il est défini plus bas dans les LocalResources donc il est spécifiques
                 * à HomePage.
                 *
                 * Les autres ressources partégées doivent être passées en paramètre à la création de la page
                 * (si on souhaite les utiliser).
                 *
                 * Des ressources locales et partagées peuvent avoir le même nom (c'est le cas avec "intro"), c'est alors
                 * la ressource locale qui sera utilisée.
                 */
                "[emptyLine]", 
                "botBar"
            };

            // Ici on définit les ressources locales, spécifiques à la page que l'on décrit (ici HomePage)
            LocalResources = new Dictionary<string, string>()
            {
                {
                    /* Nom de la ressource : */ "intro",
                    /* Valeur de la ressource : */ "Bienvenue à toi, $userName ! C'est cool walah <color value=blue>$qqch<color value=black>"
                    
                    /* 2 variables sont définies dans la chaine ci dessus : $userName et $qqch
                     * les valeurs par lesquelles les remplacer sont fournie par le paramètre pageModifiers
                     *
                     * Si les valeurs de paramètres ne sont pas fournies alors une erreur
                     * AttributeValueNotFoundException sera renvoyée.
                     * */
                },
                {
                    "test",
                    "--> | <input regex=[A-Za-z0-9]> | <--"
                },
            };

            // Une fois que le layout est défini avec ou sans pageModifiers (ça dépend de si on veut afficher 
            // de manière dynamique ou non), on a juste à créer une instance de cette page
            // --> fait dans testScript.cs
        }
    }
}