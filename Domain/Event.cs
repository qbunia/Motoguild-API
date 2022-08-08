using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public User User { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; }
        public string Place { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
