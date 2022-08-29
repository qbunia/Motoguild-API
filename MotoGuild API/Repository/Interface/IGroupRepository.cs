using Domain;

namespace MotoGuild_API.Repository.Interface
{
    public interface IGroupRepository : IDisposable
    {
        IEnumerable<Group> GetAll();
        Group Get(int groupId);
        void Insert(Group group);
        void Delete(int groupId);
        void Update(Group group);
        void Save();
    }
}
