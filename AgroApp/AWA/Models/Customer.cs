using System.ComponentModel.DataAnnotations;

namespace AWA.Models
{
    public class Customer
    {
        public int CustomerId { set; get; }
        [Required, MinLength(3)]
        public string Name { set; get; }
        [Required, MinLength(3)]
        public string Address { set; get; }
        public bool IsArchived { get; set; }
    }
}
