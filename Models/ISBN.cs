using System.ComponentModel.DataAnnotations;

namespace HarryPotter.Models
{
    public class ISBN
    {
        public string type { get; set; }
        [Display(Name = "ISBN_10")]
        public string identifier {get; set;}
    }

}