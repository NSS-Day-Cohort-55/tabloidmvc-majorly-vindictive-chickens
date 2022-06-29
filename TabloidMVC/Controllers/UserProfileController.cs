using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository, ICategoryRepository categoryRepository)
        {
            _userProfileRepository = userProfileRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var userProfiles = _userProfileRepository.GetAllUserProfiles();
            return View(userProfiles);
        }

        public IActionResult IndexDeactiveUsers()
        {
            var userProfiles = _userProfileRepository.GetAllDeactiveUserProfiles();
            return View(userProfiles);
        }

        public IActionResult Details(int id)
        {
            var userProfile = _userProfileRepository.GetUserProfileById(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        // GET: UserProfile/Edit/5
        public ActionResult Edit(int id)
        {
            var userProfile = _userProfileRepository.GetUserProfileById(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.UpdateUser(userProfile);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(userProfile);
            }
        }

        // GET: UserProfile/Edit/5
        public ActionResult Deactivate(int id)
        {
            var userProfile = _userProfileRepository.GetUserProfileById(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            EditUserViewModel vm = new EditUserViewModel()
            {
                UserProfile = userProfile
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id, EditUserViewModel vm)
        {       
            var userProfile = _userProfileRepository.GetUserProfileById(id);
            if (userProfile.IsDeactivated)
            {
                userProfile.IsDeactivated = false;
            }
            else
            {
                if ((userProfile.UserTypeId == 1 && userProfile.DeactivatorId != GetCurrentUserProfileId() 
                     && userProfile.DeactivatorId != 0) ||
                    userProfile.UserTypeId == 2)
                {
                    userProfile.DeactivatorId = GetCurrentUserProfileId();
                    _userProfileRepository.UpdateUser(userProfile);
                    userProfile.IsDeactivated = true;
                }
                else if (userProfile.UserTypeId == 1 && userProfile.DeactivatorId == 0)
                {
                    userProfile.DeactivatorId = GetCurrentUserProfileId();
                    _userProfileRepository.UpdateUser(userProfile);
                    return RedirectToAction("Index");
                }
            }

            try
            {
                _userProfileRepository.DeactivateUser(userProfile);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(vm);
            }
        }

        /*
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

                _userProfileRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        */

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

