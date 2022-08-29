using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Repository
{

    public class CommentRepository : ICommentRepository
    {
        private MotoGuildDbContext _context;

        public CommentRepository(MotoGuildDbContext context)
        {
            _context = context;
        }
        public void Delete(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
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

        public Comment? Get(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment != null) {

                return comment;
            }
            return null;
        }

        public IEnumerable<Comment>? GetAll(int postId)
        {
            var post = _context.Posts.Include(u => u.Author).Include(c => c.Comments).FirstOrDefault(c => c.Id == postId);
            if (post == null)
            {
                return null;
            }
            return post.Comments;
            
        }

        public IEnumerable<Comment>? GetAll()
        {
            return _context.Comments.ToList();
            
        }

        public void Insert(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
        }
    }
}
