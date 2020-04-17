using coursework02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coursework02.ViewModels
{
    public class AlbumsAllVM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }
        public string StudioName { get; set; }
        public string CoverImagePath { get; set; }
        public string[] ArtistNames { get; set; }
        public string[] ProducerNames { get; set; }
    }
}