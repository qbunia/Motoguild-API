using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private MotoGuildDbContext _context;

        public GroupRepository(MotoGuildDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Group> GetAll()
        {
            return _context.Groups
                .Include(g => g.Owner)
                .Include(g => g.Participants)
                .Include(g => g.PendingUsers)
                .Include(g => g.Posts).ThenInclude(p => p.Author)
                .ToList();
        }

        public Group Get(int id)
        {
            return _context.Groups.Find(id);
        }

        public void Insert(Group group)
        {
            var ownerFull = _context.Users.FirstOrDefault(u => u.Id == group.Owner.Id);
            group.Owner = ownerFull;
            _context.Groups.Add(group);
            group.Participants.Add(group.Owner);
        }

        public void Delete(int groupId)
        {
            Group group = _context.Groups
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == groupId);
            _context.Groups.Remove(group);
        }

        public void Update(Group group)
        {
            _context.Entry(group).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
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
    }
}
