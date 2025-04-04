using Core.Services;
using Domain.Models;
using Domain.ViewModels;
using Infrasitructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostService _postService;
        private readonly CategoryService _catService;
        private readonly string[] _allowedExtentions = { ".jpg", ".png", ".jpeg" };
        private readonly IWebHostEnvironment _webHost;
        public PostsController(PostService postService,CategoryService categoryService,IWebHostEnvironment webHost)
        {
            _postService = postService;
            _catService = categoryService;
            _webHost = webHost;
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
        public async Task<IActionResult> Create() // add
        {
            var postViewModel = new PostViewModel();
            postViewModel.Categories = await _catService.GetCategoriesWithSelectListItem();
            return View(postViewModel);
        }

        [HttpPost] // get data from user
        public async Task<IActionResult> Create(/*Post post*/ PostViewModel postViewModel) // add
        {
            if (ModelState.IsValid)
            {
                // extention validation 
                var fileExtention = Path.GetExtension(postViewModel.File.FileName).ToLower();
                bool isAllowed = _allowedExtentions.Contains(fileExtention);
                if (!isAllowed)
                {
                    ModelState.AddModelError("File", "File extention is not allowed");
                    postViewModel.Categories = await _catService.GetCategoriesWithSelectListItem();
                    return View(postViewModel);
                }
                // upload file
                postViewModel.Post.Image = await UploadFiles.UploadFile(postViewModel.File,"Posts",_webHost.WebRootPath);
                _postService.AddPost(postViewModel.Post);
                return RedirectToAction(nameof(Index));
            }
            postViewModel.Categories = await _catService.GetCategoriesWithSelectListItem();
            return View(postViewModel);
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
                // get item 
                var PostData = _postService.GetPostById(id);
                // delete image
                string filePath = Path.Combine(_webHost.WebRootPath, PostData.Image);
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _postService.DeletePost(id);
                return RedirectToAction(nameof(Index));
            }
            var post = _postService.GetPostById(id);
            return View(post);

        }

    }
}
