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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        // GET: TagController
        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        // GET: TagController/Details/5
        public IActionResult Details(int id)
        {
            Tag tag = _tagRepository.GetTag(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: TagController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.AddTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(tag);
            }
        }

        // GET: TagController/Edit/5
        public IActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTag(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(tag);
            }
        }

        // GET: TagController/Delete/5
        public IActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTag(id);

            return View(tag);
        }
        

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(tag);
            }
        }
    }
}
