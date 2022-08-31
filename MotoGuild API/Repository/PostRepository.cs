using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;
using System.Text.RegularExpressions;

namespace MotoGuild_API.Repository
{
    public class PostRepository : IPostRepository
    {
        private MotoGuildDbContext _context;

        public PostRepository(MotoGuildDbContext context)
        {
            _context = context;
        }

        public void Delete(int postId)
        {
            var post = _context.Posts.Include(c => c.Comments).FirstOrDefault(x => x.Id == postId);
            if(post != null)
            {
                _context.Posts.Remove(post);
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Post? Get(int postId)
        {
            var post = _context.Posts.Include(c => c.Comments).FirstOrDefault(x => x.Id == postId);
            return post != null ? post : null;
        }

        public IEnumerable<Post>? GetAll()
        {
            return _context.Posts.Include(c => c.Comments).ToList();
        }

        public IEnumerable<Post>? GetAllFeed(int feedId)
        {
            var posts = _context.Feed.Include(f =>f.Posts).FirstOrDefault(f => f.Id == feedId).Posts;
            return posts != null ? posts : Enumerable.Empty<Post>();

        }

        public IEnumerable<Post>? GetAllGroup(int groupId)
        {
            var posts = _context.Groups.Include(g => g.Posts).FirstOrDefault(g => g.Id == groupId).Posts;
            return posts != null ? posts : Enumerable.Empty<Post>();
        }

        public IEnumerable<Post>? GetAllRide(int rideId)
        {
            var posts = _context.Rides.Include(r => r.Posts).FirstOrDefault(r => r.Id == rideId).Posts;
            return posts != null ? posts : Enumerable.Empty<Post>();
        }

        public IEnumerable<Post>? GetAllRoute(int routeId)
        {
            var posts = _context.Groups.Include(r => r.Posts).FirstOrDefault(r => r.Id == routeId).Posts;
            return posts != null ? posts : Enumerable.Empty<Post>();
        }

        public void InsertToFeed(Post post, int feedId)
        {
            _context.Feed.Include(f => f.Posts).FirstOrDefault(f => f.Id == feedId).Posts.Add(post);
        }
        public void InsertToGroup(Post post, int groupId)
        {
            _context.Groups.Include(g => g.Posts).FirstOrDefault(g => g.Id == groupId).Posts.Add(post);
        }

        public void InsertToRide(Post post, int rideId)
        {
            _context.Groups.Include(r => r.Posts).FirstOrDefault(r => r.Id == rideId).Posts.Add(post);
        }

        public void InsertToRoute(Post post, int routeId)
        {
            _context.Groups.Include(r => r.Posts).FirstOrDefault(r => r.Id == routeId).Posts.Add(post);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
        }
    }
}
