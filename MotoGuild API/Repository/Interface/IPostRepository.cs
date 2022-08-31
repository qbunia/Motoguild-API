using Domain;

namespace MotoGuild_API.Repository.Interface
{
    public interface IPostRepository : IDisposable
    {
        IEnumerable<Post>? GetAll();
        IEnumerable<Post>? GetAllFeed(int feedId);
        IEnumerable<Post>? GetAllGroup(int groupId);
        IEnumerable<Post>? GetAllRide(int rideId);
        IEnumerable<Post>? GetAllRoute(int routeId);
        Post? Get(int postId);
        void InsertToFeed(Post post, int feedId);
        void InsertToGroup(Post post, int groupId);
        void InsertToRide(Post post, int rideId);
        void InsertToRoute(Post post, int routeId);
        void Delete(int postId);
        void Update(Post post);
        void Save();
    }
}
