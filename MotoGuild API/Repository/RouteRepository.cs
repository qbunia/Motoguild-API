using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;
using Route = Domain.Route;

namespace MotoGuild_API.Repository
{
    public class RouteRepository : IRouteRepository
    {
        private MotoGuildDbContext _context;

        public RouteRepository(MotoGuildDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Route> GetAll()
        {
            return _context.Routes
                .Include(r => r.Owner)
                .Include(r=>r.Stops)
                .ToList();
        }
        public IEnumerable<Route> GetFiveOrderByRating()
        {
            return _context.Routes
                .Include(r => r.Owner)
                .Include(r => r.Stops)
                .OrderByDescending(r=>r.Rating)
                .Take(5)
                .ToList();
        }

        public Route Get(int id)
        {
            return _context.Routes
                .Include(r => r.Owner)
                .Include(r=>r.Posts).ThenInclude(p => p.Author)
                .Include(r=>r.Stops)
                .FirstOrDefault(r=>r.Id == id);
        }

        public void Insert(Route route)
        {
            var ownerFull = _context.Users.FirstOrDefault(u => u.Id == route.Owner.Id);
            route.Owner = ownerFull;
            _context.Routes.Add(route);
        }

        public void Delete(int id)
        {
            Route route = _context.Routes
                .Include(g => g.Posts)
                .Include(r => r.Posts)
                .FirstOrDefault(g => g.Id == id);
            _context.Routes.Remove(route);
        }

        public void Update(Route route)
        {
            _context.Entry(route).State = EntityState.Modified;
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
