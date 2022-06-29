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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.AddCategory(category);
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }

        public IActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetCategory(id);

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.DeleteCategory(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }
    }
}
