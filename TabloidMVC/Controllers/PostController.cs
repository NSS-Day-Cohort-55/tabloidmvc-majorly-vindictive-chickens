using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System.Collections.Generic;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public IActionResult Delete(int id)
        {

            Post post = _postRepository.GetUserPostById(id, GetCurrentUserProfileId());
            if (post.UserProfileId == GetCurrentUserProfileId())
            {
                return View(post);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Delete(Post post)
        {
            try
            {
                _postRepository.Delete(post);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Details", new { id = post.Id });
            }
        }

        public IActionResult CreatePostTag(int id)
        {
            var tags = _tagRepository.GetAllTags();
            var post = _postRepository.GetPublishedPostById(id);
            var vm = new PostTagViewModel()
            {
                TagOptions = tags,
                Post = post,
                TagIds = new List<int>()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult CreatePostTag(PostTagViewModel vm, int id)
        {
            try
            {


                foreach (int tagId in vm.TagIds)
                {
                    
                    
                    _postRepository.InsertTag(id, tagId);
                }

                return RedirectToAction("Details", "Post", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
