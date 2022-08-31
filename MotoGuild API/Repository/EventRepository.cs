using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Repository
{
    public class EventRepository : IEventRepository
    {
        private MotoGuildDbContext _context;

        public EventRepository(MotoGuildDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAll()
        {
            return _context.Events
                .Include(g => g.Owner)
                //.Include(g => g.Participants)
                //.Include(g => g.Posts)                
                .ToList();
        }

        public Event Get(int id)
        {
            return _context.Events.Find(id);
        }

        public void Insert(Event eve)
        {
            var ownerFull = _context.Users.FirstOrDefault(u => u.Id == eve.Owner.Id);
            eve.Owner = ownerFull;
            _context.Events.Add(eve);
            eve.Participants.Add(eve.Owner);
        }

        public void Delete(int eveId)
        {
            Event eve = _context.Events
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == eveId);
            _context.Events.Remove(eve);
        }

        public void Update(Event eve)
        {
            _context.Entry(eve).State = EntityState.Modified;
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
