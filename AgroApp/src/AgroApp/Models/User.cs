using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class User
    {
        public int IdWerknemer { set; get; }
        public string Name { set; get; }
        public string Username { set; get; }
        public UserRol Rol { set; get; }

        public User() { }

        public User(int idWerknemer, string name, string username, string rol)
        {
            IdWerknemer = idWerknemer;
            Name = name;
            Username = username;
            UserRol finalRol = UserRol.Gebruiker;
            Enum.TryParse<User.UserRol>(rol, true, out finalRol);
            Rol = finalRol;
        }

        public override string ToString()
        {
            return Name;
        }

        public enum UserRol
        {
            Gebruiker, Admin
        }
    }
}
