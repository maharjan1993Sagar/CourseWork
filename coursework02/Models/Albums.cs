using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace coursework02.Models
{
    public class Albums
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CopyNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }
        public string StudioName { get; set; }
        public bool IsAgeBar { get; set; }
        public decimal FinePerDay { get; set; }
        [NotMapped]
        public HttpPostedFile CoverImage { get; set; }
        public string CoverImagePath { get; set; }
        public  ICollection<Artist> Artists { get; set; } //navigatiion properties
        public  ICollection<Producer> Producers { get; set; } //navigatiion properties

        
    }
}