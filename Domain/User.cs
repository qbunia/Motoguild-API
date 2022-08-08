using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public double Rating { get; set; }
        public ICollection<Group> Groups { get; set; } 
        public ICollection<Event> Events { get; set; } 
        public ICollection<Ride> Rides { get; set; } 
        public ICollection<Route> Routes { get; set; } 
    }
}
