using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IReactionRepository _reactionRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, IUserProfileRepository userProfileRepository, ITagRepository tagRepository, IReactionRepository reactionRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userProfileRepository = userProfileRepository;
            _tagRepository = tagRepository;
            _reactionRepository = reactionRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }
        public IActionResult UserPosts()
        {
            var posts = _postRepository.GetAllPostsByUser(GetCurrentUserProfileId());
            return View(posts);
        }

        public IActionResult PostsByUser()
        {
            var vm = new PostsByUserViewModel
            {
                UserProfiles = _userProfileRepository.GetAllUserProfiles(),
                Posts = _postRepository.GetAllPublishedPosts()
            };
      
            return View(vm);
        }

        [HttpPost]
        public IActionResult PostsByUser(PostsByUserViewModel vm)
        {
            vm.UserProfiles = _userProfileRepository.GetAllUserProfiles();
            if (vm.SelectedProfileId == 0)
            {
                vm.Posts = _postRepository.GetAllPublishedPosts();
            }
            else
            {
                vm.Posts = _postRepository.GetAllPostsByUser(vm.SelectedProfileId);
            }
            return View(vm);
        }

        public IActionResult PostsByCategory()
        {
            var vm = new PostsByCategoryViewModel
            {
                Categories = _categoryRepository.GetAll(),
                Posts = _postRepository.GetAllPublishedPosts()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult PostsByCategory(PostsByCategoryViewModel vm)
        {
            vm.Categories = _categoryRepository.GetAll();
            if (vm.SelectedCategoryId == 0)
            {
                vm.Posts = _postRepository.GetAllPublishedPosts();
            }
            else
            {
                vm.Posts = _postRepository.GetAllPostsByCategory(vm.SelectedCategoryId);
            }
            return View(vm);
        }

        public IActionResult PostsByTag()
        {
            var vm = new PostsByTagViewModel
            {
                Tags = _tagRepository.GetAllTags(),
                Posts = _postRepository.GetAllPublishedPosts()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult PostsByTag(PostsByTagViewModel vm)
        {
            vm.Tags = _tagRepository.GetAllTags();
            if (vm.SelectedTagId == 0)
            {
                vm.Posts = _postRepository.GetAllPublishedPosts();
            }
            else
            {
                vm.Posts = _postRepository.GetAllPostsByTag(vm.SelectedTagId);
            }
            return View(vm);
        }
        public IActionResult Details(int id)
        {
            var vm = new PostDetailViewModel();
            var post = _postRepository.GetPublishedPostById(id);
            var tags = _postRepository.GetTagsByPost(id);
            var reactions = _postRepository.GetReactionsByPost(id);
            int subscriberId = GetCurrentUserProfileId();
            var subscription = _postRepository.GetSubscriptionByAuthorId(subscriberId, post.UserProfileId);
            vm.Tags = tags;
            vm.Post = post;
            vm.Reactions = reactions;
            vm.Subscription = subscription;

            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostFormViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostFormViewModel vm)
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
        public IActionResult Edit(int id)
        {
            var vm = new PostFormViewModel
            {
                CategoryOptions = _categoryRepository.GetAll(),
                Post = _postRepository.GetPostByPostId(id)
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(PostFormViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Update(vm.Post);

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


        public ActionResult DeletePostTags(int id)
        {
            var tags = _tagRepository.GetAllTags();
            var post = _postRepository.GetPublishedPostById(id);
            var vm = new PostTagViewModel()
            {
                TagOptions = tags,
                Post = post,
                PostTags = _postRepository.GetTagsByPost(id)
            };
            return View(vm);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostTags(PostTagViewModel vm, int id)
        {
            try
            {


                foreach (int tagId in vm.TagIds)
                {


                    _postRepository.DeleteTag(id, tagId);
                }

                return RedirectToAction("Details", "Post", new { id = id });
            }
            catch
            {
                return View();
            }
        }


        public IActionResult CreatePostReaction(int id)
        {
            var reactions = _reactionRepository.GetAllReactions();
            var post = _postRepository.GetPublishedPostById(id);
            var userId = GetCurrentUserProfileId();
            var vm = new PostReactionViewModel()
            {
                ReactionOptions = reactions,
                Post = post,
                ReactionIds = new List<int>(),
                UserId = userId,
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult CreatePostReaction(PostReactionViewModel vm, int id)
        {
            var reactions = _reactionRepository.GetAllReactions();
            var post = _postRepository.GetPublishedPostById(id);
            var userId = GetCurrentUserProfileId();
            vm.ReactionOptions = reactions;
            vm.Post = post;
            vm.UserId = userId;
           
            try
            {
                foreach (int reactionId in vm.ReactionIds)
                {


                    _postRepository.InsertReaction(id, reactionId, vm.UserId);
                }

                return RedirectToAction("Details", "Post", new { id = id });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeletePostReactions(int id)
        {
            var tags = _tagRepository.GetAllTags();
            var post = _postRepository.GetPublishedPostById(id);
            var vm = new PostTagViewModel()
            {
                TagOptions = tags,
                Post = post,
                PostTags = _postRepository.GetTagsByPost(id)
            };
            return View(vm);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostReactions(PostTagViewModel vm, int id)
        {
            try
            {


                foreach (int tagId in vm.TagIds)
                {


                    _postRepository.DeleteTag(id, tagId);
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
