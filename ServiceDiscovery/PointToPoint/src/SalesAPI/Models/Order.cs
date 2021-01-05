using System.ComponentModel.DataAnnotations;

namespace SalesAPI.Models
{
    public class Order
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsFullTime { get; set; }
        [Required]
        public string Email { get; set; }
    }
}