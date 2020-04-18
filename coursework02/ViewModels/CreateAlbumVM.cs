using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace coursework02.ViewModels
{
    public class CreateAlbumVM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string CopyNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }
        public string StudioName { get; set; }
        public bool IsAgeBar { get; set; }
        public decimal FinePerDay { get; set; }
        public HttpPostedFileBase CoverImage { get; set; }
        public int[] ProducerIds { get; set; }
        public int[] ArtistIds { get; set; }
        public string CoverImagePath { get; set; }
        public SelectList Artists { get; set; }
        public SelectList Producers { get; set; }
        public List<ArtistVM> ArtistList { get; set; }
        public List<ProducerVM> ProducerList { get; set; }

    }
    public class ArtistVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ProducerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Studio { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}