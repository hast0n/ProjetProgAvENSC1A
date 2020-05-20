using System;
using System.Collections.Generic;
using System.Text;
using ProjetProgAvENSC1A.Services;

namespace ProjetProgAvENSC1A.Models
{
    public enum Privilege
    {
        Unauthorized,
        Administrator,
        Moderator,
        Spectator
    }

    class User : EntryType
    {
        public Privilege Privilege { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }

        public User()
        {
            Privilege = Privilege.Unauthorized;
            Name = String.Empty;
            PasswordHash = String.Empty;
        }
    }
}