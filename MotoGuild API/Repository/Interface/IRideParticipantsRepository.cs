using Domain;

namespace MotoGuild_API.Repository.Interface;

public interface IRideParticipantsRepository : IDisposable
{
    IEnumerable<User> GetAll(int rideId);
    User Get(int rideId, int participantId);

    User GetUser(int userId);
    void AddParticipantByUserId(int rideId, int userId);
    void DeleteParticipantByUserId(int rideId, int userId);
    void Update(User user);
    bool RideExist(int rideId);
    bool UserExits(int userId);
    bool UserInRide(int rideId, int userId);
    void Save();
}