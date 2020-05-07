using System;
using System.Collections.Generic;
using System.Text;

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
        public Privilege Privilege { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
