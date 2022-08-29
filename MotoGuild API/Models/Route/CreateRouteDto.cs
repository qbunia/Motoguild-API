using System.ComponentModel.DataAnnotations;
using MotoGuild_API.Models.User;

namespace MotoGuild_API.Models.Route
{
    public class CreateRouteDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartPlace { get; set; }
        public string EndingPlace { get; set; }
        public int Rating { get; set; }
        public UserDto Owner { get; set; }
    }

 
}
