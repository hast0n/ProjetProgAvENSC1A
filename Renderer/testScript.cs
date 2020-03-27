using System;
using System.Collections.Generic;
using ProjetProgAvENSC1A.Views;

namespace CliLayoutRenderTools
{
    class RenderTests
    {
        public static void Test()
        {

            // [Commencer par regarder HomePage.cs]


            // On crée une instance du renderer avec les paramètres voulus
            var renderer = new Renderer()
            {
                FrameWidth = 70
            };

            // Ici c'est juste pour dire d'utiliser les ressources visuelles déjà faites
            // (topBar, botBar et emptyLine)
            renderer.SetDefaultResources();

            // Ici c'est pour ajouter au renderer des ressources générales, à partagées.
            renderer.AddVisualResources(new Dictionary<string, string>()
            {
                {
                    // Ici c'est presque la même ressource que dans HomePage.cs
                    // a l'exception qu'il n'y a pas de variables dedans
                    // Ce n'est pas cette ressource qui sera utilisée par HomPage
                    // puisqu'on lui a défini sa propre version
                    "intro",
                    "Bienvenue ! C'est cool walah <color:blue>yolooo<color:black>"
                },
            });

            // Ici on crée l'instance de HomePage
            // EN lui passant les paramètres suivants :
            //      - renderer.VisualResources : les ressources globales du renderer
            //      - un dictionnaire <string, string> qui défini les valeurs des variables
            //        qui sont définies dans HomePage
            var homePage = new HomePage(
                renderer.VisualResources,
                new Dictionary<string, string>()
                {
                    {
                        "userName",
                        "Martin Devreese"
                    },
                    {
                        "qqch",
                        "[voici qqch]"
                    },
                }
            );

            //var test = homePage.SerializedResources;
            //var test2 = homePage.VisualResources;

            // Et voici l'instruction qui permet de dire au renderer de construire la page sur la console
            // Idéalement il y a des paramètre de retours qui permettront de savoir ce qu'a saisi l'utilisateur
            renderer.Render(homePage);


            /* Le but final est de créer une ContentPage (comme HomePage.cs) par écran
             * auxquelles on pourra passer les paramètre voulus pour modifier dynamiquement l'affichage.
             *
             * Les pages (écrans - Views) doivent toutes hériter de ContentPage
             */
        }
    }
}