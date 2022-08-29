using Domain;

namespace MotoGuild_API.Repository.Interface
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetAll();
        User Get(int userId);
        void Insert(User user);
        void Delete(int userId);
        void Update(User user);
        void Save();
    }
}

