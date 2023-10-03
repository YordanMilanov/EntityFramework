using Forum.Services.Interfaces;
using Forum.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<PostListViewModel> allPosts = 
                await this.postService.ListAllAsync();

            return View(allPosts);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PostAddFormModel PostAddBindingModel)
        {
            if(!ModelState.IsValid)
            {
                return View(PostAddBindingModel);
            }

            try
            {
                await this.postService.AddPostAsync(PostAddBindingModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Unexpected error occurred while adding your post");
                return View(PostAddBindingModel);
            }

            return RedirectToAction("All", "Post");
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                PostAddFormModel postModel = await this.postService.GetForEditByIdAsync(id.ToString());

                return View(postModel);
            }
            catch (Exception)
            {
                return this.RedirectToAction("All", "Post");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PostAddFormModel postModel) 
        {
            if(!ModelState.IsValid)
            {
                return this.View(postModel);
            }

            try
            {
                await this.postService.EditByIdAsync(id, postModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError(String.Empty, "Unexpected error occurred while updating your post");
                return View(postModel);
            }

            return RedirectToAction("All", "Post");
        }
    }
}
