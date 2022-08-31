using Domain;
using Microsoft.EntityFrameworkCore;

namespace MotoGuild_API.Repository.Interface
{
    public interface IEventRepository : IDisposable
    {
        IEnumerable<Event> GetAll();
        Event Get(int eveId);
        void Insert(Event eve);
        void Delete(int eveId);
        void Update(Event eve);
        void Save();
    }
}
