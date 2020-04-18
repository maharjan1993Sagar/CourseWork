using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace coursework02.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MemberNo { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }

        public string Email { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        public int MemberCategoryId { get; set; }

        [ForeignKey("MemberCategoryId")]
        public virtual MemberCategory MemberCatagories { get; set; }
    }
}