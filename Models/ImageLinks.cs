using System.ComponentModel.DataAnnotations;

namespace HarryPotter.Models
{
    public class ImageLinks
    {
        [Display(Name = "Cover")]
        public string smallThumbnail { get; set; }
    }
}