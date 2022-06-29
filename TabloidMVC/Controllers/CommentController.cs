using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public IActionResult Index(int id)
        {
                var comments = _commentRepository.GetCommentByPostId(id);
           if (comments != null)
            {
                return View(comments);
            }
            else
            {
                var post = _postRepository.GetPublishedPostById(id);
                return View(post);
            }
        }

        public IActionResult Create(int id)
        {
            var vm = new CommentCreateViewModel();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(CommentCreateViewModel vm, int id)
        {
            try
            {
                vm.Comment.PostId = id;
                vm.Comment.UserProfileId = GetCurrentUserProfileId();
                _commentRepository.Add(vm.Comment);
                return RedirectToAction("Index", new {id});

            }
            catch (Exception ex)
            {
                return View(vm);
            }
        }

        public IActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);
            if (comment.UserProfileId == GetCurrentUserProfileId())
            {
                return View(comment);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id, Comment comment)
        {
            try
            {
                _commentRepository.Delete(comment);

                return RedirectToAction("Index", new {id = comment.PostId});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", new { id = comment.Id });
            }
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }



    }
}
