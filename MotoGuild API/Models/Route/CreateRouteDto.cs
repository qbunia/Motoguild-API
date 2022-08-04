using System.ComponentModel.DataAnnotations;

namespace MotoGuild_API.Models.Route
{
    public class CreateRouteDto
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        [MaxLength(32)]
        public string StartPlace { get; set; }
        [Required]
        [MaxLength(32)]
        public string EndingPlace { get; set; }
        [Required]
        [MaxLength(32)]
        public string Description { get; set; }
        
        public int Rating { get; set; }
    }
}
