using System;
using System.ComponentModel.DataAnnotations;

namespace SalesService.Models
{
    public class Part
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string PartNumber { get; set; }
        public DateTime StartTime { get; set; }
    }
}