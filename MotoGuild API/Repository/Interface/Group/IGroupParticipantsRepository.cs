using Domain;

namespace MotoGuild_API.Repository.Interface
{
    public interface IGroupParticipantsRepository : IDisposable
    {
        IEnumerable<User> GetAll(int groupId);
        User Get(int groupId, int participantId);

        User GetUser(int userId);
        void AddParticipantByUserId(int groupId, int userId);
        void DeleteParticipantByUserId(int groupId, int userId);
        void Update(User user);
        bool GroupExist(int groupId);
        bool UserExits(int userId);
        bool UserInGroup(int groupId, int userId);
        void Save();
    }
}
