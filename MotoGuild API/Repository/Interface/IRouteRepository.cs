using Domain;
using Route = Domain.Route;

namespace MotoGuild_API.Repository.Interface
{
    public interface IRouteRepository : IDisposable
    {
        IEnumerable<Route> GetAll();
        IEnumerable<Route> GetFiveOrderByRating();
        Route Get(int routeId);
        void Insert(Route route);
        void Delete(int routeId);
        void Update(Route route);
        void Save();
    }
}
