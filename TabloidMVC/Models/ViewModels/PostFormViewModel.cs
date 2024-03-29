﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace TabloidMVC.Models.ViewModels
{
    public class PostFormViewModel
    {
        public Post Post { get; set; }
        public List<Category> CategoryOptions { get; set; }

        public PostImage PostImage { get; set; }

        public IFormFile File { get; set; }
    }
}
