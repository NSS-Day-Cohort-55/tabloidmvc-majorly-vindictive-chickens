using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace TabloidMVC.Controllers
{
    public class ReactionController : Controller
    {
        private readonly IReactionRepository _reactionRepository;

        public ReactionController(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }
        // GET: ReactionController
        public ActionResult Index()
        {
            var reactions = _reactionRepository.GetAllReactions();
            return View(reactions);
        }

        // GET: ReactionController/Details/5
        public ActionResult Details(int id)
        {
            Reaction reaction = _reactionRepository.GetReaction(id);

            if (reaction == null)
            {
                return NotFound();
            }

            return View(reaction);
        }

        // GET: ReactionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReactionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reaction reaction)
        {
            try
            {
                _reactionRepository.AddReaction(reaction);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(reaction);
            }
        }

        // GET: ReactionController/Edit/5
        public ActionResult Edit(int id)
        {
            Reaction reaction = _reactionRepository.GetReaction(id);

            if (reaction == null)
            {
                return NotFound();
            }

            return View(reaction);
        }

        // POST: ReactionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Reaction reaction)
        {
            try
            {
                _reactionRepository.UpdateReaction(reaction);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(reaction);
            }
        }

        // GET: ReactionController/Delete/5
        public ActionResult Delete(int id)
        {
            Reaction reaction = _reactionRepository.GetReaction(id);

            return View(reaction);
        }

        // POST: ReactionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Reaction reaction)
        {
            try
            {
                _reactionRepository.GetReaction(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(reaction);
            }
        }
    }
}
