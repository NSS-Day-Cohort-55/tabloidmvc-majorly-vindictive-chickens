using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository, IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public IActionResult Index()
        {
            var subscriptions = _subscriptionRepository.GetAllSubscriptions();
            return View(subscriptions);
        }

        // GET: Subscription/Create/[UserProfileId from Post]
        public IActionResult Create(int id)
        {
            Subscription subscription = new Subscription
            {
                ProviderUserProfileId = id,
                SubscriberUserProfileId = GetCurrentUserProfileId(),
                BeginDateTime = DateTime.Now
            };
            return View(subscription);
        }

        // GET: Subscription/Create/[UserProfileId from Post]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, Subscription sub)
        {
            try
            {
                _subscriptionRepository.Add(sub);

                return RedirectToAction("Index", "Post");
            }
            catch
            {
                var subscriptions = _subscriptionRepository.GetAllSubscriptions();
                return View(subscriptions);
            }
        }

        // GET: Subscription/Edit/[UserProfileId from Post]
        public IActionResult Edit(int id)
        {
            Subscription subscription = new Subscription
            {
                ProviderUserProfileId = id,
                SubscriberUserProfileId = GetCurrentUserProfileId(),
                EndDateTime = DateTime.Now
            };

            try 
            { 
                return View(subscription);
            }
            catch
            {
                var subscriptions = _subscriptionRepository.GetAllSubscriptions();
                return View(subscriptions);
            }
        }

        // POST: Subscription/Edit/[UserProfileId from Post]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Subscription sub)
        {
            try
            {
                _subscriptionRepository.Update(sub);

                return RedirectToAction("Index", "Post");
            }
            catch
            {
                var subscriptions = _subscriptionRepository.GetAllSubscriptions();
                return View(subscriptions);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
