using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;
using Route = Domain.Route;

namespace MotoGuild_API.Repository
{
    public class RouteStopsRepository : IRouteStopsRepository
    {
        private MotoGuildDbContext _context;

        public RouteStopsRepository(MotoGuildDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Stop> GetAll(int routeId)
        {
            return _context.Routes.Include(r=>r.Stops).FirstOrDefault(r => r.Id == routeId).Stops;
        }

        public Stop Get(int id, int routeId)
        {
            return _context.Routes.Include(r => r.Stops)
                .FirstOrDefault(r => r.Id == routeId)
                .Stops.FirstOrDefault(s => s.Id == id);
        }

        public void Insert(Stop stop, int routeId)
        {
            _context.Routes.Include(r=>r.Stops)
                .FirstOrDefault(r => r.Id == routeId).Stops.Add(stop);
        }

        public void Delete(int id, int routeId)
        {
            Stop stop = _context.Routes.Include(r=>r.Stops)
                .FirstOrDefault(r=>r.Id == routeId)
                .Stops.FirstOrDefault(s => s.Id == id);
            _context.Stops.Remove(stop);
        }

        public void Update(Stop stop)
        {
            _context.Entry(stop).State = EntityState.Modified;
        }
        public bool StopExistsInRoute(int stopId, int routeId)
        {
            return (_context.Routes.Include(r => r.Stops).AsNoTracking()
                .FirstOrDefault(r => r.Id == routeId)
                .Stops.FirstOrDefault(s => s.Id == stopId) != null);
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
