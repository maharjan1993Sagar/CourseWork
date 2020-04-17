using coursework02.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace coursework02.Models
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Studio { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public ICollection<Albums> Albums { get; set; }
    }
}