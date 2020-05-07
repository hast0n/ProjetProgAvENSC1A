using System;
using System.Collections.Generic;
using System.Text;
using CliLayoutRenderTools;
using ProjetProgAvENSC1A.Models;

namespace ProjetProgAvENSC1A.Controllers
{
    class SortController
    {



        public void SortByPerson(Person p)
        {
            var projects = p.Projects;

            ContentView view = new ContentView()
            {
                LocalResources =
                {
                    {
                        "projets",
                        "[liste des projets formattées]"
                    }
                }
            };
            // construction affichage
            // lancement affichage
            // return choix de l'utilisateur
        }
    }
}
