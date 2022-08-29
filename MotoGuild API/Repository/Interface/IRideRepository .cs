using Domain;

namespace MotoGuild_API.Repository.Interface
{
    public interface IRideRepository : IDisposable
    {
        IEnumerable<Ride> GetAll();
        Ride Get(int rideId);
        void Insert(Ride ride);
        void Delete(int rideId);
        void Update(Ride ride);
        void Save();
    }
}
