using System.ComponentModel.DataAnnotations;

namespace HarryPotter.Models
{
    public class VolumeInfo
    {
        [Display(Name = "Book Title")]
        public string title { get; set; }
        [Display(Name = "Author(s)")]
        public string[] authors {get;set;}
        [Display(Name = "Publisher")]
        public string publisher {get;set;}
        [Display(Name = "Published Date")]
        public string publishedDate { get; set; }
        [Display(Name = "Description")]
        public string description {get; set;}

        public ISBN[] industryIdentifiers {get; set;}
        public ImageLinks imageLinks { get; set; }
    }
}