using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostService _postService;
        public PostsController(PostService postService)
        {
            _postService = postService;
        }
        public async Task<IActionResult> Index() // get all
        {
            var posts = await _postService.GetAllPosts();
            //return View(); // controller : Posts =>  view : index
            //return View("GetAll"); // controller : Posts =>  view : GetAll
            return View(posts.ToList()); // controller : Posts =>  view : index   var : posts
            //return View("getAll",posts);// controller : Posts => view : getAll var : posts
        }

    }
}
