using Domain.Models;
using Infrasitructure.IRepository;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PostService
    {
        private readonly IBaseRepository<Post> _postRepo;
        private readonly Action<string> _logAction;

        public PostService(IBaseRepository<Post> postRepo)
        {
            _postRepo = postRepo;
            _logAction = message => Console.WriteLine(message);
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _postRepo.GetAll();
        }

        public Post GetPostById(int id)
        {
            return _postRepo.GetById(id);
        }
        public void AddPost(Post post)
        {
            _postRepo.Add(post, _logAction);
        }
        public void UpdatePost(Post post)
        {
            _postRepo.Update(post, _logAction);
        }
        public void DeletePost(int id)
        {
            _postRepo.Delete(id, _logAction);

        }
    }
}
