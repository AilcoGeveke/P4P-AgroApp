using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class User
    {
        public int IdEmployee { set; get; }
        public string Name { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public UserRole Role { set; get; }

        public User() { }

        public User(int idWerknemer, string name, string username, string rol)
        {
            IdEmployee = idWerknemer;
            Name = name;
            Username = username;
            UserRole finalRol = UserRole.User;
            Enum.TryParse<User.UserRole>(rol, true, out finalRol);
            Role = finalRol;
        }

        public override string ToString()
        {
            return Name;
        }

        public enum UserRole
        {
            User, Admin
        }
    }
}
