using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class User
    {
        public int IdWerknemer { private set; get; }
        public string Name { private set; get; }
        public string Email { private set; get; }
        public UserRol Rol { private set; get; }
        public bool IsDeleted { private set; get; }

        public User(int idWerknemer, string name, string email, string rol, bool isDeleted)
        {
            IdWerknemer = idWerknemer;
            Name = name;
            Email = email;
            UserRol finalRol = UserRol.Gebruiker;
            Enum.TryParse<User.UserRol>(rol, true, out finalRol);
            Rol = finalRol;
            IsDeleted = isDeleted;
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
