using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public User User { get; set; }
        public ICollection<User> Users { get; set; }
    
        public ICollection<Post> Posts { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime CreationDate { get; set; }
        //public ICollection<User> PendingUsers { get; set; }
    }
}
