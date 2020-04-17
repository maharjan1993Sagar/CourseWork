using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace coursework02.Models
{
    public class ArtistAlbum
    {
        [Key, Column(Order = 1)]
        public int ArtistId { get; set; }
        [Key, Column(Order = 2)]
        public int AlbumId { get; set; }
        public Artist Artist { get; set; }
        public Albums Albums { get; set; }
    }
}