using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AWA.Models
{
    public class User
    {
        public int UserId { set; get; }

        [Required, MinLength(3)]
        public string Name { set; get; }

        [Required, MinLength(5), EmailAddress]
        public string Username { set; get; }

        [JsonIgnore]
        public string PasswordEncrypted { get; set; }

        [Required, MinLength(4), NotMapped]
        public string Password { get; set; }

        [Required]
        public UserRole Role { set; get; }

        public bool IsArchived { set; get; }

        public enum UserRole
        {
            User, Admin
        }
    }
}
