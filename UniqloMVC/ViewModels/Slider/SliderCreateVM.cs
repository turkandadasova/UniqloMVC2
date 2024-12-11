using System.ComponentModel.DataAnnotations;

namespace UniqloMVC.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [MaxLength(32, ErrorMessage = "Title length must be less than 32"), Required(ErrorMessage = "Basliq yazmaq vacibdir!")]
        public string Title { get; set; } = null!;



        [MaxLength(64, ErrorMessage = "Subtitle length must be less than 32"), Required(ErrorMessage = "Alt basliq yazmaq vacibdir!")]

        public string Subtitle { get; set; } = null!;

        public string? Link { get; set; }
        [Required(ErrorMessage ="File is required")]
        public IFormFile File { get; set; } = null!;
    }
}
