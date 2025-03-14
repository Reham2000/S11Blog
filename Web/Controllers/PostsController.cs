using Core.Services;
using Domain.Models;
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
        [HttpGet]
        public async Task<IActionResult> Index() // get all
        {
            var posts = await _postService.GetAllPosts();
            //return View(); // controller : Posts =>  view : index
            //return View("GetAll"); // controller : Posts =>  view : GetAll
            return View(posts.ToList()); // controller : Posts =>  view : index   var : posts
            //return View("getAll",posts);// controller : Posts => view : getAll var : posts
        }
        public IActionResult Details (int id) // get by id
        {
            var post = _postService.GetPostById(id);
            return View(post);
        }




        [HttpGet]
        public IActionResult Create() // add
        {
            return View();
        }

        [HttpPost] // get data from user
        public IActionResult Create(Post post) // add
        {
            if(!ModelState.IsValid)
            {
                return View(post);
            }
            _postService.AddPost(post);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var post = _postService.GetPostById(id);
            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }
            _postService.UpdatePost(post);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var post = _postService.GetPostById(id);
            return View(post);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            if (ModelState.IsValid)
            {
                _postService.DeletePost(id);
                return RedirectToAction(nameof(Index));
            }
            var post = _postService.GetPostById(id);
            return View(post);

        }

    }
}
