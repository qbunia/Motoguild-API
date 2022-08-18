using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotoGuild_API.Models.Post;

namespace MotoGuild_API.Models.Feed
{
    public class FeedDto
    {
        public int Id { get; set; }
        public ICollection<PostDto> Posts { get; set; }

    }
}
