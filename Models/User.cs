using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    public enum Privilege
    {
        Administrator,
        Moderator,
        Spectator,
        Unauthorized
    }

    class User : EntryType
    {
        public Privilege Privilege { get; protected set; }
        public string Name { get; protected set; }
        public string PasswordHash { get; protected set; }
    }

    class TestUser : User
    {
        public TestUser()
        {
            Privilege = Privilege.Administrator;
            Name = "Administrateur";
            PasswordHash = SHA.GenerateSHA512String("admin");
        }
    }
}